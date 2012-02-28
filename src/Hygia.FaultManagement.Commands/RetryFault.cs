using System.Collections.Generic;

namespace Hygia.FaultManagement.Commands
{
    using System;
    using Operations.Communication;

    public class RetryFault : ILaunchPadCommand
    {
        public IDictionary<string, string> Parameters { get; set; }
        public Guid MessageId { get; set; }
        public Guid EnvironmentId { get; set; }
    }
}