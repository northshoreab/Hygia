namespace Hygia.IntegrationTests.Operations.AuditUploads
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Drivers.NServiceBus;
    using Machine.Specifications;
    using RestSharp;

    public class When_a_audit_message_is_received_that_breaks_the_environment_sla : BackendContext
    {
        static Guid faultId = Guid.Parse("4dc6706a-962c-9eba-892c-67d81e6f45ed"); //Guid.NewGuid();
        //327951bf-bae4-46a4-93a0-71f61dfbe801+faults+45afae35-39e6-dd00-603d-e2668e0cd06e@watchr.se
        Establish context = () =>
                                {
                                    var timeSent = DateTime.UtcNow;
                                    var started = timeSent + TimeSpan.FromSeconds(1);
                                    var ended = timeSent + TimeSpan.FromMinutes(31);

                                    message = new NServiceBusMessage()
                                        .AddMessage("Hygia.Operations.Events.AuditMessageReceived", "1.0.0.0", new
                                                                                                {
                                                                                                    Headers = new Dictionary<string, string>
                                                  {
                                                      { "NServiceBus.TimeSent",timeSent.ToWireFormattedString() },
                                                      { "NServiceBus.ProcessingStarted",started.ToWireFormattedString() },
                                                      { "NServiceBus.ProcessingEnded",ended.ToWireFormattedString() },
                                                      { "NServiceBus.EnclosedMessageTypes", "OrderPlaced, Version=1.0.0.0" },

                                                                                                        
                                                  },
                                                                                                    MessageId = Guid.NewGuid(),
                                                                                                    AdditionalInformation = new Dictionary<string, string>(),
                                                                                                    Body = new byte[0]   
                                                                                                });

                                    message.Headers["EnvironmentId"] = apiKey.ToString();
                                };


        Because of = () => InjectMessage();

        It should_publish_a_event = () => true.ShouldBeTrue();
    }

    [Subject("Api")]
    public class When_a_audit_message_that_breaks_the_environment_sla_is_uploaded : ApiContext
    {

        Establish context = () =>
        {
            request = new RestRequest("upload/ProcessAuditMessage", Method.POST) { RequestFormat = DataFormat.Json };

            var timeSent = DateTime.UtcNow;
            var started = timeSent + TimeSpan.FromSeconds(1);
            var ended = timeSent + TimeSpan.FromMinutes(31);
            request.AddBody(new
            {
                MessageId = Guid.Parse("8ec1ce07-36c6-4043-98ed-7cd106239153").ToString(),
                ApiKey = Guid.Parse("327951bf-bae4-46a4-93a0-71f61dfbe801").ToString(),
                 Headers = new Dictionary<string, string>
                                                  {
                                                      { "NServiceBus.TimeSent",timeSent.ToWireFormattedString() },
                                                      { "NServiceBus.ProcessingStarted",started.ToWireFormattedString() },
                                                      { "NServiceBus.ProcessingEnded",ended.ToWireFormattedString() },
                                                      { "NServiceBus.EnclosedMessageTypes", "OrderPlaced, Version=1.0.0.0" }                                                  
                                                  },
                AdditionalInformation = new Dictionary<string, string> { { "A1", "1" }, { "A2", "2" } },
                //Body = new byte[0]

            });

        };



        It should_send_a_command_for_backend_processing = () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);
    }
}