namespace Hygia.Web.Controllers
{
    using System.Linq;
    using FaultManagement.Domain;
    using FubuMVC.Core;

    public class FaultsController:WebController
    {
        [JsonEndpoint]
        public dynamic get_faults()
        {
            return Session.Query<Fault>()
                .ToList();
        }
    }

    public class FaultsModel
    {
    }
}