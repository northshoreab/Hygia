namespace Hygia.LaunchPad.AuditProcessing
{
    using System;
    using global::NServiceBus;

    public class LoggingHandler:IHandleMessages<object>
    {
        public void Handle(object message)
        {
            Console.WriteLine(message.ToString());
        }
    }
}