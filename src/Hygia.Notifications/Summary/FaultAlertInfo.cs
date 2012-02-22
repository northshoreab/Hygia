using Hygia.Alarms.Events;

namespace Hygia.Notifications.Summary
{
    public class FaultAlertInfo
    {
        public FaultAlarm FaultMessage { get; set; }
        public int NumberOfTimesAlerted { get; set; }
    }
}