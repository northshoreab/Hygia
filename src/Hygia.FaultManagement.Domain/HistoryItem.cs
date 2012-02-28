namespace Hygia.FaultManagement.Domain
{
    using System;

    public class HistoryItem
    {
        public DateTime Time { get; set; }

        public string Status { get; set; }
    }
}