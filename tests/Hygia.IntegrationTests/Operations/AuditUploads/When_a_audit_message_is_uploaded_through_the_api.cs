namespace Hygia.IntegrationTests.Operations.AuditUploads
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Machine.Specifications;
    using RestSharp;

    [Subject("Api")]
    public class When_a_audit_message_is_uploaded_through_the_api_with_a_valid_apikey
    {
        static RestResponse response;
        static RestClient client;
        static RestRequest request;


        Establish context = () =>
        {
            client = new RestClient("http://localhost:43852/");
            request = new RestRequest("upload", Method.POST) {RequestFormat = DataFormat.Json};

            request.AddBody(new
                                {
                                    MessageId = Guid.Parse("8ec1ce07-36c6-4043-98ed-7cd106239153").ToString(),
                                    ApiKey = Guid.Parse("12345678-36c6-4043-98ed-7cd106239153").ToString(),
                                    Headers = new Dictionary<string, string> { { "H1", "1" }, { "H2", "2" } },
                                    AdditionalInformation = new Dictionary<string, string> { { "A1", "1" }, { "A2", "2" } },
                                    //Body = new byte[0]

                                });

        };

        Because of = () =>
                         {
                             response = client.Execute(request);
                         };


        It should_send_a_command_for_backend_processing = () =>
                                                           {
                                                               response.StatusCode.ShouldEqual(HttpStatusCode.OK);
                                                           };    
    }
}

