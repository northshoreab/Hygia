namespace Hygia.Operations
{
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.MessageMutator;
    using NServiceBus.Unicast.Transport;

    public class EnvironmentIdPropagatingMutator:IMutateOutgoingTransportMessages,INeedInitialization
    {
        public IBus Bus { get; set; }

        public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
        {
            if (Bus.CurrentMessageContext == null)
                return;
            if (!Bus.CurrentMessageContext.Headers.ContainsKey("EnvironmentId"))
                return;

            transportMessage.Headers["EnvironmentId"] = Bus.CurrentMessageContext.Headers["EnvironmentId"];
        }

        public void Init()
        {

            Configure.Instance.Configurer.ConfigureComponent<EnvironmentIdPropagatingMutator>(
                DependencyLifecycle.InstancePerCall);
        }
    }
}