namespace Hygia.FaultManagement
{
    using System;
    using Commands;
    using Domain;
    using NServiceBus;
    using Raven.Client;

    public class RegisterSuccessfullRetryHandler : IHandleMessages<RegisterSuccessfullRetry>
    {
        public IDocumentSession Session { get; set; }

        public void Handle(RegisterSuccessfullRetry message)
        {
            var fault = Session.Load<Fault>(message.FaultId);

            if(fault == null)
                throw new InvalidOperationException("No fault with id " + message.FaultId + "found");

            fault.Status = FaultStatus.RetryPerformed;
            fault.History.Add(new HistoryItem
                                  {
                                      Time = message.TimeOfRetry,
                                      Status = "Fault retried"
                                  });


        }
    }
}