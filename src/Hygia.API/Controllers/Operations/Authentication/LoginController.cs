using System.Collections.Specialized;
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
    [Authorize]
    public class LoginController : ApiController
    {
        [AllowAnonymous]
        [GET(""), HttpGet]
        //[ValidateAntiForgeryToken]
        public void Login(string provider, string returnUrl)
        {
            var url = "http://localhost:8088/Hygia.API/api/login/authenticationcallback?returnUrl=" + returnUrl;

            OAuthWebSecurity.RequestAuthentication(provider, url);
        }

        [GET("test"), HttpGet]
        public string Test()
        {
            return "You have access";
        }

        [AllowAnonymous]
        [GET("authenticationcallback"), HttpGet]
        public HttpResponseMessage AuthenticationCallback(string returnUrl)
        {
            NameValueCollection queryString = ControllerContext.Request.RequestUri.ParseQueryString();
            
            if (queryString.AllKeys.All(x => x != "__sid__") && ControllerContext.Request.RequestUri != null && queryString.AllKeys.Any(x => x == "state"))
            {
                var response = ControllerContext.Request.CreateResponse(HttpStatusCode.TemporaryRedirect);
                var stateData = queryString.Get("state").Split(',');
                
                if(stateData.Count() != 2)
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);

                response.Headers.Location =
                    new Uri(ControllerContext.Request.RequestUri.AbsoluteUri + "&__sid__=" + stateData[0] +
                            "&__provider__=" + stateData[1]);

                return response;
            }

            var result = OAuthWebSecurity.VerifyAuthentication();

            if (result.IsSuccessful)
            {
                var userDataFromProvider = result.ExtraData;

                var response = new HttpResponseMessage(HttpStatusCode.Redirect);
                var session = ObjectFactory.GetInstance<IDocumentStore>().OpenSession();

                var userAccount = session.Query<UserAccount>().SingleOrDefault(
                        u => u.IdentityProviders.Any(x => x.Issuer == "github.com" && x.UserId == result.ProviderUserId)) ??
                    new UserAccount
                    {
                        Id = Guid.Empty,
                        UserName = userDataFromProvider["username"]
                    };

                response.Headers.AddCookies(new[] { new CookieHeaderValue("jwt", AuthenticationHelper.CreateJsonWebToken(userAccount, userDataFromProvider["accessToken"])) { Expires = new DateTimeOffset(new DateTime(2022, 1, 1)), Path = "/", Secure = false } });
                response.Headers.Location = new Uri(returnUrl);

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