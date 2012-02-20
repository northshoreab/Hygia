using System;

namespace Hygia.Notifications.Commands
{
    public class RSSNotificationCommand
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}
