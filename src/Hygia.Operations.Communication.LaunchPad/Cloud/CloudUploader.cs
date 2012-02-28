namespace Hygia.Operations.Communication.LaunchPad.Cloud
{
    using System;
    using System.Net;
    using RestSharp;
    using log4net;

    public class CloudUploader:IUploadToTheBackend
    {
        public string ApiUrl { get; set; }

        public string ApiKey { get; set; }

        public void Upload(object message)
        {
            var client = new RestClient(ApiUrl);
            var action = "upload/" + message.GetType().Name;

            var request = new RestRequest(action, Method.POST) { RequestFormat = DataFormat.Json };

            request.AddBody(message);
            request.AddHeader("apikey", ApiKey);

            var response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Failed to upload message: " + response.StatusDescription);

            logger.InfoFormat("{0} uploaded successfully to {1}",message.GetType().Name,ApiUrl+action);
        }

        static readonly ILog logger = LogManager.GetLogger("communication");
    }
}