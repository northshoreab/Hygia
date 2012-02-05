namespace Hygia.PhysicalMonitoring.Handlers
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using Domain;
    using NServiceBus;
    using Raven.Client;

    public class StartMessageChainHandler:IHandleMessages<StartMessageChain>
    {
        public IDocumentSession Session { get; set; }

        public void Handle(StartMessageChain message)
        {
            var chain = new MessageChain
                            {
                                Id = message.MessageChainId.ToString(),
                                StartedByMessages = new List<Guid> {message.MessageId},
                            };

            Session.Store(chain);
        }
    }
}