namespace Hygia.Operations.Communication.Api
{
    using System;
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
            //Session.Store(new LaunchPadCommand
            //                  {
            //                      Delivered = false,
            //                      Command = new RetryFault
            //                                    {
            //                                        MessageId = Guid.NewGuid()
            //                                    },
            //                  });

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