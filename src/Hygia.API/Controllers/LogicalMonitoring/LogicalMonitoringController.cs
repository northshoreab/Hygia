using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;

namespace Hygia.API.Controllers.LogicalMonitoring
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/{environment}/logicalmonitoring")]
    [Authorize]
    public class LogicalMonitoringController : EnvironmentController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link {Href = "/api/" + Environment + "/logicalmonitoring/messagetypes", Rel = "MessageTypes"}
                                       }
                       };
        }
    }
}