using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Hygia.API.App_Start;
using Microsoft.IdentityModel.Claims;
using Raven.Client;
using StructureMap;
using Thinktecture.IdentityModel.Claims;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Tokens;

namespace Hygia.API.Infrastructure.Authentication
{
    public static class AuthenticationHelper
    {
        public static bool ValidateUser(string userName, string password)
        {
            var session = ObjectFactory.GetInstance<IDocumentStore>().OpenSession();

            return session.Query<AuthenticationUser>().Single(x => x.Name == userName).ValidatePassword(password);
        }

        public static ClaimsIdentity GetApiKeyIdentity(string key)
        {
            if (key == Constants.ApiKey)
                return IdentityFactory.Create(AuthenticationTypes.Signature,
                                              new Claim(ClaimTypes.Name, "ApiKey"),
                                              AuthenticationInstantClaim.Now);

            return null;
        }

        public static string CreateJsonWebToken(string username, string accessToken)
        {
            var jsonWebToken = new JsonWebToken
                                   {
                                       Header = new JwtHeader
                                                    {
                                                        SignatureAlgorithm = JwtConstants.SignatureAlgorithms.HMACSHA256,
                                                        SigningCredentials =
                                                            new HmacSigningCredentials(Constants.JWTKeyEncoded)
                                                    },

                                       Issuer = "http://watchr.com",
                                       Audience = new Uri(Constants.Realm),

                                       Claims = new List<Claim>
                                                    {
                                                        new Claim(ClaimTypes.Name, username),
                                                        new Claim(Constants.ClaimTypes.GithubAccessToken,
                                                                  accessToken),
                                                    }
                                   };

            return new JsonWebTokenHandler().WriteToken(jsonWebToken);
        }
    }
}