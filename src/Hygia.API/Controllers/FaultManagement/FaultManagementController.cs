using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;

namespace Hygia.API.Controllers.FaultManagement
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/{environment}/faultmanagement")]
    [Authorize]
    public class FaultManagementController : EnvironmentController
    {        
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link{ Href = "/api/" + Environment + "/faultmanagement/faults", Rel = "Faults"},
                                           new Link{ Href = "/api/" + Environment + "/faultmanagement/statistics", Rel = "Statistics"}
                                       },
                       };
        }
    }
}