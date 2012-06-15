using System.Linq;
using Microsoft.IdentityModel.Claims;

namespace Hygia.API.Authentication
{
    public static class ClaimsIdentityExtensions
    {
        public static string GetClaimValue(this IClaimsIdentity claimsIdentity, string claimType)
        {
            if (claimsIdentity == null)
                return null;

            var claim = claimsIdentity.Claims.SingleOrDefault(x => x.ClaimType == claimType);

            if(claim == null)
                return null;

            return claim.Value;
        }
    }
}