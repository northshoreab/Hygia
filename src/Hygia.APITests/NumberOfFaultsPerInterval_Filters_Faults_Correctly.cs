using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Hygia.API;
using Hygia.API.Controllers.FaultManagement.Statistics;
using Hygia.API.Models.FaultManagement.Statistics;
using Hygia.FaultManagement.Domain;
using Machine.Specifications;

namespace Hygia.APITests
{
    [Subject("Api")]
    public class NumberOfFaultsPerInterval_Filters_Faults_Correctly : ApiContext
    {
        private static ResponseItem<IEnumerable<FaultsPerInterval>> faultsPerInterval;
        private static NumberOfFaultsPerIntervalController controller;

        Establish context = () =>
                                {
                                    var session = DocumentStore.OpenSession();

                                    session.Store(new Fault { TimeOfFailure = DateTime.Now });
                                    session.Store(new Fault { TimeOfFailure = DateTime.Now.AddMinutes(-30) });
                                    session.Store(new Fault { TimeOfFailure = DateTime.Now.AddMinutes(-80) });

                                    session.SaveChanges();

                                    controller = new NumberOfFaultsPerIntervalController(session);

                                    controller.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:61000/api/faultmanagement/statistics/numberoffaultsperinterval");

                                    controller.Configuration = new System.Web.Http.HttpConfiguration(new System.Web.Http.HttpRouteCollection());
                                };

        Because of = () =>
        {
            var now = DateTime.Now;
            faultsPerInterval = controller.Get(new IntervalInputModel
                                                   {
                                                       From = now.AddHours(-1),
                                                       To = now,
                                                       Interval = new TimeSpan(0, 0, 30, 0)
                                                   });
        };

        It should_return_two_intervals = () => faultsPerInterval.Data.Count().ShouldEqual(2);

        It should_return_one_fault_per_interval = () => faultsPerInterval.Data.Where(x => x.NumberOfFaults == 1).Count().ShouldEqual(2);
    }
}
