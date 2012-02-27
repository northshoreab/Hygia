namespace Hygia.Web.Controllers
{
    using Models;

    public class HomeController
    {
        public HomeViewModel get_home()
        {
            return new HomeViewModel();
        }
    }
}