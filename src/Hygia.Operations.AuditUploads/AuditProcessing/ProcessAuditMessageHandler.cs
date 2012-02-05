namespace Hygia.Operations.AuditUploads.AuditProcessing
{
    using AuditFeed.Commands;
    using Events;
    using NServiceBus;

    public class ProcessAuditMessageHandler : IHandleMessages<ProcessAuditMessage>
    {
        readonly IBus bus;

        public ProcessAuditMessageHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(ProcessAuditMessage message)
        {
            //todo - check tennant id, enfore message limits etc

            //todo - de duplicate

            bus.Publish<AuditMessageReceived>(e =>
                                                  {
                                                      e.SetHeader("TennantId", message.TennantId.ToString());

                                                      e.MessageId = message.MessageId;
                                                      e.Headers = message.Headers;
                                                      e.AdditionalInformation = message.AdditionalInformation;
                                                      e.Body = message.Body;
                                                  });

        }
    }
}