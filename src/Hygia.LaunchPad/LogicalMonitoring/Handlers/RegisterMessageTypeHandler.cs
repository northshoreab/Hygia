namespace Hygia.LaunchPad.LogicalMonitoring.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using NServiceBus;
    using Raven.Client;

    public class RegisterMessageTypeHandler : IHandleMessages<RegisterMessageType>
    {
        readonly IDocumentSession session;

        public RegisterMessageTypeHandler(IDocumentSession session)
        {
            this.session = session;
        }


        public void Handle(RegisterMessageType message)
        {
            var messageTypeId = message.MessageTypeId.ToString();

            var messageType = session.Load<MessageType>(messageTypeId);

            if (messageType == null)
            {

                messageType = new MessageType
                {
                    Id = messageTypeId,
                    Type = message.MessageType,
                    Versions = new List<string> { message.MessageVersion }
                };

            }

            messageType.Intent = message.MessageIntent;

            var version = message.MessageVersion;

            //improve - current version, version updated event
            if (!messageType.Versions.Contains(version))
                messageType.Versions.Add(version);

            session.Store(messageType);

            session.SaveChanges();

        }
    }
}