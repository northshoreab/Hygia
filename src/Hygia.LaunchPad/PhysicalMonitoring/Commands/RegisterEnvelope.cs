namespace Hygia.LaunchPad.PhysicalMonitoring.Commands
{
    using System;
    using System.Collections.Generic;

    public class RegisterEnvelope
    {
        public Guid EnvelopeId { get; set; }

        public DateTime TimeSent { get; set; }

        public Guid CorrelatedEnvelopeId { get; set; }

        List<PhysicalMessage> messages;
        public List<PhysicalMessage> Messages
        {
            get
            {
                return messages ?? new List<PhysicalMessage>();
            }
            set { messages = value; }
        }
    }

    public class PhysicalMessage
    {
        public Guid MessageId { get; set; }
        public Guid MessageTypeId { get; set; }
    }
}