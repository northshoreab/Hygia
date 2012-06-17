using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Hygia.API;
using Hygia.API.Controllers.FaultManagement.Statistics;
using Hygia.API.Models.FaultManagement.Statistics;
using Hygia.FaultManagement.Domain;
using Machine.Specifications;
using Raven.Bundles.Authentication;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace Hygia.APITests
{
    [Subject("Api")]
    public class AddUser
    {
        private Establish context = () => { };

        private Because of = () =>
                                 {
                                     var store = new DocumentStore {DefaultDatabase = "WatchR", Url = "http://localhost:8080"}.Initialize();

                                     var session = store.OpenSession();

                                     var user = new AuthenticationUser
                                                    {
                                                        Admin = false,
                                                        AllowedDatabases = new[] {"*"},
                                                        Name = "Test"
                                                    };

                                     user.SetPassword("test");
                                     session.Store(user);
                                     session.SaveChanges();
                                 };

        It should = () => {};
    }

    [Subject("Api")]
    public class NumberOfFaultsPerInterval_Filters_Faults_Correctly : ApiContext
    {
        private static IQueryable<FaultsPerInterval> faultsPerInterval;
        private static NumberOfFaultsPerIntervalController controller;

        Establish context = () =>
                                {
                                    var session = DocumentStore.OpenSession();

                                    session.Store(new Fault { TimeOfFailure = DateTime.Now });
                                    session.Store(new Fault { TimeOfFailure = DateTime.Now.AddHours(-1) });
                                    session.Store(new Fault { TimeOfFailure = DateTime.Now.AddHours(-2) });

                                    session.SaveChanges();

                                    //TODO: Finns nåt bättre sätt att hantera det här i test?
                                    while (((EmbeddableDocumentStore)DocumentStore).DocumentDatabase.Statistics.StaleIndexes.Length != 0)
                                        Thread.Sleep(10);

                                    controller = new NumberOfFaultsPerIntervalController(session);

                                    controller.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:61000/api/faultmanagement/statistics/numberoffaultsperinterval");

                                    controller.Configuration = new System.Web.Http.HttpConfiguration(new System.Web.Http.HttpRouteCollection());
                                };

        Because of = () =>
        {
            var now = DateTime.Now;
            faultsPerInterval = controller.Get(new IntervalInputModel
                                                   {
                                                       From = now.AddHours(-2),
                                                       To = now,
                                                       Interval = Interval.Hour
                                                   });
        };

        It should_return_two_intervals = () => faultsPerInterval.Count().ShouldEqual(2);

        It should_return_one_fault_per_interval = () => faultsPerInterval.Where(x => x.NumberOfFaults == 1).Count().ShouldEqual(2);
    }
}
