namespace Hygia.Operations.AuditUploads.Api
{
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
        public string post_upload(ProcessAuditMessage input)
        {
            //todo - check tennant id, enfore message limits etc

            bus.Send(input);
            return "ok";
        }
    }
}
