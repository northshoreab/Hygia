using System;

namespace Hygia.Notifications.Domain
{
    public class FaultNotificationSetting
    {
        public Guid Id { get; set; }
        public Guid? MessageTypeId { get; set; }
        public bool AllMessages { get; set; }
        public string EmailAdress { get; set; }
        public string NotificationType { get; set; }
    }
}