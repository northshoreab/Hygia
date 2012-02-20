using System;

namespace Hygia.Notifications.Domain
{
    public class SLANotificationSetting
    {
        public Guid SLAId { get; set; }
        public string NotificationType { get; set; }
    }
}