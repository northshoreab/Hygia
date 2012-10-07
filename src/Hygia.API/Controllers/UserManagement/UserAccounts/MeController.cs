namespace Hygia.API.Controllers.UserManagement.UserAccounts
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Hygia.API.Infrastructure.Authentication;
    using Hygia.API.Models.UserManagement.UserAccounts;
    using Microsoft.IdentityModel.Claims;

    [DefaultHttpRouteConvention]
    [RoutePrefix("api/users/me")]
    [Authorize]
    public class MeController : ApiController
    {
        public Me Get()
        {
            var identity = User.Identity as IClaimsIdentity;

            if (identity == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

            return new Me
                       {
                           AccessToken = identity.GetClaimValue(Constants.ClaimTypes.GithubAccessToken),
                           Name = identity.Name
                       };
        }
    }
}