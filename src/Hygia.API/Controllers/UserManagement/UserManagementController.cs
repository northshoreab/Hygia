using System.Collections.Generic;
using AttributeRouting;
using AttributeRouting.Web.Http;

namespace Hygia.API.Controllers.UserManagement
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api")]
    public class UserManagementController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link {Href = "/api/usermanagement/github", Rel = "GitHub"},
                                           new Link {Href = "/api/usermanagement/useraccounts", Rel = "UserAccounts"},
                                       }
                       };
        }
    }
}