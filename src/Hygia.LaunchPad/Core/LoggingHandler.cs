namespace Hygia.LaunchPad.Core
{
    using System;
    using NServiceBus;

    public class LoggingHandler:IHandleMessages<object>
    {
        readonly IBus bus;

        public LoggingHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(object message)
        {
            Console.WriteLine(bus.CurrentMessageContext.Id + " - " + message);
        }
    }
}