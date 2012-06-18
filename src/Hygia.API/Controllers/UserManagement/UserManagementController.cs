using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;

namespace Hygia.API.Controllers.UserManagement
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/usermanagement")]
    [Authorize]
    public class UserManagementController : ApiController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link {Href = "/api/usermanagement/me", Rel = "Me"},
                                           new Link {Href = "/api/usermanagement/useraccounts", Rel = "UserAccounts"},
                                       }
                       };
        }
    }
}