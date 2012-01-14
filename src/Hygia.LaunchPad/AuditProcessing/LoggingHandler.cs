namespace Hygia.LaunchPad.AuditProcessing
{
    using System;
    using NServiceBus;

    public class LoggingHandler:IHandleMessages<object>
    {
        public void Handle(object message)
        {
            Console.WriteLine(message.ToString());
        }
    }
}