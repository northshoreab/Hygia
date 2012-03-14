using System.Collections.Generic;

namespace Hygia.FaultManagement.Events
{
    using System;

    public class RetryFailed
    {
        public Guid FaultId { get; set; }
        public List<Guid> MessageTypes { get; set; }
    }
}