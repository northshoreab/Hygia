namespace Hygia.LaunchPad.Specs.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hygia.LaunchPad.Core;
    using Machine.Specifications;
    using NServiceBus;
    using NServiceBus.Unicast.Transport;

    public class WithInspector<T>
    {
        protected static dynamic Inspector;
        protected static IEnumerable<object> Results;

        Establish context = () => {
                                      Inspector = Activator.CreateInstance<T>();
        };

        
        protected static void MessageInspection(TransportMessage transportMessage)
        {
            Results = Inspector.Inspect(transportMessage);
        }


        protected static void AssertFirst<TCommand>(Func<TCommand, bool> condition) where TCommand : class
        {
            var first = Results.First() as TCommand;

            condition(first).ShouldBeTrue();
        }

    }
}