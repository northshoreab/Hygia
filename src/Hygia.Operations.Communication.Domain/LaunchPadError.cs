using System;

namespace Hygia.Operations.Communication.Domain
{
    public class LaunchPadError
    {
        public Guid Id { get; set; }
        public DateTime TimeOfReport { get; set; }

        public string StackTrace { get; set; }
        public string Message { get; set; }
        public Exception InnerException { get; set; }
    }
}