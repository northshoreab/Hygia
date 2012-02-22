using System;

namespace Hygia.Notifications.Domain
{
    public class FaultNotification : Notification
    {
        public FaultNotification(Guid messageTypeId, string exceptionReason, DateTime time)
        {
            Title = "Fault message for messagetype: " + messageTypeId.ToString() + "!";
            Description = "Time: " + time + "Reason:\n" + exceptionReason;
            Summary = "Time: " + time + "\nMessageType: " + messageTypeId.ToString() + "\nReason:\n" + exceptionReason;
        }
    }
}