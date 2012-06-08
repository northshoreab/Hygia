using System;
using System.Collections.Generic;

namespace Hygia.API.Models.Operations.Uploads.FaultMessages
{
    public class FaultMessage
    {
        public string FaultEnvelopeId { get; set; }

        public Guid ApiKey { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string Body { get; set; }
    }
}