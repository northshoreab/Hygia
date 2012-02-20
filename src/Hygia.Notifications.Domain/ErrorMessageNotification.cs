using System;

namespace Hygia.Notifications.Domain
{
    public class ErrorMessageNotification : Notification
    {
        public ErrorMessageNotification(Guid messageTypeId, string exception, string messageBody)
        {
            Title = "Error message for messagetype: " + messageTypeId.ToString() + "!";
            Description = "Exception:\n" + exception + "\n\nMessage:\n" + messageBody;
            Summary = "MessageType: " + messageTypeId.ToString() + "\nException:\n" + exception;
        }
    }
}