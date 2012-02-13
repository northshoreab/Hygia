namespace Hygia.Operations.AuditUploads.Feed
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using Messages;
    using NServiceBus;
    using NServiceBus.Faults.InMemory;
    using NServiceBus.Unicast.Queuing.Msmq;
    using NServiceBus.Unicast.Transport;
    using NServiceBus.Unicast.Transport.Transactional;
    using RestSharp;

    public class Uploader : IWantCustomInitialization, IWantToRunAtStartup
    {
        static ITransport inputTransport;
        static bool includeMessageBody;
        static Guid apiKey;
        static Address auditQueueAddress;
        static string apiUrl;

        public void Init()
        {
            var key = ConfigurationManager.AppSettings["hygia.apikey"];
         
            if (string.IsNullOrEmpty(key))
                throw new ConfigurationErrorsException("hygia.api is required to start the launchpad");


            var audit = ConfigurationManager.AppSettings["hygia.input"];
            if (string.IsNullOrEmpty(audit))
                throw new ConfigurationErrorsException("hygia.input is required to start the launchpad");

            apiUrl = ConfigurationManager.AppSettings["hygia.apiurl"];

            if (string.IsNullOrEmpty(apiUrl))
            {
                uploadAction = UploadToOnPremiseBackend;
            }
            else
            {
                uploadAction = UploadToApi;
            }
                
         

            auditQueueAddress = Address.Parse(audit);

            apiKey = Guid.Parse(key);

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

            uploadAction(message);

            

        }

        Action<ProcessAuditMessage> uploadAction;


        void UploadToOnPremiseBackend(ProcessAuditMessage message)
        {
            //todo- shoudl we use a more explicit environmentid from app.config instead?
            message.SetHeader("EnvironmentId", apiKey.ToString());

            Configure.Instance.Builder.Build<IBus>()
                .Send(message);
        }

        void UploadToApi(ProcessAuditMessage message)
        {
            var client = new RestClient(apiUrl);

            var request = new RestRequest("upload", Method.POST) { RequestFormat = DataFormat.Json };

            request.AddBody(new
                                {
                                    message.MessageId,
                                    ApiKey = apiKey,
                                    message.Headers,
                                    message.AdditionalInformation,
                                    message.Body
                                });

            var response = client.Execute(request);

            if(response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Failed to upload message: " +  response.StatusDescription);
        }
    }
}