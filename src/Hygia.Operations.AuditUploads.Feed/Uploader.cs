namespace Hygia.Operations.AuditUploads.Feed
{
    using System.Collections.Generic;
    using System.Configuration;
    using Commands;
    using Communication.LaunchPad;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Unicast;
    using NServiceBus.Unicast.Transport;
    using log4net;

    public class Uploader : INeedInitialization, IWantToRunWhenTheBusStarts
    {

        public TransportFactory TransportFactory { get; set; }
        public IUploadToTheBackend BackendUploader { get; set; }
        
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
        
        }

        public void Run()
        {
            if (auditQueueAddress == null)
                return;

            inputTransport = TransportFactory.GetTransport(OnTransportMessageReceived); 
            inputTransport.Start(auditQueueAddress);
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

            BackendUploader.Upload(message);
        }

        ITransport inputTransport;
        static bool includeMessageBody;
        static Address auditQueueAddress;
        static readonly ILog logger = LogManager.GetLogger("Audit");

      
    }
}