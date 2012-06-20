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
    [RoutePrefix("api/accounts/{account:guid}/systems/{system:guid}/environments")]
    [Authorize]
    public class EnvironmentsController : AccountController
    {
        public ResponseItem<Environment> Post(string name)
        {
            var account = Session.Load<Account>(Account);

            var environment = new Environment
                                  {
                                      ApiKey = Guid.NewGuid(),
                                      Id = Guid.NewGuid(),
                                      Name = name
                                  };

            account.Systems.Single(x => x.Id == System).Environments.Add(environment);

            Session.Store(account);

            Bus.Send(new EnvironmentCreated
                         {
                             Account = Account,
                             System = System,
                             Environment = environment.Id
                         });

            return environment.AsResponseItem();
        }
    }
}