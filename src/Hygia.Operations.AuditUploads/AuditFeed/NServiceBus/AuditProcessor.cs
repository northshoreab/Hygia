namespace Hygia.Operations.AuditUploads.AuditFeed.NServiceBus
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using global::NServiceBus;
    using global::NServiceBus.Faults.InMemory;
    using global::NServiceBus.Unicast.Queuing.Msmq;
    using global::NServiceBus.Unicast.Transport;
    using global::NServiceBus.Unicast.Transport.Transactional;

    public class AuditProcessor : IWantCustomInitialization, IWantToRunAtStartup
    {
        static ITransport inputTransport;
        bool includeMessageBody;
        Guid tennantId;
        public void Init()
        {
            tennantId = Guid.Parse("327951bf-bae4-46a4-93a0-71f61dfbe801");//todo - read from config
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
            var message = new ProcessAuditMessage
                              {
                                  TennantId = tennantId,
                                  MessageId = transportMessage.IdForCorrelation,
                                  Headers = transportMessage.Headers,
                                  AdditionalInformation = new Dictionary<string, string>()
                              };

            message.AdditionalInformation["MessageIntent"] = transportMessage.MessageIntent.ToString();
            message.AdditionalInformation["CorrelationId"] = transportMessage.CorrelationId;

            if (includeMessageBody)
                message.Body = transportMessage.Body;

            Configure.Instance.Builder.Build<IBus>()
                .SendLocal(message);//todo - vary this for on premise mode/cloud mode
            
        }
    }
}