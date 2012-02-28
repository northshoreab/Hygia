namespace Hygia.FaultManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Operations.Events;
    using NServiceBus;

    public class FaultEmailCommandHandler : IHandleMessages<EmailReceived>
    {

        private class AddressInfo
        {
            public string Area { get; set; }
            public Guid MessageId { get; set; }
            public Guid EnvironmentId { get; set; }
        }

        public IBus Bus { get; set; }
        public void Handle(EmailReceived emailReceived)
        {
            foreach (var addressInfo in GetAddressInfo(emailReceived))
            {
                if (addressInfo.Area.ToUpper() != "FAULTS")
                    break;

                if(emailReceived.Body.ToUpper().StartsWith(EmailLaunchPadCommandTypes.Delete))
                {
                    switch (command.Name.ToUpper())
                    {
                        case EmailCommandTypes.Retry:
                            //todo, this should probably be a command to our self to record the fact that we did a retry (and then another message to tell the launchpad to actually do it)
                            Bus.Send(new RetryFault
                                                        {
                                                            RetryType = command.Values.First(x => x.Key == "TYPE").Value,
                                                            MessageId = messageId
                                                        });
                            break;
                        case EmailCommandTypes.Archive:
                            Bus.Send(new ArchiveFault { MessageId = messageId });
                            break;
                    }
                }                
            }
        }

        private IEnumerable<AddressInfo> GetAddressInfo(EmailReceived emailReceived)
        {
            foreach (var toAddress in emailReceived.To)
            {
                string[] address = toAddress.Split('+');
                
                if (address.Count() < 2 || address[1].Length < 39)
                    break;

                string area = address[1].Substring(0, address[1].IndexOf('-') - 1);
                Guid messageId;
                Guid environmentId;

                Guid.TryParse(address[0], out environmentId);
                Guid.TryParse(address[0].Substring(area.Length + 1, 38), out messageId);

                yield return new AddressInfo
                                 {
                                     Area = area,
                                     EnvironmentId = environmentId,
                                     MessageId = messageId
                                 };
            }
        }

        private EmailCommand GetCommand(string emailBody)
        {
            EmailCommand emailCommand = null;
            if(emailBody.ToUpper().StartsWith(EmailLaunchPadCommandTypes.Delete))
            {
                emailCommand = new EmailCommand(EmailLaunchPadCommandTypes.Delete);
            }

            if(emailCommand == null)
                return null;

            int rowLength = emailBody.IndexOf('\n');

            if (rowLength > emailCommand.Name.Length + 1)
                emailCommand.Values = ParseCommandValues(emailBody.Substring(emailCommand.Name.Length + 1, rowLength));

            return emailCommand;
        }

        private IDictionary<string, string> ParseCommandValues(string valueString)
        {
            return valueString.Split(';').Select(x => x.Split('=')).ToDictionary(k => k[0], v => v[1]);
        }
    }
}