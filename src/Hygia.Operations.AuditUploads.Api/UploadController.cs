using Hygia.Operations.AuditUploads.Commands;

namespace Hygia.Operations.AuditUploads.Api
{
    using FubuMVC.Core;
    using NServiceBus;

    public class UploadController
    {
        public IBus Bus { get; set; }

        [JsonEndpoint]
        public string post_upload_processauditmessage(UploadInputModel input)
        {
            var command = new ProcessAuditMessage
                              {
                                  MessageId =input.MessageId,
                                  Headers = input.Headers,
                                  AdditionalInformation = input.AdditionalInformation,
                                  Body = input.Body
                              };
                         
            Bus.Send(command);
            return "ok";
        }
    }
}
