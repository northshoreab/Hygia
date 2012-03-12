namespace Hygia.FaultManagement.Events
{
    using System;
    using System.Collections.Generic;

    public class FaultRegistered
    {
        public Guid FaultId{ get; set; }

        public List<Guid> MessageTypes { get; set; }
    }
}