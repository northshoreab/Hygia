using System;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;
using Hygia.API.Models.Operations.Accounts;
using Hygia.Operations.Accounts.Commands;
using Environment = Hygia.API.Models.Operations.Accounts.Environment;

namespace Hygia.API.Controllers.AccountManagement.Environments
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/accounts/{account}/environments")]
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

            account.Environments.Add(environment);

            Session.Store(account);

            Bus.Send(new EnvironmentCreated {Environment = environment.Id});

            return environment.AsResponseItem();
        }
    }
}