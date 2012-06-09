using System;
using System.Collections.Generic;
using System.Linq;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.Operations.Communication.Domain;
using Raven.Client;

namespace Hygia.API.Controllers.Operations.LaunchPad.Commands
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/operations/launchpad/commands")]
    public class CommandsController
    {
        private readonly IDocumentSession session;

        public CommandsController(IDocumentSession session)
        {
            this.session = session;
        }

        public IEnumerable<LaunchPadCommand> GetAll()
        {
            return session.Query<LaunchPadCommand>()
                .Where(c => !c.Delivered)
                .ToList();
        }

        public LaunchPadCommand Get(Guid id)
        {
            return session.Load<LaunchPadCommand>(id);
        }

        public string Post(MarkAsProcessedInputModel model)
        {
            foreach (var commandId in model.Commands)
            {
                var command = session.Load<LaunchPadCommand>(commandId);

                if (command != null)
                    command.Delivered = true;
            }

            return "ok";
        } 
    }

    public class MarkAsProcessedInputModel
    {
        public List<Guid> Commands { get; set; }
    }
}