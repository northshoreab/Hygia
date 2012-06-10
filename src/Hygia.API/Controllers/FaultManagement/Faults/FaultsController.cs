using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Models.FaultManagement.Faults;
using Raven.Client;
using Hygia.FaultManagement.Domain;
using Fault = Hygia.API.Models.FaultManagement.Faults.Fault;

namespace Hygia.API.Controllers.FaultManagement.Faults
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/faultmanagement/faults")]
    public class FaultsController : ApiController
    {
        private readonly IDocumentSession _session;
       
        public FaultsController(IDocumentSession session)
        {
            links = fault => new List<Link>
                                 {
                                     new Link
                                         {
                                             Href = "/api/faults/" + fault.FaultId + "/archive", 
                                             Rel = "archive"
                                         },
                                     new Link
                                         {
                                             Href = "/api/faults/" + fault.FaultId + "/retry",
                                             Rel = "retry"
                                         },
                                     new Link
                                         {
                                             Href = "/api/faults/" + fault.FaultId + "/retried",
                                             Rel = "retried"
                                         },
                                 };
            _session = session;
        }

        private readonly Func<Fault, IEnumerable<Link>> links;

        public IEnumerable<ResponseItem<Fault>> GetAll()
        {
            return _session.Query<Hygia.FaultManagement.Domain.Fault>()
                .Where(f => f.Status != FaultStatus.Archived && f.Status != FaultStatus.Resolved && f.Status != FaultStatus.RetryPerformed)
                .OrderByDescending(f=>f.TimeOfFailure)
                .ToList()
                .ToOutputModels()
                .Select(x => x.AsResponseItem().AddLinks(links));
        }
        
        public ResponseItem<Fault> Get(Guid id)
        {
            return _session.Load<Hygia.FaultManagement.Domain.Fault>(id)
                .ToOutputModel()
                .AsResponseItem()
                .AddLinks(links);
        }
    }
}