namespace Hygia.IntegrationTests.Operations.Faults
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Hygia.Drivers.NServiceBus;
    using Machine.Specifications;

    public class When_a_email_arrives_with_a_fault_that_should_be_retried:BackendContext
    {
        static Guid faultId = Guid.NewGuid();

        Establish context = () =>
        {
            message = new NServiceBusMessage()
                .AddMessage("Hygia.Operations.Events.EmailReceived","1.0.0.0",new
                                                                        {
                                                                              To = string.Format("{0}+faults-{1}@watchr.se",apiKey,faultId),
                                                                              From ="whatever",
                                                                              Subject = "Subject",
                                                                              Body = "Retry"
                                                                        });

            message.Headers["EnvironmentId"] = apiKey.ToString();
        };


        Because of = () => InjectMessage();

        It should_store_a_command_for_the_launchpad_to_pickup = () => true.ShouldBeTrue() ;    
    }

    public class When_a_email_arrives_with_a_fault_that_should_be_archived : BackendContext
    {
        static Guid faultId = Guid.NewGuid();

        Establish context = () =>
        {
            using(var session = Store.OpenSession())
            {
                var fault = new { Status = "New" };
                session.Store(fault, "Faults/" + faultId.ToString());

                var metadata = session.Advanced.GetMetadataFor(fault);

                metadata["Raven-Entity-Name"] = "Faults";
                //metadata["Raven-Clr-Type"] =
                //    "Hygia.FaultManagement.Domain.Fault, Hygia.FaultManagement.Domain";
                session.SaveChanges();
            }
            message = new NServiceBusMessage()
                .AddMessage("Hygia.Operations.Events.EmailReceived", "1.0.0.0", new
                {
                    To = string.Format("{0}+faults-{1}@watchr.se", apiKey, faultId),
                    From = "whatever",
                    Subject = "Subject",
                    Body = "Archive"
                });

            message.Headers["EnvironmentId"] = apiKey.ToString();
        };


        Because of = () => InjectMessage();

        It should_store_a_command_for_the_launchpad_to_pickup = () => true.ShouldBeTrue();
    }
}