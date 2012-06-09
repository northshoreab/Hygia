using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Models.FaultManagement.Statistics;
using Hygia.FaultManagement.Domain;
using Raven.Client;
using Raven.Client.Linq;

namespace Hygia.API.Controllers.FaultManagement.Statistics
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/faultmanagement/statistics/numberoffaultsperinterval")]
    public class NumberOfFaultsPerIntervalController : ApiController
    {
        private readonly IDocumentSession session;

        public NumberOfFaultsPerIntervalController(IDocumentSession session)
        {
            this.session = session;
        }

        public ResponseItem<IEnumerable<FaultsPerInterval>> Get(IntervalInputModel model)
        {
            IList<DateTime> starts = new List<DateTime>();

            DateTime next = model.From;
            int counter = 0;

            while(next <= model.To || counter >= 20)
            {
                starts.Add(next);
                next = next.Add(model.Interval);
                counter ++;
            }
            //TODO: Implement map reduce for grouping
            return session.Query<Fault>()
                .Where(x => x.TimeOfFailure >= model.From && x.TimeOfFailure <= starts.Max())
                .Select(s => new
                                 {
                                     MesageType = s.ContainedMessages.First(),
                                     From = starts.Single(x => s.TimeOfFailure >= x && s.TimeOfFailure < x.Add(model.Interval))
                                 })
                .GroupBy(x => x.From)
                .Select(x => new FaultsPerInterval
                                 {
                                     From = x.Key,
                                     To = x.Key.Add(model.Interval),
                                     NumberOfFaults = x.Count()
                                 })
                .AsEnumerable()
                .AsResponseItem();
        }
    }

    public class IntervalInputModel
    {
        public TimeSpan Interval { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}