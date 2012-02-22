namespace Hygia.LogicalMonitoring.Handlers
{
    using System;
    using System.Collections.Generic;
    using Inspectors;

    public class MessageType
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public MessageIntent Intent { get; set; }

        public ICollection<string> Versions { get; set; }

        public ICollection<Guid> ConsumedByACs { get; set; }

        public ICollection<Guid> ProducedByACs { get; set; }

        public ICollection<Guid> PreceedingMessageTypes { get; set; }
    }
}