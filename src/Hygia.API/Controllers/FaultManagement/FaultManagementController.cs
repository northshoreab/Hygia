using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;

namespace Hygia.API.Controllers.FaultManagement
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api")]
    public class FaultManagementController : ApiController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link{ Href = "/api/faultmanagement/faults", Rel = "Faults"}
                                       }
                       };
        }
    }
}