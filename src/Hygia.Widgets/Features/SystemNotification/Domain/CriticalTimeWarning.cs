using System;

namespace Hygia.Widgets.Features.SystemNotification.Domain
{
    public class CriticalTimeWarning : Notification
    {
        public CriticalTimeWarning(string messageType, TimeSpan messageCriticalTime, TimeSpan warningLevel)
        {
            Title = "Critical Time Warning!";
            Description = "Critical time for messagetype: " + messageType + "\n\nCritical time for message is: " +
                          messageCriticalTime + "\n\nWarning level is: " + warningLevel;

            Summary = "Critical time warning for: " + messageType;
        }
    }
}