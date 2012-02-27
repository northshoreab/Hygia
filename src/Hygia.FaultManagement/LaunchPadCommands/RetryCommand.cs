using System;

namespace Hygia.FaultManagement.LaunchPadCommands
{
    public class RetryCommand
    {
        public string RetryType { get; set; }
        public Guid MessageId { get; set; }
    }
}