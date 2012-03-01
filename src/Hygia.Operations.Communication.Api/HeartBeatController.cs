namespace Hygia.Operations.Communication.Api
{
    using FubuMVC.Core;
    using Raven.Client;

    public class HeartBeatController
    {
        public IDocumentSession Session { get; set; }

        [JsonEndpoint]
        public dynamic post_heartbeat()
        {
            //no op for now - in the future we can store endpoint info here to give users a "is my launchpad(s) connected"
        
            return "ok";
        }
    }
}