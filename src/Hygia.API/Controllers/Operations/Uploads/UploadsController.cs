using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;

namespace Hygia.API.Controllers.Operations.Uploads
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/{environment}/operations/uploads")]
    [Authorize]
    public class UploadsController : EnvironmentController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link {Href = "/api/" + Environment + "/operations/uploads/auditmessages", Rel = "AuditMessages"},
                                           new Link {Href = "/api/" + Environment + "/operations/uploads/faultmessages", Rel = "FaultMessages"},
                                       }
                       };
        }
    }
}