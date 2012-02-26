namespace Hygia.FaultManagement.Domain
{
    using System;

    public class HistoryItem
    {
        public DateTime TimeOfFailure { get; set; }

        public ExceptionInfo Exception { get; set; }
    }
}