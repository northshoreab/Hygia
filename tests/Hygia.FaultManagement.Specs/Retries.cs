using System;

namespace Hygia.FaultManagement.Specs
{
    using Commands;
    using Domain;
    using Hygia.Specs;
    using Machine.Specifications;
    using Retries;

    [Subject("Retries")]
    public class When_retrying_a_fault_that_is_already_beeing_retried : WithHandler<IssueRetryForFaultHandler>
    {
        static readonly Guid faultId = Guid.NewGuid();

        Establish context = () => Session.Store(new Fault
                                                    {
                                                        Id = faultId,
                                                        Status = FaultStatus.RetryRequested
                                                    });

        Because of = () => Handle<IssueRetryForFault>(m => { m.FaultId = faultId; });


        It should_not_issue_a_retry = () => AssertWasNotSent<RetryFault>();

    }

    [Subject("Retries")]
    public class When_retrying_a_fault_that_has_been_retried : WithHandler<IssueRetryForFaultHandler>
    {
        static readonly Guid faultId = Guid.NewGuid();

        Establish context = () => Session.Store(new Fault
        {
            Id = faultId,
            Status = FaultStatus.RetryPerformed
        });

        Because of = () => Handle<IssueRetryForFault>(m => { m.FaultId = faultId; });


        It should_not_issue_a_retry = () => AssertWasNotSent<RetryFault>();

    }
}
