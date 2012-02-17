using System;
using System.Linq;
using HtmlTags;
using Hygia.Notifications.Domain;
using Hygia.Widgets.Features.SystemNotification.Models;
using Hygia.Widgets.Features.Tests.Testdata;
using Raven.Client;

namespace Hygia.Widgets.Features.SystemNotification.Controllers
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
            //TODO: Change from testnotification to notification from raven
            //return _session.Load<Notification>(model.ItemId).AsHtmlDocument();
            return TestdataHelper.Notifications.Single(x => x.Id == Guid.Parse(model.ItemId)).AsHtmlDocument();
        }        
    }
}