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

        private class FaultDetailsInMail
        {
            public IEnumerable<EmailCommand> Commands { get; set; }
            public IEnumerable<Guid> MessageIds { get; set; }
        }

        public IBus Bus { get; set; }
        public void Handle(EmailReceived emailReceived)
        {
            FaultDetailsInMail faultDetailsInMail = ParseEmail(emailReceived);

            foreach (var messageId in faultDetailsInMail.MessageIds)
            {
                foreach (var command in faultDetailsInMail.Commands)
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

        private FaultDetailsInMail ParseEmail(EmailReceived emailReceived)
        {
            return new FaultDetailsInMail
                       {
                           Commands = GetCommands(emailReceived.Body),
                           MessageIds = ParseToAddressToMessageId(emailReceived.To)
                       };
        }

        private IEnumerable<Guid> ParseToAddressToMessageId(IEnumerable<string> to)
        {
            foreach (var toAdress in to)
            {
                Guid messageId;
                Guid.TryParse(toAdress.Substring(0, toAdress.IndexOf('@') - 1), out messageId);
                if (messageId != default(Guid))
                    yield return messageId;
            }
        }

        private IEnumerable<EmailCommand> GetCommands(string stringContainingCommands)
        {
            while (stringContainingCommands.Contains("[["))
            {
                int start = stringContainingCommands.IndexOf("[[") + 2;
                int end = stringContainingCommands.IndexOf("]]");

                if (end <= start)
                    break;

                string[] commandAndValues = stringContainingCommands.Substring(start, end).Split(':');

                var mailCommand = new EmailCommand(commandAndValues[0].ToUpper());

                if (commandAndValues.Count() > 0)
                    mailCommand.Values = ParseCommandValues(commandAndValues[1]);

                yield return mailCommand;

                stringContainingCommands = stringContainingCommands.Substring(end, stringContainingCommands.Length - end);
            }
        }

        private IDictionary<string, string> ParseCommandValues(string valueString)
        {
            return valueString.Split(';').Select(x => x.Split('=')).ToDictionary(k => k[0], v => v[1]);
        }
    }
}