namespace Hygia.IntegrationTests
{
    using Machine.Specifications;
    using RestSharp;

    public class WebContext : RestSharpContext
    {

        Establish context = () =>
                                {
                                    client = new RestClient("http://localhost:61000/");
                                };
    }
}