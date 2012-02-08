using Hygia.Backend.Notifications.Command;
using NServiceBus;

namespace Hygia.Backend.Notifications.Handlers
{
    public class SendCriticalTimeEmailNotificationHandler : IHandleMessages<SendCriticalTimeEmailNotification>
    {
        public void Handle(SendCriticalTimeEmailNotification message)
        {
            throw new System.NotImplementedException();
        }
    }
}