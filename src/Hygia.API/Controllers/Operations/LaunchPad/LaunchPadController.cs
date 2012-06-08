using System;
using System.Collections.Generic;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Raven.Client;
using Hygia.API.Models.Operations.LaunchPad;

namespace Hygia.API.Controllers.Operations.LaunchPad
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/operations/{controller}")]
    public class LaunchPadController : ApiController
    {
        private readonly IDocumentSession _session;

        public LaunchPadController(IDocumentSession session)
        {
            _session = session;
        }

        public IEnumerable<LaunchPadStatus> GetAll()
        {
            return _session.Query<Hygia.Operations.Communication.Domain.LaunchPadStatus>().ToOutputModel();
        }

        public LaunchPadStatus Get(Guid id)
        {
            return _session.Load<Hygia.Operations.Communication.Domain.LaunchPadStatus>(id).ToOutputModel();
        }
    }
}