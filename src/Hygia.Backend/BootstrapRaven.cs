namespace Hygia.Backend
{
    using System;
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBus.UnitOfWork;
    using Operations;
    using Raven.Client;
    using Raven.Client.Document;
    using StructureMap;
    using StructureMap.Graph;

    class BootstrapRaven : IWantCustomInitialization
    {
        public void Init()
        {
            environmentIdToDatabaseLookup.Add(Guid.Parse("327951bf-bae4-46a4-93a0-71f61dfbe801"),"Hygia.Acme");

            var store = new DocumentStore
            {
                ConnectionStringName = "RavenDB"
            };

            store.Initialize();

            ObjectFactory.Configure(c =>
                                        {
                                            c.ForSingletonOf<IDocumentStore>()
                                                .Use(store);
                                            c.For<IDocumentSession>()
                                                .Use(OpenSession);

                                            PluginCache.AddFilledType(typeof(IDocumentSession));

                                            c.For<IManageUnitsOfWork>()
                                                .Use<RavenUnitOfWork>();
                                        });
        }

        static IDocumentSession OpenSession(IContext ctx)
        {
            var bus = ctx.GetInstance<IBus>();
            string database = null;

            if (bus.CurrentMessageContext != null && bus.CurrentMessageContext.Headers.ContainsKey("EnvironmentId"))
                database = environmentIdToDatabaseLookup[Guid.Parse(bus.CurrentMessageContext.Headers["EnvironmentId"])];

            var s = ctx.GetInstance<IDocumentStore>();

            if (string.IsNullOrEmpty(database))
                return s.OpenSession();

            return s.OpenSession(database);
        }

        static readonly IDictionary<Guid,string> environmentIdToDatabaseLookup = new Dictionary<Guid, string>();
    }
}