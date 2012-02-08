using System.Linq;
using Hygia.Backend.Notifications.Domain;
using Raven.Client.Indexes;

namespace Hygia.Backend.Notifications.Index
{
    public class All_NotificationConfiguration : AbstractMultiMapIndexCreationTask
    {
        public All_NotificationConfiguration()
        {
            AddMap<CriticalTimeNotificationConfiguration>(criticalTimeAlerts => criticalTimeAlerts.Select(x => new 
                                                                                                            {
                                                                                                                x.Id,
                                                                                                                x.MessageType,
                                                                                                                x.AlertLevel
                                                                                                            }));

            AddMap<BusinessProcessCompletionTimeNotificationConfiguration>(bp => bp.Select(x => new
                                                                                             {
                                                                                                 x.AlertLevel,
                                                                                                 x.Id,
                                                                                                 x.StartMessageType,
                                                                                                 x.EndMessageType
                                                                                             }));
        }
    }
}
