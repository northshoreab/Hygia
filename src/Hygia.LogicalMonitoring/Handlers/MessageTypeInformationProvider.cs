namespace Hygia.LogicalMonitoring.Handlers
{
    using System;
    using Notifications.Provide;
    using Raven.Client;

    public class MessageTypeInformationProvider : IProvideMessageTypeInformation
    {
        public IDocumentSession Session { get; set; }
        public dynamic ProvideFor(Guid messageTypeId)
        {

            string messageTypeName = null;

            var messageType = Session.Load<MessageType>(messageTypeId);

            if (messageType == null)
                throw new InvalidOperationException("No message type found, id: " + messageTypeId);

            messageTypeName = messageType.Type;


            return new
                       {
                           MessageTypeName = messageTypeName
                       };
        }
    }
}