using System.Web.Http;
using System.Web.Routing;

namespace Hygia.API.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            /*
            routes.MapHttpRoute(
                name: "DefaultArea",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            routes.MapHttpRoute(
                name: "DefaultDomain",
                routeTemplate: "api/{area}/{controller/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            routes.MapHttpRoute(
                name: "Default", 
                routeTemplate: "api/{area}/{domain}/{id}/{controller}",
                defaults: new {id = RouteParameter.Optional}
                );
             * */
        }
    }
}