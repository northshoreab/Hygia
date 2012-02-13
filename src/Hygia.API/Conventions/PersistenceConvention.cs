using System.Collections.Generic;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using Hygia.API.Behaviors;

namespace Hygia.API.Conventions
{
    public class PersistenceConvention : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            graph.Actions()
                .Each(actionCall => actionCall.AddBefore(Wrapper.For<PersistenceBehavior>()));
        }
    }
}