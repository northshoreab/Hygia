namespace Hygia.Operations.Email.Commands
{
    public class SendEmailCommand
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
