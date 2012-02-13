using System;
using System.Collections.Generic;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using Hygia.API.Models;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
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
            
            IncludeDiagnostics(true);

            Applies
                .ToThisAssembly()
                .ToAssembly("Hygia.Operations.AuditUploads.Api"); //todo- Better way?

            // All public methods from concrete classes ending in "Controller"
            // in this assembly are assumed to be action methods
            Actions.IncludeClassesSuffixedWithController();

           

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

    public class RavenDatabaseSelectBehavior : BasicBehavior
    {
        private readonly IFubuRequest _fubuRequest;
        private IDocumentStore Store { get; set; }

        public RavenDatabaseSelectBehavior(IFubuRequest fubuRequest) : base(PartialBehavior.Executes)
        {
            _fubuRequest = fubuRequest;
        }

        protected override DoNext performInvoke()
        {
            var p = _fubuRequest.Get<ModelBase>();

            Store.OpenSession(p.Environment);
            return base.performInvoke();
        }
    }

    //public class UnitOfWorkBehavior : IActionBehavior
    //{
    //    private readonly IDocumentStore _store;
    //    private readonly IFubuRequest _fubuRequest;
    //    private readonly IDocumentSession _session = null;
    //    public IActionBehavior InnerBehavior { get; set; }

    //    public UnitOfWorkBehavior(IDocumentStore store, IFubuRequest fubuRequest)
    //    {
    //        _store = store;
    //        _fubuRequest = fubuRequest;
    //    }

    //    public void Invoke()
    //    {
    //        _fubuRequest.Set(
    //            new Lazy<IDocumentSession>(
    //                () => _session ??
    //                (_fubuRequest.Has<ModelBase>()
    //                    ? _store.OpenSession(_fubuRequest.Get<ModelBase>().Environment)
    //                    : _store.OpenSession())));

    //        InnerBehavior.Invoke();

    //        _session.SaveChanges();
    //    }

    //    public void InvokePartial()
    //    {
    //        InnerBehavior.InvokePartial();
    //    }
    //}

    //class BootstrapRaven : IWantCustomInitialization
    //{
    //    public void Init()
    //    {
    //        environmentIdToDatabaseLookup.Add(Guid.Parse("327951bf-bae4-46a4-93a0-71f61dfbe801"), "Hygia.Acme");
    //        var store = new DocumentStore
    //        {
    //            Url = "http://localhost:8080",
    //            DefaultDatabase = Configure.EndpointName

    //        };

    //        store.Initialize();

    //        ObjectFactory.Configure(c =>
    //        {
    //            c.ForSingletonOf<IDocumentStore>()
    //                .Use(store);
    //            c.For<IDocumentSession>()
    //                .Use(OpenSession);

    //            PluginCache.AddFilledType(typeof(IDocumentSession));

    //            c.For<IManageUnitsOfWork>()
    //                .Use<RavenUnitOfWork>();
    //        });
    //    }

    //    static IDocumentSession OpenSession(IFubuRequest request)
    //    {
    //        var bus = ctx.GetInstance<IBus>();
    //        string database = null;

    //        if (bus.CurrentMessageContext != null && bus.CurrentMessageContext.Headers.ContainsKey("EnvironmentId"))
    //            database = environmentIdToDatabaseLookup[Guid.Parse(bus.CurrentMessageContext.Headers["EnvironmentId"])];

    //        var s = ctx.GetInstance<IDocumentStore>();

    //        if (string.IsNullOrEmpty(database))
    //            return s.OpenSession();

    //        return s.OpenSession(database);
    //    }

    //    static readonly IDictionary<Guid, string> environmentIdToDatabaseLookup = new Dictionary<Guid, string>();
    //}
}