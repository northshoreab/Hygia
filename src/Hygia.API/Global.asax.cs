using System;
using System.Web;
using System.Web.Http;
using Hygia.API.App_Start;
using Hygia.API.Infrastructure;
using Hygia.API.Infrastructure.Authentication;
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

        void Configure(HttpConfiguration configuration)
        {
            configuration.Filters.Add(new EnvironmentFilter(ObjectFactory.Container));

            configuration.DependencyResolver = new StructureMapResolver(ObjectFactory.Container);
            configuration.MessageHandlers.Add(new CommandsToPickUpHandler(ObjectFactory.Container));
            configuration.MessageHandlers.Add(new RavenSessionHandler(ObjectFactory.Container));
            configuration.MessageHandlers.Add(new MetadataHandler());

            //this needs to be registered after the securityconfig AuthenticationHandler in order to be invoked before it.
            configuration.MessageHandlers.Add(new GitHubLoginHandler());

            //this one needs to be registered last in order to be invoked first
            configuration.MessageHandlers.Add(new TransactionScopeHandler());
        }
    }
}