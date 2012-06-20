using System;
using System.Collections.Specialized;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.Operations.Communication.Domain;

namespace Hygia.API.Controllers.Operations.LaunchPad
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/environments/{environment:guid}/operations/launchpad/heartbeat")]
    [Authorize]
    public class HeartbeatController : EnvironmentController
    {
        public HeartBeatInputModel GetAll()
        {
            return new HeartBeatInputModel();
        }

        public string Post(HeartBeatInputModel model)
        {
            //no op for now - in the future we can store endpoint info here to give users a "is my launchpad(s) connected"
            var status = Session.Load<LaunchPadStatus>(Environment) ?? new LaunchPadStatus
                                                                   {
                                                                       Id = Environment,
                                                                       EnvironmentId = Environment
                                                                   };

            status.TimeOfLastHeartBeat = DateTime.UtcNow;
            status.Version = model.Version;

            Session.Store(status);

            return "ok";
        }
    }

    public class HeartBeatInputModel
    {
        public NameValueCollection Headers { get; set; }

        public string Version { get; set; }
    }
}