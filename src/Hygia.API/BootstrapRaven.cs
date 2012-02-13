using System;
using System.Collections.Generic;
using FubuMVC.Core.Runtime;
using Hygia.API.Models;
using NServiceBus;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Graph;

namespace Hygia.API
{
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
}