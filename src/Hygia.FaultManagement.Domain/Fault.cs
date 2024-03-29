using System;
using System.Collections.Generic;

namespace Hygia.FaultManagement.Domain
{
    public class Fault
    {
        public Guid Id { get; set; }

        public long Number { get; set; }

        public FaultStatus Status { get; set; }
        
        public ExceptionInfo Exception{ get; set; }
        
        public string FaultEnvelopeId { get; set; }

        public string Body { get; set; }

        public string Endpoint { get; set; }

        public Guid EndpointId { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public IEnumerable<PhysicalMessage> ContainedMessages { get; set; }

        public DateTime TimeOfFailure { get; set; }

        public Guid AssignedTo { get; set; }


        ICollection<HistoryItem> history;

        public ICollection<HistoryItem> History { 
            get { return history ?? (history = new List<HistoryItem>()); }
            set { history = value; } 
        }

        public int Retries { get; set; }

        public DateTime ResolvedAt { get; set; }
    }
}