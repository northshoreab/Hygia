namespace Hygia.IntegrationTests.Operations.Simulations
{
    using System;
    using Drivers.NServiceBus;
    using Machine.Specifications;

    [Subject("Simulations")]
    public class Order_placed_simulation : SimulationContext
    {

        Establish context = () =>
                                {
                                    GetSleepTime = ()=> TimeSpan.FromSeconds(1);

                                    GetMessage = (iteration) => new NServiceBusMessage()
                                                                    .AddMessage("Sales.OrderPlaced, Sales.Events", "1.0.0.0");
                                    iterations = 1;
                                };




        It should_finish = () => true.ShouldBeTrue();
    }
}