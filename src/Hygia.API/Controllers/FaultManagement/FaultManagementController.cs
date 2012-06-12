using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Thinktecture.IdentityModel.Http;

namespace Hygia.API.Controllers.FaultManagement
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/faultmanagement")]
    //[ApiAuthorize]
    public class FaultManagementController : ApiController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link{ Href = "/api/faultmanagement/faults", Rel = "Faults"},
                                           new Link{ Href = "/api/faultmanagement/statistics", Rel = "Statistics"}
                                       },
                       };
        }
    }
}