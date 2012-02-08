using System;
using System.Collections.Generic;
using Hygia.Backend.Notifications.Command;
using Hygia.Backend.Notifications.Domain;
using Hygia.PhysicalMonitoring.Domain;
using NServiceBus;

namespace Hygia.Backend.Notifications.Extension
{
    public static class NotificationConfigurationExtensions
    {
        public static IEnumerable<IMessage> GetNotificationMessagesFor(this CriticalTimeNotificationConfiguration notification, Envelope envelope)
        {
            if (envelope.CriticalTime > notification.AlertLevel)
            {
                foreach (var alertTypes in notification.NotificationTypes)
                {
                    switch (alertTypes)
                    {
                        case NotificationTypes.Rss:
                            yield return new RegisterCriticalTimeRssNotification
                                             {
                                                 AlertLevel = notification.AlertLevel,
                                                 MessageType = notification.MessageType,
                                                 MessageCriticalTime = envelope.CriticalTime.Value
                                             };
                            break;
                        case NotificationTypes.Email:
                            yield return new SendCriticalTimeEmailNotification();
                            break;
                    }
                }
            }
        }

        public static IEnumerable<IMessage> GetNotificationMessagesFor(this BusinessProcessCompletionTimeNotificationConfiguration notification, Envelope envelope)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<IMessage> GetNotificationMessagesFor(this NotificationConfiguration notification, Envelope envelope)
        {
            throw new NotImplementedException();            
        }

    }
}