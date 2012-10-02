using Hygia.API.Infrastructure.Authentication;
using Microsoft.Web.WebPages.OAuth;

namespace Hygia.API.App_Start
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();
            OAuthWebSecurity.RegisterClient(new GithubClient("5473cd5bd72dbdc6dbab", "2d3d87ce38452436d3eb58925b59be6e4ce151c2"), "github", null);
        }
    }
}