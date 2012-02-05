namespace Hygia.LogicalMonitoring.Commands
{
    using System;

    public class RegisterMessageOwner
    {
        public Guid OwnedByService { get; set; }
        
        public Guid OwnedByComponent { get; set; }

        public Guid MessageTypeId { get; set; }


        public override string ToString()
        {
            return string.Format("Message {0} owned by service:{1} (component:{2})", MessageTypeId, OwnedByService, OwnedByComponent);
        }
    }
}