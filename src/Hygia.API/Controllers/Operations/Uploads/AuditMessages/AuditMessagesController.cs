using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Models.Operations.Uploads.AuditMessages;
using Hygia.Operations.AuditUploads.Commands;
using NServiceBus;

namespace Hygia.API.Controllers.Operations.Uploads.AuditMessages
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/operations/uploads/auditmessages")]
    [Authorize]
    public class AuditMessagesController : ApiController
    {
        private readonly IBus _bus;

        public AuditMessagesController(IBus bus)
        {
            _bus = bus;
        }

        public AuditMessage GetAll()
        {
            return new AuditMessage();
        }

        public string Post(AuditMessage input)
        {
            var command = new ProcessAuditMessage
                              {
                                  MessageId = input.MessageId,
                                  Headers = input.Headers,
                                  AdditionalInformation = input.AdditionalInformation,
                                  Body = input.Body
                              };
                         
            _bus.Send(command);
            return "ok";
        }
    }
}
