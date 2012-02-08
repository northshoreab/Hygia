using System;
using NServiceBus;
using NServiceBus.Unicast.Subscriptions;

namespace Hygia.Backend.Notifications.Command
{
    public class RegisterCriticalTimeRssNotification : IMessage
    {
        public MessageType MessageType { get; set; }
        public TimeSpan AlertLevel { get; set; }
        public TimeSpan MessageCriticalTime { get; set; }
    }
}
