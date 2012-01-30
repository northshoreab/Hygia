namespace Hygia.LaunchPad.LogicalMonitoring.Commands
{
    using System;

    public class RegisterAutonomousComponent
    {
        public Guid ServiceId { get; set; }

        public Guid AutonomousComponentId { get; set; }

        public string AutonomousComponentName { get; set; }


        public override string ToString()
        {
            return string.Format("Registering AC - {0}{1} owned by service {2}", AutonomousComponentName,AutonomousComponentId,ServiceId);
        }
    }
}