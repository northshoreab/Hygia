using System.Linq;
using Hygia.Alarms.Events;
using Hygia.Notifications.Domain;
using Hygia.Operations.Email.Commands;
using NServiceBus;
using Raven.Client;
using Raven.Client.Linq;

namespace Hygia.Notifications
{
    public class FaultAlarmHandler : IHandleMessages<FaultAlarm>
    {
        private readonly IDocumentSession _session;
        private readonly IBus _bus;

        public FaultAlarmHandler(IDocumentSession session, IBus bus)
        {
            _session = session;
            _bus = bus;
        }

        public void Handle(FaultAlarm message)
        {
            foreach (var faultNotificationSetting in _session.Query<FaultNotificationSetting>().Where(x => x.AllMessages || message.MessageTypeId.Contains(x.MessageTypeId)))
            {
                var notification = new FaultNotification(faultNotificationSetting.MessageTypeId, message.ExceptionReason,
                                                                message.TimeOfFailure);

                switch (faultNotificationSetting.NotificationType)
                {
                    case NotificationTypes.Email:
                        _bus.Publish<SendEmailCommand>(e =>
                                                           {
                                                               e.Body = notification.Description;
                                                               e.Subject = notification.Title;
                                                               e.To = faultNotificationSetting.EmailAdress;
                                                           });
                        break;
                    case NotificationTypes.RSS:
                        _session.Store(notification);
                        break;
                }
            }
        }
    }
}