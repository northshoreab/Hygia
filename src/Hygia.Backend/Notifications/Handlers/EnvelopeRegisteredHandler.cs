using System.Collections.Generic;
using System.Linq;
using Hygia.Backend.Notifications.Domain;
using Hygia.Backend.Notifications.Index;
using Hygia.PhysicalMonitoring.Commands;
using NServiceBus;
using Raven.Client;

namespace Hygia.Backend.Notifications.Handlers
{
    public class EnvelopeRegisteredHandler : IHandleMessages<EnvelopeRegistered>
    {
        private readonly IBus _bus;
        public IDocumentSession Session { get; set; }

        public EnvelopeRegisteredHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(EnvelopeRegistered message)
        {
            var notificationConfigurations = Session.Query<NotificationConfiguration, All_NotificationConfigurations>().ToList();

            IEnumerable<IMessage> notifications = notificationConfigurations.SelectMany(x => x.EnvelopeNotifications(message.RegisteredEnvelope));

            _bus.Publish(notifications);
        }
    }
}
