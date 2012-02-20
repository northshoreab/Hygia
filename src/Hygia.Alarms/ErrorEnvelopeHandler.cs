using System;
using Hygia.Alarms.Events;
using NServiceBus;

namespace Hygia.Alarms
{
    //This class is just for test purposes, should use the real error message
    public class ErrorMessage
    {
        public Guid MessageTypeId { get; set; }
    }

    public class ErrorEnvelopeHandler : IHandleMessages<ErrorMessage>
    {
        private readonly IBus _bus;

        public ErrorEnvelopeHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(ErrorMessage message)
        {
            _bus.Send(new ErrorMessageAlarm
                          {
                              MessageTypeId = message.MessageTypeId
                          });
        }
    }
}
