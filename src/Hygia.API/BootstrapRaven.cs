using System;

namespace Hygia.API
{
    using System;
    using System.Collections.Generic;
    using FubuMVC.Core.Runtime;
    using Raven.Client;
    using Raven.Client.Document;
    using StructureMap;
    using StructureMap.Graph;

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
                                            c.ForSingletonOf<DocumentStore>()
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

            string database = string.Empty;

            if (!string.IsNullOrEmpty(environmentId))
                database = environmentIdToDatabaseLookup[Guid.Parse(environmentId)];

            var currentStore = ctx.GetInstance<IDocumentStore>();

            return string.IsNullOrEmpty(database) ? currentStore.OpenSession() : currentStore.OpenSession(database);                      
        }
        static readonly IDictionary<Guid, string> environmentIdToDatabaseLookup = new Dictionary<Guid, string>
                                                                                      {
                                                                                          { Guid.Parse("327951bf-bae4-46a4-93a0-71f61dfbe801"), "Hygia.Acme" },
                                                                                          { Guid.Parse("918490ce-5a0c-4260-aaf8-a4d080c1f5cf"), "WatchR.RavenHQ.Production" }
                                                                                      };
    }

    public class ContextInputModel
    {
        public System.Web.HttpCookieCollection Cookies { get; set; }
        public System.Collections.Specialized.NameValueCollection Headers { get; set; }
        public System.Collections.Specialized.NameValueCollection Params { get; set; }
        public Uri Url { get; set; }

        public string EnvironmentId
        {
            // ta fram key på nått bra sätt...
            get
            {
                var cookie = Cookies["environment"];
                if (cookie != null)
                    return cookie.Value;

                var header = Headers["environment"];

                if (header != null)
                    return header;

                var param = Params["environment"];

                if (param != null)
                    return param;

                return string.Empty;
            }
        }
    }
}