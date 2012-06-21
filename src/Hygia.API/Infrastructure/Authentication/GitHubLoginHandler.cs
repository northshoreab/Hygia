using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Hygia.UserManagement.Domain;
using Microsoft.IdentityModel.Claims;
using Raven.Client;
using StructureMap;

namespace Hygia.API.Infrastructure.Authentication
{
    //TODO: Work in progress, maybe this would be enough to handle github login.
    public class GitHubLoginHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Action action = GetAction(request.RequestUri);

         
            if(action != Action.NotAuthentication)
            {
                string userName;
                string accessToken;

                //todo - get rid of this
                if(action == Action.Backdoor)
                {
                    userName = "administrator";
                    accessToken = "";
                }
                else
                {
                    string code = request.RequestUri.ParseQueryString()["code"];
                    accessToken = GithubHelper.GetAccessToken(code);

                    var githubUser = GithubHelper.GetGithubUser(accessToken);

                    userName = githubUser.name;

                    if (action == Action.Login)
                    {
                        var session = ObjectFactory.GetInstance<IDocumentStore>().OpenSession();

                        var account = session.Query<UserAccount>().SingleOrDefault(u => u.IdentityProviders.Any(x => x.Issuer == "github.com" && x.UserId == githubUser.id));

                        if (account == null)
                            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

                        userName = account.UserName;
                    }

                   
                    
                }
                var jwt = AuthenticationHelper.CreateJsonWebToken(userName, accessToken);

                request.Headers.Authorization = new AuthenticationHeaderValue("JWT", jwt);
            }
            else
            {
                if(request.Headers.Authorization == null)
                {
                    var jwt = request.Headers.GetCookies().SelectMany(x => x.Cookies).SingleOrDefault(x => x.Name == "jwt");

                    if (jwt != null)
                        request.Headers.Authorization = new AuthenticationHeaderValue("JWT", jwt.Value);                    
                }
            }

            return base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                                  {
                                      HttpResponseMessage response = task.Result;

                                      if (action != Action.NotAuthentication)
                                      {
                                          var user = Thread.CurrentPrincipal.Identity as IClaimsIdentity;

                                          var authorizationHeader = response.RequestMessage.Headers.Authorization;

                                          if (user == null || authorizationHeader.Scheme != "JWT")
                                              return response;

                                          response.Headers.AddCookies(new[] { new CookieHeaderValue("jwt", authorizationHeader.Parameter) { Expires = new DateTimeOffset(new DateTime(2022, 1, 1)), Path = "/", Secure = false } });
                                      }

                                      return response;
                                  });
        }

        private Action GetAction(Uri requestUri)
        {
            string lastSegment = requestUri.Segments.Last().ToLower().Replace("/", "");
            
            if(lastSegment == "withgithub" && requestUri.Segments.Select(x => x.ToLower()).Contains("login/"))
                return Action.Login;

            if(lastSegment == "withgithub" && requestUri.Segments.Select(x => x.ToLower()).Contains("signup/"))
                return Action.SignUp;

            if (lastSegment == "backdoor" && requestUri.Segments.Select(x => x.ToLower()).Contains("login/"))
                return Action.Backdoor;

            return Action.NotAuthentication;
        }

        enum Action
        {
            NotAuthentication,
            SignUp,
            Login,
            Backdoor
        }
    }

    public class AccessTokenResponse
    {
        public string access_token { get; set; }
    }
}