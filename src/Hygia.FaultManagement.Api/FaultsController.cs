using System;

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
        public IEnumerable<FaultEnvelopeOutputModel> get_api_faults()
        {
            return Session.Query<Fault>()
                .Where(f => f.Status != FaultStatus.Archived && f.Status != FaultStatus.Resolved && f.Status != FaultStatus.RetryPerformed)
                .OrderByDescending(f=>f.TimeOfFailure)
                .ToList()
                .ToOutputModels();
        }
        
        [JsonEndpoint]
        public Fault get_api_faults_FaultId(FaultEnvelopeInputModel model)
        {

            return Session.Load<Fault>(model.FaultId);
        }

        [JsonEndpoint]
        public dynamic post_api_faults_retry(FaultEnvelopeInputModel model)
        {
            Bus.Send(new IssueRetryForFault
                         {
                             FaultId =model.FaultId,
                             IssuedAt = DateTime.UtcNow
                         });
            return string.Empty;
        }

        [JsonEndpoint]
        public dynamic post_api_faults_archive(FaultEnvelopeInputModel model)
        {
            Bus.Send(new ArchiveFault {FaultId = model.FaultId});
            return string.Empty;
        }

        [JsonEndpoint]
        public dynamic post_api_faults_retried(FaultRetriedInputModel model)
        {
            Bus.Send(new RegisterSuccessfullRetry
                         {
                             FaultId = model.FaultId,
                             TimeOfRetry = model.TimeOfRetry
                         });
            return string.Empty;
        }      
    }
}