using System;

namespace Hygia.API
{
    using System;
    using System.Collections.Generic;
    using FubuMVC.Core.Runtime;
    using Operations;
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


            var currentStore = ctx.GetInstance<IDocumentStore>();


            return RavenSession.OpenSession(environmentId, currentStore);
        }

    }

    public class ContextInputModel
    {
        public System.Web.HttpCookieCollection Cookies { get; set; }
        public System.Collections.Specialized.NameValueCollection Headers { get; set; }
        public System.Collections.Specialized.NameValueCollection Params { get; set; }
        public Uri Url { get; set; }

        public string EnvironmentId
        {
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


                var apiKey = Headers["apikey"];

                if (apiKey != null)
                    return apiKey;

                return string.Empty;
            }
        }
    }
}