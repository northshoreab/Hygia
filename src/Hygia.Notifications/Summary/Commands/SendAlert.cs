using System.Collections.Generic;
using Hygia.Notifications.Summary.Domain;
using NServiceBus;

namespace Hygia.Notifications.Summary.Commands
{
    public class SendAlert : ICommand
    {
        public List<AlertInfo> AlertList { get; set; }
    }
}