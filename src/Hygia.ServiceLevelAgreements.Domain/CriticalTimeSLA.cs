using System;

namespace Hygia.ServiceLevelAgreements.Domain
{
    public class CriticalTimeSLA
    {
        public string Id { get; set; }
        public string MessageTypeId { get; set; }
        public TimeSpan CriticalTime { get; set; }
    }
}