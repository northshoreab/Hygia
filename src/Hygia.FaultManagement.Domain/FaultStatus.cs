namespace Hygia.FaultManagement.Domain
{
    public enum FaultStatus
    {
        New,
        Archived,
        RetryRequested,
        RetryPerformed,
        RetryFailed,
        Resolved
    }
}