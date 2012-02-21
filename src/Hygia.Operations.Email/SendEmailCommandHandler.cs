using System.Configuration;
using System.Net;
using System.Net.Mail;
using Hygia.Operations.Email.Commands;
using NServiceBus;

namespace Hygia.Operations.Email
{
    public class SendEmailCommandHandler : IHandleMessages<SendEmailCommand>
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

        public void Handle(SendEmailCommand message)
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

            smtpClient.Send(mailMessage);
        }
    }
}