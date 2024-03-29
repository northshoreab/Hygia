using System;
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

    public class FaultNotificationHandler : IHandleMessages<FaultRegistered>,
                                            IHandleMessages<RetryFailed>
    {
        public IDocumentSession Session { get; set; }

        public IBus Bus { get; set; }

        public IInvokeProviders Providers { get; set; }

        public void Handle(RetryFailed message)
        {
            SendEmailToAllNotificationSettings(message.FaultId, message.MessageTypes);
        }

        public void Handle(FaultRegistered message)
        {
            SendEmailToAllNotificationSettings(message.FaultId, message.MessageTypes);
        }

        private void SendEmailToAllNotificationSettings(Guid faultId, IList<Guid> messageTypes)
        {
            var faultInformation = Providers.Invoke<IProvideFaultInformation>(new
                                                                                  {
                                                                                      FaultEnvelopeId = faultId,
                                                                                      MessageTypes = messageTypes
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
                            e.Parameters = faultId.ToString();
                            e.DisplayName = "WatchR - Faults";
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
            string messageType = faultInformation.MessageTypeName;
            
            if(string.IsNullOrEmpty(messageType))
                messageType = faultInformation.Headers.ContainsKey("NServiceBus.EnclosedMessageTypes") ? 
                    faultInformation.Headers["NServiceBus.EnclosedMessageTypes"] :
                    "Unknown";

            messageType = messageType.Split(',')[0].Split('.').LastOrDefault();
            
            var reason = "NewFault"; //todo -  get retries and set this to RetryFailed if Retries>0
            
            //todo: Use templating engine
            return string.Format("[Production|{1}] Message of type {0} has failed", messageType,reason);
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