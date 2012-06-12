using System;
using Hygia.API.Authentication;
using Hygia.Operations;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Hygia.API
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

                                            PluginCache.AddFilledType(typeof (IDocumentSession));
                                        });
        }

        static IDocumentSession OpenSession(IContext ctx)
        {
            var currentStore = ctx.GetInstance<IDocumentStore>();
            var apiRequest = ctx.GetInstance<IApiRequest>();

            return RavenSession.OpenSession(apiRequest.EnvironmentId, currentStore);
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