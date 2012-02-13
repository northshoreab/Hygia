using System;

namespace Hygia.Backend.SLA.Domain
{
    public class CriticalTimeSLA
    {
        public string Id { get; set; }
        public string MessageTypeId { get; set; }
        public TimeSpan CriticalTime { get; set; }
    }
}