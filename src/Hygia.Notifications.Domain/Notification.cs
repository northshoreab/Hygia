using System;

namespace Hygia.Notifications.Domain
{
    public class Notification
    {
        public Notification()
        {
            Author = new Author { Name = "System" };
            NotificationDate = DateTime.Now;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public Author Author { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}