using System.Collections.Generic;

namespace Hygia.Widgets.Models
{
    using FaultManagement.Domain;

    public class FaultsViewModel
    {
        public IList<Fault> Alerts { get; set; }
    }
}