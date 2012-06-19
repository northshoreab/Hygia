using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Models.Operations.Uploads.FaultMessages;
using Hygia.Operations.Faults.Commands;

namespace Hygia.API.Controllers.Operations.Uploads.FaultMessages
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/{environment}/operations/uploads/faultmessages")]
    [Authorize]
    public class FaultMessagesController : EnvironmentController
    {
        public FaultMessage GetAll()
        {
            return new FaultMessage();
        }

        public string Post(FaultMessage input)
        {
            var command = new ProcessFaultMessage
                              {
                                  FaultEnvelopeId = input.FaultEnvelopeId,
                                  Headers = input.Headers,
                                  Body = input.Body
                              };

            Bus.Send(command);
            return "ok";
        }
    }
}
