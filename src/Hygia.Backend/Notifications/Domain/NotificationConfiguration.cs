using System.Collections.Generic;
using Hygia.PhysicalMonitoring.Domain;
using NServiceBus;

namespace Hygia.Backend.Notifications.Domain
{
    public abstract class NotificationConfiguration
    {
        public abstract IEnumerable<IMessage> EnvelopeNotifications(Envelope envelope);
    }
}