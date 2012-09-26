using System.Web.Optimization;
using Hygia.Spa.App_Start;

namespace Hygia.Spa
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();

            // Tell WebApi to use our custom Ioc (Ninject)
            //IocConfig.RegisterIoc(GlobalConfiguration.Configuration);

            // Web API template created these 3
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //GlobalConfig.CustomizeConfig(GlobalConfiguration.Configuration);
        }
    }
}