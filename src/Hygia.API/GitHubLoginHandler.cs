using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Hygia.API.Controllers.UserManagement.GitHub;
using Microsoft.IdentityModel.Claims;
using RestSharp;
using Thinktecture.IdentityModel.Claims;
using Thinktecture.IdentityModel.Http;

namespace Hygia.API
{
    //TODO: Work in progress, maybe this would be enough to handle github login.
    public class GitHubLoginHandler : DelegatingHandler
    {
        private readonly AuthenticationConfiguration configuration;

        public GitHubLoginHandler(AuthenticationConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(request.Method == HttpMethod.Post && request.RequestUri.Segments.Last() == "LoginToGithub")
            {
                var client = new RestClient("https://github.com");

                var code = request.GetRouteData().Values["code"] as string;

                var accessTokenRequest = new RestRequest("/login/oauth/access_token", Method.POST) { RequestFormat = DataFormat.Json };

                accessTokenRequest.AddParameter("client_id", "933251074a0f47066f44");
                accessTokenRequest.AddParameter("client_secret", "11c5684e3f03e9efd49d3c7b663dcc0d36cf6bda");
                accessTokenRequest.AddParameter("code", code);

                var response = client.Execute<AccessTokenResponse>(accessTokenRequest);

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

                if (response.Data.access_token == null)
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

                var claims = new List<Claim>
                                 {
                                     //new Claim(ClaimTypes.Name, unToken.UserName),
                                     new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.Password),
                                     new Claim("GitHubAccessToken", response.Data.access_token),
                                     AuthenticationInstantClaim.Now
                                 };

                var identity = new ClaimsIdentity(claims, "Basic");

                IClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentityCollection {identity});

                if (configuration.ClaimsAuthenticationManager != null)
                {
                    principal = configuration.ClaimsAuthenticationManager.Authenticate(request.RequestUri.AbsoluteUri, principal);
                }

                Thread.CurrentPrincipal = principal;

                request.Headers.Authorization = new BasicAuthenticationHeaderValue("WatchRLoggedInUser", "ThisShouldBeAVerySecretKey123012912390asdfasdf'ASF---AA!!5656/дц");
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}