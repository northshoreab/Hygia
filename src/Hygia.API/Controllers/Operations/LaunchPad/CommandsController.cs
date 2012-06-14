using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.Operations.Communication.Domain;
using Raven.Client;

namespace Hygia.API.Controllers.Operations.LaunchPad
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/operations/launchpad/commands")]
    public class CommandsController : ApiController
    {
        private readonly IDocumentSession session;

        public CommandsController(IDocumentSession session)
        {
            this.session = session;
        }

        public List<LaunchPadCommand> Get()
        {
            return session.Query<LaunchPadCommand>()
                .Where(c => !c.Delivered)
                .ToList();
        }

        public LaunchPadCommand Get(Guid commandId)
        {
            return session.Load<LaunchPadCommand>(commandId);
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