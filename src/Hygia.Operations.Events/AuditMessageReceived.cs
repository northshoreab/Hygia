namespace Hygia.Operations.Events
{
    using System;
    using System.Collections.Generic;

    public class AuditMessageReceived
    {
        public Guid MessageId { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public Dictionary<string, string> AdditionalInformation { get; set; }

        public byte[] Body { get; set; }


    }
}