using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hygia.FaultManagement
{
    using NServiceBus;
    using Operations.Events;
    using Raven.Client;

    public class FaultHistoryHandler:IHandleMessages<FaultMessageReceived>
    {
        public IDocumentSession Session { get; set; }
        public void Handle(FaultMessageReceived message)
        {
            var messageId = message.Headers["NServiceBus.OriginalId"].ToGuid();

            Session.Store(new Fault
                              {
                                  Id = messageId,
                                  FaultMessageId = message.FaultMessageId.ToGuid(),
                                  Body= message.Body,
                                  Exception = new ExceptionInfo
                                                  {
                                                      Message = message.Headers["NServiceBus.ExceptionInfo.Message"],
                                                      Reason = message.Headers["NServiceBus.ExceptionInfo.Reason"],
                                                      ExceptionType = message.Headers["NServiceBus.ExceptionInfo.ExceptionType"],
                                                      Source = message.Headers["NServiceBus.ExceptionInfo.Source"],
                                                      StackTrace = message.Headers["NServiceBus.ExceptionInfo.StackTrace"],
                                                  },
                                 Endpoint = message.Headers["NServiceBus.FailedQ"],
                                 EndpointId =message.Headers["NServiceBus.FailedQ"].ToGuid(),
                                 Headers = message.Headers
                              });
        }
    }

    public class ExceptionInfo
    {
        public string Message { get; set; }

        public string Reason { get; set; }

        public string ExceptionType { get; set; }

        public string Source { get; set; }

        public string StackTrace { get; set; }
    }

    public class Fault
    {
        public ExceptionInfo Exception{ get; set; }
        public Guid Id { get; set; }

        public Guid FaultMessageId { get; set; }

        public string Body { get; set; }

        public string Endpoint { get; set; }

        public Guid EndpointId { get; set; }

        public Dictionary<string, string> Headers { get; set; }
    }
}
