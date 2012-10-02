using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace Hygia.API.Controllers.Operations.Authentication
{
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