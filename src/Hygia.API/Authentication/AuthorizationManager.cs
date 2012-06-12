using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.IdentityModel.Claims;
using Thinktecture.IdentityModel.Http;

namespace Hygia.API.Authentication
{
    public class AuthorizationManager : IAuthorizationManager
    {
        public bool CheckAccess(HttpActionContext context)
        {
            var principal = context.Request.GetUserClaims();

            if (principal.GetType() == typeof(WindowsClaimsPrincipal))
                return false;

            //if(!principal.ClaimExists(Constants.ClaimTypes.WatchRRole))
            //{
            //    return false;
            //}

            if(!principal.IsInRole("Users"))
                return false;

            return true;
        }
    }
}