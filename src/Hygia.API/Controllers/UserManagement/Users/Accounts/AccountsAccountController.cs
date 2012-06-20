using System;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;
using Hygia.Operations.Accounts.Commands;
using Hygia.Operations.Accounts.Domain;

namespace Hygia.API.Controllers.UserManagement.Users.Accounts
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/users/{user}/accounts")]
    [Authorize]
    public class AccountsAccountController : UserAccountController
    {
        public ResponseItem<Account> Post(string name)
        {
            var account = new Account
                              {
                                  Id = Guid.NewGuid(),
                                  Name = name,
                              };

            Session.Store(account);

            Bus.Send(new AccountCreated
                         {
                             UserAccountId = UserAccountId,
                             AccountId = account.Id
                         });

            return account.AsResponseItem();
        }
    }
}