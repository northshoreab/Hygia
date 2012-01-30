namespace Hygia.LaunchPad.LogicalMonitoring.Handlers
{
    using System;
    using System.Collections.Generic;

    public class ServiceStructure
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<AutonomousComponent> AutonomousComponents { get; set; }
    }

    public class AutonomousComponent
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Equals(AutonomousComponent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (AutonomousComponent)) return false;
            return Equals((AutonomousComponent) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}