namespace Hygia.LaunchPad.LogicalMonitoring.Inspectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Core;
    using Hygia.LaunchPad.Inspectors;
    using NServiceBus.Unicast;
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
                //todo - need to add a send side enricher to capture the intent
                var messageIntent = MessageIntent.Unknown;
                
                yield return new RegisterMessageType
                                 {
                                     MessageTypeId = messageType.TypeName.ToGuid(),
                                     MessageType = messageType.TypeName, 
                                     MessageVersion = messageType.Version.ToString(),
                                     MessageIntent = messageIntent
                                 };
            }
                
        }
    }

    public enum MessageIntent
    {
        Request,
        Response,
        Command,
        Event,
        Unknown
    }
}