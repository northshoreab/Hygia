namespace Hygia.IntegrationTests.Operations.Faults
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Machine.Specifications;
    using RestSharp;

    [Subject("Api")]
    public class When_a_fault_message_is_uploaded_through_the_api_with_a_valid_apikey : ApiContext
    {
      
        Establish context = () =>
        {
            request = new RestRequest("upload/processfaultmessage", Method.POST) {RequestFormat = DataFormat.Json};

            request.AddHeader("apikey", Guid.Parse("327951bf-bae4-46a4-93a0-71f61dfbe801").ToString());
            request.AddBody(new
                                {
                                    MessageId = Guid.Parse("8ec1ce07-36c6-4043-98ed-7cd106239153").ToString(),
                                    Headers = new Dictionary<string, string>
                                                  {
                                                      { "NServiceBus.OriginalId", "xyz" }, 
                                                      { "NServiceBus.TimeSent", "2012-02-21 21:38:57:236209 Z" },
                                                      { "NServiceBus.EnclosedMessageTypes", "X.y, Version=1.0.0.0" },
                                                      { "NServiceBus.FailedQ", "orders@myserver" },
                                                      { "NServiceBus.ExceptionInfo.Message", "Test exception message" },
                                                      { "NServiceBus.ExceptionInfo.Reason", "Some reason" },
                                                      { "NServiceBus.ExceptionInfo.ExceptionType", "TestException" },
                                                      { "NServiceBus.ExceptionInfo.Source", "source" },
                                                      { "NServiceBus.ExceptionInfo.StackTrace", "A stacktrace" }
                                                     
                                                  },
                                    Body = "<xml/>"

                                });
            
        };



        It should_send_a_command_for_backend_processing = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);    
    }
}

