namespace Hygia.LaunchPad.LogicalMonitoring.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using NServiceBus;
    using Raven.Client;

    public class RegisterMessageConsumersHandler : IHandleMessages<RegisterMessageConsumers>
    {
        readonly IDocumentSession session;

        public RegisterMessageConsumersHandler(IDocumentSession session)
        {
            this.session = session;
        }


        public void Handle(RegisterMessageConsumers message)
        {
            var messageTypeId = message.MessageTypeId.ToString();

            var messageType = session.Load<MessageType>(messageTypeId) ?? new MessageType{ Id = messageTypeId,};

            if (messageType.ConsumedByACs == null)
                messageType.ConsumedByACs = new List<Guid>();

            messageType.ConsumedByACs = messageType.ConsumedByACs.Union(message.ConsumedBy).ToList();

            session.Store(messageType);
        }
    }
}