using System;
using System.Collections.Generic;

namespace Hygia.API.Models.Operations.Uploads.AuditMessages
{
    public class AuditMessage
    {
        public string MessageId { get; set; }

        public Guid ApiKey { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public Dictionary<string, string> AdditionalInformation { get; set; }

        public byte[] Body { get; set; }
    }
}