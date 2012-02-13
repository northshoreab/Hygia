using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using Raven.Client;

namespace Hygia.Web.Behaviors
{    
    public class PersistenceBehavior : BasicBehavior
    {
        private IDocumentSession _session;

        public PersistenceBehavior(IDocumentSession documentSession) : base(PartialBehavior.Ignored)
        {
            _session = documentSession;
        }

        protected override void afterInsideBehavior()
        {
            _session.SaveChanges();
            _session.Dispose();
        }        
    }

    public class PersistenceConvention : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            graph.Actions()
                .Each(actionCall => actionCall.AddBefore(Wrapper.For<PersistenceBehavior>()));
        }
    }
}