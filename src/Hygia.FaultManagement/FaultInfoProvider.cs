using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Hygia.FaultManagement
{
    using Domain;
    using Notifications.Provide;
    using Raven.Client;
    
    public static class XmlDocumentExtensions
    {
        public static string ToIndentedString(this XmlDocument doc)
        {
            var stringWriter = new StringWriter(new StringBuilder());
            var xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
            doc.Save(xmlTextWriter);
            return stringWriter.ToString();
        }
    }

    public class FaultInfoProvider:IProvideFaultInformation
    {
        public IDocumentSession Session { get; set; }
        public dynamic ProvideFor(dynamic parameters)
        {
            var fault = Session.Load<Fault>(parameters.FaultEnvelopeId);

            string prettyPrintedBody = fault.Body;

            try
            {
                var bodyXmlDoc = new XmlDocument();
                bodyXmlDoc.LoadXml(fault.Body);
                prettyPrintedBody = bodyXmlDoc.ToIndentedString();
            }
            catch { }

            return new
                       {
                           fault.TimeOfFailure,
                           fault.Exception.Message,
                           fault.Exception.Reason,
                           fault.Body,
                           fault.Headers,
                           PrettyPrintedBody = prettyPrintedBody
                       };
        }
    }
}