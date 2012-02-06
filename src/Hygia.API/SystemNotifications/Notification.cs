using System;

namespace Hygia.API.SystemNotifications
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public Author Author { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}