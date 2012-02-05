namespace Hygia.PhysicalMonitoring.Commands
{
    using System;

    public class StartMessageChain
    {
        public Guid MessageId { get; set; }

        public Guid EnvelopeId { get; set; }

        public Guid MessageChainId { get; set; }
    }
}