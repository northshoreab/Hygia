namespace Hygia.Operations.Communication.LaunchPad.Cloud
{
    using System;
    using System.Linq;
    using System.Net;
    using Commands;
    using NServiceBus;
    using RestSharp;
    using log4net;

    public class RestSharpApiCall : IApiCall
    {
        public string ApiUrl { get; set; }

        public string ApiKey { get; set; }

        public string Invoke(string method, string action, object message)
        {
            Method m;

            Method.TryParse(method, true, out m);
            var client = new RestClient(ApiUrl);

            var request = new RestRequest(action, m) { RequestFormat = DataFormat.Json };

            if (message != null)
                request.AddBody(message);
            
            request.AddHeader("apikey", ApiKey);

            var response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception("Api call failed: " + response.StatusDescription);

            logger.InfoFormat("{0} invoked successfully",ApiUrl + action);

            if (response.Headers.Any(h => h.Name == "watchr.commandsavailable"))
                Bus.SendLocal(new FetchCommands());

            return response.Content;
        }


        public IBus Bus { get; set; }

        static readonly ILog logger = LogManager.GetLogger("communication");
    }
}