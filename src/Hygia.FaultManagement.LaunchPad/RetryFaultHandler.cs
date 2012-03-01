namespace Hygia.FaultManagement.LaunchPad
{
    using Commands;
    using NServiceBus;

    public class RetryFaultHandler : IHandleMessages<RetryFault>
    {
        public void Handle(RetryFault message)
        {
            //todo
        }
    }
}