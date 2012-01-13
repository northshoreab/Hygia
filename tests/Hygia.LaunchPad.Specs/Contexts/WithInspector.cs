namespace Hygia.LaunchPad.Specs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Inspectors;
    using Machine.Specifications;
    using NServiceBus.Unicast.Transport;

    public class WithInspector<T> where T : IInspectEnvelopes, new()
    {
        protected static IInspectEnvelopes Inspector;
        protected static IEnumerable<object> Results;

        Establish context = () => {
                                      Inspector = new T();
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