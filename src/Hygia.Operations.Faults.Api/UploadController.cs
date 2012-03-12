namespace Hygia.Operations.Faults.Api
{
    using Commands;
    using FubuMVC.Core;
    using NServiceBus;

    public class UploadController
    {

        public IBus Bus { get; set; }
     
        [JsonEndpoint]
        public string post_upload_processfaultmessage(UploadInputModel input)
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
