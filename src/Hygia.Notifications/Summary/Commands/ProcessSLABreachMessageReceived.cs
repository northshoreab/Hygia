using System;
using Hygia.ServiceLevelAgreements.Events;
using NServiceBus;

namespace Hygia.Notifications.Summary.Commands
{
    public class ProcessSLABreachMessageReceived : ICommand
    {
        public Guid AlerterInstanceId { get; set; }
        public SLABreachMessage MessageDetails { get; set; }        
    }
}