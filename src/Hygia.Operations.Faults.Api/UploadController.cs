namespace Hygia.Operations.Faults.Api
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using FubuMVC.Core;
    using NServiceBus;

    public class UploadController
    {

        public IBus Bus { get; set; }
     
        [JsonEndpoint]
        public string post_upload_processfaultmessage(UploadInputModel input)
        {
            //todo - check apikey, enfore message limits etc

            var environmentId = input.ApiKey;//todo make a real lookup


            var command = new ProcessFaultMessage
                              {
                                  FaultEnvelopeId = input.FaultEnvelopeId,
                                  Headers = input.Headers,
                                  Body = input.Body
                              };

            command.SetHeader("EnvironmentId", environmentId.ToString());
                                   
            Bus.Send(command);
            return "ok";
        }
    }

    public class UploadInputModel
    {
        public string FaultEnvelopeId { get; set; }

        public Guid ApiKey { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string Body { get; set; }
    }
}
