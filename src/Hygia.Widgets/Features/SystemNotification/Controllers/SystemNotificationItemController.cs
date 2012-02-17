using System;
using System.Linq;
using HtmlTags;
using Hygia.Widgets.Features.SystemNotification.Domain;
using Hygia.Widgets.Features.SystemNotification.Models;
using Hygia.Widgets.Features.Tests.Testdata;

namespace Hygia.Widgets.Features.SystemNotification.Controllers
{
    public class SystemNotificationItemController
    {
        public HtmlDocument get_widgets_systemnotificationitem_ItemId(SystemNotificationItemModel model)
        {
            return TestdataHelper.Notifications.Single(x => x.Id == Guid.Parse(model.ItemId)).AsHtmlDocument();
        }        
    }
}