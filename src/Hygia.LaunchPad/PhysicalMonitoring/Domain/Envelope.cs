namespace Hygia.LaunchPad.PhysicalMonitoring.Handlers
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using Domain;

    public class Envelope
    {
        public string Id { get; set; }

        public DateTime TimeSent { get; set; }

        public Guid CorrelatedEnvelopeId { get; set; }

        public ICollection<PhysicalMessage> ContainedMessages { get; set; }
    }
}