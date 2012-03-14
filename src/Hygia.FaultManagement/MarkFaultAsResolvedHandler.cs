namespace Hygia.FaultManagement
{
    using System;
    using Commands;
    using Domain;
    using NServiceBus;
    using Raven.Client;

    public class MarkFaultAsResolvedHandler : IHandleMessages<MarkFaultAsResolved>
    {
        public IDocumentSession Session { get; set; }

        public void Handle(MarkFaultAsResolved message)
        {
            var fault = Session.Load<Fault>(message.FaultId);

            if(fault == null)
                throw new InvalidOperationException("No fault with id " + message.FaultId + "found");

            fault.Status = FaultStatus.Resolved;
            fault.ResolvedAt = message.ResolvedAt;
            fault.History.Add(new HistoryItem
                                  {
                                      Time = message.ResolvedAt,
                                      Status = "Fault resolved by a retry"
                                  });


        }
    }
}