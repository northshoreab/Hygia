namespace Hygia.LaunchPad.PhysicalMonitoring.Handlers
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using NServiceBus;
    using Raven.Client;

    public class RegisterEnvelopeHandler:IHandleMessages<RegisterEnvelope>
    {
        public IDocumentSession Session { get; set; }

        public void Handle(RegisterEnvelope message)
        {
            var chain = new Envelope
                            {
                                Id = message.EnvelopeId.ToString(),
                                TimeSent = message.TimeSent,
                                CorrelatedEnvelopeId = message.CorrelatedEnvelopeId,
                                ContainedMessages = message.Messages
                            };

            Session.Store(chain);
        }
    }
}