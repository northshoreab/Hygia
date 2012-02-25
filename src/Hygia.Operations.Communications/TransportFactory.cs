namespace Hygia.Operations.Communications
{
    using System;
    using NServiceBus;
    using NServiceBus.Faults.Forwarder;
    using NServiceBus.Unicast.Queuing.Msmq;
    using NServiceBus.Unicast.Transport;
    using NServiceBus.Unicast.Transport.Transactional;

    public class TransportFactory
    {
        public ITransport GetTransport(EventHandler<TransportMessageReceivedEventArgs> onTransportMessageReceived)
        {
            var transport = new TransactionalTransport
            {
                MessageReceiver = new MsmqMessageReceiver(),
                IsTransactional = true,
                NumberOfWorkerThreads = 1,
                MaxRetries = 5,
                FailureManager = new FaultManager
                {
                    ErrorQueue = Address.Parse("WatchR.Error")
                }
            };

            transport.TransportMessageReceived += onTransportMessageReceived;

            return transport;
        }

    }
}