using FubuMVC.Spark;
using FubuMVC.Core;

namespace Hygia.Widgets
{
    public class ConfigureFubuMVC : FubuPackageRegistry
    {
        public ConfigureFubuMVC()
        {
            Applies
                .ToThisAssembly();

            // All public methods from concrete classes ending in "Controller"
            // in this assembly are assumed to be action methods
            Actions.IncludeClassesSuffixedWithController();

           // Policies
            Routes
                .IgnoreControllerNamesEntirely()
                .IgnoreMethodSuffix("Html")
                .RootAtAssemblyNamespace();

            this.UseSpark();

            // Match views to action methods by matching
            // on model type, view name, and namespace
            Views.TryToAttachWithDefaultConventions();
        }
    }
}