namespace Hygia.FaultManagement
{
    using System;
    using Commands;
    using Domain;
    using NServiceBus;
    using Raven.Client;

    public class IssueRetryForFaultHandler : IHandleMessages<IssueRetryForFault>
    {
        public IDocumentSession Session { get; set; }


        public IBus Bus { get; set; }


        public void Handle(IssueRetryForFault message)
        {
            var fault = Session.Load<Fault>(message.MessageId);

            if (fault == null)
                throw new InvalidOperationException("No fault with id " + message.MessageId + "found");

            fault.Status = FaultStatus.RetryIssued;
            fault.History.Add(new HistoryItem
                                  {
                                      Time = DateTime.UtcNow,
                                      Status = "Retry issued"
                                  });

            Bus.Send(new RetryFault
            {
                FaultEnvelopeId = fault.FaultEnvelopeId
            });
        }
    }
}