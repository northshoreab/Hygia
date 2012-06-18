using System.Linq;
using System.Web.Security;
using Hygia.API.App_Start;
using Microsoft.IdentityModel.Claims;
using Newtonsoft.Json;
using Raven.Client;
using StructureMap;
using Thinktecture.IdentityModel.Claims;
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

        public static ClaimsIdentity GetTicketIdentity(string ticket)
        {
            if (ticket != null)
            {
                var encTicket = FormsAuthentication.Decrypt(ticket);

                return IdentityFactory.Create("Github",
                                              new Claim(Constants.ClaimTypes.GithubAccessToken, encTicket.UserData),
                                              new Claim(ClaimTypes.Name, encTicket.Name),
                                              AuthenticationInstantClaim.Now);
            }

            return null;
        }

        public static SimpleSecurityTokenHandler GetGithubTokenHandler()
        {
            return new SimpleSecurityTokenHandler(Constants.GithubScheme, token =>
                                                                              {
                                                                                  var githubToken = JsonConvert.DeserializeObject<GithubLoginToken>(token);
                                                                                  if (githubToken.LoginKey == Constants.GithubLoginKey)
                                                                                  {
                                                                                      return IdentityFactory.Create("Github",
                                                                                                                    new Claim(Constants.ClaimTypes.GithubAccessToken, githubToken.AccessToken),
                                                                                                                    new Claim(ClaimTypes.Name, githubToken.UserName),
                                                                                                                    AuthenticationInstantClaim.Now);
                                                                                  }

                                                                                  return null;
                                                                              });
        }
    }
}