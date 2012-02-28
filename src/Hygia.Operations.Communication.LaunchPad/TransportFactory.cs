namespace Hygia.Operations.Communication.LaunchPad
{
    using System;
    using System.Configuration;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Faults.Forwarder;
    using NServiceBus.Unicast.Queuing.Msmq;
    using NServiceBus.Unicast.Transport;
    using NServiceBus.Unicast.Transport.Transactional;

    public class TransportFactory : INeedInitialization
    {
        static Address errorQueue;

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
                    ErrorQueue = errorQueue
                }
            };

            transport.TransportMessageReceived += onTransportMessageReceived;

            return transport;
        }

        public void Init()
        {
            var error = ConfigurationManager.AppSettings["watchr.errorqueue"];

            if (string.IsNullOrEmpty(error))
                error = "WatchR.Error";

            errorQueue = Address.Parse(error);
            NServiceBus.Utils.MsmqUtilities.CreateQueueIfNecessary(errorQueue, Thread.CurrentPrincipal.Identity.Name);

        }
    }

}