using Hygia.Backend.Notifications.Command;
using Hygia.Backend.Notifications.Domain;
using NServiceBus;
using Raven.Client;

namespace Hygia.Backend.Notifications.Handlers
{
    public class RegisterCriticalTimeRssNotificationHandler : IHandleMessages<RegisterCriticalTimeRssNotification>
    {
        public IDocumentSession Session { get; set; }

        public void Handle(RegisterCriticalTimeRssNotification message)
        {
            Session.Store(new CriticalTimeRssNotification(message.MessageType.TypeName, message.MessageCriticalTime, message.AlertLevel));
        }
    }
}
