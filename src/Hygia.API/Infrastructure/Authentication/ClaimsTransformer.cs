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

            Claim issuer = id.Claims.SingleOrDefault(c => c.ClaimType == Constants.ClaimTypes.AuthenticationProvider) ?? new Claim(Constants.ClaimTypes.AuthenticationProvider, Constants.Issuers.Unknown);

            var claims = new List<Claim>
                             {
                                new Claim(ClaimTypes.Name, id.Name),
                                new Claim(ClaimTypes.Role, "Users"),
                                authMethod
                             };

            if(issuer.Value == Constants.Issuers.Github)
            {
                //TODO add claims for github users
                claims.Add(id.Claims.Single(c => c.ClaimType == Constants.ClaimTypes.GithubAccessToken));
            }

            if (issuer.Value == Constants.Issuers.ApiKey)
            {
                //TODO add claims for apikey users
            }

/*
            var accessToken = id.GetClaimValue(Constants.ClaimTypes.GithubAccessToken);

            if(!string.IsNullOrEmpty(accessToken))
                claims.Add(new Claim(Constants.ClaimTypes.GithubAccessToken, accessToken));
*/
            var claimsIdentity = new ClaimsIdentity(claims, "Local");
            return new ClaimsPrincipal(new IClaimsIdentity[] { claimsIdentity });
        }
    }
}