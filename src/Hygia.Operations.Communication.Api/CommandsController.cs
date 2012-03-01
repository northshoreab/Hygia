namespace Hygia.Operations.Communication.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using FaultManagement.Commands;
    using FubuMVC.Core;
    using Raven.Client;

    public class CommandsController
    {
        public IDocumentSession Session { get; set; }

        [JsonEndpoint]
        public dynamic get_commands()
        {
            return Session.Query<LaunchPadCommand>()
                .Where(c => !c.Delivered)
                .ToList();
        }

        [JsonEndpoint]
        public dynamic post_commands_markasprocessed(MarkAsProcessedViewModel model)
        {
            foreach (var commandId in model.Commands)
            {
                var command = Session.Load<LaunchPadCommand>(commandId);

                if (command != null)
                    command.Delivered = true;

            }

            return "ok";
        }
    }

    public class MarkAsProcessedViewModel
    {
        public List<Guid> Commands { get; set; }
    }
}