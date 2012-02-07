using System;
using System.Linq;
using HtmlTags;
using Hygia.API.Models;
using Hygia.API.SystemNotifications;
using Hygia.API.Testdata;

namespace Hygia.API.Controllers
{
    public class SystemNotificationItemController
    {
        public HtmlDocument get_Environment_systemnotifications_ItemId(SystemNotificationItemModel model)
        {
            return TestdataHelper.Notifications.Single(x => x.Id == Guid.Parse(model.ItemId)).AsHtmlDocument();
        }        
    }
}