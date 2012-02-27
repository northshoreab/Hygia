namespace Hygia.API.Controllers
{
    using Hygia.Web.Models;

    public class HomeController
    {
        public HomeViewModel get_home()
        {
            return new HomeViewModel();
        }
    }
}