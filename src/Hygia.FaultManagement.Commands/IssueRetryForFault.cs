namespace Hygia.FaultManagement.Commands
{
    using System;

    public class IssueRetryForFault
    {
        public Guid FaultId { get; set; }

        public DateTime IssuedAt { get; set; }
    }
}