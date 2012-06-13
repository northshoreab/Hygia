namespace Hygia.API.Controllers.Operations.Authentication
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Http;
    using Accounts;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Models.Operations.Accounts;
    using Models.UserManagement.UserAccounts;
    using Raven.Client;
    using Raven.Client.Linq;
    using RestSharp;
    using UserManagement.GitHub;
    using HttpCookie = System.Web.HttpCookie;

    [DefaultHttpRouteConvention]
    [RoutePrefix("api/login/github")]
    public class GithubLoginController : ApiController
    {
        public IDocumentSession Session { get; set; }

        public ResponseItem<UserAccount> Post(GithubLoginInputModel inputModel)
        {
            var userDetailsRequest = new RestRequest("/user", Method.GET) { RequestFormat = DataFormat.Json };

            userDetailsRequest.AddParameter("access_token", inputModel.AccessToken);

            var client = new RestClient("https://api.github.com");

            var response = client.Execute<GithubUserResponse>(userDetailsRequest);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception(response.StatusDescription);

            var account = Session.Query<UserAccount>().FirstOrDefault(a => a.GithubUserId == response.Data.id);

            if (account == null)
                throw new Exception("No account found for github user " + response.Data.id);

            var token = account.Id.ToString();

            /***** Add your wif code here Danne */
            HttpContext.Current.Response.Cookies.Remove("watchr.authtoken");
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("watchr.authtoken", token));

            return new ResponseItem<UserAccount> { Data = account };
        }
    }

    public class GithubLoginInputModel
    {
        public string AccessToken { get; set; }
    }
}