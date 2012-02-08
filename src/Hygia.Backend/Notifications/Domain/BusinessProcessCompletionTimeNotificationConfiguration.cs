using System;
using System.Collections.Generic;
using Hygia.PhysicalMonitoring.Domain;
using NServiceBus;
using NServiceBus.Unicast.Subscriptions;

namespace Hygia.Backend.Notifications.Domain
{
    public class BusinessProcessCompletionTimeNotificationConfiguration : NotificationConfiguration
    {
        public string Id { get; set; }
        public MessageType StartMessageType { get; set; }
        public MessageType EndMessageType { get; set; }
        public TimeSpan AlertLevel { get; set; }

        public override IEnumerable<IMessage> EnvelopeNotifications(Envelope envelope)
        {
            throw new NotImplementedException();
        }
    }
}