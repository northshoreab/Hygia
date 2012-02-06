using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;
using HtmlTags;
using Hygia.API.Models;
using Hygia.API.SystemNotifications;
using Hygia.API.Testdata;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Formatting = Newtonsoft.Json.Formatting;
using System.Linq;

namespace Hygia.API.Controllers
{
    public class SystemNotificationItemController
    {
        public HtmlDocument get_Tennant_systemnotifications_ItemId(SystemNotificationItemModel model)
        {
            return TestdataHelper.Notifications.Single(x => x.Id == Guid.Parse(model.ItemId)).AsHtmlDocument();
        }        
    }

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

    public class DashboardWidgetController
    {
        public const string OrderFunnel = "ORDERFUNNEL";
        public const string PieChart = "PIECHART";
        public const string LineChart = "LINECHART";
        public const string HighChartsLineChart = "HIGHCHARTSLINECHART";

        public string get_Tennant_dashboard_Widget(DashboardWidgetModel input)
        {
            switch (input.Widget.ToUpper())
            {
                case OrderFunnel:
                    return JsonConvert.SerializeObject(TestdataHelper.GetFunnel(),Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver()});
                case PieChart:
                    return JsonConvert.SerializeObject(TestdataHelper.GetPieChart(), Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                case LineChart:
                    return JsonConvert.SerializeObject(TestdataHelper.GetLineChart(), Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                case HighChartsLineChart:
                    return TestdataHelper.GetHighChart().ToString();
            }

            return "No widget found";
        }
    }
}