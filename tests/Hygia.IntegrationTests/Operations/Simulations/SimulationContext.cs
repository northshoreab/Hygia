namespace Hygia.IntegrationTests.Operations.Simulations
{
    using System;
    using System.Threading;
    using Drivers.NServiceBus;
    using Machine.Specifications;

    public class SimulationContext
    {
        protected static Func<TimeSpan> GetSleepTime = () => TimeSpan.FromSeconds(1);


        protected static Func<int,NServiceBusMessage> GetMessage = (iteration) =>
                                                                   {
                                                                       throw new NotImplementedException(
                                                                           "You need to set the get message func");
                                                                   };

        protected static int iterations = 1;
        
        static int iteration;
        
        static Injector injector = new Injector();
        
        protected static Func<string> TargetQueue = ()=>"audit";


        Because of = () =>
        {
            while (!Done())
            {
          
                injector.Inject(GetMessage(iteration),TargetQueue());

                Thread.Sleep(GetSleepTime());
            }
        };


        static bool Done()
        {
            return iteration++ >= iterations;
        }

    }
}