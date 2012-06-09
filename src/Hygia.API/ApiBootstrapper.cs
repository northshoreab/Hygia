using System.Web;
using NServiceBus;
using NServiceBus.Unicast;
using StructureMap;

namespace Hygia.API
{
    public static class ApiBootstrapper
    {
        public static void Bootstrap()
        {
            Configure
               .With(HttpRuntime.BinDirectory)
               .HygiaMessageConventions()
               .DefineEndpointName("Hygia.API")
               .StructureMapBuilder(ObjectFactory.Container)
               .XmlSerializer()
               .MsmqTransport()
               .DontUseTransactions()
               .UnicastBus()
               .SendOnly();

            ObjectFactory.Configure(c =>
                                        {
                                            c.Scan(s =>
                                                       {
                                                           s.LookForRegistries();
                                                           s.AssembliesFromApplicationBaseDirectory();
                                                       });

                                            c.For<IApiRequest>().HybridHttpOrThreadLocalScoped().Use<ApiRequest>();
                                            c.For<IBus>().Use<UnicastBus>();
                                        });
        }
    }
}