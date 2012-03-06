using System;

namespace Hygia.Notifications.Domain
{
    public class SLANotificationSetting
    {
        public Guid Id { get; set; }
        public string EmailAdress { get; set; }
        public string NotificationType { get; set; }
    }
}