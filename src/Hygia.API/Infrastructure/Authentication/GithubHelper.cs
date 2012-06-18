using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestSharp;

namespace Hygia.API.Infrastructure.Authentication
{
    public static class GithubHelper
    {
        public static GithubUserResponse GetGithubUser(string accessToken)
        {
            var userDetailsRequest = new RestRequest("/user", Method.GET) { RequestFormat = DataFormat.Json };

            userDetailsRequest.AddParameter("access_token", accessToken);

            var githubApiClient = new RestClient("https://api.github.com");

            var userResponse = githubApiClient.Execute<GithubUserResponse>(userDetailsRequest);

            if (userResponse.StatusCode != HttpStatusCode.OK)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

            return userResponse.Data;
        }

        public static string GetAccessToken(string code)
        {
            var client = new RestClient("https://github.com");

            var accessTokenRequest = new RestRequest("/login/oauth/access_token", Method.POST) { RequestFormat = DataFormat.Json };

            accessTokenRequest.AddParameter("client_id", "5473cd5bd72dbdc6dbab");
            accessTokenRequest.AddParameter("client_secret", "2d3d87ce38452436d3eb58925b59be6e4ce151c2");

            //accessTokenRequest.AddParameter("client_id", "933251074a0f47066f44");
            //accessTokenRequest.AddParameter("client_secret", "11c5684e3f03e9efd49d3c7b663dcc0d36cf6bda");
            accessTokenRequest.AddParameter("code", code);

            var response = client.Execute<AccessTokenResponse>(accessTokenRequest);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

            if (response.Data.access_token == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

            return response.Data.access_token;
        }
    }

    public class GithubUserResponse
    {
        public string id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string gravatar_id { get; set; }
        public string login { get; set; }
    }
}