using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Claims;

namespace Hygia.API.Infrastructure.Authentication
{
    public class ClaimsTransformer : ClaimsAuthenticationManager
    {
        public override IClaimsPrincipal Authenticate(string resourceName, IClaimsPrincipal incomingPrincipal)
        {
            if (!incomingPrincipal.Identity.IsAuthenticated)
            {
                return base.Authenticate(resourceName, incomingPrincipal);
            }

            return CreateClientIdentity(incomingPrincipal.Identity as IClaimsIdentity);
        }

        private IClaimsPrincipal CreateClientIdentity(IClaimsIdentity id)
        {
            // insert authentication method (windows, passsword, x509) as a claim
            var authMethod = id.Claims.SingleOrDefault(c => c.ClaimType == ClaimTypes.AuthenticationMethod) ??
                             new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.Unspecified);

            var claims = new List<Claim>
                             {
                                new Claim(ClaimTypes.Name, id.Name),
                                new Claim(ClaimTypes.Role, "Users"),
                                authMethod
                             };

            var accessToken = id.GetClaimValue(Constants.ClaimTypes.GithubAccessToken);

            if(!string.IsNullOrEmpty(accessToken))
                claims.Add(new Claim(Constants.ClaimTypes.GithubAccessToken, accessToken));

            var claimsIdentity = new ClaimsIdentity(claims, "Local");
            return new ClaimsPrincipal(new IClaimsIdentity[] { claimsIdentity });
        }
    }
}