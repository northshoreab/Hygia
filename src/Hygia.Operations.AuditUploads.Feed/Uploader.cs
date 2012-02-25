using Hygia.Operations.AuditUploads.Commands;

namespace Hygia.Operations.AuditUploads.Feed
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using Communications;
    using NServiceBus;
    using NServiceBus.ObjectBuilder;
    using NServiceBus.Unicast.Queuing.Msmq;
    using NServiceBus.Unicast.Transport;
    using NServiceBus.Unicast.Transport.Transactional;
    using RestSharp;
    using log4net;
    using FaultManager = NServiceBus.Faults.Forwarder.FaultManager;

    public class Uploader : IWantCustomInitialization, IWantToRunAtStartup
    {
        static ITransport inputTransport;
        static bool includeMessageBody;
        static Guid apiKey;
        static Address auditQueueAddress;
        static string apiUrl;
        static ILog logger = LogManager.GetLogger("Audit");

        public void Init()
        {

            var audit = ConfigurationManager.AppSettings["watchr.audit.input"];
            if (string.IsNullOrEmpty(audit))
            {
                logger.Warn("No audit input queue defined, audit feed won't start");
                return;
            }

            auditQueueAddress = Address.Parse(audit);

            
            includeMessageBody = false;
            inputTransport = new TransactionalTransport
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
            builder = Configure.Instance.Builder;

            inputTransport.TransportMessageReceived += OnTransportMessageReceived;
        }

        public void Run()
        {
            inputTransport.Start(auditQueueAddress);
        }

        public void Stop()
        {
        }

        void OnTransportMessageReceived(object sender, TransportMessageReceivedEventArgs e)
        {
            var transportMessage = e.Message;
            var message = new ProcessAuditMessage
                              {
                                  MessageId = transportMessage.IdForCorrelation,
                                  Headers = transportMessage.Headers,
                                  AdditionalInformation = new Dictionary<string, string>()
                              };

            message.AdditionalInformation["MessageIntent"] = transportMessage.MessageIntent.ToString();
            message.AdditionalInformation["CorrelationId"] = transportMessage.CorrelationId;

            if (includeMessageBody)
                message.Body = transportMessage.Body;

            builder.Build<IUploadToTheBackend>().Upload(message);

            

        }

        static IBuilder builder;
    }
}