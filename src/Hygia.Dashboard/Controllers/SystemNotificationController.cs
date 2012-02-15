using Hygia.Widgets.Models;
using Hygia.Widgets.SystemNotifications;
using Hygia.Widgets.Testdata;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hygia.Widgets.Controllers
{
    public static class ContentTypes
    {
        public const string Atom = "application/atom+xml";
        public const string Rss = "application/rss+xml";
        public const string Json = "application/json";
    }

    public class SystemNotificationController
    {
        public string get_widgets_systemnotifications(SystemNotificationModel model)
        {
            switch (model.ContentType)
            {
                case ContentTypes.Atom:
                case ContentTypes.Rss:
                    return TestdataHelper.Notifications.GetSyndicationFeed(model.ContentType);
                case ContentTypes.Json:
                    return JsonConvert.SerializeObject(TestdataHelper.Notifications, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                default:
                    return TestdataHelper.Notifications.GetSyndicationFeed(ContentTypes.Atom);
            }
        }
    }
}