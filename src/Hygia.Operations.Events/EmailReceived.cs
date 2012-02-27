using System.Collections.Generic;

namespace Hygia.Operations.Events
{
    public class EmailReceived
    {
        public IEnumerable<string> To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}