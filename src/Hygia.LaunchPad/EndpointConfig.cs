namespace Hygia.LaunchPad
{
    using System;
    using System.Collections.Generic;
    using Core;
    using NServiceBus;
    using NServiceBus.UnitOfWork;
    using Raven.Client;
    using Raven.Client.Document;
    using StructureMap;
    using StructureMap.Graph;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith(".Messages"))
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Events"))
                .StructureMapBuilder()
                .XmlSerializer();
        }
    }

    class BootstrapRaven : IWantCustomInitialization
    {
        public void Init()
        {
            tennantIdToDatabaseLookup.Add(Guid.Parse("327951bf-bae4-46a4-93a0-71f61dfbe801"),"Hygia.Acme");
            var store = new DocumentStore
                            {
                                Url = "http://localhost:8080",
                                DefaultDatabase = Configure.EndpointName

                            };

            store.Initialize();

            ObjectFactory.Configure(c =>
                                        {
                                            c.ForSingletonOf<IDocumentStore>()
                                                .Use(store);
                                            c.For<IDocumentSession>()
                                                .Use(ctx =>
                                                         {
                                                             var bus = ctx.GetInstance<IBus>();
                                                             string database = null;

                                                             if(bus.CurrentMessageContext != null && bus.CurrentMessageContext.Headers.ContainsKey("TennantId"))
                                                                 database =
                                                                     tennantIdToDatabaseLookup[
                                                                         Guid.Parse(
                                                                             bus.CurrentMessageContext.Headers[
                                                                                 "TennantId"])];
                                                             var s = ctx.GetInstance<IDocumentStore>();

                                                             if (string.IsNullOrEmpty(database))
                                                                 return s.OpenSession();
                                                             
                                                             return s.OpenSession(database);
                                                         });

                                            PluginCache.AddFilledType(typeof(IDocumentSession));

                                            c.For<IManageUnitsOfWork>()
                                                .Use<RavenUnitOfWork>();

                                        });
        }
        static IDictionary<Guid,string> tennantIdToDatabaseLookup = new Dictionary<Guid, string>();
    }
}