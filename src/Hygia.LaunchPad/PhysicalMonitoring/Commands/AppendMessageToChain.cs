namespace Hygia.LaunchPad.PhysicalMonitoring.Commands
{
    using System;

    public class AppendMessageToChain
    {
        public Guid MessageId { get; set; }

        public Guid EnvelopeId { get; set; }

        public Guid PreviousEnvelopeId { get; set; }
    }
}