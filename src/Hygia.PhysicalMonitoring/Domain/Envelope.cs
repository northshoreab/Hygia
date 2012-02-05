namespace Hygia.PhysicalMonitoring.Domain
{
    using System;
    using System.Collections.Generic;

    public class Envelope
    {
        public string Id { get; set; }

        public DateTime? TimeSent { get; set; }

        public DateTime? ProcessingStarted { get; set; }

        public DateTime? ProcessingEnded { get; set; }

        public TimeSpan? CriticalTime { get; set; }

        public TimeSpan? ProcessingTime { get; set; }

        public Guid CorrelatedEnvelopeId { get; set; }

        public ICollection<PhysicalMessage> ContainedMessages { get; set; }

        public Guid ParentEnvelopeId { get; set; }
    }
}