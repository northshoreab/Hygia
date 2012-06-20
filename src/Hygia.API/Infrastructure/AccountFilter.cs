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
    public class AccountFilter : ActionFilterAttribute
    {
        private readonly IContainer _container;
        readonly IDocumentStore _documentStore;
        readonly IBus _bus;

        public AccountFilter(IContainer container)
        {
            _container = container;
            _documentStore = container.GetInstance<IDocumentStore>();
            _bus = container.GetInstance<IBus>();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controller = actionContext.ControllerContext.Controller as AccountController;

            if (controller == null)
                return;

            Guid account;

            if (!Guid.TryParse(actionContext.ActionArguments["account"] as string, out account))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

            controller.Account = account;

            var apiRequest = _container.GetInstance<IApiRequest>();

            apiRequest.EnvironmentId = controller.Account.ToString();

            controller.Session = _documentStore.OpenSession(account.ToString());
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