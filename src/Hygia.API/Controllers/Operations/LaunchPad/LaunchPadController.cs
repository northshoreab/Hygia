using System;
using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;
using Hygia.Operations.Communication.Domain;

namespace Hygia.API.Controllers.Operations.LaunchPad
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/launchpads")]
    [Authorize]
    public class LaunchPadController : ApiController
    {
        public ResponseMetaData Get(Guid launchPadId)
        {
            //TODO: Get links for current launchpad, load environment with apikey
            //var launchPadStatus = Session.Load<LaunchPadStatus>(launchPadId);
            //return new ResponseMetaData
            //           {
            //               Links = new List<Link>
            //                           {
            //                               new Link {Href = "/api/" + launchPadStatus. + "/operations/launchpad/status", Rel = "Status"},
            //                               new Link {Href = "/api/" + Environment + "/operations/launchpad/commands", Rel = "Commands"},
            //                               new Link {Href = "/api/" + Environment + "/operations/launchpad/heartbeat", Rel = "Heartbeat"},
            //                               new Link {Href = "/api/" + Environment + "/operations/launchpad/error", Rel = "Error"}
            //                           }
            //           };
            
            return new ResponseMetaData();
        }
    }
}