using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Hygia.API.Authentication;
using Hygia.UserManagement.Domain;
using Microsoft.IdentityModel.Claims;
using Raven.Client;
using RestSharp;
using StructureMap;
using Thinktecture.IdentityModel.Claims;
using Thinktecture.IdentityModel.Tokens.Http;

namespace Hygia.API
{
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

                var claims = new List<Claim>
                                 {
                                     new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.Password),
                                     new Claim(Constants.ClaimTypes.GithubAccessToken, accessToken),
                                     AuthenticationInstantClaim.Now
                                 };

                if(action == Action.Login)
                {
                    var session = ObjectFactory.GetInstance<IDocumentStore>().OpenSession();

                    var account = session.Query<UserAccount>().SingleOrDefault(u => u.IdentityProviders.Any(x => x.Issuer == "github.com" && x.UserId == githubUser.id));

                    if (account == null)
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

                    userName = account.UserName;
                    claims.Add(new Claim(Constants.ClaimTypes.UserAccountId, account.Id.ToString()));
                }

                claims.Add(new Claim(ClaimTypes.Name, userName));

                var identity = new ClaimsIdentity(claims, "Basic");

                IClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentityCollection {identity});

                if (_configuration.ClaimsAuthenticationManager != null)
                {
                    principal = _configuration.ClaimsAuthenticationManager.Authenticate(request.RequestUri.AbsoluteUri, principal);
                }

                Thread.CurrentPrincipal = principal;
            }

            return base.SendAsync(request, cancellationToken);
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