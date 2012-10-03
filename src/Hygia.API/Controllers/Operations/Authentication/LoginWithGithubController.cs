using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Security;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Threading;
using AttributeRouting;
using AttributeRouting.Web.Http;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using System;

namespace Hygia.API.Controllers.Operations.Authentication
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api")]
    [Authorize]
    public class LoginController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [POST("login")]
        //[ValidateAntiForgeryToken]
        public void Login(string provider, string returnUrl)
        {
            OAuthWebSecurity.RequestAuthentication(provider, "http://localhost/api/login/authenticationcallback");
        }

        [AllowAnonymous]
        [POST("authenticationcallback")]
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
                var email = userDataFromProvider["email"];
                var gender = userDataFromProvider["gender"];

                return new HttpResponseMessage(HttpStatusCode.Accepted);
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