namespace Hygia.Operations.Faults.Feed
{
    using System.Configuration;
    using System.Text;
    using System.Threading;
    using Commands;
    using Communications;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Faults.Forwarder;
    using NServiceBus.ObjectBuilder;
    using NServiceBus.Unicast;
    using NServiceBus.Unicast.Queuing;
    using NServiceBus.Unicast.Queuing.Msmq;
    using NServiceBus.Unicast.Transport;
    using NServiceBus.Unicast.Transport.Transactional;
    using log4net;

    public class Uploader : INeedInitialization, IWantToRunWhenTheBusStarts
    {
       
        public void Init()
        {

            var error = ConfigurationManager.AppSettings["hygia.errors.input"];
            if (string.IsNullOrEmpty(error))
            {
                logger.Warn("No error input queue defined, error feed won't start");
                return;
            }
            var errorLog = ConfigurationManager.AppSettings["hygia.errors.log"];
            if (string.IsNullOrEmpty(error))
                throw new ConfigurationErrorsException("No error log queue defined, error feed won't start");
            
            errorQueueAddress = Address.Parse(error);
            errorLogAddress = Address.Parse(errorLog);

            inputTransport = new TransactionalTransport
                                 {
                                     MessageReceiver = new MsmqMessageReceiver(),
                                     IsTransactional = true,
                                     NumberOfWorkerThreads = 1,
                                     MaxRetries = 5,
                                     FailureManager = new FaultManager
                                                          {
                                                              ErrorQueue = Address.Parse("LaunchPad.Error")
                                                          }
                                 };

            builder = Configure.Instance.Builder;

            inputTransport.TransportMessageReceived += OnTransportMessageReceived;
        }

        public void Run()
        {
            NServiceBus.Utils.MsmqUtilities.CreateQueueIfNecessary(errorLogAddress, Thread.CurrentPrincipal.Identity.Name);
            inputTransport.Start(errorQueueAddress);
        }

        void OnTransportMessageReceived(object sender, TransportMessageReceivedEventArgs e)
        {
            var transportMessage = e.Message;
            var message = new ProcessFaultMessage
                              {
                                  MessageId = transportMessage.IdForCorrelation,
                                  Headers = transportMessage.Headers,
                                  Body = Encoding.UTF8.GetString(transportMessage.Body)//wil only work for text serialization
                              };

            builder.Build<IUploadToTheBackend>().Upload(message);

            builder.Build<ISendMessages>().Send(transportMessage,errorLogAddress);
        }

        static IBuilder builder;
        static ITransport inputTransport;
        static Address errorQueueAddress;
        static Address errorLogAddress;
        static ILog logger = LogManager.GetLogger("Errors");

    }
}