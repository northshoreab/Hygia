using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Models.Operations.Uploads.FaultMessages;
using Hygia.Operations.Faults.Commands;

namespace Hygia.API.Controllers.Operations.Uploads.FaultMessages
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/environments/{environment:guid}/operations/uploads/processfaultmessage")]
    [Authorize]
    public class ProcessFaultMessagesController : EnvironmentController
    {
        public FaultMessage GetAll()
        {
            return new FaultMessage();
        }

        public HttpResponseMessage Post(FaultMessage input)
        {
            var command = new ProcessFaultMessage
                              {
                                  FaultEnvelopeId = input.FaultEnvelopeId,
                                  Headers = input.Headers,
                                  Body = input.Body
                              };

            Bus.Send(command);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
