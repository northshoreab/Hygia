namespace Hygia.Operations.Communication.LaunchPad.Cloud
{
    using System.Collections.Generic;
    using Commands;
    using Domain;
    using NServiceBus;
    using Newtonsoft.Json;
    using RestSharp;

    public class ApiCommandFetcher : IHandleMessages<FetchCommands>
    {
        public IBus Bus { get; set; }

        public IApiCall ApiCall { get; set; }

        public JsonConverter Converter { get; set; }

        public void Handle(FetchCommands message)
        {
            var response = ApiCall.Invoke<List<LaunchPadCommand>>(Method.GET, "commands");


            var commands = JsonConvert.DeserializeObject<List<LaunchPadCommand>>(response.Content, Converter);

            foreach (var launchPadCommand in commands)
                Bus.SendLocal(launchPadCommand.Command);
        }
    }

   

}