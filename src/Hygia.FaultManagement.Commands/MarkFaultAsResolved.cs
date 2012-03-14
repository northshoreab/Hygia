namespace Hygia.FaultManagement.Commands
{
    using System;

    public class MarkFaultAsResolved 
    {
        public Guid FaultId { get; set; }
        public DateTime ResolvedAt{ get; set; }
    }
}