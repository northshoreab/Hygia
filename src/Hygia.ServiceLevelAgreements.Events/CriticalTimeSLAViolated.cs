using System;

namespace Hygia.ServiceLevelAgreements.Events
{
    public class CriticalTimeSLAViolated : SLABreachMessage
    {
        public Guid SLAId { get; set; }
        public Guid MessageTypeId { get; set; }
        public TimeSpan CriticalTimeSetting { get; set; }
        public TimeSpan MessageCriticalTime { get; set; }
    }
}