namespace Hygia.Operations.Events
{
    public class EmailReceived
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Service { get; set; }
        public string Parameters { get; set; }
    }
}