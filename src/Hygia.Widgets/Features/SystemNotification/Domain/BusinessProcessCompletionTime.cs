using System;

namespace Hygia.Widgets.Features.SystemNotification.Domain
{
    public class BusinessProcessCompletionTime : Notification
    {
        public BusinessProcessCompletionTime(string messageStartType, string messageEndType, string ProcessName, TimeSpan warningLevel)
        {
            Title = "Business Process Completion Time Warning!";
            Description = "Business Process Completion Time for Process: " + ProcessName + " passed warning level: " +
                          warningLevel + "\n\nMessage start type is: " + messageStartType + " and message end type is: " + messageEndType;
            Summary = "Business Process Completion Time Warning for process: " + ProcessName;
        }
    }
}