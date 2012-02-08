namespace Hygia.PhysicalMonitoring.Handlers
{
    using Commands;
    using Domain;
    using NServiceBus;
    using Raven.Client;

    public class RegisterEnvelopeHandler:IHandleMessages<RegisterEnvelope>
    {
        private readonly IBus _bus;
        public IDocumentSession Session { get; set; }

        public RegisterEnvelopeHandler(IBus bus)
        {
            _bus = bus;
        }

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

            _bus.Publish<EnvelopeRegistered>(e => { e.RegisteredEnvelope = envelope; });
        }
    }
}