namespace Hygia.FaultManagement.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;

    public static class FaultEnvelopeViewModelExtensions
    {
        public static IEnumerable<FaultEnvelopeOutputModel> ToOutputModels(this IEnumerable<Fault> faults)
        {
            return faults.Select(fault => ToOutputModel(fault));
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
                                    FaultNumber = fault.Number,                                    
                                    TimeSent = fault.TimeOfFailure.ToString(),
                                    Retries = fault.Retries
                                };

            return viewModel;
        }
    }
}