namespace Hygia.LaunchPad.LogicalMonitoring.Commands
{
    using System;
    using System.Collections.Generic;

    public class RegisterMessageConsumers
    {
        public Guid MessageTypeId { get; set; }

        public List<Guid> ConsumedBy { get; set; }

        public override string ToString()
        {
            return string.Format("Message {0} was consumed by AC's {1}", MessageTypeId, string.Join("|", ConsumedBy));
        }
    }
}