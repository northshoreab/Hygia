using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;
using Hygia.API.Infrastructure.Authentication;
using Hygia.Operations.Accounts.Commands;
using Microsoft.IdentityModel.Claims;
using Environment = Hygia.Operations.Accounts.Domain.Environment;

namespace Hygia.API.Controllers.Systems.Environments
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/environments")]
    [Authorize]
    public class EnvironmentController : ApiController
    {
        public IQueryable<Resource<Environment>> Get()
        {
            var claimsIdentity = User.Identity as IClaimsIdentity;
            var environments = Session.Query<Environment>().Where(x => x.Users.Contains(Guid.Parse(claimsIdentity.Claims.Single(c => c.ClaimType == Constants.ClaimTypes.UserAccountId).Value)));

            return environments.Select(x => x.AsResponseItem()).AsQueryable();
        }

        public Resource<Environment> Post(Guid environmentId, string name)
        {
            var claimsIdentity = User.Identity as IClaimsIdentity;

            var environment = new Environment
                                  {
                                      ApiKey = Guid.NewGuid(),
                                      Id = environmentId,
                                      Name = name,
                                      Users = new List<Guid>{ Guid.Parse(claimsIdentity.Claims.Single(x => x.ClaimType == Constants.ClaimTypes.UserAccountId).Value) }
                                  };

            Session.Store(environment);

            Bus.Send(new EnvironmentCreated
                         {
                             EnvironmentId = environment.Id
                         });

            return environment.AsResponseItem(new List<Link>
                                                  {
                                                     new Link { Href = "/api/environments/{?environmentid}/adduser", Rel = "addUser", Templated = true }
                                                  });
        }
    }
}