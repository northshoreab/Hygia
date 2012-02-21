using Hygia.Alarms.Events;
using Hygia.Notifications.Domain;
using Hygia.Operations.Email.Commands;
using NServiceBus;
using Raven.Client;
using Raven.Client.Linq;

namespace Hygia.Notifications
{
    public class ErrorMessageAlarmHandler : IHandleMessages<ErrorMessageAlarm>
    {
        private readonly IDocumentSession _session;
        private readonly IBus _bus;

        public ErrorMessageAlarmHandler(IDocumentSession session, IBus bus)
        {
            _session = session;
            _bus = bus;
        }

        public void Handle(ErrorMessageAlarm message)
        {
            foreach (var errorNotificationSetting in _session.Query<ErrorNotificationSetting>().Where(x => x.AllMessages || x.MessageTypeId == message.MessageTypeId))
            {
                var notification = new ErrorMessageNotification(message.MessageTypeId, message.Exception,
                                                                message.MessageBody);

                switch (errorNotificationSetting.NotificationType)
                {
                    case NotificationTypes.Email:
                        _bus.Publish<SendEmailCommand>(e =>
                                                           {
                                                               e.Body = notification.Description;
                                                               e.Subject = notification.Title;
                                                               e.To = errorNotificationSetting.EmailAdress;
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