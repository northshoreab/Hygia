using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using HtmlTags;

namespace Hygia.API.SystemNotifications
{
    public static class NotificationExtensions
    {
        public static IEnumerable<HtmlDocument> AsHtmlDocument(this IEnumerable<Notification> notifications)
        {
            return notifications.Select(x => AsHtmlDocument((Notification) x));
        }

        public static HtmlDocument AsHtmlDocument(this Notification notification)
        {
            var doc = new HtmlDocument();
            var body = new HtmlTag("h1").Text(notification.Title);
            doc.RootTag.Add("body");
            body.Next = new HtmlTag("p").Text(notification.NotificationDate.ToShortDateString() + " - Author: " + notification.Author.Name + " (" + notification.Author.Email + ")");
            body.Next.Next = new HtmlTag("p").Text(notification.Description);
            doc.Add(body);

            return doc;            
        }

        public static IEnumerable<SyndicationItem> ASyndicationItems(this IEnumerable<Notification> notifications, SyndicationFeed feed = null)
        {
            return notifications.Select(x => x.AsSyndicationItem(feed));
        }

        public static SyndicationItem AsSyndicationItem(this Notification notification, SyndicationFeed feed = null)
        {
            var item = new SyndicationItem
                           {
                               SourceFeed = feed,
                               Title = SyndicationContent.CreatePlaintextContent(notification.Title),
                               Summary = SyndicationContent.CreatePlaintextContent(notification.Summary),
                               PublishDate = notification.NotificationDate,
                               Content = SyndicationContent.CreatePlaintextContent(notification.Description),                               
                           };

            item.Authors.Add(new SyndicationPerson
                                 {
                                     Name = notification.Author.Name,
                                     Email = notification.Author.Email
                                 });

            var uri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri + "/" + notification.Id);
            item.Links.Add(SyndicationLink.CreateAlternateLink(uri, "application/atom+xml"));

            return item;
        }
    }
}