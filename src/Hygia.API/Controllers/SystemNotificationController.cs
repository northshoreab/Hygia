using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;
using Hygia.API.Models;
using Hygia.API.SystemNotifications;
using Hygia.API.Testdata;

namespace Hygia.API.Controllers
{
    public class SystemNotificationController
    {
        public string get_Tennant_systemnotifications(SystemNotificationModel model)
        {
            var feed = new SyndicationFeed("System notification", "Publishes system notifications for tennant: " + model.Tennant,new Uri("http://localhost"));
            feed.Authors.Add(new SyndicationPerson("test@test.com","Testor Testorsson", "http://localhost"));
            feed.Links.Add(SyndicationLink.CreateSelfLink(new Uri(HttpContext.Current.Request.Url.AbsoluteUri), "application/atom+xml"));

            feed.Items = TestdataHelper.Notifications.ASyndicationItems(feed);

            var stringWriter = new StringWriter();

            XmlWriter feedWriter = XmlWriter.Create(stringWriter,new XmlWriterSettings
                                                                     {
                                                                         OmitXmlDeclaration = true
                                                                     });

            feed.Copyright = SyndicationContent.CreatePlaintextContent("Copyright hygia");
            feed.Language = "en-us";
            feed.SaveAsAtom10(feedWriter);

            feedWriter.Close();

            return stringWriter.ToString();
        }
    }
}