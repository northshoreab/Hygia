using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;

namespace Hygia.API.Controllers.Operations.Uploads
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/environments/{environment:guid}/operations/uploads")]
    [Authorize]
    public class UploadsController : EnvironmentController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link {Href = "/api/" + Environment + "/operations/uploads/processauditmessage", Rel = "AuditMessages"},
                                           new Link {Href = "/api/" + Environment + "/operations/uploads/processfaultmessage", Rel = "FaultMessages"},
                                       }
                       };
        }
    }
}