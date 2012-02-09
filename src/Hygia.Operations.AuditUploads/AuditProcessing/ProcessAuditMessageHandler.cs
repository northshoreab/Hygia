namespace Hygia.Operations.AuditUploads.AuditProcessing
{
    using Events;
    using Messages;
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
            //todo - de duplicate

            bus.Publish<AuditMessageReceived>(e =>
                                                  {
                                                      //todo - this should be the environment id instead
                                                      e.SetHeader("EnvironmentId", message.ApiKey.ToString());

                                                      e.MessageId = message.MessageId;
                                                      e.Headers = message.Headers;
                                                      e.AdditionalInformation = message.AdditionalInformation;
                                                      e.Body = message.Body;
                                                  });

        }
    }
}