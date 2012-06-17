using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;
using Hygia.API.Authentication;
using Hygia.UserManagement.Domain;
using Microsoft.IdentityModel.Claims;
using Newtonsoft.Json;
using Raven.Client;
using StructureMap;
using Thinktecture.IdentityModel.Tokens.Http;

namespace Hygia.API
{
    public class GithubLoginToken
    {
        public string AccessToken { get; set; }
        public string UserName { get; set; }
        public string LoginKey { get; set; }
    }
    //TODO: Work in progress, maybe this would be enough to handle github login.
    public class GitHubLoginHandler : DelegatingHandler
    {
        private readonly AuthenticationConfiguration _configuration;

        public GitHubLoginHandler(AuthenticationConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Action action = GetAction(request.RequestUri);

            if(action != Action.NotAuthentication)
            {
                string code = request.RequestUri.ParseQueryString()["code"];
                var accessToken = GithubHelper.GetAccessToken(code);

                var githubUser = GithubHelper.GetGithubUser(accessToken);

                string userName = githubUser.name;

                if(action == Action.Login)
                {
                    var session = ObjectFactory.GetInstance<IDocumentStore>().OpenSession();

                    var account = session.Query<UserAccount>().SingleOrDefault(u => u.IdentityProviders.Any(x => x.Issuer == "github.com" && x.UserId == githubUser.id));

                    if (account == null)
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

                    userName = account.UserName;
                }

                var githubLoginToken = new GithubLoginToken
                                           {
                                               AccessToken = accessToken,
                                               LoginKey = Constants.GithubLoginKey,
                                               UserName = userName
                                           };

                string token = JsonConvert.SerializeObject(githubLoginToken);

                request.Headers.Authorization = new AuthenticationHeaderValue(Constants.GithubScheme, token);
            }
            else
            {
                var ticket = request.Headers.GetCookies().SelectMany(x => x.Cookies).SingleOrDefault(x => x.Name == "ticket");
                
                if(ticket != null)
                {
                    var ticketValue = ticket.Value;
                    var encTicket = FormsAuthentication.Decrypt(ticketValue);

                    var githubLoginToken = new GithubLoginToken
                                               {
                                                   AccessToken = encTicket.UserData,
                                                   LoginKey = Constants.GithubLoginKey,
                                                   UserName = encTicket.Name
                                               };

                    string token = JsonConvert.SerializeObject(githubLoginToken);

                    request.Headers.Authorization = new AuthenticationHeaderValue(Constants.GithubScheme, token);
                }
            }

            return base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                                  {
                                      HttpResponseMessage response = task.Result;

                                      if (action != Action.NotAuthentication)
                                      {
                                          var user = Thread.CurrentPrincipal.Identity as IClaimsIdentity;

                                          var ticket = new FormsAuthenticationTicket(1, user.Name, DateTime.Now,
                                                                                     DateTime.Now.AddMinutes(60), true,
                                                                                     user.GetClaimValue(Constants.ClaimTypes.GithubAccessToken),"/");
                                          
                                          // Encrypt the ticket.
                                          string encTicket = FormsAuthentication.Encrypt(ticket);
                                          response.Headers.AddCookies(new [] {new CookieHeaderValue("ticket",encTicket){Expires=new DateTimeOffset(new DateTime(2022,1,1)), Path = "/", Secure = false}, });
                                      }

                                      return response;
                                  });
        }

        private Action GetAction(Uri requestUri)
        {
            string lastSegment = requestUri.Segments.Last().ToLower();
            if(lastSegment == "withgithub" && requestUri.Segments.Select(x => x.ToLower()).Contains("login/"))
                return Action.Login;

            if(lastSegment == "withgithub" && requestUri.Segments.Select(x => x.ToLower()).Contains("signup/"))
                return Action.SignUp;

            return Action.NotAuthentication;
        }

        enum Action
        {
            NotAuthentication,
            SignUp,
            Login
        }
    }

    public class AccessTokenResponse
    {
        public string access_token { get; set; }
    }
}