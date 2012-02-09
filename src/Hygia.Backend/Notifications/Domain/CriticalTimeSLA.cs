using System;

namespace Hygia.Backend.Notifications.Domain
{
    public class CriticalTimeSLA
    {
        public string Id { get; set; }
        public Guid MessageTypeId { get; set; }
        public TimeSpan AlarmThreshold { get; set; }
        public TimeSpan WarnThreshold { get; set; }
    }
}