namespace Hygia.Operations.Communication.LaunchPad.Cloud
{
    using RestSharp;

    public class CloudUploader : IUploadToTheBackend
    {
        public void Upload(object message)
        {
            ApiCall.Invoke( Method.POST,"upload/" + message.GetType().Name,message);
        }

        public IApiCall ApiCall { get; set; }
    }
}