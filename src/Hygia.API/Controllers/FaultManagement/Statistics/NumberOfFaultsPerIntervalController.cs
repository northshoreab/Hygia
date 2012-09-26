using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Models.FaultManagement.Statistics;
using Hygia.FaultManagement.Domain;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace Hygia.API.Controllers.FaultManagement.Statistics
{
    public class NumberOfFaultsPerHour : AbstractIndexCreationTask<Fault, FaultsPerInterval>
    {
        public NumberOfFaultsPerHour()
        {
            Map = items => items.Select(x => new
                                                 {
                                                     From =
                                                 new DateTime(x.TimeOfFailure.Year, x.TimeOfFailure.Month,
                                                              x.TimeOfFailure.Day, x.TimeOfFailure.Hour, 0, 0),
                                                     To = DateTime.MinValue,
                                                     NumberOfFaults = 1
                                                 });

            Reduce = results => results.GroupBy(x => x.From)
                                    .Select(x => new
                                                     {
                                                         From = x.Key,
                                                         To = x.Key.AddMinutes(59).AddSeconds(59),
                                                         NumberOfFaults = x.Sum(s => s.NumberOfFaults)
                                                     });
        }
    }

    public class NumberOfFaultsPerDay : AbstractIndexCreationTask<Fault, FaultsPerInterval>
    {
        public NumberOfFaultsPerDay()
        {
            Map = items => items.Select(x => new
                                                 {
                                                     From =
                                                 new DateTime(x.TimeOfFailure.Year, x.TimeOfFailure.Month,
                                                              x.TimeOfFailure.Day),
                                                     To = DateTime.MinValue,
                                                     NumberOfFaults = 1
                                                 });

            Reduce = results => results.GroupBy(x => x.From)
                                    .Select(x => new
                                                     {
                                                         From = x.Key,
                                                         To = x.Key.AddHours(23).AddMinutes(59).AddSeconds(59),
                                                         NumberOfFaults = x.Sum(s => s.NumberOfFaults)
                                                     });
        }
    }

    [DefaultHttpRouteConvention]
    [RoutePrefix("api/environments/{environment:guid}/faultmanagement/statistics/numberoffaultsperinterval")]
    [Authorize]
    public class NumberOfFaultsPerIntervalController : EnvironmentController
    {
        public IQueryable<FaultsPerInterval> Get(IntervalInputModel model)
        {
            IList<DateTime> starts = new List<DateTime>();

            DateTime next = model.From;
            int counter = 0;
            
            while(next <= model.To || counter >= 400)
            {
                starts.Add(next);

                switch (model.Interval)
                {
                    case Interval.Hour:
                        next = next.AddHours(1);
                        break;
                    case Interval.Day:
                        next = next.AddDays(1);
                        break;
                    case Interval.Week:
                        next = next.AddDays(7);
                        break;
                    case Interval.Month:
                        next = next.AddMonths(1);
                        break;
                }

                counter ++;
            }

            IQueryable<FaultsPerInterval> faultsPerIntervals;

            if (model.Interval == Interval.Hour)
                faultsPerIntervals = Session.Query<FaultsPerInterval, NumberOfFaultsPerHour>()
                    .Where(x => x.From >= model.From && x.From <= starts.Max())
                    .ToList()
                    .GroupBy(x => x.From)
                    .Select(x => new FaultsPerInterval
                                     {
                                         From = x.Key,
                                         To = GetToDateTime(x.Key, model.Interval),
                                         NumberOfFaults = x.Count()
                                     })
                    .AsQueryable();
            else
                faultsPerIntervals = Session.Query<FaultsPerInterval, NumberOfFaultsPerDay>()
                    .Where(x => x.From >= model.From && x.From <= starts.Max())
                    .ToList()
                    .GroupBy(x => x.From)
                    .Select(x => new FaultsPerInterval
                                     {
                                         From = x.Key,
                                         To = GetToDateTime(x.Key, model.Interval),
                                         NumberOfFaults = x.Count()
                                     })
                    .AsQueryable();

            return faultsPerIntervals;
        }

        private DateTime GetToDateTime(DateTime start, Interval interval)
        {
            switch (interval)
            {
                case Interval.Hour:
                    return start.AddHours(1).AddMilliseconds(-1);
                case Interval.Day:
                    return start.AddDays(1).AddMilliseconds(-1);
                case Interval.Week:
                    return start.AddDays(7).AddMilliseconds(-1);
                case Interval.Month:
                    return start.AddMonths(1).AddMilliseconds(-1);
            }

            throw new ArgumentException("Interval does not exist", "interval");
        }
    }

    public enum Interval
    {
        Hour,
        Day,
        Week,
        Month
    }

    public class IntervalInputModel
    {
        public Interval Interval { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}