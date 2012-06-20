using System;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;
using Hygia.Operations.Accounts.Commands;
using Hygia.Operations.Accounts.Domain;
using NServiceBus;
using Raven.Client;

namespace Hygia.API.Controllers.AccountManagement.Systems
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/accounts")]
    [Authorize]
    public class SystemsController : AccountController
    {
        public ResponseItem<Hygia.Operations.Accounts.Domain.System> Post(string name)
        {
            var system = new Hygia.Operations.Accounts.Domain.System
                              {
                                  Id = Guid.NewGuid(),
                                  Name = name
                              };

            var account = Session.Load<Account>(Account);

            account.Systems.Add(system);

            Session.Store(account);

            Bus.Send(new SystemCreated {AccountId = account.Id, SystemId = system.Id});

            return system.AsResponseItem();
        }
    }
}