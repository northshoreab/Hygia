using Hygia.FaultManagement.Domain;

namespace Hygia.FaultManagement.Events
{
    public class FaultRegistered
    {
        public Fault Fault { get; set; }
    }
}