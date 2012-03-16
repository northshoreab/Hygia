namespace Hygia.FaultManagement.Retries
{
    using System;
    using Hygia.FaultManagement.Commands;
    using Hygia.FaultManagement.Domain;
    using NServiceBus;
    using Raven.Client;

    public class IssueRetryForFaultHandler : IHandleMessages<IssueRetryForFault>
    {
        public IDocumentSession Session { get; set; }


        public IBus Bus { get; set; }


        public void Handle(IssueRetryForFault message)
        {
            var fault = Session.Load<Fault>(message.FaultId);

            if (fault == null)
                throw new InvalidOperationException("No fault with id " + message.FaultId + "found");


            if (fault.Status == FaultStatus.RetryRequested || fault.Status == FaultStatus.RetryPerformed)
                return;


            if (fault.Status == FaultStatus.Archived)
                throw new InvalidOperationException("Can't retry an archived fault - " + message.FaultId);



            fault.Status = FaultStatus.RetryRequested;
            fault.History.Add(new HistoryItem
                                  {
                                      Time = message.IssuedAt,
                                      Status = "Retry issued"
                                  });

            Bus.Send(new RetryFault
            {
                FaultId = fault.Id,
                FaultEnvelopeId = fault.FaultEnvelopeId
            });
        }
    }
}