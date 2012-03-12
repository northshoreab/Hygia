namespace Hygia.Operations.Communication.LaunchPad.Cloud
{
    using System;
    using System.Configuration;
    using System.Threading;
    using NServiceBus.Unicast;
    using RestSharp;
    using log4net;

    public class HeartBeat:IWantToRunWhenTheBusStarts
    {
        static Timer timer;
        public void Run()
        {
            var heartbeat = ConfigurationManager.AppSettings["watchr.heartbeat"];
            if (string.IsNullOrEmpty(heartbeat))
                heartbeat = "30";


            timer = new Timer(DoHeartBeatCall, null, 0, int.Parse(heartbeat)* 1000);
        }

        void DoHeartBeatCall(object state)
        {
            try
            {
                ApiCall.Invoke("POST", "launchpad/heartbeat", new
                {
                    //empty for now
                });

            }
            catch (Exception)
            {
                logger.Warn("Heart beat failed");
            }
        }

        static ILog logger = LogManager.GetLogger("communications");
        public IApiCall ApiCall { get; set; }
    }
}