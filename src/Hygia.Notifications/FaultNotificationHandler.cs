using System.IO;
using System.Reflection;
using Hygia.Notifications.Domain;
using Hygia.Operations.Email.Commands;
using NServiceBus;
using Raven.Client;
using RazorEngine;

namespace Hygia.Notifications
{
    using System.Collections.Generic;
    using System.Linq;
    using FaultManagement.Events;
    using Provide;

    public class FaultNotificationHandler : IHandleMessages<FaultRegistered>
    {
        public IDocumentSession Session { get; set; }

        public IBus Bus { get; set; }

        public IInvokeProviders Providers { get; set; }


        public void Handle(FaultRegistered message)
        {
            var faultInformation = Providers.Invoke<IProvideFaultInformation>( new
                            {
                                FaultEnvelopeId = message.FaultId,
                                message.MessageTypes
                            });

            foreach (var faultNotificationSetting in Session.Query<FaultNotificationSetting>().Where(x => x.AllMessages))
            {
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
                                                           e.Service = "faults";
                                                           e.Parameters = message.FaultId.ToString();
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
            var messageType = faultInformation.MessageTypeName ?? faultInformation.Headers["NServiceBus.EnclosedMessageTypes"];

            //todo: Use templating engine
            return string.Format("[WatchR|NewFault] Message of type {0} has failed", messageType);
        }

        string FormatBody(dynamic faultInformation)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var stream = assembly.GetManifestResourceStream("Hygia.Notifications.EmailTemplate.FaultEmail.htm");

            if (stream == null)
                return ((IDictionary<string, object>)faultInformation).Aggregate("", (current, pair) => current + (pair.Key + " - " + pair.Value));

            var textStreamReader = new StreamReader(stream);

            return Razor.Parse(textStreamReader.ReadToEnd(), faultInformation);
        }
    }
}