using AttributeRouting;
using AttributeRouting.Web.Http;
using System.Diagnostics;
using System.Reflection;
using System.Web.Http;

namespace Hygia.API.Controllers.Version
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/version")]
    public class VersionController : ApiController
    {
        public string GetAll()
        {            
            var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return string.Format("{0}.{1}.{2}", version.FileMajorPart, version.FileMinorPart, version.FileBuildPart);
        }
    }
}