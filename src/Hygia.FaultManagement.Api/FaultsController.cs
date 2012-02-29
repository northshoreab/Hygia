using System.Collections.Generic;

namespace Hygia.FaultManagement.Api
{
    using System.Linq;
    using Domain;
    using FubuMVC.Core;
    using Raven.Client;

    public class FaultsController
    {
        public IDocumentSession Session { get; set; }
       
        [JsonEndpoint]
        public dynamic get_faults()
        {
            /*
            return new List<Fault>
                       {
                           getFakeFault(),
                           getFakeFault(),
                           getFakeFault(),
                           getFakeFault(),
                           getFakeFault(),
                           getFakeFault(),
                           getFakeFault()
                       };
            */
            return Session.Query<Fault>()
                .Where(f=>f.Status != FaultStatus.Archived)
                .ToList();
        }

        /*
        private Fault getFakeFault()
        {
            return new Fault
                       {
                           Body = "body",
                           Status = FaultStatus.New,
                           AssignedTo = System.Guid.NewGuid(),
                           ContainedMessages = new List<PhysicalMessage>(),
                           Endpoint = "endpoint",
                           EndpointId = System.Guid.NewGuid(),
                           Exception =
                               new ExceptionInfo
                                   {
                                       ExceptionType = "backboneIsHardException",
                                       Message = "do something easier",
                                       Reason = "bad coder",
                                       Source = "?",
                                       StackTrace = "..."
                                   },
                           FaultEnvelopeId = System.Guid.NewGuid(),
                           Headers =
                               new Dictionary<string, string>
                                   {{"NServiceBus.OriginalId", System.Guid.NewGuid().ToString()}},
                           History = new List<HistoryItem>
                                         {
                                             new HistoryItem
                                                 {
                                                     Status = "status",
                                                     Time = System.DateTime.Now
                                                 }
                                         }

                       };
        }
        */
    }

}