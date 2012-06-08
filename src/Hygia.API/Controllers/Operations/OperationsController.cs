using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;

namespace Hygia.API.Controllers.Operations
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api")]
    public class OperationsController : ApiController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link {Href = "/api/operations/accounts", Rel = "Accounts"},
                                           new Link {Href = "/api/operations/launchpad", Rel = "LaunchPad"},
                                           new Link {Href = "/api/operations/uploads", Rel = "Uploads"}
                                       }
                       };
        }
    }
}