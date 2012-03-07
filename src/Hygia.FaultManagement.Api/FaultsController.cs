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
        public IEnumerable<FaultEnvelopeOutputModel> get_faults()
        {
            return Session.Query<Fault>()
                .Where(f => f.Status != FaultStatus.Archived && f.Status != FaultStatus.RetryIssued)
                .ToList()
                .ToOutputModels();
        }

        [JsonEndpoint]
        public dynamic post_faults_retry(FaultEnvelopeInputModel model)
        {
            Bus.Send(new IssueRetryForFault {MessageId = Guid.Parse(model.FaultEnvelopeId)});
            return string.Empty;
        }

        [JsonEndpoint]
        public dynamic post_faults_archive(FaultEnvelopeInputModel model)
        {
            Bus.Send(new ArchiveFault {MessageId = Guid.Parse(model.FaultEnvelopeId)});
            return string.Empty;
        }       
    }

    public static class FaultEnvelopeViewModelExtensions
    {
        public static IEnumerable<FaultEnvelopeOutputModel> ToOutputModels(this IEnumerable<Fault> faults)
        {
            return faults.Select(fault => fault.ToOutputModel());
        }

        public static FaultEnvelopeOutputModel ToOutputModel(this Fault fault)
        {
            string enclosedMessageTypes;

            try
            {
                enclosedMessageTypes = fault.Headers["NServiceBus.EnclosedMessageTypes"].Split(',')[0].Split('.').LastOrDefault();
            }
            catch (Exception)
            {
                enclosedMessageTypes = string.Empty;                
            }
            
            var viewModel = new FaultEnvelopeOutputModel
                                {
                                    BusinessService = "", 
                                    EnclosedMessageTypes = enclosedMessageTypes ?? string.Empty,
                                    ExceptionMessage = fault.Exception.Message,
                                    FaultEnvelopeId = fault.FaultEnvelopeId,
                                    FaultId = 0,
                                    Status = fault.Status,
                                    TimeSent = fault.TimeOfFailure.ToString()
                                };

            return viewModel;
        }
    }

    public class FaultEnvelopeOutputModel
    {
        public long FaultId { get; set; }
        public string FaultEnvelopeId { get; set; }
        public string ExceptionMessage { get; set; }
        public string TimeSent { get; set; }
        public FaultStatus Status { get; set; }
        public string EnclosedMessageTypes { get; set; }                
        public string BusinessService { get; set; }
    }

    public class FaultEnvelopeInputModel : FaultEnvelopeOutputModel { }
}