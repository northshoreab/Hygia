namespace Hygia.LaunchPad.Inspectors
{
    using System.Collections.Generic;
    using AuditProcessing.Messages;
    using Commands;
    using Core;
    using NServiceBus;
    using NServiceBus.Unicast.Transport;
    using Monitoring = NServiceBus.Unicast.Monitoring;

    public class EnvelopeProcessingStatisticsInspector : IHandleMessages<AuditMessageProcessed>
    {
        IBus bus;

        public EnvelopeProcessingStatisticsInspector(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(AuditMessageProcessed messageProcessed)
        {
            if (!messageProcessed.HasHeader(Monitoring.Headers.TimeSent) ||
                !messageProcessed.HasHeader(Monitoring.Headers.ProcessingStarted) ||
                !messageProcessed.HasHeader(Monitoring.Headers.ProcessingEnded))
                return;

            var sent = messageProcessed.Headers[Monitoring.Headers.TimeSent].ToUtcDateTime();

            var begin = messageProcessed.Headers[Monitoring.Headers.ProcessingStarted].ToUtcDateTime();

            var end = messageProcessed.Headers[Monitoring.Headers.ProcessingEnded].ToUtcDateTime();

            bus.Send(new RegisterEnvelopeProcessingStatistics
                             {
                                 EnvelopeId = messageProcessed.EnvelopeId(),
                                 CriticalTime = (end-sent).TotalSeconds,
                                 ProcessingTime = (end - begin).TotalSeconds
                             });
        }

    }
}