namespace Hygia.Operations.AuditUploads.Messages
{
    using System;
    using System.Collections.Generic;

    public class ProcessAuditMessage
    {
        public string MessageId { get; set; }

        public Guid ApiKey { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public Dictionary<string, string> AdditionalInformation { get; set; }

        public byte[] Body { get; set; }
    }
}