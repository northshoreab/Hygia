namespace Hygia.Operations.Faults
{
    using Commands;
    using Events;
    using NServiceBus;

    public class ProcessFaultMessageHandler : IHandleMessages<ProcessFaultMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(ProcessFaultMessage message)
        {
            //todo - de duplicate

            Bus.Publish<FaultMessageReceived>(e =>
                                                  {
                                                      e.FaultMessageId = message.MessageId;
                                                      e.Headers = message.Headers;
                                                      e.Body = message.Body;
                                                  });

        }
    }
}