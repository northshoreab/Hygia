namespace Hygia.IntegrationTests.Operations.Provisioning
{
    using System;
    using System.Net;
    using Machine.Specifications;
    using RestSharp;

    [Subject("Web")]
    public class When_creating_a_new_account_with_valid_information : WebContext
    {

        Establish context = () =>
                                {
                                    request = new RestRequest("api/operations/accounts", Method.POST) { RequestFormat = DataFormat.Json };

                                    request.AddBody(new
                                                        {
                                                            AccountId = Guid.Parse("8fc1ce07-36c6-4043-98ed-7cd106239153"),
                                                            Name = "Acme"
                                                        });

                                };

        It should_create_the_account = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }
}