using System;

namespace Hygia.Backend.Notifications.Domain
{
    public class CriticalTimeNotification : Notification
    {
        public CriticalTimeNotification(string messageType, TimeSpan messageCriticalTime, TimeSpan warningLevel, NotificationTypes notificationType)
        {
            Title = "Critical Time " + Enum.GetName(typeof(NotificationTypes), notificationType) + "!";
            Description = "Critical time for messagetype: " + messageType + "\n\nCritical time for message is: " +
                          messageCriticalTime + "\n\nWarning level is: " + warningLevel;

            Summary = "Critical time warning for: " + messageType;
        }
    }
}