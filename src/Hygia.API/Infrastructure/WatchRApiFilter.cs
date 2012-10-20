using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Hygia.API.Controllers;
using NServiceBus;
using Raven.Client;
using StructureMap;

namespace Hygia.API.Infrastructure
{
    public class WatchRApiFilter : ActionFilterAttribute
    {
        readonly IDocumentStore _documentStore;
        readonly IBus _bus;

        public WatchRApiFilter(IContainer container)
        {
            _documentStore = container.GetInstance<IDocumentStore>();
            _bus = container.GetInstance<IBus>();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controller = actionContext.ControllerContext.Controller as WatchRApiController;

            if (controller == null)
                return;

            controller.Session = _documentStore.OpenSession();
            controller.Bus = _bus;

            //TODO: Add authorization for user
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var controller = actionExecutedContext.ActionContext.ControllerContext.Controller as WatchRApiController;

            if (controller == null)
                return;

            controller.Session.SaveChanges();
            controller.Session.Dispose();
        }
    }
}