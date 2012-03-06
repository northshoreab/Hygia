namespace Hygia.Backend
{
    using NServiceBus;
    using NServiceBus.UnitOfWork;
    using Operations;
    using Raven.Client;
    using Raven.Client.Document;
    using StructureMap;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class RavenRegistry : Registry
    {
        public  RavenRegistry()
        {
            
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
            string environmentId = null;

            if (bus.CurrentMessageContext != null && bus.CurrentMessageContext.Headers.ContainsKey("EnvironmentId"))
                environmentId = bus.CurrentMessageContext.Headers["EnvironmentId"];

            var store = ctx.GetInstance<IDocumentStore>();

            return RavenSession.OpenSession(environmentId, store);
        }
    }
}