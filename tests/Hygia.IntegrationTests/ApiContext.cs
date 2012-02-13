namespace Hygia.IntegrationTests
{
    using Machine.Specifications;
    using RestSharp;

    public class ApiContext:RestSharpContext
    {
      
        Establish context = () =>
                                {
                                    client = new RestClient("http://localhost:43852/");
                                };
    }
}