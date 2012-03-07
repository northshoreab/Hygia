using System.Configuration;
using System.Net;
using System.Net.Mail;
using Hygia.Operations.Email.Commands;
using NServiceBus;

namespace Hygia.Operations.Email
{
    public class SendEmailCommandHandler : IHandleMessages<SendEmailRequest>
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUser;
        private readonly string smtpPassword;
        private readonly string defaultFromEmail;

        public SendEmailCommandHandler()
        {
            smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
            smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
            defaultFromEmail = ConfigurationManager.AppSettings["DefaultFromEmail"];
            int.TryParse(ConfigurationManager.AppSettings["SmtpPort"], out smtpPort);
        }

        public void Handle(SendEmailRequest message)
        {
            string from = string.IsNullOrEmpty(message.From) ? defaultFromEmail : message.From;

            var mailMessage = new MailMessage(from, message.To)
                                  {
                                      Subject = message.Subject,
                                      Body = message.Body,
                                      IsBodyHtml = true
                                  };

            var smtpClient = new SmtpClient(smtpServer, smtpPort)
                                 {
                                     Credentials = new NetworkCredential(smtpUser, smtpPassword),
                                     DeliveryMethod = SmtpDeliveryMethod.Network,
                                 };
            mailMessage.ReplyToList.Add(GenerateFromAddress(message));

            smtpClient.Send(mailMessage);
        }

        string GenerateFromAddress(SendEmailRequest message)
        {
            var from = "";
            var environmentId = message.GetHeader("EnvironmentId");

            if (!string.IsNullOrEmpty(environmentId))
                from += environmentId;

            if (!string.IsNullOrEmpty(message.Service))
                from += "+"+message.Service;
            
            if (!string.IsNullOrEmpty(message.Parameters))
                from += "+" + message.Parameters;

            if (string.IsNullOrEmpty(from))
                from = defaultFromEmail;
            else
                from += "@watchr.se";

            return from;
        }
    }
}