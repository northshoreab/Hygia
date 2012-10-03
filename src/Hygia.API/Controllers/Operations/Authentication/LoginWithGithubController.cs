using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Security;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Threading;
using AttributeRouting;
using AttributeRouting.Web.Http;
using DotNetOpenAuth.AspNet;
using Hygia.API.Infrastructure.Authentication;
using Hygia.UserManagement.Domain;
using Microsoft.Web.WebPages.OAuth;
using System;
using Raven.Client;
using StructureMap;

namespace Hygia.API.Controllers.Operations.Authentication
{
    //[DefaultHttpRouteConvention]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [AllowAnonymous]
        [GET(""), HttpGet]
        //[ValidateAntiForgeryToken]
        public void Login(string provider, string returnUrl)
        {
            OAuthWebSecurity.RequestAuthentication(provider, "http://localhost:38105/api/login/authenticationcallback");
        }

        [AllowAnonymous]
        [GET("authenticationcallback"), HttpGet]
        public HttpResponseMessage AuthenticationCallback()
        {
            var queryString = ControllerContext.Request.RequestUri.ParseQueryString();
            
            if (queryString.AllKeys.All(x => x != "__sid__") && ControllerContext.Request.RequestUri != null && queryString.AllKeys.Any(x => x == "state"))
            {
                var response = ControllerContext.Request.CreateResponse(HttpStatusCode.RedirectKeepVerb);
                response.Headers.Location = new Uri(ControllerContext.Request.RequestUri.AbsoluteUri + "&__sid__=" + queryString.Get("state"));
                return response;
            }

            var result = OAuthWebSecurity.VerifyAuthentication();

            if (result.IsSuccessful)
            {
                // name of the provider we just used
                var provider = result.Provider;
                // provider's unique ID for the user
                var uniqueUserID = result.ProviderUserId;
                // since we might use multiple identity providers, then 
                // our app uniquely identifies the user by combination of 
                // provider name and provider user id
                var uniqueID = provider + "/" + uniqueUserID;

                // we then log the user into our application
                // we could have done a database lookup for a 
                // more user-friendly username for our app
                FormsAuthentication.SetAuthCookie(uniqueID, false);

                // dictionary of values from identity provider
                var userDataFromProvider = result.ExtraData;
                //var email = userDataFromProvider["email"];
                //var gender = userDataFromProvider["gender"];

                var response = new HttpResponseMessage(HttpStatusCode.Accepted);
                var session = ObjectFactory.GetInstance<IDocumentStore>().OpenSession();

                var userAccount = session.Query<UserAccount>().SingleOrDefault(
                        u => u.IdentityProviders.Any(x => x.Issuer == "github.com" && x.UserId == result.ProviderUserId)) ??
                    new UserAccount
                    {
                        Id = Guid.Empty,
                        UserName = userDataFromProvider["username"]
                    };

                response.Headers.AddCookies(new[] { new CookieHeaderValue("jwt", AuthenticationHelper.CreateJsonWebToken(userAccount, userDataFromProvider["accessToken"])) { Expires = new DateTimeOffset(new DateTime(2022, 1, 1)), Path = "/", Secure = false } });

                return response;
            }

            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
    }

    [DefaultHttpRouteConvention]
    [RoutePrefix("api/login/withgithub")]
    [Authorize]
    public class LoginWithGithubController : ApiController
    {
        public string Get()
        {
            return Request.Headers.Authorization.Parameter;
        }
    }

    [DefaultHttpRouteConvention]
    [RoutePrefix("api/login/backdoor")]
    [Authorize]
    public class LoginWithBackdoorController : ApiController
    {
        public string Get()
        {
            return Request.Headers.Authorization.Parameter;
        }
    }
}