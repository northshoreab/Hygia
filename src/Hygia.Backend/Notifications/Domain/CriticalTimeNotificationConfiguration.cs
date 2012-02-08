using System;
using System.Collections.Generic;
using Hygia.Backend.Notifications.Command;
using Hygia.PhysicalMonitoring.Domain;
using NServiceBus;
using NServiceBus.Unicast.Subscriptions;

namespace Hygia.Backend.Notifications.Domain
{
    public class CriticalTimeNotificationConfiguration : NotificationConfiguration
    {
        public string Id { get; set; }
        public MessageType MessageType { get; set; }
        public TimeSpan AlertLevel { get; set; }
        public IEnumerable<NotificationTypes> NotificationTypes { get; set; }

        public override IEnumerable<IMessage> EnvelopeNotifications(Envelope envelope)
        {
            if(envelope.CriticalTime > AlertLevel)
            {
                foreach (var alertTypes in NotificationTypes)
                {
                    switch (alertTypes)
                    {
                        case Domain.NotificationTypes.Rss:
                            yield return new RegisterCriticalTimeRssNotification
                                             {
                                                 AlertLevel = AlertLevel,
                                                 MessageType = MessageType,
                                                 MessageCriticalTime = envelope.CriticalTime.Value
                                             };
                            break;
                        case Domain.NotificationTypes.Email:
                            yield return new SendCriticalTimeEmailNotification();
                            break;
                    }
                }
            }
        }
    }
}
