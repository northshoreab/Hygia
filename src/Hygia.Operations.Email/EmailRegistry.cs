namespace Hygia.Operations.Email
{
    using System.Configuration;
    using AE.Net.Mail;
    using StructureMap.Configuration.DSL;

    public class EmailRegistry:Registry
    {
        public EmailRegistry()
        {
            var _pop3Server = ConfigurationManager.AppSettings["POP3Server"];
            var _pop3Password = ConfigurationManager.AppSettings["POP3Password"];
            var _pop3User = ConfigurationManager.AppSettings["POP3User"];
            int _pop3Port;
            bool _pop3IsSsl;


            int.TryParse(ConfigurationManager.AppSettings["POP3Port"], out _pop3Port);
            bool.TryParse(ConfigurationManager.AppSettings["Pop3IsSSL"], out _pop3IsSsl);

            For<IMailClient>()
                .Use(ctx => new Pop3Client(_pop3Server, _pop3User, _pop3Password, _pop3Port, _pop3IsSsl));

            Profile("Integration", c => c.For<IMailClient>()
                                            .Use<FakePop3Client>()
                );
        }
    }

    public class FakePop3Client : IMailClient
    {
        public void Dispose()
        {
            
        }

        public int GetMessageCount()
        {
            return 0;
        }

        public MailMessage GetMessage(int index, bool headersonly = false)
        {
            throw new System.NotImplementedException();
        }

        public MailMessage GetMessage(string uid, bool headersonly = false)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteMessage(string uid)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteMessage(MailMessage msg)
        {
            throw new System.NotImplementedException();
        }
    }
}