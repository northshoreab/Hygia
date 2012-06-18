using System;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.FaultManagement.Commands;
using NServiceBus;

namespace Hygia.API.Controllers.FaultManagement.Faults.Commands
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/faultmanagement/faults/{id:guid}/archive")]
    [Authorize]
    public class ArchiveController : ApiController
    {
        private readonly IBus bus;

        public ArchiveController(IBus bus)
        {
            this.bus = bus;
        }

        public string GetAll()
        {
            return string.Empty;
        }

        public string Post(Guid id)
        {
            bus.Send(new ArchiveFault { FaultId = id });
            return string.Empty;
        }
    }
}