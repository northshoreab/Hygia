namespace Hygia
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LaunchPad.AuditProcessing.Events;
    using NServiceBus.Unicast.Monitoring;
    using NServiceBus.Unicast.Subscriptions;
    using NServiceBus.Unicast.Transport;

    public static class ProcessAuditMessageExtensions
    {
        public static bool HasHeader(this AuditMessageReceived transportMessageReceived, string header)
        {
            if (transportMessageReceived.Headers == null)
                return false;

            return transportMessageReceived.Headers.ContainsKey(header);
        }


        public static Guid EnvelopeId(this AuditMessageReceived transportMessageReceived)
        {
            return transportMessageReceived.MessageId.ToGuid();
        }
        public static bool IsControlMessage(this AuditMessageReceived transportMessageReceived)
        {
            return transportMessageReceived.Headers.ContainsKey(ControlMessage.ControlMessageHeader);
        }

        public static string CorrelationId(this AuditMessageReceived transportMessageReceived)
        {
            if (transportMessageReceived.AdditionalInformation.ContainsKey("CorrelationId"))
                return transportMessageReceived.AdditionalInformation["CorrelationId"];
            return null;
        }



        public static IEnumerable<MessageType> MessageTypes(this AuditMessageReceived transportMessageReceived)
        {
            var result = new List<MessageType>();

            if (!transportMessageReceived.HasHeader(Headers.EnclosedMessageTypes))
                return result;

            return transportMessageReceived.Headers[Headers.EnclosedMessageTypes].Split(';').ToList()
                .Select(s => new MessageType(s));

        }
    }
}
