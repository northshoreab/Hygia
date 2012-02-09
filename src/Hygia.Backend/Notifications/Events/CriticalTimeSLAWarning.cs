using System;
using NServiceBus;

namespace Hygia.Backend.Notifications.Events
{
    public class CriticalTimeSLAWarning : IMessage
    {
        public string SLAId { get; set; }
        public Guid MessageTypeId { get; set; }
        public TimeSpan ActualValue { get; set; }        
    }
}