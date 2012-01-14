namespace Hygia.LaunchPad.LogicalMonitoring.Handlers
{
    using System.Collections.Generic;
    using Commands;
    using Inspectors;
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

            var existingType = session.Load<MessageType>(messageTypeId);
            if (existingType == null)
                existingType = new MessageType
                                   {
                                       Id = message.MessageTypeId.ToString(),
                                       Type = message.MessageType,
                                       Intent = message.MessageIntent
                                   };

            //todo - current version, version updated event
            if (!existingType.Versions.Contains(message.MessageVersion))
                existingType.Versions.Add(message.MessageVersion);

            session.Store(existingType);

            session.SaveChanges();

        }
    }

    public class MessageType
    {
        ICollection<string> versions;

        public string Id { get; set; }

        public string Type { get; set; }

        public MessageIntent Intent { get; set; }

        public ICollection<string> Versions
        {
            get
            {
                return versions ?? new List<string>();
            }
            set
            {
                versions = value;
            }
        }
    }
}