using System;
using System.Collections.Specialized;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.Operations.Communication.Domain;
using Raven.Client;

namespace Hygia.API.Controllers.Operations.LaunchPad
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/operations/launchpad/heartbeat")]
    public class HeartbeatController : ApiController
    {
        private readonly IDocumentSession _session;

        public HeartbeatController(IDocumentSession session)
        {
            _session = session;
        }

        public HeartBeatInputModel GetAll()
        {
            return new HeartBeatInputModel();
        }

        public string Post(HeartBeatInputModel model)
        {
            var apiKey = model.Headers["apikey"];

            if (apiKey == null)
                throw new InvalidOperationException("Heartbeats must contain a valid api key");

            var key = Guid.Parse(apiKey);

            //no op for now - in the future we can store endpoint info here to give users a "is my launchpad(s) connected"
            var status = _session.Load<LaunchPadStatus>(key) ?? new LaunchPadStatus
                                                                   {
                                                                       Id = key,
                                                                       EnvironmentId = key
                                                                   };

            status.TimeOfLastHeartBeat = DateTime.UtcNow;
            status.Version = model.Version;

            _session.Store(status);

            return "ok";
        }
    }

    public class HeartBeatInputModel
    {
        public NameValueCollection Headers { get; set; }

        public string Version { get; set; }
    }
}