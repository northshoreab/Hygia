using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
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

    public static class XmlDocumentExtensions
    {
        public static string ToIndentedString(this XmlDocument doc)
        {
            var stringWriter = new StringWriter(new StringBuilder());
            var xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
            doc.Save(xmlTextWriter);
            return stringWriter.ToString();
        }
    }

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
            string messageType = faultInformation.MessageTypeName ?? faultInformation.Headers["NServiceBus.EnclosedMessageTypes"];

            messageType = messageType.Split(',')[0].Split('.').LastOrDefault();

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

            string prettyPrintedBody = faultInformation.Body;

            try
            {
                var bodyXmlDoc = new XmlDocument{ XmlResolver =  null};
                bodyXmlDoc.LoadXml(prettyPrintedBody);
                prettyPrintedBody = bodyXmlDoc.ToIndentedString();
                prettyPrintedBody = HttpUtility.HtmlEncode(prettyPrintedBody);
                prettyPrintedBody = prettyPrintedBody.Replace(" ", "&nbsp;").Replace("\r\n", "<br />");
            }
            catch { }

            string body = Razor.Parse(textStreamReader.ReadToEnd(), new { FaultInfo = faultInformation, PrettyPrintedBody = prettyPrintedBody });

            return body.Replace("[[BODY]]", prettyPrintedBody);
        }
    }
}