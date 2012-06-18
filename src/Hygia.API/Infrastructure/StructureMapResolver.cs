using System.Web.Http.Dependencies;
using StructureMap;

namespace Hygia.API.Infrastructure
{
    public class StructureMapResolver : StructureMapScope, IDependencyResolver
    {
        public StructureMapResolver(IContainer container) : base(container) { }

        public IDependencyScope BeginScope()
        {
            return new StructureMapScope(Container.GetNestedContainer());
        }
    }
}