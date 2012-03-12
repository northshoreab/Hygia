namespace Hygia.FaultManagement.Events
{
    using System;

    public class RetryFailed
    {
        public Guid FaultId { get; set; }
    }
}