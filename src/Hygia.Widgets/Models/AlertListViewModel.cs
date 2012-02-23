using System.Collections.Generic;
using Hygia.Notifications.Domain;
using System;

namespace Hygia.Widgets.Models
{
    public class AlertListViewModel
    {
        public IList<Notification> Alerts = new List<Notification>
                                                       {
                                                           new Notification{Title = "F�rsta", NotificationDate = DateTime.Now, Summary = "Ett allvarligt fel har intr�ffat"},
                                                           new Notification{Title = "Andra", NotificationDate = DateTime.Now, Summary = "Ett allvarligt fel har intr�ffat"},
                                                           new Notification{Title = "Tredje", NotificationDate = DateTime.Now, Summary = "Ett allvarligt fel har intr�ffat"},
                                                       };
    }
}