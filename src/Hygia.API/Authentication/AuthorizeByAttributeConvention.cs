namespace Hygia.API.Authentication
{
    using System;
    using System.Linq;
    using FubuMVC.Core.Registration;
    using FubuMVC.Core.Runtime;
    using FubuMVC.Core.Security;

    public class AuthorizeByAttributeConvention : IConfigurationAction
    {
        public void Configure(BehaviorGraph endpoints)
        {
            var viewEntityActions = endpoints.Behaviors.Select(x => x.FirstCall());

            foreach (var action in viewEntityActions)
            {
                var endpoint = action.ParentChain();

                //todo - only add where [NoAuthentication] is set
                endpoint.Authorization.AddPolicy(new AttributePolicy());
            }
        }
    }

    public class AttributePolicy : IAuthorizationPolicy
    {
        public AuthorizationRight RightsFor(IFubuRequest request)
        {
            //todo
            return AuthorizationRight.Allow;
        }
    }
}