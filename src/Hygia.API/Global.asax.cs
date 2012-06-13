using System;
using System.Web;
using System.Web.Http;
using Hygia.API.App_Start;
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

        }

        void Configure(HttpConfiguration configuration)
        {
            configuration.DependencyResolver = new StructureMapResolver(ObjectFactory.Container);
            configuration.MessageHandlers.Add(new CommandsToPickUpHandler(ObjectFactory.Container));
            configuration.MessageHandlers.Add(new ApiRequestHandler(ObjectFactory.Container));

            configuration.MessageHandlers.Add(new RavenSessionHandlerHandler(ObjectFactory.Container));
            //this one needs to be registered last in order to be invoked first
            configuration.MessageHandlers.Add(new TransactionScopeHandler());
        }
    }
}