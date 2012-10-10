using System;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;
using Hygia.Operations.Accounts.Commands;
using Hygia.Operations.Accounts.Domain;
using Environment = Hygia.Operations.Accounts.Domain.Environment;

namespace Hygia.API.Controllers.AccountManagement.Systems.Environments
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/{system:guid}/environments")]
    [Authorize]
    public class CreateEnvironmentController : ApiController
    {
        public ResponseItem<Environment> Post(Guid systemId,Guid environmentId, string name)
        {
            var system = Session.Load<Hygia.Operations.Accounts.Domain.System>(systemId);

            var environment = new Environment
                                  {
                                      ApiKey = environmentId,
                                      Id = environmentId,
                                      Name = name,
                                  };

            system.Environments.Add(environment);

            Session.Store(system);

            Bus.Send(new EnvironmentCreated
                         {
                             SystemId = systemId,
                             EnvironmentId = environment.Id
                         });

            return environment.AsResponseItem();
        }
    }
}