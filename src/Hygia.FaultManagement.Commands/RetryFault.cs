using System.Collections.Generic;

namespace Hygia.FaultManagement.Commands
{
    using System;
    using Operations.Communication;

    public class RetryFault : ILaunchPadCommand
    {
        public Dictionary<string, string> Parameters { get; set; }
        public Guid MessageId { get; set; }
    }
}