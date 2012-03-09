using System.Linq;
using Hygia.Notifications.Domain;
using Hygia.ServiceLevelAgreements.Events;
using NServiceBus;
using Raven.Client;

namespace Hygia.Notifications
{
    using System.Collections.Generic;
    using Operations.Email.Commands;
    using Provide;
    using Hygia;

    public class CriticalTimeSLAViolatedNotificationHandler : IHandleMessages<CriticalTimeSLAForMessageTypeViolated>
    {
        public IDocumentSession Session { get; set; }
        
        public IBus Bus { get; set; }

        public IInvokeProviders Providers { get; set; }
        
        public void Handle(CriticalTimeSLAForMessageTypeViolated message)
        {
            var information = Providers.Invoke<IProvideMessageTypeInformation>(new { message.MessageTypeId });

            foreach (var slaNotificationSetting in Session.Query<SLANotificationSetting>())
            {
                string body = FormatBody(information);
                string subject = FormatSubject(information);

                switch (slaNotificationSetting.NotificationType)
                {
                    case NotificationTypes.Email:
                        Bus.Send<SendEmailRequest>(e =>
                        {
                            e.Body = body;
                            e.Subject = subject;
                            e.To = slaNotificationSetting.EmailAdress;
                            e.Service = "sla";
                            e.Parameters = "";
                        });
                        break;
                    case NotificationTypes.RSS:
                        //Session.Store(new FaultNotification(subject, body));
                        break;
                }
            }
        }


        string FormatSubject(dynamic information)
        {
            var messageType = information.MessageTypeName;

            //todo: Use templating engine
            return string.Format("[WatchR|SLA Breach] Critical time for message {0} has gone past the allowed limit", messageType);
        }

        string FormatBody(dynamic information)
        {
            //todo: Use templating engine
            return ((IDictionary<string, object>)information).Aggregate("", (current, pair) => current + (pair.Key + " - " + pair.Value));
        }
    }
}