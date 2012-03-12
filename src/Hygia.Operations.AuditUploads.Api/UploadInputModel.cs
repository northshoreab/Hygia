namespace Hygia.Operations.AuditUploads.Api
{
    using System;
    using System.Collections.Generic;

    public class UploadInputModel
    {
        public string MessageId { get; set; }

        public Guid ApiKey { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public Dictionary<string, string> AdditionalInformation { get; set; }

        public byte[] Body { get; set; }
    }
}