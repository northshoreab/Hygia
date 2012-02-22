using Hygia.Alarms.Events;
using NServiceBus;

namespace Hygia.Notifications.Summary
{
    public class AlertTooManyErrorsInQueue : ICommand
    {
        public int Count { get; set; }
        public FaultAlarm FirstFaultMessage { get; set; }
    }
}