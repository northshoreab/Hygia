using System.Linq;
using Hygia.PhysicalMonitoring.Domain;
using Raven.Client.Indexes;

namespace Hygia.Backend.Infrastructure
{
    public class EnvelopeReduceResult : Envelope
    {
        public int Count { get; set; }
    }

    public class MessageTypePerMinuteIndex : AbstractIndexCreationTask<Envelope, EnvelopeReduceResult>
    {
        public MessageTypePerMinuteIndex()
        {
            Map = envelopes => from envelope in envelopes
                               select new
                                          {
                                              envelope.ContainedMessages.First().MessageTypeId,
                                              //envelope.TimeSent,
                                              Count = 1
                                          };

            Reduce = results => from result in results
                                group result by new
                                                    {
                                                        result.ContainedMessages.First().MessageTypeId,
                                                        result.TimeSent.Value.Minute,
                                                        result.TimeSent.Value.Hour,
                                                        result.TimeSent.Value.Day,
                                                        result.TimeSent.Value.Month,
                                                        result.TimeSent.Value.Year
                                                    }
                                into aggregate
                                select new
                                           {
                                               aggregate.Key.MessageTypeId,                                               
                                               Count = aggregate.Sum(x => x.Count)
                                           };
        }
    }
}