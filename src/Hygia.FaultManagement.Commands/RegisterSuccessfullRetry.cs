namespace Hygia.FaultManagement.Commands
{
    using System;

    public class RegisterSuccessfullRetry
    {
        public Guid FaultId { get; set; }

        public DateTime TimeOfRetry { get; set; }
    }
}