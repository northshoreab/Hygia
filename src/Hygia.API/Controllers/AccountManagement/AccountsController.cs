using System;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;
using Hygia.Operations.Accounts.Commands;
using Hygia.Operations.Accounts.Domain;
using NServiceBus;
using Raven.Client;

namespace Hygia.API.Controllers.AccountManagement
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/accounts")]
    [Authorize]
    public class AccountsController : ApiController
    {
        private readonly IDocumentSession session;
        private readonly IBus bus;

        public AccountsController(IDocumentSession session, IBus bus)
        {
            this.session = session;
            this.bus = bus;
        }

        public ResponseItem<Account> Post(string name)
        {
            var account = new Account
                              {
                                  Id = Guid.NewGuid(),
                                  Name = name
                              };
        
            session.Store(account);

            bus.Send(new AccountCreated {AccountId = account.Id});
            return account.AsResponseItem();
        }
    }
}