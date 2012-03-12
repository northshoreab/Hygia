namespace Hygia.Operations.Communication.Api
{
    using System;
    using Domain;
    using FubuMVC.Core;
    using Raven.Client;

    public class LaunchPadController
    {
        public IDocumentSession Session { get; set; }

        [JsonEndpoint]
        public dynamic post_launchpad_heartbeat(ContextInputModel model)
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

        [JsonEndpoint]
        public dynamic post_launchpad_reporterror(ReportErrorInputModel model)
        {

            Session.Store(new LaunchPadError
                              {
                                  TimeOfReport = DateTime.UtcNow,
                                  StackTrace = model.StackTrace,
                                  Message = model.Message,
                                  InnerException = model.InnerException
                              });

            return "ok";
        }
    }

    public class LaunchPadError
    {
        public Guid Id { get; set; }
        public DateTime TimeOfReport { get; set; }

        public string StackTrace { get; set; }
        public string Message { get; set; }
        public Exception InnerException { get; set; }
    }

    public class ReportErrorInputModel
    {
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public Exception InnerException { get; set; }
    }
}