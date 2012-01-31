namespace Hygia.LaunchPad.PhysicalMonitoring.Handlers
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using Domain;
    using NServiceBus;
    using Raven.Client;

    public class RegisterEnvelopeHandler:IHandleMessages<RegisterEnvelope>
    {
        public IDocumentSession Session { get; set; }

        public void Handle(RegisterEnvelope message)
        {
            var envelope = new Envelope
                            {
                                Id = message.EnvelopeId.ToString(),
                                TimeSent = message.TimeSent,
                                ProcessingStarted = message.ProcessingStarted,
                                ProcessingEnded = message.ProcessingEnded,
                                CorrelatedEnvelopeId = message.CorrelatedEnvelopeId,
                                ParentEnvelopeId = message.ParentEnvelopeId,
                                ContainedMessages = message.Messages
                            };

            if (envelope.TimeSent.HasValue && envelope.ProcessingEnded.HasValue)
                envelope.CriticalTime = envelope.ProcessingEnded - envelope.TimeSent;

            if (envelope.ProcessingStarted.HasValue && envelope.ProcessingEnded.HasValue)
                envelope.ProcessingTime = envelope.ProcessingEnded - envelope.ProcessingStarted;

            Session.Store(envelope);
        }
    }
}