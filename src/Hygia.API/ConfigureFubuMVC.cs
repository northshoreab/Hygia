namespace Hygia.API
{
    using StructureMap;
    using FubuMVC.Core;
    using NServiceBus;

    public class ConfigureFubuMVC : FubuRegistry
    {
        public ConfigureFubuMVC()
        {
            NServiceBus.Configure
               .WithWeb()
               .HygiaMessageConventions()
               .DefineEndpointName("Hygia.API")
               .StructureMapBuilder(ObjectFactory.Container)
               .XmlSerializer()
               .MsmqTransport()
               .DontUseTransactions()
               .UnicastBus()
               .SendOnly();  // This line turns on the basic diagnostics and request tracing                        

            new BootstrapRaven().Init();

            IncludeDiagnostics(true);

            Applies
                .ToThisAssembly()
                .ToAssembly("Hygia.Operations.Faults.Api")
   .ToAssembly("Hygia.Operations.AuditUploads.Api"); //todo- Better way?

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
        }
    }
}