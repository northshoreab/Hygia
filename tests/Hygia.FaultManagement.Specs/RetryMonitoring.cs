namespace Hygia.FaultManagement.Specs
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using Events;
    using Machine.Specifications;
    using Operations.Events;
    

    [Subject("Fault lifecycle management")]
    public class When_a_new_fault_is_registered : FaultRetrySagaContext
    {
        Because of = () => Handle<FaultRegistered>(m => { m.FaultId = faultId; });


        It should_start_the_saga_up = () => SagaData.FaultId.ShouldEqual(faultId);
    }

    [Subject("Fault lifecycle management")]
    public class When_a_audit_message_arrives_for_the_same_fault_id : FaultRetrySagaContext
    {
        static DateTime sentTime = DateTime.UtcNow;

        Establish context = () =>
                                {
                                    SagaData.FaultId = faultId;
                                };

        Because of = () => Handle<AuditMessageReceived>(m =>
                                                            {
                                                                m.MessageId = faultId;
                                                                m.Headers = new Dictionary<string, string> { { "NServiceBus.SentTime", sentTime.ToWireFormattedString() } };
                                                            });


        It should_detect_that_the_fault_is_now_resolved = () => AssertWasSent<MarkFaultAsResolved>(m => m.ResolvedAt.ToWireFormattedString() == sentTime.ToWireFormattedString());
    }
}
