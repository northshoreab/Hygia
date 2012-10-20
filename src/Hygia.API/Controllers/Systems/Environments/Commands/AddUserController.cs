using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;
using Hygia.Operations.Accounts.Domain;
using Hygia.UserManagement.Domain;

namespace Hygia.API.Controllers.Systems.Environments.Commands
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/environments/{environment:guid}/adduser")]
    [Authorize]
    public class AddUserController : Controllers.EnvironmentController
    {
        public Resource<Environment> Post(string email)
        {
            var environment = Session.Query<Environment>().SingleOrDefault(x => x.Id == Environment);
            var user = Session.Query<UserAccount>().SingleOrDefault(x => x.Email == email);

            if (environment == null || user == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

            environment.Users.Add(user.Id);

            Session.Store(environment);

            return environment.AsResourceItem();
        }
    }
}