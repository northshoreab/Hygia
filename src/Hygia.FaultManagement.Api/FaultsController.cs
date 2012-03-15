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
		    /*
            return new List<Fault>
                       {
                            new Fault{ FaultEnvelopeId = "234234-234324-234234", Exception = new ExceptionInfo {Message = "message message message message message message message", Reason = "reason", StackTrace = "stacktrace"}},
                            new Fault{ FaultEnvelopeId = "7673456265767-657564", Exception = new ExceptionInfo {Message = "message", Reason = "reason", StackTrace = "stacktrace"}},
                            new Fault{ FaultEnvelopeId = "989789-234324-234234", Exception = new ExceptionInfo {Message = "message", Reason = "reason", StackTrace = "stacktrace"}, Retries = 2}
                       }.ToOutputModels();
			*/
            return Session.Query<Fault>()
                .Where(f => f.Status != FaultStatus.Archived && f.Status != FaultStatus.Resolved)
                .OrderByDescending(f=>f.TimeOfFailure)
                .ToList()
                .ToOutputModels();
        }
        
        [JsonEndpoint]
        public Fault get_faults_FaultId(FaultEnvelopeInputModel model)
        {
			/*
            return new Fault
                       {
                           Exception =
                               new ExceptionInfo {Message = "message", Reason = "reason", StackTrace = "stacktrace"}
                       };
			*/
            return Session.Load<Fault>(model.FaultId);
        }

        [JsonEndpoint]
        public dynamic post_faults_retry(FaultEnvelopeInputModel model)
        {
            Bus.Send(new IssueRetryForFault {FaultId =model.FaultId});
            return string.Empty;
        }

        [JsonEndpoint]
        public dynamic post_faults_archive(FaultEnvelopeInputModel model)
        {
            Bus.Send(new ArchiveFault {FaultId = model.FaultId});
            return string.Empty;
        }

        [JsonEndpoint]
        public dynamic post_faults_retried(FaultRetriedInputModel model)
        {
            Bus.Send(new RegisterSuccessfullRetry
                         {
                             FaultId = model.FaultId,
                             TimeOfRetry = model.TimeOfRetry
                         });
            return string.Empty;
        }      
    }

    public class FaultRetriedInputModel
    {
        public Guid FaultId { get; set; }
        public DateTime TimeOfRetry { get; set; }
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
                                    FaultId = fault.Id,
                                    FaultNumber = 0,                                    
                                    TimeSent = fault.TimeOfFailure.ToString(),
                                    Retries = fault.Retries
                                };

            return viewModel;
        }
    }

    public class FaultEnvelopeOutputModel
    {
        public Guid FaultId { get; set; }
        public long FaultNumber { get; set; }
        public string Title 
        {
            get
            {
                if (!string.IsNullOrEmpty(ExceptionMessage))
                {
                    if (ExceptionMessage.Length > 30)
                    {
                        return ExceptionMessage.Substring(0, 27) + "...";
                    }

                    return ExceptionMessage;
                }

                return string.Empty;
            }
        }
        public string ExceptionMessage { get; set; }
        public string TimeSent { get; set; }        
        public int Retries { get; set; }
        public string EnclosedMessageTypes { get; set; }                
        public string BusinessService { get; set; }
    }

    public class FaultEnvelopeInputModel : FaultEnvelopeOutputModel { }
}