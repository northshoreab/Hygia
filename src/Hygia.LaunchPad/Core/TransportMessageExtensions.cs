namespace Hygia
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NServiceBus.Unicast;
    using NServiceBus.Unicast.Subscriptions;
    using NServiceBus.Unicast.Transport;

    public static class TransportMessageExtensions
    {
        public static bool HasHeader(this TransportMessage transportMessage, string header)
        {
            if (transportMessage.Headers == null)
                return false;

            return transportMessage.Headers.ContainsKey(header);
        }


        public static Guid EnvelopeId(this TransportMessage transportMessage)
        {
            return transportMessage.IdForCorrelation.ToGuid();
        }


        public static IEnumerable<MessageType> MessageTypes(this TransportMessage transportMessage)
        {
            var result = new List<MessageType>();

            if (!transportMessage.HasHeader(EnclosedMessageTypesMutator.EnclosedMessageTypes))
                return result;

            return transportMessage.Headers[EnclosedMessageTypesMutator.EnclosedMessageTypes].Split(';').ToList()
                .Select(s => new MessageType(s));

        }
    }
}
