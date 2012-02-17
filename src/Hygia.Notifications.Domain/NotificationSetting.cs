namespace Hygia.Notifications.Domain
{
    public class NoticiationTypes
    {
        public const string Alarm = "ALARM";
        public const string Warning = "WARNING";
    }

    public class NotificationSetting
    {
        public string SLAId { get; set; }
        public string NotificationType { get; set; }
    }
}