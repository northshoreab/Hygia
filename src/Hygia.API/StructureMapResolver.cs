namespace Hygia.API
{
    using System.Web.Http.Dependencies;
    using StructureMap;

    public class StructureMapResolver : StructureMapScope, IDependencyResolver
    {
        public StructureMapResolver(IContainer container) : base(container) { }

        public IDependencyScope BeginScope()
        {
            return new StructureMapScope(Container.GetNestedContainer());
        }
    }
}