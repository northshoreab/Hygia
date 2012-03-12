namespace Hygia.Operations.Communication.LaunchPad.Cloud
{
    using System.Collections.Generic;
    using System.Linq;
    using Commands;
    using Domain;
    using NServiceBus;
    using Newtonsoft.Json;

    public class FetchCommandsHandler : IHandleMessages<FetchCommands>
    {
        public IBus Bus { get; set; }

        public IApiCall ApiCall { get; set; }

        public JsonConverter Converter { get; set; }

        public void Handle(FetchCommands message)
        {
            var response = ApiCall.Invoke("GET", "commands");


            var commands = JsonConvert.DeserializeObject<List<LaunchPadCommand>>(response, Converter);

            if(!commands.Any())
                return;

            foreach (var launchPadCommand in commands)
                Bus.SendLocal(launchPadCommand.Command);
            
               
            //mark all as fetched
            ApiCall.Invoke("POST","commands/markasprocessed", new{ Commands = commands.Select(c=>c.Id).ToList()});
        }
    }

   

}