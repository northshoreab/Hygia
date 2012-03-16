namespace Hygia.FaultManagement.Emails
{
    using System;
    using Hygia.FaultManagement.Commands;
    using Hygia.Operations.Events;
    using NServiceBus;

    public class FaultEmailReplyHandler : IHandleMessages<EmailReceived>
    {
        public IBus Bus { get; set; }
        public void Handle(EmailReceived emailReceived)
        {
            if (emailReceived.Service.ToUpper() != "FAULTS")
                return;

            var emailCommand = GetCommand(emailReceived.Body);

            var messageId = Guid.Parse(emailReceived.Parameters);

            switch (emailCommand.ToUpper())
            {
                case EmailCommandTypes.Retry:

                    Bus.Send(new IssueRetryForFault
                                 {
                                     FaultId = messageId,
                                     IssuedAt = emailReceived.TimeSent
                                 });
                    break;

                case EmailCommandTypes.Archive:
                    Bus.Send(new ArchiveFault { FaultId = messageId });
                    break;
                default:
                    //todo just add a comment to the fault
                    break;
            }

        }

        string GetCommand(string emailBody)
        {

            if (emailBody.ToUpper().StartsWith(EmailCommandTypes.Retry))
            {
                return EmailCommandTypes.Retry;
            }

            if (emailBody.ToUpper().StartsWith(EmailCommandTypes.Archive))
            {
                return EmailCommandTypes.Archive;
            }

            return "";
        }
    }
}