using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Models.Operations.Uploads.AuditMessages;
using Hygia.Operations.AuditUploads.Commands;

namespace Hygia.API.Controllers.Operations.Uploads.AuditMessages
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/environments/{environment:guid}/operations/uploads/auditmessages")]
    [Authorize]
    public class AuditMessagesController : EnvironmentController
    {
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
                         
            Bus.Send(command);
            return "ok";
        }
    }
}
