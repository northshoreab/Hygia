namespace Hygia.Operations.Events
{
    using System.Collections.Generic;

    public class FaultMessageReceived
    {
        public string FaultMessageId { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string Body { get; set; }
    }
}