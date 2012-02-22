namespace Hygia.FaultManagement.Domain
{
    public class ExceptionInfo
    {
        public string Message { get; set; }

        public string Reason { get; set; }

        public string ExceptionType { get; set; }

        public string Source { get; set; }

        public string StackTrace { get; set; }
    }
}