using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Models.LogicalMonitoring.MessageType;
using Raven.Client;

namespace Hygia.API.Controllers.LogicalMonitoring.MessageTypes
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/logicalmonitoring/{controller}")]
    public class MessageTypesController : ApiController
    {
        private readonly IDocumentSession _session;

        public MessageTypesController(IDocumentSession session)
        {
            _session = session;
        }

        public IEnumerable<ResponseItem<MessageType>> GetAll()
        {
            return _session.Query<Hygia.LogicalMonitoring.Handlers.MessageType>()
                .ToOutputModels()
                .Select(x => x.AsResponseItem());
        }

        public ResponseItem<MessageType> Get(Guid id)
        {
            return _session.Load<Hygia.LogicalMonitoring.Handlers.MessageType>(id)
                .ToOutputModel()
                .AsResponseItem();
        }
    }
}
