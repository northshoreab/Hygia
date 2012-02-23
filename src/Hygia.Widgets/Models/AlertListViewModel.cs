using System.Collections.Generic;
using Hygia.Notifications.Domain;
using System;

namespace Hygia.Widgets.Models
{
    public class AlertListViewModel
    {
        public IList<Notification> Alerts = new List<Notification>
                                                       {
                                                           new Notification{Title = "Första", NotificationDate = DateTime.Now, Summary = "Ett allvarligt fel har inträffat"},
                                                           new Notification{Title = "Andra", NotificationDate = DateTime.Now, Summary = "Ett allvarligt fel har inträffat"},
                                                           new Notification{Title = "Tredje", NotificationDate = DateTime.Now, Summary = "Ett allvarligt fel har inträffat"},
                                                       };
    }
}