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
    public class UserAccountFilter : ActionFilterAttribute
    {
        readonly IDocumentStore _documentStore;
        readonly IBus _bus;

        public UserAccountFilter(IContainer container)
        {
            _documentStore = container.GetInstance<IDocumentStore>();
            _bus = container.GetInstance<IBus>();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controller = actionContext.ControllerContext.Controller as UserAccountController;

            if (controller == null)
                return;

            Guid userAccountId;

            if (!Guid.TryParse(actionContext.ActionArguments["user"] as string, out userAccountId))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

            controller.UserAccountId = userAccountId;

            controller.Session = _documentStore.OpenSession();
            controller.Bus = _bus;

            //TODO: Add authorization for user
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var controller = actionExecutedContext.ActionContext.ControllerContext.Controller as UserAccountController;

            if (controller == null)
                return;

            controller.Session.SaveChanges();
            controller.Session.Dispose();
        }
    }
}