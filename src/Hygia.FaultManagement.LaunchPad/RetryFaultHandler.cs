namespace Hygia.FaultManagement.LaunchPad
{
    using Commands;
    using NServiceBus;

    public class RetryFaultHandler : IHandleMessages<RetryFault>
    {
        public ErrorManager ErrorManager { get; set; }
       
        public void Handle(RetryFault message)
        {
            ErrorManager.ReturnMessageToSourceQueue(message.FaultEnvelopeId);
        }
    }
}