using System;

namespace Hygia.Notifications.Domain
{
    public class NoticiationTypes
    {
        public const string RSS = "RSS";
    }

    public class NotificationSetting
    {
        public Guid SLAId { get; set; }
        public string NotificationType { get; set; }
    }
}