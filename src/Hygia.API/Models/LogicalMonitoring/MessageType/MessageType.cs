using System;

namespace Hygia.API.Models.LogicalMonitoring.MessageType
{
    public class MessageType
    {
        public Guid MessageTypeId { get; set; }
        public string Type { get; set; }
        public string Intent { get; set; }
        public string Versions { get; set; }
        public string ConsumedByACs { get; set; }
        public string ProducedByACs { get; set; }
        public string PreceedingMessageTypes { get; set; }
    }
}