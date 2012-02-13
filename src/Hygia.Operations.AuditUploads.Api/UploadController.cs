namespace Hygia.Operations.AuditUploads.Api
{
    using System;
    using System.Collections.Generic;
    using FubuMVC.Core;
    using Messages;
    using NServiceBus;

    public class UploadController
    {
        readonly IBus bus;

        public UploadController(IBus bus)
        {
            this.bus = bus;
        }

        [JsonEndpoint]
        public string post_upload(UploadInputModel input)
        {
            //todo - check apikey, enfore message limits etc

            var environmentId = input.ApiKey;//todo make a real lookup


            var command = new ProcessAuditMessage
                              {
                                  MessageId =input.MessageId,
                                  Headers = input.Headers,
                                  AdditionalInformation = input.AdditionalInformation,
                                  Body = input.Body
                              };

            command.SetHeader("EnvironmentId", environmentId.ToString());
                                   
            bus.Send(command);
            return "ok";
        }
    }

    public class UploadInputModel
    {
        public string MessageId { get; set; }

        public Guid ApiKey { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public Dictionary<string, string> AdditionalInformation { get; set; }

        public byte[] Body { get; set; }
    }
}
