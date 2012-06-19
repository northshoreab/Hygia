using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;

namespace Hygia.API.Controllers.Operations.LaunchPad
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/{environment}/operations/launchpad")]
    [Authorize]
    public class LaunchPadController : EnvironmentController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link {Href = "/api/" + Environment + "/operations/launchpad/status", Rel = "Status"},
                                           new Link {Href = "/api/" + Environment + "/operations/launchpad/commands", Rel = "Commands"},
                                           new Link {Href = "/api/" + Environment + "/operations/launchpad/heartbeat", Rel = "Heartbeat"},
                                           new Link {Href = "/api/" + Environment + "/operations/launchpad/error", Rel = "Error"}
                                       }
                       };
        }
    }
}