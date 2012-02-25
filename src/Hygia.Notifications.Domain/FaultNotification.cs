using System;

namespace Hygia.Notifications.Domain
{
    public class FaultNotification : Notification
    {
        public FaultNotification(string subject, string body)
        {
            Title = "Fault message for messagetype: " + subject + "!";
            Description = body;
            Summary = subject;
        }
    }
}