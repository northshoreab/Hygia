using System;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.FaultManagement.Commands;
using NServiceBus;

namespace Hygia.API.Controllers.FaultManagement.Faults.Commands
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/faultmanagement/faults/{id:guid}/retried")]
    public class RetriedController : ApiController
    {
        private readonly IBus bus;

        public RetriedController(IBus bus)
        {
            this.bus = bus;
        }

        public FaultRetriedInputModel GetAll()
        {
            return new FaultRetriedInputModel();
        }

        public string Post(FaultRetriedInputModel model)
        {
            bus.Send(new RegisterSuccessfullRetry
                         {
                             FaultId = model.FaultId,
                             TimeOfRetry = model.TimeOfRetry
                         });

            return string.Empty;
        }
    }

    public class FaultRetriedInputModel
    {
        public Guid FaultId { get; set; }
        public DateTime TimeOfRetry { get; set; }
    }
}