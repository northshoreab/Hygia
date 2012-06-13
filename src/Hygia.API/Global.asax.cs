using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Hygia.API.App_Start;
using Hygia.Operations.Communication.Domain;
using Raven.Client;
using StructureMap;

namespace Hygia.API
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            SecurityConfig.ConfigureGlobal(GlobalConfiguration.Configuration);

            ApiBootstrapper.Bootstrap();

            Configure(GlobalConfiguration.Configuration);
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //TODO: Snabb hack..
            ObjectFactory.GetInstance<IDocumentSession>().SaveChanges();
            
            //todo: don't think we need this since I added nested containers + dispose to the dependency scope?
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }

        void Configure(HttpConfiguration configuration)
        {
            configuration.DependencyResolver = new StructureMapResolver(ObjectFactory.Container);
            configuration.MessageHandlers.Add(new CommandsToPickUpHandler(ObjectFactory.Container));
            configuration.MessageHandlers.Add(new ApiRequestHandler(ObjectFactory.Container));
            
            //this one needs to be registered last in order to be invoked first
            configuration.MessageHandlers.Add(new TransactionScopeHandler());
        }
    }
}