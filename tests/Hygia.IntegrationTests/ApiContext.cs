namespace Hygia.IntegrationTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Messaging;
    using System.Xml.Serialization;
    using Machine.Specifications;
    using NServiceBus;
    using NServiceBus.Utils;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Json.Linq;
    using RestSharp;

    public class ApiContext : RestSharpContext
    {
        protected static IDocumentStore Store;
        static Address backendAddress = Address.Parse("hygia.backend");

        static MessageQueue backendQ;

        Establish context = () =>
                                {
                                    Store = new DocumentStore
                                                {
                                                    Url = "http://localhost:8080",
                                                    DefaultDatabase = "WatchR"
                                                };
                                    Store.Initialize();
                                    client = new RestClient("http://localhost:8088/");

                                    //purge the input queues
                                    backendQ = new MessageQueue(MsmqUtilities.GetFullPath(backendAddress));
                                    var mpf = new MessagePropertyFilter();
                                    mpf.SetAll();

                                    backendQ.MessageReadPropertyFilter = mpf;
                                    backendQ.Purge();

                                };


        protected static void VerifyStore(string id)
        {
            using (var session = Store.OpenSession())
            {
                session.Load<RavenJObject>(id).ShouldNotBeNull();
            }
        }

        protected static void VerifyMessageSent(string messageName)
        {
            var messages = backendQ.GetAllMessages().ToList()
                .Where(m=>m.Extension.Length > 0)
                .Select(m =>
                                                           {
                                                               var stream = new MemoryStream(m.Extension);
                                                               var o = headerSerializer.Deserialize(stream)as List<HeaderInfo>;
                                                               var headers = new Dictionary<string, string>();

                                                               o.ForEach(hi => headers.Add(hi.Key,hi.Value));

                                                               return new
                                                                          {
                                                                              Message = m,
                                                                              Headers = headers
                                                                          };

                                                           }).ToList();

            messages.Any(h => h.Headers["NServiceBus.EnclosedMessageTypes"].Contains(messageName))
                .ShouldBeTrue();

        }

        private static readonly XmlSerializer headerSerializer = new XmlSerializer(typeof(List<HeaderInfo>));
    }
}