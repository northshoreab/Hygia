using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Controllers.FaultManagement.Statistics;
using Hygia.API.Infrastructure;
using Hygia.API.Infrastructure.Authentication;
using Hygia.Operations.Accounts.Commands;
using Microsoft.IdentityModel.Claims;
using Raven.Client;
using Raven.Client.Extensions;
using Raven.Client.Indexes;
using StructureMap;
using Environment = Hygia.Operations.Accounts.Domain.Environment;

namespace Hygia.API.Controllers.Systems.Environments
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/environments")]
    [Authorize]
    public class EnvironmentController : WatchRApiController
    {
        public IQueryable<Resource<Environment>> Get()
        {
            var claimsIdentity = User.Identity as IClaimsIdentity;

            var userId = Guid.Parse(claimsIdentity.Claims.Single(c => c.ClaimType == Constants.ClaimTypes.UserAccountId).Value);

            var environments = Session.Query<Environment>().ToList();
            
            var myEnvs = environments.Where(x => x.Users.Contains(userId));

            return myEnvs.Select(x => x.AsResourceItem()).AsQueryable();
        }

        public Resource<Environment> Post(Environment env)
        {
            var claimsIdentity = User.Identity as IClaimsIdentity;

            var userId = Guid.Parse(claimsIdentity.Claims.Single(c => c.ClaimType == Constants.ClaimTypes.UserAccountId).Value);

            var environment = new Environment
                                  {
                                      ApiKey = Guid.NewGuid(),
                                      Id = Guid.NewGuid(),
                                      Name = env.Name,
                                      Users = new List<Guid>{ userId }
                                  };

            Session.Store(environment);

            var store = ObjectFactory.GetInstance<IDocumentStore>();

            store.DatabaseCommands.EnsureDatabaseExists(environment.Id.ToString());

            var catalog = new CompositionContainer(new AssemblyCatalog(typeof(NumberOfFaultsPerDay).Assembly));
            IndexCreation.CreateIndexes(catalog, store.DatabaseCommands.ForDatabase(environment.Id.ToString()), store.Conventions);
            
            Bus.Send(new EnvironmentCreated
                         {
                             EnvironmentId = environment.Id
                         });

            return environment.AsResourceItem(new List<Link>
                                                  {
                                                     new Link { Href = "/api/environments/{?environmentid}/adduser", Rel = "addUser", Templated = true }
                                                  });
        }
    }
}