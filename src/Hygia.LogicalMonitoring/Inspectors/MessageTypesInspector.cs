using Hygia.Core;

namespace Hygia.LogicalMonitoring.Inspectors
{
    using System;
    using System.Linq;
    using Commands;
    using NServiceBus;
    using NServiceBus.Unicast.Subscriptions;
    using Operations.Events;

    public class MessageTypesInspector : IHandleMessages<AuditMessageReceived>, IHandleMessages<FaultMessageReceived>
    {
        readonly IBus bus;

        public MessageTypesInspector(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(AuditMessageReceived messageReceived)
        {
            var messageTypes = messageReceived.MessageTypes().ToList();

            if (!messageTypes.Any())
                return;

            foreach (var messageType in messageTypes)
                RegisterMessageType(DetectIntent(messageReceived, messageType), messageType);

        }

        public void Handle(FaultMessageReceived message)
        {
            var messageTypes = message.Headers.MessageTypes().Select(s => new MessageType(s)).ToList();

            if (!messageTypes.Any())
                return;

            foreach (var messageType in messageTypes)
                RegisterMessageType(MessageIntent.Unknown, messageType);

        }

        void RegisterMessageType(MessageIntent messageIntent, MessageType messageType)
        {
            bus.Send(new RegisterMessageType
                         {
                             MessageTypeId = messageType.TypeName.ToGuid(),
                             MessageType = messageType.TypeName,
                             MessageVersion = messageType.Version.ToString(),
                             MessageIntent = messageIntent
                         });
        }

        MessageIntent DetectIntent(AuditMessageReceived envelope, MessageType messageType)
        {
            if (envelope.AdditionalInformation.ContainsKey("MessageIntent"))
                switch (envelope.AdditionalInformation["MessageIntent"])
                {
                    case "Publish":
                        return MessageIntent.Event;
                    case "Unsubscribe":
                        return MessageIntent.Unsubscribe;
                    case "Subscribe":
                        return MessageIntent.Subscribe;
                }

            if (envelope.IsControlMessage())
                return MessageIntent.Control;

            if (envelope.CorrelationId() != Guid.Empty)
                return MessageIntent.Response;

            if (IsCommand(messageType))
                return MessageIntent.Command;


            return MessageIntent.Request;
        }

        bool IsCommand(MessageType messageType)
        {
            //improve to detect first tense
            return messageType.TypeName.Contains("Command");
        }


    }

    public enum MessageIntent
    {
        Request,
        Response,
        Command,
        Event,
        Subscribe,
        Unsubscribe,
        Control,
        Unknown
    }
}