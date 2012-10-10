using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using DotNetOpenAuth.AspNet.Clients;
using Hygia.API.Extensions;
using Hygia.API.Infrastructure.Authentication.GitHubDomain;
using Newtonsoft.Json.Linq;


namespace Hygia.API.Infrastructure.Authentication
{
    public class GithubClient : OAuth2Client
    {
        private const string AuthorizationEndpoint = "https://github.com/login/oauth/authorize?client_id={0}&redirect_uri={1}&state={2}";
        private const string TokenEndpoint = "https://github.com/login/oauth/access_token";
        private const string TokenPostFormat = "client_id={0}&client_secret={1}&code={2}&state";
        private readonly string applicationId_;
        private readonly string applicationSecret_;

        public GithubClient(string appId, string appSecret)
            : base("github")
        {
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentException("appId");

            if (string.IsNullOrEmpty(appSecret))
                throw new ArgumentException("appSecret");

            this.applicationId_ = appId;
            this.applicationSecret_ = appSecret;
        }

        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            var queryString = returnUrl.ParseQueryString();

            var uri = new Uri(
                string.Format(AuthorizationEndpoint, applicationId_, returnUrl.AbsoluteUri, queryString.Get("__sid__") + "," + queryString.Get("__provider__"))
                );
            return uri;
        }

        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            var message = string.Format(TokenPostFormat, applicationId_, applicationSecret_, authorizationCode);

            var tokenRequest = WebRequest.Create(TokenEndpoint);
            tokenRequest.ContentType = "application/x-www-form-urlencoded";
            tokenRequest.ContentLength = message.Length;
            tokenRequest.Method = "POST";

            using (var requestStream = tokenRequest.GetRequestStream())
            {
                var writer = new StreamWriter(requestStream);
                writer.Write(message);
                writer.Flush();
            }

            var tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            if (tokenResponse.StatusCode == HttpStatusCode.OK)
            {
                using (var responseStream = tokenResponse.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream);
                    var responseText = reader.ReadToEnd();
                    try
                    {
                        var token = Regex.Match(responseText, "access_token=(.*)&token_type=(.*)").Groups[1].Value;
                        return token;
                    }
                    catch (Exception e)
                    {
                        throw new UriFormatException("Unexpected format", e);
                    }
                }
            }

            return null;
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            var request = WebRequest.Create("https://api.github.com/user?access_token=" + accessToken);
            GitHubUser gitHubUser;

            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream);
                    var jsonString = reader.ReadToEnd();
                    gitHubUser = Newtonsoft.Json.JsonConvert.DeserializeObject<GitHubUser>(jsonString);
                }
            }

            var userData = new Dictionary<string, string>();

            userData.AddItemIfNotEmpty("id", Convert.ToString(gitHubUser.Id));
            userData.AddItemIfNotEmpty("username", gitHubUser.Email);
            userData.AddItemIfNotEmpty("name", gitHubUser.Name);
            userData.AddItemIfNotEmpty("link", gitHubUser.Url);
            userData.AddItemIfNotEmpty("email", gitHubUser.Email);
            userData.AddItemIfNotEmpty("accessToken", accessToken);
            userData.AddItemIfNotEmpty("gravatarId",gitHubUser.Gravatar_Id);

            return userData;
        }
    }
}