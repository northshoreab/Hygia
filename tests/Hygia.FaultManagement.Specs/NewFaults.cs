using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hygia.FaultManagement.Specs
{
    using Core;
    using Domain;
    using Hygia.Specs;
    using Machine.Specifications;
    using Operations.Events;

    [Subject("New faults")]
    public class When_registering_a_new_fault : WithHandler<FaultMessageReceivedHandler>
    {
        static readonly Guid FaultEnvelopeId = Guid.NewGuid();
        static string originalId = Guid.NewGuid().ToString();

        Establish context = () =>
        {
        };

        Because of = () => Handle<FaultMessageReceived>(m =>
                                                            {

                                                                m.FaultEnvelopeId = FaultEnvelopeId.ToString();
                                                                m.Headers = new Dictionary<string, string>
                                                                    {
                                                      { "NServiceBus.OriginalId", originalId }, 
                                                      { "NServiceBus.TimeSent", "2012-02-21 21:38:57:236209 Z" },
                                                      { "NServiceBus.EnclosedMessageTypes", "OrderPlaced, Version=1.0.0.0" },
                                                      { "NServiceBus.FailedQ", "orders@myserver" },
                                                      { "NServiceBus.ExceptionInfo.Message", "Test exception message" },
                                                      { "NServiceBus.ExceptionInfo.Reason", "Some reason" },
                                                      { "NServiceBus.ExceptionInfo.ExceptionType", "TestException" },
                                                      { "NServiceBus.ExceptionInfo.Source", "source" },
                                                      { "NServiceBus.ExceptionInfo.StackTrace", "A stacktrace" }
                                                     
                                                  };
                                                                m.Body = "<xml/>";
                                                            });


        It should_assign_a_human_readable_number_to_the_fault = () => Session.Load<Fault>(originalId.ToGuid()).Number.ShouldBeGreaterThan(0);
    }
}
