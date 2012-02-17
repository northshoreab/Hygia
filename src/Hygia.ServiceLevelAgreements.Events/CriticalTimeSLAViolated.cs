using System;

namespace Hygia.ServiceLevelAgreements.Events
{
    public class CriticalTimeSLAViolated : SLAMessage
    {
        public string SLAId { get; set; }
        public string MessageTypeId { get; set; }
        public TimeSpan CriticalTimeSetting { get; set; }
        public TimeSpan MessageCriticalTime { get; set; }
    }
}