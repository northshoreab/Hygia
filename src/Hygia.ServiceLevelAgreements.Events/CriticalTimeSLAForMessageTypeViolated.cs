using System;

namespace Hygia.ServiceLevelAgreements.Events
{
    public class CriticalTimeSLAForMessageTypeViolated
    {   
        public DateTime TimeOfSLABreach { get; set; }
        public Guid MessageTypeId { get; set; }
        public TimeSpan ActiveSLA { get; set; }
        public TimeSpan ActualCriticalTime { get; set; }
    }
}