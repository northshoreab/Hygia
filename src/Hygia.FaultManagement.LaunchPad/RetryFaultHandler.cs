namespace Hygia.FaultManagement.LaunchPad
{
    using System;
    using Commands;
    using NServiceBus;
    using Operations.Communication.LaunchPad.Cloud;

    public class RetryFaultHandler : IHandleMessages<RetryFault>
    {
        public ErrorManager ErrorManager { get; set; }
        public IApiCall ApiCall { get; set; }

        public void Handle(RetryFault message)
        {
            ErrorManager.ReturnMessageToSourceQueue(message.FaultEnvelopeId);

            ApiCall.Invoke("POST", "faults/retried", new
            {
                message.FaultId,
                TimeOfRetry = DateTime.UtcNow
            });
        }
    }
}