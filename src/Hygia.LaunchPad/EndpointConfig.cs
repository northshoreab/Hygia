namespace Hygia.LaunchPad
{
    using Core;
    using NServiceBus;
    using NServiceBus.UnitOfWork;
    using Raven.Client;
    using Raven.Client.Document;
    using StructureMap;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Commands"))
                .StructureMapBuilder()
                .XmlSerializer();
        }
    }

    class BootstrapRaven : IWantCustomInitialization
    {
        public void Init()
        {
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
                                                .Use(ctx => ctx.GetInstance<IDocumentStore>().OpenSession());
                                            c.For<IManageUnitsOfWork>()
                                                .Use<RavenUnitOfWork>();

                                        });
        }
    }
}