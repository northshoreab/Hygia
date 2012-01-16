namespace Hygia.LaunchPad.PhysicalMonitoring.Inspectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Hygia.LaunchPad.Core;
    using NServiceBus;
    using NServiceBus.Unicast.Transport;
    using Headers = NServiceBus.Unicast.Monitoring.Headers;

    public class RegisterEnvelopeInspector : IInspectEnvelopes
    {
        public IEnumerable<object> Inspect(TransportMessage transportMessage)
        {
            var sent = transportMessage.Headers[Headers.TimeSent].ToUtcDateTime();

            var messages = transportMessage.MessageTypes()
                .Select((messageType,ordinal) =>new PhysicalMessage
                            {
                                MessageId = (transportMessage.IdForCorrelation + ordinal.ToString()).ToGuid(),
                                MessageTypeId = messageType.TypeName.ToGuid()
                            }).ToList();

           

            yield return new RegisterEnvelope
                             {
                                 EnvelopeId = transportMessage.EnvelopeId(),
                                 TimeSent = sent,
                                 CorrelatedEnvelopeId = transportMessage.CorrelationId != null ? transportMessage.CorrelationId.ToGuid():Guid.Empty,
                                 Messages = messages
                             };
        }
    }
}