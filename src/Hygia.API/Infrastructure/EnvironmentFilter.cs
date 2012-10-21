using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Hygia.API.Controllers;
using NServiceBus;
using Raven.Client;
using StructureMap;

namespace Hygia.API.Infrastructure
{
    public class EnvironmentFilter : ActionFilterAttribute
    {
        private readonly IContainer _container;
        readonly IDocumentStore _documentStore;
        readonly IBus _bus;

        public EnvironmentFilter(IContainer container)
        {
            _container = container;
            _documentStore = container.GetInstance<IDocumentStore>();
            _bus = container.GetInstance<IBus>();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controller = actionContext.ControllerContext.Controller as EnvironmentController;

            if (controller == null)
                return;

            HttpRequestMessage request = actionContext.ControllerContext.Request;
            var segments = request.RequestUri.Segments.ToList();

            if (segments.All(x => x != "environments/") || segments.IndexOf("environments/") > segments.Count)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

            controller.Environment = Guid.Parse(segments[segments.IndexOf("environments/") + 1].Replace("/",""));

            var apiRequest = _container.GetInstance<IApiRequest>();

            apiRequest.EnvironmentId = controller.Environment.ToString();

            controller.Session = _documentStore.OpenSession(controller.Environment.ToString());
            controller.Bus = _bus;

            //TODO: Add authorization environment
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var controller = actionExecutedContext.ActionContext.ControllerContext.Controller as EnvironmentController;

            if (controller == null)
                return;

            controller.Session.SaveChanges();
            controller.Session.Dispose();
        }
    }
}