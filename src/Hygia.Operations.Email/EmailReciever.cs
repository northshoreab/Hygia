using System.Linq;
using Hygia.Operations.Events;
using NServiceBus;
using NServiceBus.Unicast;

namespace Hygia.Operations.Email
{
    using System;
    using System.Configuration;
    using System.Threading;
    using AE.Net.Mail;
    using log4net;

    public class EmailReciever : IWantToRunWhenTheBusStarts
    {
        static Timer timer;

        int checkInterval;

        public IBus Bus { get; set; }

        readonly IMailClient emailClient;

        private void TimerElapsed(object sender)
        {
            try
            {
                for (var i = emailClient.GetMessageCount() - 1; i >= 0; i--)
                {
                    var msg = emailClient.GetMessage(i);
                    var to = msg.To.First().Address;

                    var tokens = to.Split('+');

                    var environmentId = tokens.FirstOrDefault();
                    Guid temp;

                    if (!Guid.TryParse(environmentId, out temp))
                        environmentId = null;

                    var service = string.Empty;

                    if (tokens.Length > 1)
                        service = tokens[1];

                    var parameters = string.Empty;
                    if (tokens.Length > 2)
                        parameters = tokens[2].Split('@').First();

                    Bus.Publish<EmailReceived>(email =>
                                                   {

                                                       if (!string.IsNullOrEmpty(environmentId))
                                                           email.SetHeader("EnvironmentId", environmentId);

                                                       email.To = to;
                                                       email.Body = msg.Body;
                                                       email.From = msg.From.Address;
                                                       email.Subject = msg.Subject;
                                                       email.Service = service;
                                                       email.Parameters = parameters;
                                                       email.TimeSent = msg.Date;

                                                   });

                    emailClient.DeleteMessage(msg);
                }
            }
            catch (Exception ex)
            {
                logger.Error("There was a problem fetching emails", ex);
            }
            finally
            {
                timer.Change(checkInterval * 1000, int.MaxValue);
            }
        }

        public void Run()
        {
            if (!int.TryParse(ConfigurationManager.AppSettings["CheckIntervalInSeconds"], out checkInterval))
                checkInterval = 60;
            timer = new Timer(TimerElapsed, null, 0, int.MaxValue);
        }

        static ILog logger = LogManager.GetLogger("emails");

        public EmailReciever(IMailClient emailClient)
        {
            this.emailClient = emailClient;
        }
    }
}