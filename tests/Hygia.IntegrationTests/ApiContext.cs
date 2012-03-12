namespace Hygia.IntegrationTests
{
    using Machine.Specifications;
    using Raven.Client;
    using Raven.Client.Document;
    using RestSharp;

    public class ApiContext:RestSharpContext
    {
        protected static IDocumentStore Store;
      
        Establish context = () =>
                                {
                                    Store = new DocumentStore
                                                {
                                                    Url = "http://localhost:8080",
                                                    DefaultDatabase = "WatchR"
                                                };
                                    Store.Initialize();
                                    client = new RestClient("http://localhost:61000/");
                                };



    }
}