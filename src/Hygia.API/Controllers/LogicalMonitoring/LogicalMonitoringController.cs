using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;

namespace Hygia.API.Controllers.LogicalMonitoring
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/logicalmonitoring")]
    [Authorize]
    public class LogicalMonitoringController : ApiController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link {Href = "/api/logicalmonitoring/messagetypes", Rel = "MessageTypes"}
                                       }
                       };
        }
    }
}