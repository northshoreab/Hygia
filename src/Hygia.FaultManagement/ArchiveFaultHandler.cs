namespace Hygia.FaultManagement
{
    using System;
    using Commands;
    using Domain;
    using NServiceBus;
    using Raven.Client;

    public class ArchiveFaultHandler:IHandleMessages<ArchiveFault>
    {
        public IDocumentSession Session { get; set; }

        public void Handle(ArchiveFault message)
        {
            var fault = Session.Load<Fault>(message.MessageId);

            if(fault == null)
                throw new InvalidOperationException("No fault with id " + message .MessageId+ "found");

            fault.Status = FaultStatus.Archived;
            fault.History.Add(new HistoryItem
                                  {
                                      Time = DateTime.UtcNow,
                                      Status = "Fault archived"
                                  });


        }
    }
}