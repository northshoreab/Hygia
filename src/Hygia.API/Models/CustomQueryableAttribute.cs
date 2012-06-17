using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Hygia.API.Models
{
    //public class ActionFilterTest : ActionFilterAttribute
    //{
    //    public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
    //    {
    //        actionExecutedContext.ActionContext.ControllerContext.Controller.
    //        base.OnActionExecuted(actionExecutedContext);
    //    }
    //}

    public class CustomQueryableAttribute : QueryableAttribute
    {
        protected override IQueryable ApplyResultLimit(HttpActionExecutedContext actionExecutedContext, IQueryable query)
        {
            object responseObject;
            actionExecutedContext.Response.TryGetContentValue(out responseObject);
            var originalquery = responseObject as IQueryable<object>;

            if (originalquery != null)
            {
                var originalSize = new string[] { originalquery.Count().ToString() };
                actionExecutedContext.Response.Headers.Add("originalSize", originalSize as IEnumerable<string>);
            }
            return base.ApplyResultLimit(actionExecutedContext, query);
        }
    }
}