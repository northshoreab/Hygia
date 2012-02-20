using System;

namespace Hygia.PhysicalMonitoring.Domain
{
    public class MessgeTypeInstance
    {
        public Guid MessageId { get; set; }
        public Guid MessageTypeId { get; set; }

        public DateTime ProcessingEnded { get; set; }         
    }
}