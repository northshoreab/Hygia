namespace Hygia.FaultManagement.Api
{
    using System;

    public class FaultRetriedInputModel
    {
        public Guid FaultId { get; set; }
        public DateTime TimeOfRetry { get; set; }
    }
}