namespace Hygia.FaultManagement.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using NServiceBus;    
    using FubuMVC.Core;
    using Raven.Client;    
    using Commands;
    using Domain;

    public class FaultsController
    {
        public IDocumentSession Session { get; set; }
        public IBus Bus { get; set; }
       
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
                .Where(f=>f.Status != FaultStatus.Archived && f.Status != FaultStatus.RetryIssued)
                .ToList();          
        }
        
        [JsonEndpoint]
        public dynamic post_faults_retry(FaultEnvelopeInputModel model)
        {
            Bus.Send(new IssueRetryForFault { MessageId = System.Guid.Parse(model.FaultEnvelopeId) });

            return string.Empty;
        }

        [JsonEndpoint]
        public dynamic post_faults_archive(FaultEnvelopeInputModel model)
        {
            Bus.Send(new ArchiveFault { MessageId = System.Guid.Parse(model.FaultEnvelopeId) });

            return string.Empty;
        }

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
                           FaultEnvelopeId = System.Guid.NewGuid().ToString(),
                           Headers =
                               new Dictionary<string, string>
                                   {
                                    {"NServiceBus.OriginalId", System.Guid.NewGuid().ToString()},
                                    {"NServiceBus.TimeSent", System.DateTime.Now.ToString()},
                                    {"NServiceBus.EnclosedMessageTypes", "OrderPlaced, Version=1.0.0.0"}
                                   },
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
    }

    public class FaultEnvelopeOutputModel
    {
        public string FaultEnvelopeId { get; set; }   
    }

    public class FaultEnvelopeInputModel : FaultEnvelopeOutputModel
    {
        
    }
}