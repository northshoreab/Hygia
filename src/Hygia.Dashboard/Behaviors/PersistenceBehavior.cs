using FubuMVC.Core.Behaviors;
using Raven.Client;

namespace Hygia.Widgets.Behaviors
{
    public class PersistenceBehavior : BasicBehavior
    {
        private readonly IDocumentSession _session;

        public PersistenceBehavior(IDocumentSession session)
            : base(PartialBehavior.Ignored)
        {
            _session = session;
        }

        protected override void afterInsideBehavior()
        {
            _session.SaveChanges();
            _session.Dispose();
        }
    }
}