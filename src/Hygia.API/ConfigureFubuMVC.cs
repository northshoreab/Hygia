using System;
using System.Collections.Generic;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using Hygia.API.Models;
using NServiceBus;
using Raven.Client;
using Raven.Client.Document;
using StructureMap.Graph;

namespace Hygia.API
{
    using StructureMap;

    public class ConfigureFubuMVC : FubuRegistry
    {
        public ConfigureFubuMVC()
        {
            NServiceBus.Configure
               .WithWeb()
               .HygiaMessageConventions()
               .DefineEndpointName("Hygia.API")
               .StructureMapBuilder(ObjectFactory.Container)
               .XmlSerializer()
               .MsmqTransport()
               .DontUseTransactions()
               .UnicastBus()
               .SendOnly();  // This line turns on the basic diagnostics and request tracing                        

            new BootstrapRaven().Init();

            IncludeDiagnostics(true);

            Applies
                .ToThisAssembly()
                .ToAssembly("Hygia.Operations.AuditUploads.Api"); //todo- Better way?

            // All public methods from concrete classes ending in "Controller"
            // in this assembly are assumed to be action methods
            Actions.IncludeClassesSuffixedWithController();

            ApplyConvention<PersistenceConvention>();

            // Policies
            Routes
                .IgnoreControllerNamesEntirely()
                .IgnoreMethodSuffix("Html")
                .RootAtAssemblyNamespace();

            // Match views to action methods by matching
            // on model type, view name, and namespace
            Views.TryToAttachWithDefaultConventions();
        }

    }

    class BootstrapRaven
    {
        public void Init()
        {
            environmentIdToDatabaseLookup.Add(Guid.Parse("327951bf-bae4-46a4-93a0-71f61dfbe801"), "Hygia.Acme");
            var store = new DocumentStore
                            {
                                Url = "http://localhost:8080",
                                DefaultDatabase = Configure.EndpointName
                            };

            store.Initialize();

            ObjectFactory.Configure(c =>
                                        {
                                            c.ForSingletonOf<IDocumentStore>()
                                                .Use(store);
                                            c.For<IDocumentSession>()
                                                .HybridHttpOrThreadLocalScoped()
                                                .Use(OpenSession);

                                            PluginCache.AddFilledType(typeof (IDocumentSession));
                                        });
        }

        static IDocumentSession OpenSession(IContext ctx)
        {
            var fubuRequest = ctx.GetInstance<IFubuRequest>();

            string database = null;

            var modelBase = fubuRequest.Get<ModelBase>();

            if (modelBase != null)
            {
                try
                {
                    database = environmentIdToDatabaseLookup[Guid.Parse(modelBase.Environment)];
                }
                catch (Exception)
                {
                    throw new Exception("No environment exists for:" + modelBase.Environment);
                }                
            }

            var s = ctx.GetInstance<IDocumentStore>();

            if (string.IsNullOrEmpty(database))
                return s.OpenSession();

            return s.OpenSession(database);
        }

        static readonly IDictionary<Guid, string> environmentIdToDatabaseLookup = new Dictionary<Guid, string>();
    }

    public class PersistenceConvention : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            graph.Actions()
                .Each(actionCall => actionCall.AddBefore(Wrapper.For<PersistenceBehavior>()));
        }
    }

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