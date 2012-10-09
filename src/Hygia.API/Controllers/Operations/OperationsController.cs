using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;

namespace Hygia.API.Controllers.Operations
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/environments/{environment:guid}/operations")]
    [Authorize]
    public class OperationsController : EnvironmentController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link {Href = "/api/" + Environment + "/operations/launchpad", Rel = "LaunchPad"},
                                           new Link {Href = "/api/" + Environment + "/operations/uploads", Rel = "Uploads"},
                                           new Link {Href = "/api/" + Environment + "/operations/launchpad/download", Rel = "DownloadLaunchpad"},
                                       }
                       };
        }
    }
}