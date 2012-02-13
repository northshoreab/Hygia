using System.Linq;
using Hygia.Backend.Notifications.Domain;
using Hygia.Backend.SLA.Events;
using NServiceBus;
using Raven.Client;

namespace Hygia.Backend.Notifications.Handlers
{
    public class CriticalTimeSLAViolatedHandler : IHandleMessages<CriticalTimeSLAViolated>
    {
        public IDocumentSession Session { get; set; }

        public void Handle(CriticalTimeSLAViolated message)
        {
            var notifications = Session.Load<NotificationSetting>().Where(x => x.SLAId == message.SLA.Id);

            foreach (var notificationSetting in notifications)
            {
                Session.Store(new CriticalTimeNotification(message.SLA.MessageTypeId, message.Envelope.CriticalTime.Value, message.SLA.CriticalTime, notificationSetting.NotificationType));
            }
        }
    }
}