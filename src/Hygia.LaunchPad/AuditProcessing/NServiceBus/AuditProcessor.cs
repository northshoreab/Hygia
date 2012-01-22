namespace Hygia.LaunchPad.AuditProcessing.NServiceBus
{
    using System;
    using System.Collections.Generic;
    using Messages;
    using global::NServiceBus;
    using global::NServiceBus.Faults.InMemory;
    using global::NServiceBus.Unicast.Queuing.Msmq;
    using global::NServiceBus.Unicast.Transport;
    using global::NServiceBus.Unicast.Transport.Transactional;

    public class AuditProcessor : IWantCustomInitialization, IWantToRunAtStartup
    {
        static ITransport inputTransport;
        bool includeMessageBody;

        public void Init()
        {
            includeMessageBody = false;
            inputTransport = new TransactionalTransport
                                 {
                                     MessageReceiver = new MsmqMessageReceiver(),
                                     IsTransactional = true,
                                     NumberOfWorkerThreads = 1,
                                     MaxRetries = 5,
                                     FailureManager = new FaultManager()
                                 };

            inputTransport.TransportMessageReceived += OnTransportMessageReceived;
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
            var message = new AuditMessageProcessed
                              {
                                  MessageId = transportMessage.IdForCorrelation,
                                  Headers = transportMessage.Headers,
                                  AdditionalInformation = new Dictionary<string, string>()
                              };

            message.AdditionalInformation["MessageIntent"] = transportMessage.MessageIntent.ToString();
            message.AdditionalInformation["CorrelationId"] = transportMessage.CorrelationId;

            if (includeMessageBody)
                message.Body = transportMessage.Body;

            Console.Write("Processing message - " + transportMessage.Id);
            Configure.Instance.Builder.Build<IBus>()
                .SendLocal(message);
            Console.WriteLine(" - Complete");
        }
    }
}