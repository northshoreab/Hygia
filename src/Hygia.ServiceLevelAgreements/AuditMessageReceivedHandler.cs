using NServiceBus;

namespace Hygia.ServiceLevelAgreements
{
    using Commands;
    using Operations.Events;

    public class AuditMessageReceivedHandler : IHandleMessages<AuditMessageReceived>
    {
        public IBus Bus { get; set; }

        public void Handle(AuditMessageReceived message)
        {
            foreach (var applicativeMessage in message.Headers.MessageTypes())
                Bus.Send(new VerifySLA
                             {
                                 MessageTypeId = applicativeMessage.MessageTypeId(),
                                 ProcessedAt = message.Headers.ProcessingEnded(),
                                 CriticalTime = message.Headers.CriticalTime()
                             });

        }
    }
}