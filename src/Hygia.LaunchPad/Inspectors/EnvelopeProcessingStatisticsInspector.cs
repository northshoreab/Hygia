namespace Hygia.LaunchPad.Inspectors
{
    using System.Collections.Generic;
    using Commands;
    using Core;
    using NServiceBus;
    using NServiceBus.Unicast.Transport;
    using Monitoring = NServiceBus.Unicast.Monitoring;

    public class EnvelopeProcessingStatisticsInspector:IInspectEnvelopes
    {
        public IEnumerable<object> Inspect(TransportMessage transportMessage)
        {
            if (!transportMessage.HasHeader(Monitoring.Headers.TimeSent)||
                !transportMessage.HasHeader(Monitoring.Headers.ProcessingStarted) ||
                !transportMessage.HasHeader(Monitoring.Headers.ProcessingEnded))
                yield break;

            var sent = transportMessage.Headers[Monitoring.Headers.TimeSent].ToUtcDateTime();

            var begin = transportMessage.Headers[Monitoring.Headers.ProcessingStarted].ToUtcDateTime();

            var end = transportMessage.Headers[Monitoring.Headers.ProcessingEnded].ToUtcDateTime();

            yield return new RegisterEnvelopeProcessingStatistics
                             {
                                 EnvelopeId = transportMessage.EnvelopeId(),
                                 CriticalTime = (end-sent).TotalSeconds,
                                 ProcessingTime = (end - begin).TotalSeconds
                             };
        }
    }
}