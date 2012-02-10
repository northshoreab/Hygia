using Hygia.API.Models;
using Hygia.API.SystemNotifications;
using Hygia.API.Testdata;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Hygia.API.Controllers
{
    public static class ContentTypes
    {
        public const string Atom = "application/atom+xml";
        public const string Rss = "application/rss+xml";
        public const string Json = "application/json";
    }

    public class SystemNotificationController
    {
        public string get_Environment_systemnotifications(SystemNotificationModel model)
        {
            switch (model.ContentType)
            {
                case ContentTypes.Atom:
                case ContentTypes.Rss:
                    return TestdataHelper.Notifications.GetSyndicationFeed(model.ContentType, model.Environment);
                case ContentTypes.Json:
                    return JsonConvert.SerializeObject(TestdataHelper.Notifications, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                default:
                    return TestdataHelper.Notifications.GetSyndicationFeed(ContentTypes.Atom, model.Environment);
            }
        }
    }
}