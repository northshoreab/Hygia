namespace Hygia.Backend
{
    using System;
    using Core;
    using StructureMap;
    using StructureMap.Configuration.DSL;

    public class ProviderRegistry : Registry
    {
        public ProviderRegistry()
        {

            ObjectFactory.Configure(c => c.Scan(s =>
                                                    {
                                                        s.AssembliesFromApplicationBaseDirectory();
                                                        s.Include(t=>typeof(IProvide).IsAssignableFrom(t));
                                                        s.RegisterConcreteTypesAgainstTheFirstInterface();
                                                    }));
        }
    }
}