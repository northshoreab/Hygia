using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;
using Hygia.API.Models;
using Hygia.API.Models.FaultManagement.Faults;
using Hygia.FaultManagement.Domain;
using Fault = Hygia.API.Models.FaultManagement.Faults.Fault;

namespace Hygia.API.Controllers.FaultManagement.Faults
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/environments/{environment:guid}/faults")]
    [Authorize]
    public class FaultsController : EnvironmentController
    {
        public FaultsController()
        {
            _links = fault => new List<Link>
                                 {
                                     new Link
                                         {
                                             Href = "/api/environments/" + Environment + "/faultmanagement/faults/" + fault.FaultId + "/archive", 
                                             Rel = "archive"
                                         },
                                     new Link
                                         {
                                             Href = "/api/environments/" + Environment + "/faultmanagement/faults/" + fault.FaultId + "/retry",
                                             Rel = "retry"
                                         },
                                     new Link
                                         {
                                             Href = "/api/environments/" + Environment + "/faultmanagement/faults/" + fault.FaultId + "/retried",
                                             Rel = "retried"
                                         },
                                 };
        }

        private readonly Func<Fault, IEnumerable<Link>> _links;

        //[CustomQueryable]
        public IQueryable<Resource<Fault>> GetAll()
        {
            return Session.Query<Hygia.FaultManagement.Domain.Fault>()
                .Where(f => f.Status != FaultStatus.Archived && f.Status != FaultStatus.Resolved && f.Status != FaultStatus.RetryPerformed)
                .OrderByDescending(f=>f.TimeOfFailure)
                .ToList()
                .ToOutputModels()
                .Select(x => x.AsResourceItem().AddLinks(_links))
                .AsQueryable();
        }
        
        public Resource<Fault> Get(Guid id)
        {
            return Session.Load<Hygia.FaultManagement.Domain.Fault>(id)
                .ToOutputModel()
                .AsResourceItem()
                .AddLinks(_links);
        }
    }
}