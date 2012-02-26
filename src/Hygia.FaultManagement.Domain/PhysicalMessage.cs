namespace Hygia.FaultManagement.Domain
{
    using System;

    public class PhysicalMessage
    {
        public Guid MessageId { get; set; }
        public Guid MessageTypeId { get; set; }
    }
}