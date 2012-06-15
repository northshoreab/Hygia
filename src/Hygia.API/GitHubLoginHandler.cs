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
using Thinktecture.IdentityModel.Http;

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

            if(request.Method == HttpMethod.Post && action != Action.NotAuthentication)
            {
                var accessToken = GetAccessToken(request.GetRouteData().Values["code"] as string);

                var githubUser = GetGithubUser(accessToken);

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

        private string GetAccessToken(string code)
        {
            var client = new RestClient("https://github.com");

            var accessTokenRequest = new RestRequest("/login/oauth/access_token", Method.POST) { RequestFormat = DataFormat.Json };

            accessTokenRequest.AddParameter("client_id", "933251074a0f47066f44");
            accessTokenRequest.AddParameter("client_secret", "11c5684e3f03e9efd49d3c7b663dcc0d36cf6bda");
            accessTokenRequest.AddParameter("code", code);

            var response = client.Execute<AccessTokenResponse>(accessTokenRequest);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

            if (response.Data.access_token == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

            return response.Data.access_token;
        }

        private GithubUserResponse GetGithubUser(string accessToken)
        {
            var userDetailsRequest = new RestRequest("/user", Method.GET) { RequestFormat = DataFormat.Json };

            userDetailsRequest.AddParameter("access_token", accessToken);

            var githubApiClient = new RestClient("https://api.github.com");

            var userResponse = githubApiClient.Execute<GithubUserResponse>(userDetailsRequest);

            if (userResponse.StatusCode != HttpStatusCode.OK)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

            return userResponse.Data;
        }

        private Action GetAction(Uri requestUri)
        {
            string lastSegment = requestUri.Segments.Last().ToLower();
            if(lastSegment == "withgithub" && requestUri.Segments.Select(x => x.ToLower()).Contains("login"))
                return Action.Login;

            if(lastSegment == "withgithub" && requestUri.Segments.Select(x => x.ToLower()).Contains("signup"))
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