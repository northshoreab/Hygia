using System;
using System.Linq;
using HtmlTags;
using Hygia.Widgets.Models;
using Hygia.Widgets.SystemNotifications;
using Hygia.Widgets.Testdata;

namespace Hygia.Widgets.Controllers
{
    public class SystemNotificationItemController
    {
        public HtmlDocument get_widgets_systemnotificationitem_ItemId(SystemNotificationItemModel model)
        {
            return TestdataHelper.Notifications.Single(x => x.Id == Guid.Parse(model.ItemId)).AsHtmlDocument();
        }        
    }
}