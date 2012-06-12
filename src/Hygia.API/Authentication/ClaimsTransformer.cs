using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Claims;
using Claim = Microsoft.IdentityModel.Claims.Claim;
using ClaimTypes = Microsoft.IdentityModel.Claims.ClaimTypes;

namespace Hygia.API.Authentication
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

            // hard coded for demo purposes
            //if (id.Name == "dominick")
            //{
            //    claims = new List<Claim>
            //                 {
            //                     new Claim(ClaimTypes.Name, id.Name),
            //                     new Claim(ClaimTypes.Role, "Users"),
            //                     new Claim(Constants.ClaimTypes.ReportsTo, "christian"),
            //                     new Claim(ClaimTypes.Email, id.Name + "@thinktecture.com"),
            //                     authMethod
            //                 };
            //}
            //else
            //{
            //    claims = new List<Claim>
            //                 {
            //                     new Claim(ClaimTypes.Name, id.Name),
            //                     new Claim(ClaimTypes.Role, "Users"),
            //                     new Claim(ClaimTypes.Email, id.Name + "@thinktecture.com"),
            //                     authMethod
            //                 };
            //}

            var claims = new List<Claim>
                             {
                                 new Claim(ClaimTypes.Name, id.Name),
                                 new Claim(ClaimTypes.Role, "Users"),
                                 new Claim(ClaimTypes.Email, id.Name + "@thinktecture.com"),
                                 authMethod
                             };

            var claimsIdentity = new ClaimsIdentity(claims, "Federation");
            return ClaimsPrincipal.CreateFromIdentity(claimsIdentity);
        }
    }
}