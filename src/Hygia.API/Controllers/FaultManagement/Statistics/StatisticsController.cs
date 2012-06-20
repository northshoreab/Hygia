using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;

namespace Hygia.API.Controllers.FaultManagement.Statistics
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/environments/{environment:guid}/faultmanagement/statistics")]
    [Authorize]
    public class StatisticsController : EnvironmentController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
            {
                Links = new List<Link>
                                       {
                                           new Link {Href = "/api/environments/" + Environment + "/faultmanagement/statistics/numberoffaultsperinterval", Rel = "NumberOfFaultsPerInterval"}
                                       }
            };
        }
    }
}