namespace Hygia.PhysicalMonitoring.Index
{
    using System.Linq;
    using Domain;
    using Raven.Client.Indexes;    
    
    public class MessgeTypeInstanceReduceResult : MessgeTypeInstance
    {
        public int Count { get; set; }
    }

    public class MessageTypeInstancePerMinute : AbstractIndexCreationTask<MessgeTypeInstance, MessgeTypeInstanceReduceResult>
    {
        public MessageTypeInstancePerMinute()
        {
            Map = messages => from message in messages
                               select new 
                               {
                                   message.MessageTypeId,
                                   message.MessageId,
                                   Count = 1
                               };

            Reduce = results => from result in results
                                group result by new
                                {
                                    result.MessageTypeId,
                                    result.ProcessingEnded.Minute,
                                    result.ProcessingEnded.Hour,
                                    result.ProcessingEnded.Day,
                                    result.ProcessingEnded.Month,
                                    result.ProcessingEnded.Year
                                }
                                    into agg
                                    select new 
                                    {
                                        agg.Key.MessageTypeId,                                        
                                        Count = agg.Sum(x => x.Count)
                                    };
        }
    }
}
