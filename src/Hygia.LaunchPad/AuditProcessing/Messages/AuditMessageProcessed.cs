namespace Hygia.LaunchPad.AuditProcessing.Messages
{
    using System;
    using System.Collections.Generic;

    public class AuditMessageProcessed
    {
        public string MessageId { get; set; }

        public Guid TennantId { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public Dictionary<string, string> AdditionalInformation { get; set; }

        public byte[] Body { get; set; }
    }
}