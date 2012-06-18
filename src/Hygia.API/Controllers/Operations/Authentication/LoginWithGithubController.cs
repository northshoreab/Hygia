using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure.Authentication;
using Hygia.API.Models.UserManagement.UserAccounts;
using Microsoft.IdentityModel.Claims;
using Raven.Client;
using UserAccount = Hygia.UserManagement.Domain.UserAccount;

namespace Hygia.API.Controllers.Operations.Authentication
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/login/withgithub")]
    [Authorize]
    public class LoginWithGithubController : ApiController
    {
        private readonly IDocumentSession _session;

        public LoginWithGithubController(IDocumentSession session)
        {
            _session = session;
        }

        public string Get()
        {
            return Request.Headers.Authorization.Parameter;
            //var user = User as IClaimsIdentity;

            //return _session.Load<UserAccount>(user.GetClaimValue(Constants.ClaimTypes.UserAccountId)).ToOutputModel();
        }
    }
}