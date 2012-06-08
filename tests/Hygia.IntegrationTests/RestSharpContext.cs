namespace Hygia.IntegrationTests
{
    using Machine.Specifications;
    using RestSharp;

    public class RestSharpContext
    {
        protected static IRestResponse response;
        protected static RestClient client;
        protected static RestRequest request;

        Because of = () =>
        {
            response = client.Execute(request);
        };
    }
}