using System.Collections.Generic;
using System.Linq;
using Hygia.Backend.Notifications.Domain;
using Hygia.Backend.Notifications.Events;
using NServiceBus;
using Raven.Client;

namespace Hygia.Backend.Notifications.Handlers
{
    public class CriticalTimeSLANotificationHandler : IHandleMessages<CriticalTimeSLAViolated>,
                                                      IHandleMessages<CriticalTimeSLAWarning>
    {
        public IDocumentSession Session { get; set; }
        private readonly IEnumerable<NotificationSetting> _notificationSettings;

        public CriticalTimeSLANotificationHandler()
        {
            _notificationSettings = Session.Load<NotificationSetting>();
        }

        public void Handle(CriticalTimeSLAViolated message)
        {
            Session.Store(message);

            if(_notificationSettings.Any(x => x.SLAId == message.SLAId))
            {
                var sla = Session.Load<CriticalTimeSLA>(message.SLAId);
                Session.Store(new CriticalTimeNotification(message.MessageTypeId.ToString(), message.ActualValue, sla.AlarmThreshold, NotificationTypes.Alarm));
            }
        }

        public void Handle(CriticalTimeSLAWarning message)
        {
            Session.Store(message);

            if (_notificationSettings.Any(x => x.SLAId == message.SLAId))
            {
                var sla = Session.Load<CriticalTimeSLA>(message.SLAId);
                Session.Store(new CriticalTimeNotification(message.MessageTypeId.ToString(), message.ActualValue, sla.AlarmThreshold, NotificationTypes.Warning));
            }            
        }
    }
}