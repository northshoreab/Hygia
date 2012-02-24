using System.Collections.Generic;
using Hygia.Notifications.Domain;
using System;

namespace Hygia.Widgets.Models
{
    public class AlertListViewModel
    {
        public IList<Notification> Alerts = new List<Notification>
                                                       {
                                                           new Notification{Title = "1:a Database connection lost", NotificationDate = DateTime.Now, Summary = "Ett allvarligt fel har inträffat", Description = "Message: Database connection lost\nReason: ProcessingFailed\nExceptionType: System.Exception\nSource: MyServer"},
                                                           new Notification{Title = "2:a Database connection lost", NotificationDate = DateTime.Now, Summary = "Ett allvarligt fel har inträffat", Description = "Message: Database connection lost\nReason: ProcessingFailed\nExceptionType: System.Exception\nSource: MyServer"},
                                                           new Notification{Title = "3:e Database connection lost", NotificationDate = DateTime.Now, Summary = "Ett allvarligt fel har inträffat", Description = "Message: Database connection lost\nReason: ProcessingFailed\nExceptionType: System.Exception\nSource: MyServer"},
                                                       };
    }
}