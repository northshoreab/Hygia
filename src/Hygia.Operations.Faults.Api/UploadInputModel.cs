namespace Hygia.Operations.Faults.Api
{
    using System;
    using System.Collections.Generic;

    public class UploadInputModel
    {
        public string FaultEnvelopeId { get; set; }

        public Guid ApiKey { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string Body { get; set; }
    }
}