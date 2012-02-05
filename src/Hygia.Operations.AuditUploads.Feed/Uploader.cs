namespace Hygia.Operations.AuditUploads.Feed
{
    using System;
    using System.Collections.Generic;
    using Hygia.Operations.AuditUploads.Messages;
    using NServiceBus;
    using NServiceBus.Faults.InMemory;
    using NServiceBus.Unicast.Queuing.Msmq;
    using NServiceBus.Unicast.Transport;
    using NServiceBus.Unicast.Transport.Transactional;

    public class Uploader : IWantCustomInitialization, IWantToRunAtStartup
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
                .Send(message);//todo - vary this for on premise mode/cloud mode
            
        }
    }
}