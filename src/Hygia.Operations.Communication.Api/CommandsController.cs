namespace Hygia.Operations.Communication.Api
{
    using System;
    using System.Linq;
    using Domain;
    using FubuMVC.Core;
    using Raven.Client;

    public class CommandsController
    {
        public IDocumentSession Session { get; set; }
       
        [JsonEndpoint]
        public dynamic get_commands()
        {
            return Session.Query<LaunchPadCommand>()
                .Where(c=>!c.Delivered)
                .ToList();
        }


        public void post_commands_markasprocessed(MarkAsProcessedViewModel model)
        {
            var command = Session.Load<LaunchPadCommand>(model.CommandId);

            if (command != null)
                command.Delivered = true;

        }
    }

    public class MarkAsProcessedViewModel
    {
        public Guid CommandId { get; set; }
    }
}