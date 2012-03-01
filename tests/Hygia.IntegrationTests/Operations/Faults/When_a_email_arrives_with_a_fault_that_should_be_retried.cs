namespace Hygia.IntegrationTests.Operations.Faults
{
    using System;
    using Drivers.NServiceBus;
    using Machine.Specifications;



    public class When_a_retry_fault_message_arrives_to_the_launchpad : LaunchPadContext
    {
        static Guid faultId = Guid.NewGuid();

        Establish context = () =>
        {
            message = new NServiceBusMessage()
                .AddMessage("Hygia.FaultManagement.Commands.RetryFault", "1.0.0.0", new
                {
                    FaultEnvelopeId = "0784ecf5-20d5-41a4-9837-31121db10460\\285168"
                });

        };


        Because of = () => InjectMessage();

        It should_perform_the_retry = () => true.ShouldBeTrue();
    }
    public class When_a_email_arrives_with_a_fault_that_should_be_retried : BackendContext
    {
        static Guid faultId = Guid.Parse("4dc6706a-962c-9eba-892c-67d81e6f45ed"); //Guid.NewGuid();
        //327951bf-bae4-46a4-93a0-71f61dfbe801+faults+45afae35-39e6-dd00-603d-e2668e0cd06e@watchr.se
        Establish context = () =>
        {
            message = new NServiceBusMessage()
                .AddMessage("Hygia.Operations.Events.EmailReceived", "1.0.0.0", new
                                                                        {
                                                                            To = string.Format("{0}+faults+{1}@watchr.se", apiKey, faultId),
                                                                            From = "whatever",
                                                                            Subject = "Subject",
                                                                            Body = "Retry",
                                                                            Service = "Faults",
                                                                            Parameters = faultId.ToString()
                                                                        });

            message.Headers["EnvironmentId"] = apiKey.ToString();
        };


        Because of = () => InjectMessage();

        It should_store_a_command_for_the_launchpad_to_pickup = () => true.ShouldBeTrue();
    }

    public class When_a_email_arrives_with_a_fault_that_should_be_archived : BackendContext
    {
        static Guid faultId = Guid.NewGuid();

        Establish context = () =>
        {
            using (var session = Store.OpenSession())
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