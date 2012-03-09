namespace Hygia.Core.Providers
{
    using System.Dynamic;
    using StructureMap;

    public class DefaultInvokeProviderInvoker:IInvokeProviders
    {
        readonly IContainer container;

        public DefaultInvokeProviderInvoker(IContainer container)
        {
            this.container = container;
        }

        public dynamic Invoke<T>(dynamic parameters) where T:IProvide
        {
            //for some wierd reason we ned to convert to and from a expando to avoid blowing up
            //my guess is that is has to do with the way sturcturemap caches instances
            dynamic p = DynamicHelpers.ToDynamic(parameters);

            
            dynamic result = new ExpandoObject();

            foreach (IProvide provider in container.GetAllInstances<T>())
                result = DynamicHelpers.Combine(result, provider.ProvideFor(p));

            return result;
        }
    }
}