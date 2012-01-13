namespace Hygia.LaunchPad
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Inspectors;
    using NServiceBus;
    using NServiceBus.Faults.InMemory;
    using NServiceBus.Unicast.Queuing.Msmq;
    using NServiceBus.Unicast.Transport;
    using NServiceBus.Unicast.Transport.Transactional;

    public class AuditProcessor : IWantCustomInitialization, IWantToRunAtStartup
    {
        static ITransport inputTransport;

        public void Init()
        {
            inputTransport = new TransactionalTransport
                                 {
                                     MessageReceiver = new MsmqMessageReceiver(),
                                     IsTransactional = true,
                                     NumberOfWorkerThreads = 1,
                                     MaxRetries = 5,
                                     FailureManager = new FaultManager()
                                 };

            inputTransport.TransportMessageReceived += OnTransportMessageReceived;


           Configure.TypesToScan.Where(t => typeof(IInspectEnvelopes).IsAssignableFrom(t) && !(t.IsAbstract || t.IsInterface))
                .ToList().ForEach(t => Configure.Instance.Configurer.ConfigureComponent(t, DependencyLifecycle.InstancePerCall));

        }

        public void Run()
        {
            inputTransport.Start(Address.Parse("audit"));
        }

        public void Stop()
        {
        }

        void OnTransportMessageReceived(object sender, TransportMessageReceivedEventArgs e)
        {
            var transportMessage = e.Message;
            var bus = Configure.Instance.Builder.Build<IBus>();
    
            Console.Write("Processing message - " + transportMessage.Id);
            var commands = new List<object>();

            Configure.Instance.Builder.BuildAll<IInspectEnvelopes>().ToList()
                .ForEach(inspector => commands.AddRange(inspector.Inspect(transportMessage)));

            //for the on-premise version we use sendlocal to process the commands as default
            commands.ForEach(command=>bus.SendLocal(command));

            Console.WriteLine(" - Complete"); 
        }
    }
}