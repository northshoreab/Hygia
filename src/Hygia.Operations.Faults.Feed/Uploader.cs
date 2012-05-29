namespace Hygia.Operations.Faults.Feed
{
    using System.Configuration;
    using System.Text;
    using System.Threading;
    using Commands;
    using Communication.LaunchPad;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Unicast.Queuing;
    using NServiceBus.Unicast.Transport;
    using log4net;
    using IWantToRunWhenTheBusStarts = NServiceBus.IWantToRunWhenTheBusStarts;

    public class Uploader : INeedInitialization, IWantToRunWhenTheBusStarts
    {
        public TransportFactory TransportFactory { get; set; }

        public IUploadToTheBackend BackendUploader { get; set; }

        public ISendMessages MessageSender { get; set; }

        public void Init()
        {

            var error = ConfigurationManager.AppSettings["watchr.errors.input"];
            if (string.IsNullOrEmpty(error))
            {
                logger.Warn("No error input queue defined, error feed won't start");
                return;
            }
            var errorLog = ConfigurationManager.AppSettings["watchr.errors.log"];
            if (string.IsNullOrEmpty(errorLog))
                errorLog = error + "_log";
            
            errorQueueAddress = Address.Parse(error);
            errorLogAddress = Address.Parse(errorLog);
        }
        
        public void Run()
        {
            if (errorQueueAddress == null)
                return;

            NServiceBus.Utils.MsmqUtilities.CreateQueueIfNecessary(errorLogAddress, Thread.CurrentPrincipal.Identity.Name);

            inputTransport = TransportFactory.GetTransport(OnTransportMessageReceived);

            inputTransport.Start(errorQueueAddress);
        }

        void OnTransportMessageReceived(object sender, TransportMessageReceivedEventArgs e)
        {
            var transportMessage = e.Message;

            //send first so that we get the new id that we can use for retries
            MessageSender.Send(transportMessage,errorLogAddress);

            var message = new ProcessFaultMessage
            {
                FaultEnvelopeId = transportMessage.Id,
                Headers = transportMessage.Headers,
                Body = Encoding.UTF8.GetString(transportMessage.Body)//wil only work for text serialization
            };

            BackendUploader.Upload(message);
        }

        ITransport inputTransport;
        static Address errorQueueAddress;
        static Address errorLogAddress;
        static ILog logger = LogManager.GetLogger("Errors");

    }
}