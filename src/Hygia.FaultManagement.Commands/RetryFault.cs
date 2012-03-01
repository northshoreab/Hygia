namespace Hygia.FaultManagement.Commands
{
    using Operations.Communication;

    public class RetryFault : ILaunchPadCommand
    {
        public string FaultEnvelopeId { get; set; }
    }
}