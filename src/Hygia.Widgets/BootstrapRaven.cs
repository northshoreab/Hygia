using System;
using FubuMVC.Core.Runtime;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Graph;

namespace Hygia.Widgets
{
    class BootstrapRaven
    {
        public void Init()
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
                                                .HybridHttpOrThreadLocalScoped()
                                                .Use(OpenSession);

                                            PluginCache.AddFilledType(typeof (IDocumentSession));
                                        });
        }

        static IDocumentSession OpenSession(IContext ctx)
        {
            var request = ctx.GetInstance<IFubuRequest>();

            string environmentId = request.Get<ContextInputModel>().EnvironmentId;
            string database = environmentId == "327951bf-bae4-46a4-93a0-71f61dfbe801" ? "Hygia.Acme" : string.Empty;

            var currentStore = ctx.GetInstance<IDocumentStore>();
            return string.IsNullOrEmpty(database) ? currentStore.OpenSession() : currentStore.OpenSession(database);                      
        }

    }

    public class ContextInputModel
    {
        public System.Web.HttpCookieCollection Cookies { get; set; }
        public System.Collections.Specialized.NameValueCollection Headers { get; set; }
        public Uri Url { get; set; }

        public string EnvironmentId
        {
            // ta fram key på nått bra sätt...
            get
            {
                return string.Empty;
            }
        }
    }
}