using System;
using System.Collections.Generic;
using System.Linq;
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

            return session.Query<BasicAuthenticationCredentials>().Single(x => x.Name == userName).ValidatePassword(password);
        }

        public static ClaimsIdentity GetApiKeyIdentity(string key)
        {
            Guid keyAsGuid;
            if(!Guid.TryParse(key, out keyAsGuid))
                return null;

            var session = ObjectFactory.GetInstance<IDocumentStore>().OpenSession();

            var environment = session.Query<Operations.Accounts.Domain.Environment>().SingleOrDefault(x => x.ApiKey == keyAsGuid);

            if (environment != null)
                return IdentityFactory.Create(AuthenticationTypes.Signature,
                                              new Claim(ClaimTypes.Name, "ApiKey"),
                                              AuthenticationInstantClaim.Now, 
                                              new Claim(Constants.ClaimTypes.AuthenticationProvider, Constants.Issuers.ApiKey));

            return null;
        }

      
        
        public static string CreateJsonWebToken(UserManagement.Domain.UserAccount user,string accessToken)
        {
            var jsonWebToken = new JsonWebToken
            {
                Header = new JwtHeader
                {
                    SignatureAlgorithm = JwtConstants.SignatureAlgorithms.HMACSHA256,
                    SigningCredentials = new HmacSigningCredentials(Constants.JWTKeyEncoded),
                },
                Issuer = "http://watchr.se",
                Audience = new Uri(Constants.Realm),
                Claims = new List<Claim>
                                                    {
                                                        new Claim(ClaimTypes.Name, user.UserName),
                                                        new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                                                        new Claim(Constants.ClaimTypes.GithubAccessToken, accessToken),
                                                        new Claim(Constants.ClaimTypes.UserAccountId, user.Id.ToString()),
                                                        new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.Password),
                                                        new Claim(Constants.ClaimTypes.AuthenticationProvider, Constants.Issuers.Github)
                                                    },
            };

            return new JsonWebTokenHandler().WriteToken(jsonWebToken);
        }
    }
}