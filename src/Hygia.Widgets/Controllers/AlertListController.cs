using System.Linq;
using Hygia.Widgets.Models;
using Raven.Client;
using Raven.Client.Linq;

namespace Hygia.Widgets.Controllers
{
    using FaultManagement.Domain;

    public class AlertListController
    {
        private readonly IDocumentSession _session;

        public AlertListController(IDocumentSession session)
        {
            _session = session;
        }

        public FaultsViewModel get_alertlist()
        {
            var alertNotifications = _session.Query<Fault>().OrderByDescending(f=>f.TimeOfFailure).ToList();

            var alertListViewModel = new FaultsViewModel
                                         {
                                             Alerts = alertNotifications
                                         };

            return alertListViewModel;
        }
    }
}