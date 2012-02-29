using System.Configuration;
using System.Net;
using System.Net.Mail;
using Hygia.Operations.Email.Commands;
using NServiceBus;

namespace Hygia.Operations.Email
{
    public class SendEmailCommandHandler : IHandleMessages<SendEmailRequest>
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;
        private readonly string _defaultFromEmail;

        public SendEmailCommandHandler()
        {
            _smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            _smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
            _smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
            _defaultFromEmail = ConfigurationManager.AppSettings["DefaultFromEmail"];
            int.TryParse(ConfigurationManager.AppSettings["SmtpPort"], out _smtpPort);
        }


        public void Handle(SendEmailRequest message)
        {
            string from = string.IsNullOrEmpty(message.From) ? _defaultFromEmail : message.From;

            var mailMessage = new MailMessage(from, message.To)
                                  {
                                      Subject = message.Subject,
                                      Body = message.Body
                                  };

            var smtpClient = new SmtpClient(_smtpServer, _smtpPort)
                                 {
                                     Credentials = new NetworkCredential(_smtpUser, _smtpPassword),
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
                from = _defaultFromEmail;
            else
                from += "@watchr.se";


            return from;
        }
    }
}