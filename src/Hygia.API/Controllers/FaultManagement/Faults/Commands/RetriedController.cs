using System;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.FaultManagement.Commands;

namespace Hygia.API.Controllers.FaultManagement.Faults.Commands
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/{environment}/faultmanagement/faults/{id:guid}/retried")]
    [Authorize]
    public class RetriedController : EnvironmentController
    {
        public FaultRetriedInputModel GetAll()
        {
            return new FaultRetriedInputModel();
        }

        public string Post(FaultRetriedInputModel model)
        {
            Bus.Send(new RegisterSuccessfullRetry
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