using System.Collections.Generic;
using System.Linq;
using Hygia.Backend.Notifications.Domain;
using Hygia.Backend.SLA.Domain;
using Hygia.Backend.SLA.Events;
using NServiceBus;
using Raven.Client;

namespace Hygia.Backend.Notifications.Handlers
{
    public class SLANotificationHandler : IHandleMessages<SLAViolated>
    {
        public IDocumentSession Session { get; set; }
        private readonly IEnumerable<NotificationSetting> _notificationSettings;

        public SLANotificationHandler()
        {
            _notificationSettings = Session.Load<NotificationSetting>();
        }

        public void Handle(SLAViolated message)
        {
            if (_notificationSettings.All(x => x.SLAId != message.SLA.Id)) return;

            if(message.SLA.Rule.GetType() == typeof(CriticalTimeRule))
            {
                var rule = (CriticalTimeRule) message.SLA.Rule;

                if (message.Envelope.CriticalTime != null)
                    Session.Store(new CriticalTimeNotification(rule.MessageTypeId.ToString(), message.Envelope.CriticalTime.Value, rule.AlarmThreshold, message.SLA.Name));
            }
        }
    }
}