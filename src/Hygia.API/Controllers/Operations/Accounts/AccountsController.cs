using System;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Models.Operations.Accounts;
using Hygia.Operations.Accounts.Commands;
using NServiceBus;
using Raven.Client;

namespace Hygia.API.Controllers.Operations.Accounts
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/operations/accounts")]
    public class AccountsController : ApiController
    {
        private readonly IDocumentSession _session;
        private readonly IBus _bus;

        public AccountsController(IDocumentSession session, IBus bus)
        {
            _session = session;
            _bus = bus;
        }

        public AccountInputModel GetAll()
        {
            return new AccountInputModel();
        }

        public ResponseItem<Account> Post(AccountInputModel inputModel)
        {
            var account = _session.Query<Account>().FirstOrDefault(t => t.Name == inputModel.Name);

            if (account != null)
                throw new InvalidOperationException("A Account with that name already exists");

            account = new Account
                          {
                              Id = inputModel.AccountId,
                              Name = inputModel.Name
                          };

            _session.Store(account);

            _bus.Publish<AccountCreated>(m =>
                                             {
                                                 m.AccountId = account.Id;
                                             });
            return account.AsResponseItem();
        }
    }

    public class AccountInputModel
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
    }
}