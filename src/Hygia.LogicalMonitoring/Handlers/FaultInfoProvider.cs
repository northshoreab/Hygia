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
            string messageTypeName = null;

            if (id != null)
            {
                var messageType = Session.Load<MessageType>(id);

                if (messageType != null)
                    messageTypeName = messageType.Type;
            }


            return new
                       {
                           MessageTypeName = messageTypeName
                       };
        }
    }
}