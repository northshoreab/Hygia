namespace Hygia.FaultManagement
{
    using System;
    using Commands;
    using Operations.Events;
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
                                     MessageId = messageId
                                 });
                    break;

                case EmailCommandTypes.Archive:
                    Bus.Send(new ArchiveFault { MessageId = messageId });
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