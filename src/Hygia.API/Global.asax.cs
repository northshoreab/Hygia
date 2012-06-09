using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Hygia.Operations.Communication.Domain;
using Raven.Client;
using StructureMap;

namespace Hygia.API
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //RouteConfig.RegisterRoutes(RouteTable.Routes);

            ApiBootstrapper.Bootstrap();

            //GlobalConfiguration.Configuration.MessageHandlers.Add(new ApiKeyHandler("whatever"));
            Configure(GlobalConfiguration.Configuration);
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //TODO: Snabb hack..
            ObjectFactory.GetInstance<IDocumentSession>().SaveChanges();
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }

        void Configure(HttpConfiguration configuration)
        {
            configuration.DependencyResolver = new StructureMapResolver(ObjectFactory.Container);
            configuration.MessageHandlers.Add(new CommandsToPickUpHandler(ObjectFactory.Container));
            configuration.MessageHandlers.Add(new ApiRequestHandler(ObjectFactory.Container));
        }
    }

    public class StructureMapResolver : StructureMapScope, IDependencyResolver
    {
        public StructureMapResolver(IContainer container) : base(container) { }

        public IDependencyScope BeginScope()
        {
            return this;
        }
    }

    public class StructureMapScope : IDependencyScope
    {
        protected IContainer Container;

        public StructureMapScope(IContainer container)
        {
            Container = container;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.IsAbstract || serviceType.IsInterface)
                return Container.TryGetInstance(serviceType);

            return Container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.GetAllInstances(serviceType).Cast<object>().ToList();
        }

        public void Dispose() { }
    }

    public interface IApiRequest
    {
        string EnvironmentId { get; set; }
        string ApiKey { get; set; }
    }

    public class ApiRequest : IApiRequest
    {
        public string EnvironmentId { get; set; }
        public string ApiKey { get; set; }
    }

    public class ApiRequestHandler : DelegatingHandler
    {
        private readonly IContainer container;

        public ApiRequestHandler(IContainer container)
        {
            this.container = container;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiRequest = container.GetInstance<IApiRequest>();

            apiRequest.EnvironmentId = request.GetEnvironment();            
            apiRequest.ApiKey = request.GetApiKey();

            return base.SendAsync(request, cancellationToken);
        }
    }

    public class CommandsToPickUpHandler : DelegatingHandler
    {
        private readonly IContainer container;

        public CommandsToPickUpHandler(IContainer container)
        {
            this.container = container;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(task =>
                                                                               {
                                                                                   HttpResponseMessage response = task.Result;

                                                                                   if (CommandsAvailable(request))
                                                                                       response.Headers.Add(
                                                                                           "watchr.commandsavailable",
                                                                                           "true");
                                                                                   return response;
                                                                               });

        }

        private bool CommandsAvailable(HttpRequestMessage request)
        {
            string apiKey = null;

            if (request.Headers.Contains("apikey"))
                apiKey = request.Headers.SingleOrDefault(x => x.Key == "apikey").Value.FirstOrDefault();

            //for now assume that an api call always means that the request is coming from a launchpad
            if (apiKey != null && request.RequestUri.AbsolutePath.ToLower().StartsWith("/api/commands/"))
            {
                var session = container.GetInstance<IDocumentSession>();

                if (session.Query<LaunchPadCommand>().Any(c => !c.Delivered))
                    return true;
            }

            return false;
        }
    }

    public static class HttpRequestMessageExtensions
    {
        public static string GetEnvironment(this HttpRequestMessage request)
        {
            if (request.Headers.Contains("environment"))
                return request.Headers.Single(x => x.Key == "environment").Value.FirstOrDefault();

            if (request.Properties.ContainsKey("environment"))
                return request.Properties["environment"] as string;

            if (request.GetRouteData().Values.ContainsKey("environment"))
                return request.GetRouteData().Values["environment"] as string;

            var cookies = request.Headers.GetCookies().SelectMany(x => x.Cookies);
            var cookie = cookies.SingleOrDefault(x => x.Name == "environment");

            return cookie != null ? cookie.Value : null;
        }

        public static string GetApiKey(this HttpRequestMessage request)
        {
            return request.Headers.Single(x => x.Key == "apikey").Value.FirstOrDefault();
        }
    }
}