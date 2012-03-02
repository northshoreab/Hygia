namespace Hygia.Operations.Communication.Api
{
    using System;
    using Domain;
    using FubuMVC.Core;
    using Raven.Client;

    public class HeartBeatController
    {
        public IDocumentSession Session { get; set; }

        [JsonEndpoint]
        public dynamic post_heartbeat(ContextInputModel model)
        {
            var apiKey = model.Headers["apikey"];

            if (apiKey == null)
                throw new InvalidOperationException("Heartbeats must contain a valid api key");

            var key = Guid.Parse(apiKey);

            //no op for now - in the future we can store endpoint info here to give users a "is my launchpad(s) connected"
            var status = Session.Load<LaunchPadStatus>(key);

            if (status == null)
                status = new LaunchPadStatus
                             {
                                 Id = key,
                                 EnvironmentId = key
                             };

            status.TimeOfLastHeartBeat = DateTime.UtcNow;

            Session.Store(status);

            return "ok";
        }
    }
}