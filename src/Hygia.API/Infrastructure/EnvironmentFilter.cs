using System;
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
            Guid environment;

            if (!Guid.TryParse(actionContext.ActionArguments["environment"] as string, out environment))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

            var controller = actionContext.ControllerContext.Controller as EnvironmentController;

            if (controller == null)
                return;

            controller.Environment = environment;

            var apiRequest = _container.GetInstance<IApiRequest>();

            apiRequest.EnvironmentId = controller.Environment.ToString();

            controller.Session = _documentStore.OpenSession(environment.ToString());
            controller.Bus = _bus;
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