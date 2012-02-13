using System.Diagnostics;
using System.Reflection;
using Raven.Client;

namespace Hygia.Web.Controllers
{
    public class VersionController
    {
        private IDocumentSession _session;

        public VersionController(IDocumentSession session)
        {
            _session = session;
        }

        public string get_Version()
        {            
            var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return string.Format("{0}.{1}.{2}", version.FileMajorPart, version.FileMinorPart, version.FileBuildPart);
        }
    }
}