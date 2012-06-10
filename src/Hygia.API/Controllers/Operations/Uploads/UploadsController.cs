using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;

namespace Hygia.API.Controllers.Operations.Uploads
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/operations/uploads")]
    public class UploadsController : ApiController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link {Href = "/api/operations/uploads/auditmessages", Rel = "AuditMessages"},
                                           new Link {Href = "/api/operations/uploads/faultmessages", Rel = "FaultMessages"},
                                       }
                       };
        }
    }
}