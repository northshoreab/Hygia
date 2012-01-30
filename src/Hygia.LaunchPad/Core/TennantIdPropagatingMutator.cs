namespace Hygia.LaunchPad.Core
{
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.MessageMutator;
    using NServiceBus.Unicast.Transport;

    public class TennantIdPropagatingMutator:IMutateOutgoingTransportMessages,INeedInitialization
    {
        public IBus Bus { get; set; }

        public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
        {
            if (Bus.CurrentMessageContext == null)
                return;
            if (!Bus.CurrentMessageContext.Headers.ContainsKey("TennantId"))
                return;

            transportMessage.Headers["TennantId"] = Bus.CurrentMessageContext.Headers["TennantId"];
        }

        public void Init()
        {

            Configure.Instance.Configurer.ConfigureComponent<TennantIdPropagatingMutator>(
                DependencyLifecycle.InstancePerCall);
        }
    }
}