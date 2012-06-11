using System.Reflection;
using System.Threading;
using Hygia.API.Controllers.FaultManagement.Statistics;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Client.Listeners;

namespace Hygia.APITests
{
    public class ApiContext
    {
        protected static IDocumentStore DocumentStore { get; set; } 

        public ApiContext()
        {
            DocumentStore = new EmbeddableDocumentStore
                                {
                                    RunInMemory = true
                                }.Initialize();

            IndexCreation.CreateIndexes(Assembly.GetAssembly(typeof(NumberOfFaultsPerIntervalController)), DocumentStore);

            DocumentStore.ExecuteIndex(new NumberOfFaultsPerDay());
        }
    }

    public class ForceNonStaleQueryListener : IDocumentQueryListener
    {
        public void BeforeQueryExecuted(IDocumentQueryCustomization customization)
        {
            customization.WaitForNonStaleResults();
        }
    }
}