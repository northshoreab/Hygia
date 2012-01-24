namespace Hygia
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LaunchPad.AuditProcessing.Events;
    using NServiceBus.Unicast.Monitoring;
    using NServiceBus.Unicast.Subscriptions;
    using NServiceBus.Unicast.Transport;

    public static class AuditMessageReceivedExtensions
    {
        public static bool HasHeader(this AuditMessageReceived envelope, string header)
        {
            if (envelope.Headers == null)
                return false;

            return envelope.Headers.ContainsKey(header);
        }


        public static Guid EnvelopeId(this AuditMessageReceived envelope)
        {
            return envelope.MessageId.ToGuid();
        }
        public static bool IsControlMessage(this AuditMessageReceived transportMessageReceived)
        {
            return transportMessageReceived.Headers.ContainsKey(ControlMessage.ControlMessageHeader);
        }

        public static string CorrelationId(this AuditMessageReceived envelope)
        {
            if (envelope.AdditionalInformation.ContainsKey("CorrelationId"))
                return envelope.AdditionalInformation["CorrelationId"];
            return null;
        }



        public static IEnumerable<MessageType> MessageTypes(this AuditMessageReceived transportMessageReceived)
        {
            var result = new List<MessageType>();

            if (!transportMessageReceived.HasHeader(Headers.EnclosedMessageTypes))
                return result;

            return transportMessageReceived.Headers[Headers.EnclosedMessageTypes].Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                .Select(s => new MessageType(s));

        }

        public static IEnumerable<string> GetPipelineInfoFor(this AuditMessageReceived envelope,MessageType messageType)
        {
            var key = "NServiceBus.PipelineInfo." + messageType.TypeName;
            var result = new List<string>();

            if(!envelope.HasHeader(key))
                return result;

            return envelope.Headers[key].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        }
        
    }
}
