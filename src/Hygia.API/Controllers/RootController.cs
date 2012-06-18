using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;

namespace Hygia.API.Controllers
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api")]
    public class RootController : ApiController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
            {
                Links = new List<Link>
                                       {
                                           new Link{ Href = "/api/faultmanagement", Rel = "FaultManagement"},
                                           new Link{ Href = "/api/logicalmonitoring", Rel = "LogicalMonitoring"},
                                           new Link{ Href = "/api/operations", Rel = "Operations"},
                                           new Link{ Href = "/api/usermanagement", Rel = "UserManagement"},
                                           new Link{ Href = "/api/version", Rel = "Version"},
                                       },
            };
        }
    }
}