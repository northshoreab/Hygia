namespace Hygia.LaunchPad.LogicalMonitoring.Commands
{
    using System;

    public class RegisterLogicalService
    {
        public string ServiceName { get; set; }

        public Guid ServiceId { get; set; }

        public override string ToString()
        {
            return string.Format("Registering service {0} (Id:{1})", ServiceName, ServiceId);
        }
    }
}