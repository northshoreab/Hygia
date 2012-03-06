namespace Hygia.Operations.Events
{
    using System.Collections.Generic;

    public class AuditMessageReceived
    {
        public string MessageId { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public Dictionary<string, string> AdditionalInformation { get; set; }

        public byte[] Body { get; set; }


    }
}