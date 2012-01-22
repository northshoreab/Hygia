namespace Hygia
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LaunchPad.AuditProcessing.Messages;
    using NServiceBus.Unicast.Monitoring;
    using NServiceBus.Unicast.Subscriptions;
    using NServiceBus.Unicast.Transport;

    public static class ProcessAuditMessageExtensions
    {
        public static bool HasHeader(this AuditMessageProcessed transportMessageProcessed, string header)
        {
            if (transportMessageProcessed.Headers == null)
                return false;

            return transportMessageProcessed.Headers.ContainsKey(header);
        }


        public static Guid EnvelopeId(this AuditMessageProcessed transportMessageProcessed)
        {
            return transportMessageProcessed.MessageId.ToGuid();
        }
        public static bool IsControlMessage(this AuditMessageProcessed transportMessageProcessed)
        {
            return transportMessageProcessed.Headers.ContainsKey(ControlMessage.ControlMessageHeader);
        }

        public static string CorrelationId(this AuditMessageProcessed transportMessageProcessed)
        {
            if (transportMessageProcessed.AdditionalInformation.ContainsKey("CorrelationId"))
                return transportMessageProcessed.AdditionalInformation["CorrelationId"];
            return null;
        }



        public static IEnumerable<MessageType> MessageTypes(this AuditMessageProcessed transportMessageProcessed)
        {
            var result = new List<MessageType>();

            if (!transportMessageProcessed.HasHeader(Headers.EnclosedMessageTypes))
                return result;

            return transportMessageProcessed.Headers[Headers.EnclosedMessageTypes].Split(';').ToList()
                .Select(s => new MessageType(s));

        }
    }
}
