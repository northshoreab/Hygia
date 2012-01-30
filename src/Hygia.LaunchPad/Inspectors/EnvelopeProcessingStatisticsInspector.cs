namespace Hygia.LaunchPad.Inspectors
{
    using System.Collections.Generic;
    using AuditProcessing.Events;
    using Commands;
    using Core;
    using NServiceBus;
    using NServiceBus.Unicast.Transport;
    using Monitoring = NServiceBus.Unicast.Monitoring;

    public class EnvelopeProcessingStatisticsInspector : IHandleMessages<AuditMessageReceived>
    {
        IBus bus;

        public EnvelopeProcessingStatisticsInspector(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(AuditMessageReceived messageReceived)
        {
            if (!messageReceived.HasHeader(Monitoring.Headers.TimeSent) ||
                !messageReceived.HasHeader(Monitoring.Headers.ProcessingStarted) ||
                !messageReceived.HasHeader(Monitoring.Headers.ProcessingEnded))
                return;

            var sent = messageReceived.Headers[Monitoring.Headers.TimeSent].ToUtcDateTime();

            var begin = messageReceived.Headers[Monitoring.Headers.ProcessingStarted].ToUtcDateTime();

            var end = messageReceived.Headers[Monitoring.Headers.ProcessingEnded].ToUtcDateTime();

            bus.Send(new RegisterEnvelopeProcessingStatistics
                             {
                                 EnvelopeId = messageReceived.EnvelopeId(),
                                 CriticalTime = (end-sent).TotalSeconds,
                                 ProcessingTime = (end - begin).TotalSeconds
                             });
        }

    }
}