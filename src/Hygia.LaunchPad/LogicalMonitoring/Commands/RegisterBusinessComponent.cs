namespace Hygia.LaunchPad.LogicalMonitoring.Commands
{
    using System;

    public class RegisterBusinessComponent
    {
        public Guid BusinessComponentId { get; set; }

        public string BusinessComponentName { get; set; }

        public Guid OwnedByService { get; set; }

        public override string ToString()
        {
            return string.Format("Registering BC - {0}({1}) contained by service {2}",BusinessComponentName,BusinessComponentName,OwnedByService);
        }
    }
}