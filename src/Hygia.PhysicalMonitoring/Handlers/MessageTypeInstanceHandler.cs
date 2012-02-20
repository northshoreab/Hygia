namespace Hygia.PhysicalMonitoring.Handlers
{
    using Domain;
    using Events;
    using NServiceBus;
    using Raven.Client;

    class MessageTypeInstanceHandler : IHandleMessages<EnvelopeRegistered>
    {
        public IDocumentSession Session { get; set; }

        public void Handle(EnvelopeRegistered message)
        {
            foreach (var msg in message.RegisteredEnvelope.ContainedMessages)
            {
                var messageTypeInstance = new MessgeTypeInstance
                                  {
                                      MessageId = msg.MessageId,
                                      MessageTypeId = msg.MessageTypeId,                                      
                                  };   

                if (message.RegisteredEnvelope.ProcessingEnded.HasValue)
                {
                    messageTypeInstance.ProcessingEnded = message.RegisteredEnvelope.ProcessingEnded.Value;
                }

                Session.Store(messageTypeInstance);
            }
        }
    }
}
