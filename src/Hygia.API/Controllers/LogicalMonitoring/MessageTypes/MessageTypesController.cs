using System;
using System.Linq;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.API.Infrastructure;
using Hygia.API.Models;
using Hygia.API.Models.LogicalMonitoring.MessageType;

namespace Hygia.API.Controllers.LogicalMonitoring.MessageTypes
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/environments/{environment:guid}/logicalmonitoring/messagetypes")]
    [Authorize]
    public class MessageTypesController : EnvironmentController
    {
        [CustomQueryable]
        public IQueryable<Resource<MessageType>> GetAll()
        {
            return Session.Query<Hygia.LogicalMonitoring.Handlers.MessageType>()
                .ToOutputModels()
                .Select(x => x.AsResponseItem())
                .AsQueryable();
        }

        public Resource<MessageType> Get(Guid id)
        {
            return Session.Load<Hygia.LogicalMonitoring.Handlers.MessageType>(id)
                .ToOutputModel()
                .AsResponseItem();
        }
    }
}
