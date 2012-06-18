using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;

namespace Hygia.API.Controllers.Operations
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/operations")]
    public class OperationsController : ApiController
    {
        public ResponseMetaData GetAll()
        {
            return new ResponseMetaData
                       {
                           Links = new List<Link>
                                       {
                                           new Link {Href = "/api/operations/launchpad", Rel = "LaunchPad"},
                                           new Link {Href = "/api/operations/uploads", Rel = "Uploads"},
                                           new Link {Href = "/api/login/withgithub", Rel = "LoginWithGithub"},
                                           new Link {Href = "/api/signup/withgithub", Rel = "SignupWithGithub"},
                                           new Link {Href = "/api/operations/launchpad/download", Rel = "DownloadLaunchpad"},                                           
                                       }
                       };
        }
    }
}