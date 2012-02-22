namespace Hygia.LogicalMonitoring.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Notifications.Provide;
    using Raven.Client;

    public class FaultInfoProvider : IProvideFaultInformation
    {
        public IDocumentSession Session { get; set; }
        public dynamic ProvideFor(Guid faultEnvelopeId, IEnumerable<Guid> messageTypes)
        {
            var id = messageTypes.FirstOrDefault();
            MessageType messageType = null;

            if (id != null)
                messageType = Session.Load<MessageType>(id);

            return new
                       {
                           MessageTypeName = messageType == null ? "No message type found" : messageType.Type
                       };
        }
    }
}