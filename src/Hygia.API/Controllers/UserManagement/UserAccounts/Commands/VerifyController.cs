using System;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;
using Hygia.UserManagement.Domain;
using Raven.Client;
using UserAccount = Hygia.API.Models.UserManagement.UserAccounts.UserAccount;

namespace Hygia.API.Controllers.UserManagement.UserAccounts.Commands
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/usermanagement/useraccounts/{id:guid}/verify")]
    [Authorize]
    public class VerifyController : WatchRApiController
    {
        private readonly IDocumentSession session;

        public VerifyController(IDocumentSession session)
        {
            this.session = session;
        }

        public string GetAll()
        {
            return "{ \"id\"" + " : " + "\"\" }";
        }

        public Resource<UserAccount> Post(Guid id)
        {
            var account = session.Load<UserAccount>(id);

            if (account == null)
                throw new InvalidOperationException("No user account found for userId " + id);

            account.Status = UserAccountStatus.Verified;

            return account.AsResourceItem();
        }
    }
}