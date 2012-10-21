using System;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using Hygia.API.App_Start;
using Hygia.API.Infrastructure;
using Hygia.API.Infrastructure.Authentication;
using Hygia.Spa.App_Start;
using StructureMap;
using Newtonsoft.Json.Serialization;

namespace Hygia.API
{
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            SecurityConfig.ConfigureGlobal(GlobalConfiguration.Configuration);

            ApiBootstrapper.Bootstrap();

            Configure(GlobalConfiguration.Configuration);

            AuthConfig.RegisterAuth();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Configure(HttpConfiguration configuration)
        {
            configuration.Filters.Add(new EnvironmentFilter(ObjectFactory.Container));
            configuration.Filters.Add(new AccountFilter(ObjectFactory.Container));
            configuration.Filters.Add(new UserAccountFilter(ObjectFactory.Container));
            configuration.Filters.Add(new WatchRApiFilter(ObjectFactory.Container));

            configuration.Formatters.Remove(configuration.Formatters.XmlFormatter);
            configuration.DependencyResolver = new StructureMapResolver(ObjectFactory.Container);
            configuration.MessageHandlers.Add(new CommandsToPickUpHandler(ObjectFactory.Container));
            //configuration.MessageHandlers.Add(new RavenSessionHandler(ObjectFactory.Container));
            configuration.MessageHandlers.Add(new MetadataHandler());

            //this needs to be registered after the securityconfig AuthenticationHandler in order to be invoked before it.
            configuration.MessageHandlers.Add(new GitHubLoginHandler());

            //this one needs to be registered last in order to be invoked first
            configuration.MessageHandlers.Add(new TransactionScopeHandler());

            configuration.Services.Add(typeof(IHttpControllerActivator),new CustomHttpControllerFactory(configuration));

            var json = configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
        }
    }


    public class CustomHttpControllerFactory : System.Web.Http.Dispatcher.IHttpControllerActivator
    {
        private readonly HttpConfiguration _configuration;
        private DefaultHttpControllerActivator _defaultHttpControllerFactory;

        public CustomHttpControllerFactory(HttpConfiguration configuration)
        {
            _configuration = configuration;
            _defaultHttpControllerFactory = new DefaultHttpControllerActivator();
        }



        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            if (false)
            {

                return null;
            }
            return _defaultHttpControllerFactory.Create(request, controllerDescriptor, controllerType);

        }
    }
}