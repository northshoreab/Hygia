using System;

namespace Hygia.ServiceLevelAgreements.Domain
{
    public class CriticalTimeSLA
    {
        public Guid Id { get; set; }
        public Guid MessageTypeId { get; set; }
        public TimeSpan CriticalTime { get; set; }
    }
}