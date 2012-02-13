using System;
using FubuMVC.Core;
using FubuMVC.Core.Runtime;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Configuration.DSL;

namespace Hygia.Web
{
    public class ConfigureFubuMVC : FubuRegistry
    {
        public ConfigureFubuMVC()
        {
            // This line turns on the basic diagnostics and request tracing
            IncludeDiagnostics(true);

            // All public methods from concrete classes ending in "Controller"
            // in this assembly are assumed to be action methods
            Actions.IncludeClassesSuffixedWithController();

            // Policies
            Routes
                .IgnoreControllerNamesEntirely()
                .IgnoreMethodSuffix("Html")
                .RootAtAssemblyNamespace();            
            
            // Match views to action methods by matching
            // on model type, view name, and namespace
            Views.TryToAttachWithDefaultConventions();                       

            ApplyConvention<Hygia.Web.Behaviors.PersistenceConvention>();
        }
    }

    public class RavenDbRegistry : Registry
    {
        public RavenDbRegistry()
        { 
            var store = new DocumentStore
            {
                Url = "http://localhost:8080",
                DefaultDatabase = "Hygia.Backend"
            };

            store.Initialize();

            For<IDocumentStore>()
                .Singleton()
                .Use(store);

            For<IDocumentSession>()
                .HybridHttpOrThreadLocalScoped()
                .Use(OpenSession);        
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
            get { return "327951bf-bae4-46a4-93a0-71f61dfbe801"; }            
        }
    }
}