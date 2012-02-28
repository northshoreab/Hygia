using System;

namespace Hygia.FaultManagement.LaunchPadCommands
{
    using Operations.Communication;

    public class RetryFault : ILaunchPadCommand
    {
        public string RetryType { get; set; }
        public Guid MessageId { get; set; }
    }
}