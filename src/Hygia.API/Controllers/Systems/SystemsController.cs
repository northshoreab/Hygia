﻿namespace Hygia.API.Controllers.Systems
{
    using System;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Infrastructure;
    using Hygia.Operations.Accounts.Commands;
    using ApiController = ApiController;

    [DefaultHttpRouteConvention]
    [RoutePrefix("api/systems")]
    [Authorize]
    public class SystemsController : ApiController
    {
        public ResponseItem<Hygia.Operations.Accounts.Domain.System> Post(Guid systemId,string name)
        {
            var system = new Hygia.Operations.Accounts.Domain.System
                              {
                                  Id = systemId,
                                  Name = name,
                                  OwnedBy = CurrentUser.UserId
                              };


            Session.Store(system);

            Bus.Publish(new SystemCreated { UserAccountId = CurrentUser.UserId, SystemId = system.Id });

            return system.AsResponseItem();
        }
    }
}