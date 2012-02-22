using Hygia.Notifications.Domain;
using Hygia.Operations.Email.Commands;
using NServiceBus;
using Raven.Client;

namespace Hygia.Notifications
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Linq;
    using FaultManagement.Events;
    using Provide;

    public class FaultAlarmHandler : IHandleMessages<FaultRegistered>
    {
        public IDocumentSession Session { get; set; }

        public IBus Bus { get; set; }

        IEnumerable<IProvideFaultInformation> faultInformationProviders;

        public FaultAlarmHandler(IEnumerable<IProvideFaultInformation> faultInformationProviders)
        {
            this.faultInformationProviders = faultInformationProviders;
        }

        public void Handle(FaultRegistered message)
        {
            var faultNotificationSetting = new FaultNotificationSetting
                                               {
                                                   NotificationType = "Email",
                                                   EmailAdress = "andreasohlund2@gmail.com"
                                               };
            //todo - Danne fix this and add a json doc to the repos that we can copy paste into raven
            //foreach (var faultNotificationSetting in Session.Query<FaultNotificationSetting>().Where(x => x.AllMessages))
            //{
            dynamic faultInformation = new ExpandoObject();

            foreach (var faultInformationProvider in faultInformationProviders)
            {
                faultInformation = DynamicHelpers.Combine(faultInformation, faultInformationProvider.ProvideFor(message.EnvelopeId,message.MessageTypes));

            }

            switch (faultNotificationSetting.NotificationType)
            {
                case NotificationTypes.Email:
                    Bus.Send<SendEmailRequest>(e =>
                                                       {
                                                           e.Body = FormatBody(faultInformation);
                                                           e.Subject = FormatSubject(faultInformation);
                                                           e.To = faultNotificationSetting.EmailAdress;
                                                       });
                    break;
                case NotificationTypes.RSS:
                    Session.Store(new FaultNotification(faultNotificationSetting.MessageTypeId, faultInformation.ExceptionReason,
                                                            faultInformation.TimeOfFailure));
                    break;
            }
            // }
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