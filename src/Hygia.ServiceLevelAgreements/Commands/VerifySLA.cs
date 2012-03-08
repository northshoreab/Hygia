namespace Hygia.ServiceLevelAgreements.Commands
{
    using System;

    public class VerifySLA
    {
        public TimeSpan CriticalTime { get; set; }

        public Guid MessageTypeId { get; set; }

        public DateTime ProcessedAt { get; set; }
    }
}