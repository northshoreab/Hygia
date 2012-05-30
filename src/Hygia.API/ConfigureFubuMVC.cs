namespace Hygia.API
{
    using System.Web;
    using Authentication;
    using Behaviors;
    using Controllers;
    using FubuMVC.Core.Runtime;
    using FubuMVC.Spark;
    using Mutators;
    using NServiceBus.MessageMutator;
    using Operations.Communication.Api;
    using StructureMap;
    using FubuMVC.Core;
    using NServiceBus;

    public class ConfigureFubuMVC : FubuRegistry
    {
        public ConfigureFubuMVC()
        {
            NServiceBus.Configure
               .With(HttpRuntime.BinDirectory)
               .HygiaMessageConventions()
               .DefineEndpointName("Hygia.API")
               .StructureMapBuilder(ObjectFactory.Container)
               .XmlSerializer()
               .MsmqTransport()
               .DontUseTransactions()
               .UnicastBus()
               .SendOnly(); 

            ObjectFactory.Configure(c =>
              c.Scan(s =>
              {
                  s.LookForRegistries();
                  s.AssembliesFromApplicationBaseDirectory();
              }));


            this.UseSpark();
            IncludeDiagnostics(true);
            Applies.ToThisAssembly()
                .ToAllPackageAssemblies()
                .ToAssembly("Hygia.Operations.Faults.Api")
                .ToAssembly("Hygia.Operations.Communication.Api")
                  .ToAssembly("Hygia.Operations.Communication.Domain")
                .ToAssembly("Hygia.FaultManagement.Api")
                .ToAssembly("Hygia.LogicalMonitoring.Api")
                .ToAssembly("Hygia.Operations.AuditUploads.Api")
                .ToAssembly("Hygia.UserManagement.Api");
            //todo- Better way?

            // All public methods from concrete classes ending in "Controller"
            // in this assembly are assumed to be action methods
            Actions.IncludeClassesSuffixedWithController();
            Routes.HomeIs<HomeController>(c => c.get_home());

            // Policies
            Routes
                .IgnoreControllerNamesEntirely()
                .IgnoreMethodSuffix("Html")
                .RootAtAssemblyNamespace();

            // Match views to action methods by matching
            // on model type, view name, and namespace
            Views.TryToAttachWithDefaultConventions();


            //todo: use scanning instead
            ApplyConvention<PersistenceConvention>();
            ApplyConvention<CommandsToPickUpBehaviourConfiguration>();
            ApplyConvention<AuthorizeByAttributeConvention>(); 
        }

        public class ContextInputModel
        {
            public System.Web.HttpCookieCollection Cookies { get; set; }
            public System.Collections.Specialized.NameValueCollection Headers { get; set; }
            public System.Collections.Specialized.NameValueCollection Params { get; set; }
            public string Url { get; set; }
        }
    }


}