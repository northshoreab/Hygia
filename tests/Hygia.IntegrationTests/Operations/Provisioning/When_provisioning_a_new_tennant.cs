namespace Hygia.IntegrationTests.Operations.Provisioning
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Machine.Specifications;
    using RestSharp;

    [Subject("Web")]
    public class When_provisioning_a_new_tennant : WebContext
    {

        Establish context = () =>
                                {
                                    request = new RestRequest("provisiontennant", Method.POST) { RequestFormat = DataFormat.Json };

                                    request.AddBody(new
                                                        {
                                                            TennantId = Guid.Parse("8ec1ce07-36c6-4043-98ed-7cd106239153"),
                                                            Name = "Acme"
                                                        });

                                };

        Because of = () =>
                         {
                             response = client.Execute(request);
                         };


        It should_create_the_tennant = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }
}