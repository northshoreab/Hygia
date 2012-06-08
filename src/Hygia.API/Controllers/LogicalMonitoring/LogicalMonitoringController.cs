using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;

namespace Hygia.API.Controllers.LogicalMonitoring
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api")]
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