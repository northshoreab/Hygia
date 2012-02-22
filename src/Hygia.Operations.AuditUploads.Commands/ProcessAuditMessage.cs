using System.Collections.Generic;

namespace Hygia.Operations.AuditUploads.Commands
{
    public class ProcessAuditMessage
    {
        public string MessageId { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public Dictionary<string, string> AdditionalInformation { get; set; }

        public byte[] Body { get; set; }
    }
}