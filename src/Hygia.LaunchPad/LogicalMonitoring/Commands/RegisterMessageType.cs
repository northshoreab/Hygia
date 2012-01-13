namespace Hygia.LaunchPad.LogicalMonitoring.Commands
{
    using System;
    using Inspectors;

    public class RegisterMessageType
    {
        public Guid MessageTypeId { get; set; }
        
        public string MessageType { get; set; }

        public string MessageVersion { get; set; }

        public MessageIntent MessageIntent { get; set; }

        public override string ToString()
        {
            return string.Format("Registering message type {0}(v {1}) with intent: {2}",MessageType,MessageVersion,MessageIntent);
        }
    }
}