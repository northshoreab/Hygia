namespace Hygia.FaultManagement.Commands
{
    using System;
    using Operations.Communication;

    public class RetryFault : ILaunchPadCommand
    {
        public Guid FaultId { get; set; }
        public string FaultEnvelopeId { get; set; }
    }
}