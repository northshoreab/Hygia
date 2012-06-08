using System;
using System.Collections.Generic;
using System.Linq;

namespace Hygia.API.Models.FaultManagement.Faults
{
    public static class FaultExtensions
    {
        public static IEnumerable<Fault> ToOutputModels(this IEnumerable<Hygia.FaultManagement.Domain.Fault> faults)
        {
            return faults.Select(ToOutputModel);
        }

        public static Fault ToOutputModel(this Hygia.FaultManagement.Domain.Fault fault)
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
            
            var viewModel = new Fault
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