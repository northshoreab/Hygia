using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;

namespace Hygia.API.Controllers.FaultManagement.Statistics
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/faultmanagement")]
    public class StatisticsController : ApiController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
            {
                Links = new List<Link>
                                       {
                                           new Link {Href = "/api/faultmanagement/statistics/numberoffaultsperinterval", Rel = "NumberOfFaultsPerInterval"}
                                       }
            };
        }
    }
}