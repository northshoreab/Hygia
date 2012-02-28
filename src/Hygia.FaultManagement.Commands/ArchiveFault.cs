namespace Hygia.FaultManagement.Commands
{
    using System;

    public class ArchiveFault
    {
        public Guid EnvironmentId { get; set; }
        public Guid MessageId { get; set; }
    }
}