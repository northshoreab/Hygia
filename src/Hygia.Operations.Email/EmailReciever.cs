using System.Configuration;
using System.Linq;
using System.Timers;
using Hygia.Operations.Events;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Unicast;

namespace Hygia.Operations.Email
{
    using System;
    using log4net;

    public class EmailReciever : INeedInitialization, IWantToRunWhenTheBusStarts
    {
        static Timer _timer;
        static int _checkInterval;
        static string _pop3Server;
        static int _pop3Port;
        static bool _pop3IsSsl;
        static string _pop3User;
        static string _pop3Password;
        public IBus Bus { get; set; }

        public void Init()
        {
            _pop3Server = ConfigurationManager.AppSettings["POP3Server"];
            _pop3Password = ConfigurationManager.AppSettings["POP3Password"];
            _pop3User = ConfigurationManager.AppSettings["POP3User"];
            int.TryParse(ConfigurationManager.AppSettings["CheckIntervalInSeconds"], out _checkInterval);
            int.TryParse(ConfigurationManager.AppSettings["POP3Port"], out _pop3Port);
            bool.TryParse(ConfigurationManager.AppSettings["Pop3IsSSL"], out _pop3IsSsl);
            _timer = new Timer(_checkInterval * 1000);
            _timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                using (var pop = new AE.Net.Mail.Pop3Client(_pop3Server, _pop3User, _pop3Password, _pop3Port, _pop3IsSsl))
                {
                    for (var i = pop.GetMessageCount() - 1; i >= 0; i--)
                    {
                        var msg = pop.GetMessage(i);
                        var to = msg.To.First().Address;

                        var environmentId = to.Split('+').FirstOrDefault();

                        Bus.Publish<EmailReceived>(email =>
                        {
                            if (!string.IsNullOrEmpty(environmentId))
                                email.SetHeader("EnvironmentId",environmentId);

                            email.To = msg.To.First().Address;
                            email.Body = msg.Body;
                            email.From = msg.From.Address;
                            email.Subject = msg.Subject;
                        });

                        pop.DeleteMessage(i);
                    }
                }            

            }
            catch (Exception ex)
            {
                logger.Error("There was a problem fetching emails from " + _pop3Server, ex);
            }
        }

        public void Run()
        {
            _timer.Start();
        }

        static ILog logger = LogManager.GetLogger("emails");
    }
}