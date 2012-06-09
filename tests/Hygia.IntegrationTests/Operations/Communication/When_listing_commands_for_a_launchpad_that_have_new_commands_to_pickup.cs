namespace Hygia.IntegrationTests.Operations.Communication
{
    using System;
    using Machine.Specifications;
    using RestSharp;

    [Subject("Commands")]
    public class When_listing_commands_for_a_launchpad_that_have_new_commands_to_pickup : CommandsContext
    {
        Establish context = () =>
                                {
                                    request = new RestRequest("api/operations/launchpad/commands", Method.GET) { RequestFormat = DataFormat.Json };

                                    request.AddHeader("apikey", apiKey.ToString());

                                    StoreCommand();

                                };
       

        It should_return_the_commands = () => response.Content.ShouldContain(commandId.ToString());
    }


    [Subject("Commands")]
    public class When_marking_a_command_as_picked_up : CommandsContext
    {
        Establish context = () =>
        {
            request = new RestRequest("api/operations/launchpad/commands", Method.POST) { RequestFormat = DataFormat.Json };

            request.AddHeader("apikey", apiKey.ToString());
            request.AddBody(new
                                {
                                    CommandId = commandId
                                });

            StoreCommand();

        };



        It should_not_return_the_commands = () => response.Content.ShouldNotContain(commandId.ToString());
    }

    public class CommandsContext:ApiContext
    {
        protected static Guid commandId = Guid.NewGuid();
        protected static Guid apiKey = Guid.Parse("327951bf-bae4-46a4-93a0-71f61dfbe801");


        protected static void StoreCommand()
        {
            using (var session = Store.OpenSession())
            {
                var command = new { Delivered = false };
                session.Store(command, "LaunchPadCommands/" + commandId.ToString());

                var metadata = session.Advanced.GetMetadataFor(command);

                metadata["Raven-Entity-Name"] = "LaunchPadCommands";
                metadata["Raven-Clr-Type"] =
                    "Hygia.Operations.Communication.Domain.LaunchPadCommand, Hygia.Operations.Communication.Domain";
                session.SaveChanges();
            }
        }

    }
}