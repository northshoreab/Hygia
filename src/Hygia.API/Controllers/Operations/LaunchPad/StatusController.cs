using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Models;
using Raven.Client;
using Hygia.API.Models.Operations.LaunchPad;

namespace Hygia.API.Controllers.Operations.LaunchPad
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/operations/launchpad/status/{id:guid}")]
    public class StatusController : ApiController
    {
        private readonly IDocumentSession _session;

        public StatusController(IDocumentSession session)
        {
            _session = session;
        }

        [CustomQueryable]
        public IQueryable<LaunchPadStatus> GetAll()
        {
            return _session.Query<Hygia.Operations.Communication.Domain.LaunchPadStatus>().ToOutputModel().AsQueryable();
        }

        public LaunchPadStatus Get(Guid id)
        {
            return _session.Load<Hygia.Operations.Communication.Domain.LaunchPadStatus>(id).ToOutputModel();
        }
    }
}