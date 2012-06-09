using System;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.FaultManagement.Commands;
using NServiceBus;

namespace Hygia.API.Controllers.FaultManagement.Faults.Commands
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/faultmanagement/faults/{id:guid}/retry")]
    public class RetryController : ApiController
    {
        private readonly IBus _bus;

        public RetryController(IBus bus)
        {
            _bus = bus;
        }

        public string GetAll()
        {
            return string.Empty;
        }

        public string Post(Guid id)
        {
            _bus.Send(new IssueRetryForFault
                          {
                              FaultId = id,
                              IssuedAt = DateTime.UtcNow
                          });

            return string.Empty;
        }
    }
}