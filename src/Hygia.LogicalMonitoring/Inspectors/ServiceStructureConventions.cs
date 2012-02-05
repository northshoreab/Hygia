namespace Hygia.LogicalMonitoring.Inspectors
{
    using System;
    using System.Linq;

    public class ServiceStructureConventions
    {
        public static Guid ServiceId(string typeName)
        {
            return ServiceName(typeName).ToGuid();
        }


        public static string ServiceName(string typeName)
        {
            return typeName.Split('.').First();
        }


        public static string BusinessComponentName(string typeName)
        {
            if (typeName.Split('.').Count() > 2)
                return typeName.Split('.').ElementAt(1);

            return null;
        }


        public static Guid AutonomousComponentId(string typeName)
        {
            return AutonomousComponentName(typeName).ToGuid();
        }

        public static string AutonomousComponentName(string typeName)
        {
            return typeName.Split('.').Last();
        }
    }
}