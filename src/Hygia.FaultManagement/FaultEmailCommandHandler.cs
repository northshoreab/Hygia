namespace Hygia.FaultManagement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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
            var emailCommand = GetCommand(emailReceived.Body);

            if (emailCommand == null)
                return;

            var addressInfo = GetAddressInfo(emailReceived);

            if (addressInfo.Area.ToUpper() != "FAULTS")
                return;

            if (emailCommand.Name.ToUpper() == EmailCommandTypes.Retry)
            {
                Bus.Send(new RetryFault
                             {
                                 Parameters = emailCommand.Values,
                                 MessageId = addressInfo.MessageId
                             });
            }
            else if (emailCommand.Name.ToUpper() == EmailCommandTypes.Archive)
            {
                Bus.Send(new ArchiveFault { MessageId = addressInfo.MessageId, EnvironmentId = addressInfo.EnvironmentId });
            }

        }

        private AddressInfo GetAddressInfo(EmailReceived emailReceived)
        {
            string[] address = emailReceived.To.Split('+');

            if (address.Count() < 2 || address[1].Length < 39)
                return null;

            string area = address[1].Split('-').First();
            Guid messageId;
            Guid environmentId;

            Guid.TryParse(address[0], out environmentId);
            Guid.TryParse(address[1].Split('-').Last(), out messageId);

            return new AddressInfo
                             {
                                 Area = area,
                                 EnvironmentId = environmentId,
                                 MessageId = messageId
                             };
        }

        private EmailCommand GetCommand(string emailBody)
        {
            EmailCommand emailCommand = null;

            if (emailBody.ToUpper().StartsWith(EmailCommandTypes.Retry))
            {
                emailCommand = new EmailCommand(EmailCommandTypes.Retry);
            }
            else if (emailBody.ToUpper().StartsWith(EmailCommandTypes.Archive))
            {
                emailCommand = new EmailCommand(EmailCommandTypes.Archive);
            }

            if (emailCommand == null)
                return null;

            int rowLength = emailBody.IndexOf('\n');

            if (rowLength > emailCommand.Name.Length + 1)
                emailCommand.Values = ParseCommandValues(emailBody.Substring(emailCommand.Name.Length + 1, rowLength));

            return emailCommand;
        }

        private Dictionary<string, string> ParseCommandValues(string valueString)
        {
            return valueString.Split(';').Select(x => x.Split('=')).ToDictionary(k => k[0], v => v[1]);
        }
    }
}