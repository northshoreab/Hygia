using Hygia.PhysicalMonitoring.Domain;

namespace Hygia.Backend.SLA.Domain
{
    public interface IRule
    {
        bool Execute(Envelope envelope);
    }
}