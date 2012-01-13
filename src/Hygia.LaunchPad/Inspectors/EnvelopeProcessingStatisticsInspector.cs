namespace Hygia.LaunchPad.Inspectors
{
    using System.Collections.Generic;
    using Commands;
    using Core;
    using NServiceBus;
    using NServiceBus.Unicast.Transport;
    using Timing = NServiceBus.Unicast.Timing;

    public class EnvelopeProcessingStatisticsInspector:IInspectEnvelopes
    {
        public IEnumerable<object> Inspect(TransportMessage transportMessage)
        {
            if (!transportMessage.HasHeader(Timing.Headers.TimeSent)||
                !transportMessage.HasHeader(Timing.Headers.ProcessingStarted) ||
                !transportMessage.HasHeader(Timing.Headers.ProcessingEnded))
                yield break;

            var sent = transportMessage.Headers[Timing.Headers.TimeSent].ToUtcDateTime();

            var begin = transportMessage.Headers[Timing.Headers.ProcessingStarted].ToUtcDateTime();

            var end = transportMessage.Headers[Timing.Headers.ProcessingEnded].ToUtcDateTime();

            yield return new RegisterEnvelopeProcessingStatistics
                             {
                                 EnvelopeId = transportMessage.EnvelopeId(),
                                 CriticalTime = (end-sent).TotalSeconds,
                                 ProcessingTime = (end - begin).TotalSeconds
                             };
        }
    }
}