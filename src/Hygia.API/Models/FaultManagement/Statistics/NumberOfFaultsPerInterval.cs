using System;

namespace Hygia.API.Models.FaultManagement.Statistics
{
    public class FaultsPerInterval
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int NumberOfFaults { get; set; }        
    }
}