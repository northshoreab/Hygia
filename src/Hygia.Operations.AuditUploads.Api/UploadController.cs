namespace Hygia.Operations.AuditUploads.Api
{
    using FubuMVC.Core;
    using Messages;

    public class UploadController
    {
        [JsonEndpoint]
        public string post_upload(ProcessAuditMessage input)
        {
            return input.ApiKey.ToString();
        }
    }
}
