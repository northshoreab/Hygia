namespace Hygia.FaultManagement.Commands
{
    using System;
    using Operations.Communication;

    public class RetryFault : ILaunchPadCommand
    {
        public string RetryType { get; set; }
        public Guid MessageId { get; set; }
    }
}