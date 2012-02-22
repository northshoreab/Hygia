using Hygia.Notifications.Summary.Domain;
using NServiceBus;

namespace Hygia.Notifications.Summary.Commands
{
    public class AlertTooManyAlertsInQueue : ICommand
    {
        public int Count { get; set; }
        public AlertInfo FirstAlert { get; set; }
    }
}