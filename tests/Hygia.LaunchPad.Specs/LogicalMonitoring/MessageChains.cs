namespace Hygia.LaunchPad.Specs.LogicalMonitoring
{
    using System;
    using Hygia.Specs;
    using Machine.Specifications;
    using PhysicalMonitoring.Commands;
    using PhysicalMonitoring.Domain;
    using PhysicalMonitoring.Handlers;

    [Subject("Message chains")]
    public class When_creating_a_new_chain : WithHandler<StartMessageChainHandler>
    {
        static Guid chainId = Guid.NewGuid();

        Establish context = () => {
        };

        Because of = () => Handle<StartMessageChain>(m => m.MessageChainId = chainId);


        It should_store_a_new_chain_in_raven = () => Session.Load<MessageChain>(chainId.ToString()).ShouldNotBeNull();

    }

}
