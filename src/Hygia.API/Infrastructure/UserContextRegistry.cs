namespace Hygia.API.Infrastructure
{
    using System;
    using System.Threading;
    using Controllers;
    using Microsoft.IdentityModel.Claims;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class UserContextRegistry : Registry
    {
        public UserContextRegistry()
        {

            For<UserContext>()
                .Use(ctx =>
                         {
                             var user = Thread.CurrentPrincipal.Identity as IClaimsIdentity;

                             var context = new UserContext();

                             if (user != null)
                             {
                                 Guid userId;

                                 if (Guid.TryParse(user.Name, out userId))
                                     context.UserId = userId;

                                 context.ClaimsIdentity = user;
                             }

                             return context;
                         });
            PluginCache.AddFilledType(typeof(UserContext));



        }
    }
}