using System;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.FaultManagement.Commands;

namespace Hygia.API.Controllers.FaultManagement.Faults.Commands
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/environments/{environment:guid}/faultmanagement/faults/{id:guid}/archive")]
    [Authorize]
    public class ArchiveController : EnvironmentController
    {
        public string GetAll()
        {
            return string.Empty;
        }

        public string Post(Guid id)
        {
            Bus.Send(new ArchiveFault { FaultId = id });
            return string.Empty;
        }
    }
}