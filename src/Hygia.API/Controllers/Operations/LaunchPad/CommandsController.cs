using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Models;
using Hygia.Operations.Communication.Domain;

namespace Hygia.API.Controllers.Operations.LaunchPad
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/{environment}/operations/launchpad/commands")]
    [Authorize]
    public class CommandsController : EnvironmentController
    {
        [CustomQueryable]
        public IQueryable<LaunchPadCommand> Get()
        {
            return Session.Query<LaunchPadCommand>()
                .Where(c => !c.Delivered);
        }

        public LaunchPadCommand Get(Guid commandId)
        {
            return Session.Load<LaunchPadCommand>(commandId);
        }

        public string Post(MarkAsProcessedInputModel model)
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

    public class MarkAsProcessedInputModel
    {
        public List<Guid> Commands { get; set; }
    }
}