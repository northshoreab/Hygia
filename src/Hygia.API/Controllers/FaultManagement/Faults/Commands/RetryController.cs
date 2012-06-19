using System;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.FaultManagement.Commands;

namespace Hygia.API.Controllers.FaultManagement.Faults.Commands
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/{environment}/faultmanagement/faults/{id:guid}/retry")]
    [Authorize]
    public class RetryController : EnvironmentController
    {
        public string GetAll()
        {
            return string.Empty;
        }

        public string Post(Guid id)
        {
            Bus.Send(new IssueRetryForFault
                          {
                              FaultId = id,
                              IssuedAt = DateTime.UtcNow
                          });

            return string.Empty;
        }
    }
}