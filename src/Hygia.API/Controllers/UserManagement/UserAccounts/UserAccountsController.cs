using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;
using Hygia.API.Models;
using Hygia.API.Models.UserManagement.UserAccounts;
using Hygia.Core;
using Hygia.UserManagement.Domain;
using NServiceBus;
using Hygia.Operations.Email.Commands;
using Raven.Client;
using UserAccount = Hygia.API.Models.UserManagement.UserAccounts.UserAccount;

namespace Hygia.API.Controllers.UserManagement.UserAccounts
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/usermanagement/useraccounts")]
    public class UserAccountsController : ApiController
    {
        private readonly IDocumentSession _session;
        private readonly IBus _bus;

        public UserAccountsController(IDocumentSession session, IBus bus)
        {
            links = userAccount => new List<Link>
                                       {
                                           new Link
                                               {
                                                   Href = "/api/usermanagement/useraccounts/" + userAccount.Id + "/verify",
                                                   Rel = "verify"
                                               }
                                       };
            _session = session;
            _bus = bus;
        }

        private readonly Func<UserAccount, IEnumerable<Link>> links;


        public ResponseItem<UserAccount> Get(Guid id)
        {
            var userAccount = _session.Load<Hygia.UserManagement.Domain.UserAccount>(id);

            var outputUserAccount = userAccount.ToOutputModel();

            return outputUserAccount.AsResponseItem().AddLinks(links);
        }

        [CustomQueryable]
        public IQueryable<ResponseItem<UserAccount>> GetAll()
        {
            return _session.Query<Hygia.UserManagement.Domain.UserAccount>()
                .ToOutputModel()
                .Select(x => x.AsResponseItem().AddLinks(links))
                .AsQueryable();
        }

        public ResponseItem<UserAccount> Post(UserAccountInputModel model)
        {
            var userId = model.Email.ToGuid();

            var account = _session.Load<Hygia.UserManagement.Domain.UserAccount>(userId);

            if (account != null)//todo- add a behaviour that translates exceptions to json that backbone can use
                throw new InvalidOperationException("A user account for " + model.Email + " already exists");

            account = new Hygia.UserManagement.Domain.UserAccount
                          {
                              Id = userId,
                              UserName = model.Email,
                              Email = model.Email,
                              SignedUpAt = DateTime.UtcNow,
                              Status = UserAccountStatus.Unverified
                          };

            _session.Store(account);

            _bus.Send(new SendEmailRequest
                         {
                             DisplayName = "WatchR - SignUp",
                             To = model.Email,
                             Subject = "Please verify your email at WatchR.se",
                             Body = "http://watchr.se/#verify/" + userId,
                             Service = "usermanagement",
                             Parameters = userId.ToString()
                         });

            return account.ToOutputModel().AsResponseItem().AddLinks(links);
        }
    }

    public class UserAccountInputModel
    {
        public string Email { get; set; }
    }
}
