using Raven.Client;
using Raven.Client.Embedded;

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
        }
    }
}