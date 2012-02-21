namespace Hygia.Operations.Faults.Commands
{
    using System.Collections.Generic;

    public class ProcessFaultMessage
    {
        public string FaultEnvelopeId { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string Body { get; set; }
    }
}