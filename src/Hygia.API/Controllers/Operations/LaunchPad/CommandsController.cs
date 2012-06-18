using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Models;
using Hygia.Operations.Communication.Domain;
using Raven.Client;

namespace Hygia.API.Controllers.Operations.LaunchPad
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/operations/launchpad/commands")]
    [Authorize]
    public class CommandsController : ApiController
    {
        private readonly IDocumentSession _session;

        public CommandsController(IDocumentSession session)
        {
            _session = session;
        }

        [CustomQueryable]
        public IQueryable<LaunchPadCommand> Get()
        {
            return _session.Query<LaunchPadCommand>()
                .Where(c => !c.Delivered);
        }

        public LaunchPadCommand Get(Guid commandId)
        {
            return _session.Load<LaunchPadCommand>(commandId);
        }

        public string Post(MarkAsProcessedInputModel model)
        {
            foreach (var commandId in model.Commands)
            {
                var command = _session.Load<LaunchPadCommand>(commandId);

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