namespace Hygia.LaunchPad.PhysicalMonitoring.Domain
{
    using System;
    using System.Collections.Generic;

    public class MessageChain
    {
        public string Id{ get; set; }

        public ICollection<Guid> StartedByMessages { get; set; }
    }
}