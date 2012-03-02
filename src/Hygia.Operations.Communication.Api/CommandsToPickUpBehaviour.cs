using System;
using System.Collections.Generic;
using System.Linq;

namespace Hygia.Operations.Communication.Api
{
    using Domain;
    using FubuMVC.Core;
    using FubuMVC.Core.Behaviors;
    using FubuMVC.Core.Registration;
    using FubuMVC.Core.Registration.Nodes;
    using FubuMVC.Core.Runtime;
    using Raven.Client;

    public class CommandsToPickUpBehaviour : BasicBehavior
    {
        readonly IFubuRequest request;
        readonly IOutputWriter writer;
        readonly IDocumentSession session;

        public CommandsToPickUpBehaviour(IFubuRequest request,IOutputWriter writer,IDocumentSession session)
            : base(PartialBehavior.Ignored)
        {
            this.request = request;
            this.writer = writer;
            this.session = session;
        }

        protected override DoNext performInvoke()
        {
            var context = request.Get<ContextInputModel>();

            var apiKey = context.Headers["apikey"];

            //for now assume that an api call always means that the request is coming from a launchpad
            if (apiKey != null && context.Url.ToLower() != "/commands")
            {
                if(session.Query<LaunchPadCommand>().Any(c => !c.Delivered))
                    writer.AppendHeader("watchr.commandsavailable", "true");
            }
            
            return DoNext.Continue;
        }


    }

    public class CommandsToPickUpBehaviourConfiguration : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            graph.Actions()
            .Each(actionCall => actionCall.AddBefore(Wrapper.For<CommandsToPickUpBehaviour>()));
        }
    }
}
