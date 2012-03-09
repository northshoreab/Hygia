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
        public dynamic ProvideFor(dynamic parameters)
        {
            IEnumerable<Guid> mt = parameters.MessageTypes;

            //get the first message type
            var id = mt.FirstOrDefault();

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