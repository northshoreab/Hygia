namespace Hygia.LaunchPad.LogicalMonitoring.Inspectors
{
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Core;
    using NServiceBus.Unicast.Subscriptions;
    using NServiceBus.Unicast.Transport;

    public class MessageTypesInspector : IInspectEnvelopes
    {
        public IEnumerable<object> Inspect(TransportMessage transportMessage)
        {
            var messageTypes = transportMessage.MessageTypes().ToList();

            if (!messageTypes.Any())
                yield break;

            foreach (var messageType in messageTypes)
            {
                var messageIntent = DetectIntent(transportMessage, messageType);
                
                yield return new RegisterMessageType
                                 {
                                     MessageTypeId = messageType.TypeName.ToGuid(),
                                     MessageType = messageType.TypeName, 
                                     MessageVersion = messageType.Version.ToString(),
                                     MessageIntent = messageIntent
                                 };
            }
                
        }

        MessageIntent DetectIntent(TransportMessage transportMessage, MessageType messageType)
        {
            switch(transportMessage.MessageIntent)
            {
                case MessageIntentEnum.Publish:
                    return MessageIntent.Event;
                case MessageIntentEnum.Unsubscribe:
                    return MessageIntent.Unsubscribe;
                case MessageIntentEnum.Subscribe:
                    return MessageIntent.Subscribe;
            }

            if(transportMessage.IsControlMessage())
                return MessageIntent.Control;

            if(transportMessage.CorrelationId != null)
                return MessageIntent.Response;

            if(IsCommand(messageType))
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