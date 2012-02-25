using Hygia.Notifications.Domain;
using Hygia.Operations.Email.Commands;
using NServiceBus;
using Raven.Client;

namespace Hygia.Notifications
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using FaultManagement.Events;
    using Provide;

    public class FaultAlarmHandler : IHandleMessages<FaultRegistered>
    {
        public IDocumentSession Session { get; set; }

        public IBus Bus { get; set; }

        readonly IEnumerable<IProvideFaultInformation> _faultInformationProviders;

        public FaultAlarmHandler(IEnumerable<IProvideFaultInformation> faultInformationProviders)
        {
            _faultInformationProviders = faultInformationProviders;
        }

        public void Handle(FaultRegistered message)
        {
            foreach (var faultNotificationSetting in Session.Query<FaultNotificationSetting>().Where(x => x.AllMessages))
            {
                dynamic faultInformation = new ExpandoObject();

                foreach (var faultInformationProvider in _faultInformationProviders)
                {
                    faultInformation = DynamicHelpers.Combine(faultInformation, faultInformationProvider.ProvideFor(message.EnvelopeId, message.MessageTypes));
                }

                string body = FormatBody(faultInformation);
                string subject = FormatSubject(faultInformation);

                switch (faultNotificationSetting.NotificationType)
                {
                    case NotificationTypes.Email:
                        Bus.Send<SendEmailRequest>(e =>
                        {
                            e.Body = body;
                            e.Subject = subject;
                            e.To = faultNotificationSetting.EmailAdress;
                        });
                        break;
                    case NotificationTypes.RSS:
                        Session.Store(new FaultNotification(subject, body));
                        break;
                }
            }
        }

        string FormatSubject(dynamic faultInformation)
        {
            //todo: Use templating engine
            return string.Format("Message of type {0}", faultInformation.MessageTypeName);
        }

        string FormatBody(dynamic faultInformation)
        {
            //todo: Use templating engine
            return ((IDictionary<string, object>)faultInformation).Aggregate("", (current, pair) => current + (pair.Key + " - " + pair.Value));
        }
    }
}