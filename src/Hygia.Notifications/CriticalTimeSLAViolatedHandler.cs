using System.Linq;
using Hygia.Notifications.Domain;
using Hygia.ServiceLevelAgreements.Events;
using NServiceBus;
using Raven.Client;

namespace Hygia.Notifications
{
    public class CriticalTimeSLAViolatedHandler : IHandleMessages<CriticalTimeSLAViolated>
    {
        private readonly IDocumentSession _session;

        public CriticalTimeSLAViolatedHandler(IDocumentSession session)
        {
            _session = session;
        }

        public void Handle(CriticalTimeSLAViolated message)
        {
            var notifications = _session.Load<NotificationSetting>().Where(x => x.SLAId == message.SLAId);

            foreach (var notificationSetting in notifications)
            {
                _session.Store(new CriticalTimeNotification(message.MessageTypeId, message.MessageCriticalTime, message.CriticalTimeSetting, notificationSetting.NotificationType));
            }
        }
    }
}