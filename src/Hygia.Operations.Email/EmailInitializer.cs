namespace Hygia.Operations.Email
{
    using System.Configuration;
    using AE.Net.Mail;
    using NServiceBus;
    using NServiceBus.Config;

    public class EmailInitializer:INeedInitialization
    {
        public void Init()
        {
            var _pop3Server = ConfigurationManager.AppSettings["POP3Server"];
            var _pop3Password = ConfigurationManager.AppSettings["POP3Password"];
            var _pop3User = ConfigurationManager.AppSettings["POP3User"];
            int _pop3Port;
            bool _pop3IsSsl;

            int _checkInterval; 

            int.TryParse(ConfigurationManager.AppSettings["POP3Port"], out _pop3Port);
            bool.TryParse(ConfigurationManager.AppSettings["Pop3IsSSL"], out _pop3IsSsl);
            int.TryParse(ConfigurationManager.AppSettings["CheckIntervalInSeconds"], out _checkInterval);

            Configure.Instance.Configurer.ConfigureComponent<EmailReciever>(DependencyLifecycle.SingleInstance)
                .ConfigureProperty(p => p.CheckInterval, _checkInterval);

            Configure.Instance.Configurer.RegisterSingleton<IMailClient>(new Pop3Client(_pop3Server, _pop3User,
                                                                                        _pop3Password, _pop3Port,
                                                                                        _pop3IsSsl));
        }

        
    }
}