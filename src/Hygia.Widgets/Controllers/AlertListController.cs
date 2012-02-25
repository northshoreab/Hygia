using System;
using System.Linq;
using Hygia.Notifications.Domain;
using Hygia.Widgets.Models;
using Raven.Client;
using Raven.Client.Linq;

namespace Hygia.Widgets.Controllers
{
    public class AlertListController
    {
        private readonly IDocumentSession _session;

        public AlertListController(IDocumentSession session)
        {
            _session = session;
        }

        public AlertListViewModel get_alertlist()
        {
            var alertNotifications = _session.Query<Notification>().Where(x => x.NotificationDate >= DateTime.Now.AddDays(-2)).ToList();

            var alertListViewModel = new AlertListViewModel
                                         {
                                             Alerts = alertNotifications
                                         };

            return alertListViewModel;
        }
    }
}