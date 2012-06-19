using Hygia.Operations;
using NServiceBus;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Hygia.API.Infrastructure
{
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
                                            c.ForSingletonOf<DocumentStore>()
                                                .Use(store);
                                            c.For<IDocumentSession>()
                                                .HybridHttpOrThreadLocalScoped()
                                                .Use(OpenSession);
                                        });
        }

        static IDocumentSession OpenSession(IContext ctx)
        {
            var currentStore = ctx.GetInstance<IDocumentStore>();
            var apiRequest = ctx.GetInstance<IApiRequest>();

            return RavenSession.OpenSession(apiRequest.EnvironmentId, currentStore);
        }
    }
}