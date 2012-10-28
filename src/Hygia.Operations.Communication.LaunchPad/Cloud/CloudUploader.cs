namespace Hygia.Operations.Communication.LaunchPad.Cloud
{
    using RestSharp;

    public class CloudUploader : IUploadToTheBackend
    {
        public string ApiKey { get; set; }

        public void Upload(object message)
        {
            ApiCall.Invoke("POST", "api/environments/" + ApiKey + "/operations/upload/" + message.GetType().Name, message);
        }

        public IApiCall ApiCall { get; set; }
    }
}