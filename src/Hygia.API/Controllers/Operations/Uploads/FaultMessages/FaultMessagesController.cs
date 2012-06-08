using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Models.Operations.Uploads.FaultMessages;
using Hygia.Operations.Faults.Commands;
using NServiceBus;

namespace Hygia.API.Controllers.Operations.Uploads.FaultMessages
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/operations/uploads/{controller}")]
    public class FaultMessagesController : ApiController
    {
        private readonly IBus _bus;

        public FaultMessagesController(IBus bus)
        {
            _bus = bus;
        }

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

            _bus.Send(command);
            return "ok";
        }
    }
}
