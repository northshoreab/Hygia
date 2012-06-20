using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Models;
using Hygia.API.Models.Operations.LaunchPad;

namespace Hygia.API.Controllers.Operations.LaunchPad
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/environments/{environment:guid}/operations/launchpad/status/{id:guid}")]
    [Authorize]
    public class StatusController : EnvironmentController
    {
        [CustomQueryable]
        public IQueryable<LaunchPadStatus> GetAll()
        {
            return Session.Query<Hygia.Operations.Communication.Domain.LaunchPadStatus>().ToOutputModel().AsQueryable();
        }

        public LaunchPadStatus Get()
        {
            return Session.Load<Hygia.Operations.Communication.Domain.LaunchPadStatus>(Environment).ToOutputModel();
        }
    }
}