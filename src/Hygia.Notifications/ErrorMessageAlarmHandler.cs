using Hygia.Alarms.Events;
using Hygia.Notifications.Domain;
using NServiceBus;
using Raven.Client;

namespace Hygia.Notifications
{
    public class ErrorMessageAlarmHandler : IHandleMessages<ErrorMessageAlarm>
    {
        private readonly IDocumentSession _session;

        public ErrorMessageAlarmHandler(IDocumentSession session)
        {
            _session = session;
        }

        public void Handle(ErrorMessageAlarm message)
        {
            _session.Store(new ErrorMessageNotification(message.MessageTypeId, message.Exception, message.MessageBody));
            //TODO: send email!
        }
    }
}