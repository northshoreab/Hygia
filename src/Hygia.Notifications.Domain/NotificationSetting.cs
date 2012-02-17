namespace Hygia.Notifications.Domain
{
    public class NoticiationTypes
    {
        public const string RSS = "RSS";
    }

    public class NotificationSetting
    {
        public string SLAId { get; set; }
        public string NotificationType { get; set; }
    }
}