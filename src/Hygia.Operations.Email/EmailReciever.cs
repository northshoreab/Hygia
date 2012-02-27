using System.Configuration;
using System.Linq;
using System.Timers;
using Hygia.Operations.Events;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Unicast;

namespace Hygia.Operations.Email
{
    public class EmailReciever : INeedInitialization, IWantToRunWhenTheBusStarts
    {
        private readonly IBus _bus;
        private Timer _timer;
        private int _checkInterval;
        private string _pop3Server;
        private int _pop3Port;
        private bool _pop3IsSsl;
        private string _pop3User;
        private string _pop3Password;

        public EmailReciever(IBus bus)
        {
            _bus = bus;
        }

        public void Init()
        {
            _pop3Server = ConfigurationManager.AppSettings["POP3Server"];
            _pop3Password = ConfigurationManager.AppSettings["POP3Password"];
            _pop3User = ConfigurationManager.AppSettings["POP3User"];
            int.TryParse(ConfigurationManager.AppSettings["CheckIntervalInSeconds"], out _checkInterval);
            int.TryParse(ConfigurationManager.AppSettings["POP3Port"], out _pop3Port);
            bool.TryParse(ConfigurationManager.AppSettings["Pop3IsSSL"], out _pop3IsSsl);
            _timer = new Timer(_checkInterval);
            _timer.Elapsed += TimerElapsed;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            using (var pop = new AE.Net.Mail.Pop3Client(_pop3Server, _pop3User, _pop3Password, _pop3Port, _pop3IsSsl))
            {
                for (var i = pop.GetMessageCount() - 1; i >= 0; i--)
                {
                    var msg = pop.GetMessage(i);

                    _bus.Publish<EmailReceived>(email =>
                                                    {
                                                        email.To = msg.To.Select(x => x.Address);
                                                        email.Body = msg.Body;
                                                        email.From = msg.From.Address;
                                                        email.Subject = msg.Subject;
                                                    });

                    pop.DeleteMessage(i);
                }
            }            
        }

        public void Run()
        {
            _timer.Start();
        }
    }
}