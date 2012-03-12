namespace Hygia.Operations.Communication.LaunchPad.Cloud
{
    using System;
    using NServiceBus;
    using NServiceBus.Config;
    using NServiceBus.Faults;
    using NServiceBus.Unicast.Transport;
    using log4net;

    public class LaunchPadFaultManager : IManageMessageFailures, INeedInitialization
    {
        public void SerializationFailedForMessage(TransportMessage message, Exception e)
        {
            ReportError(e);
        }

        void ReportError(Exception e)
        {
            try
            {
                //use Service locator to avoid circular dep
                Configure.Instance.Builder.Build<IApiCall>().Invoke("POST", "launchpad/reporterror", new
                                                                                                         {
                                                                                                             e.StackTrace,
                                                                                                             e.Message,
                                                                                                             e.InnerException
                                                                                                         });
            }
            catch
            {
                logger.ErrorFormat("Failed to report error to cloud server");
            }
        }

        public void ProcessingAlwaysFailsForMessage(TransportMessage message, Exception e)
        {
            ReportError(e);
        }

        public void Init()
        {
            Configure.Instance.Configurer.ConfigureComponent<LaunchPadFaultManager>(DependencyLifecycle.InstancePerCall);
        }

        static ILog logger = LogManager.GetLogger("communication");
    }

  

}