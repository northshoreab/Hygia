using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestSharp;

namespace Hygia.API.Authentication
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