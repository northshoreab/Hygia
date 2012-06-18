using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;

namespace Hygia.API.Controllers.Operations.LaunchPad
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/operations/launchpad")]
    public class LaunchPadController : ApiController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link {Href = "/api/operations/launchpad/status", Rel = "Status"},
                                           new Link {Href = "/api/operations/launchpad/commands", Rel = "Commands"},
                                           new Link {Href = "/api/operations/launchpad/heartbeat", Rel = "Heartbeat"},
                                           new Link {Href = "/api/operations/launchpad/error", Rel = "Error"}
                                       }
                       };
        }
    }
}