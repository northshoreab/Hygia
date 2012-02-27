using HtmlTags;
using Hygia.API.Features.SystemNotification.Models;
using Hygia.Notifications.Domain;
using Raven.Client;

namespace Hygia.API.Features.SystemNotification.Controllers
{
    public class SystemNotificationItemController
    {
        private readonly IDocumentSession _session;

        public SystemNotificationItemController(IDocumentSession session)
        {
            _session = session;
        }

        public HtmlDocument get_widgets_systemnotificationitem_ItemId(SystemNotificationItemModel model)
        {
            return _session.Load<Notification>(model.ItemId).AsHtmlDocument();
            //return TestdataHelper.Notifications.Single(x => x.Id == Guid.Parse(model.ItemId)).AsHtmlDocument();
        }        
    }
}