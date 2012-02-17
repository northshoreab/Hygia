using System.Linq;
using Hygia.Notifications.Domain;
using Hygia.Widgets.Features.SystemNotification.Models;
using Hygia.Widgets.Features.Tests.Testdata;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client;
using Raven.Client.Linq;

namespace Hygia.Widgets.Features.SystemNotification.Controllers
{
    public static class ContentTypes
    {
        public const string Atom = "application/atom+xml";
        public const string Rss = "application/rss+xml";
        public const string Json = "application/json";
    }

    public class SystemNotificationController
    {
        private readonly IDocumentSession _session;

        public SystemNotificationController(IDocumentSession session)
        {
            _session = session;
        }

        public string get_widgets_systemnotifications(SystemNotificationModel model)
        {
            //TODO: Call raven to get notifications
            //var notifications = _session.Query<Notification>().OrderByDescending(x => x.NotificationDate).ToList();
            var notifications = TestdataHelper.Notifications;

            switch (model.ContentType)
            {
                case ContentTypes.Atom:
                case ContentTypes.Rss:
                    return notifications.GetSyndicationFeed(model.ContentType);
                case ContentTypes.Json:
                    return JsonConvert.SerializeObject(notifications, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                default:
                    return notifications.GetSyndicationFeed(ContentTypes.Atom);
            }
        }
    }
}