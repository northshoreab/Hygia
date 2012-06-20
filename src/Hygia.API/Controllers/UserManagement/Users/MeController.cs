using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure.Authentication;
using Hygia.API.Models.UserManagement.UserAccounts;
using Microsoft.IdentityModel.Claims;

namespace Hygia.API.Controllers.UserManagement.Users
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/users/me")]
    [Authorize]
    public class MeController : ApiController
    {
        public Me Get()
        {
            var user = User as IClaimsIdentity;

            if(user == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

            return new Me
                       {
                           AccessToken = user.GetClaimValue(Constants.ClaimTypes.GithubAccessToken),
                           Name = user.Name
                       };
        }
    }
}