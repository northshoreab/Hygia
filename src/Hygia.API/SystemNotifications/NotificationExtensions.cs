using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;
using HtmlTags;
using Hygia.API.Controllers;
using Hygia.API.Testdata;

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

        public static string GetSyndicationFeed(this IEnumerable<Notification> notifications, string contentType, string environment)
        {
            var feed = new SyndicationFeed("System notification", "Publishes system notifications for environment: " + environment, new Uri("http://localhost"));
            feed.Authors.Add(new SyndicationPerson("test@test.com", "Testor Testorsson", "http://localhost"));
            feed.Links.Add(SyndicationLink.CreateSelfLink(new Uri(HttpContext.Current.Request.Url.AbsoluteUri), "application/atom+xml"));

            feed.Items = notifications.ASyndicationItems(feed);

            var stringWriter = new StringWriter();

            XmlWriter feedWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
            {
                OmitXmlDeclaration = true
            });

            feed.Copyright = SyndicationContent.CreatePlaintextContent("Copyright hygia");
            feed.Language = "en-us";

            if (contentType == ContentTypes.Atom)
                feed.SaveAsAtom10(feedWriter);
            else
                feed.SaveAsRss20(feedWriter);

            feedWriter.Close();

            return stringWriter.ToString(); 
        }
    }
}