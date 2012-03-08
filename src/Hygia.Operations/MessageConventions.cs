namespace Hygia
{
    using NServiceBus;
    using NServiceBus.Saga;

    public static class MessageConventions
    {
         public static Configure HygiaMessageConventions(this Configure config)
         {
             return config.DefiningMessagesAs(t => t.Namespace != null && (t.Namespace.EndsWith(".Messages") || typeof(ITimeoutState).IsAssignableFrom(t)))
                 .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Commands"))
                 .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Events"));
         }
    }
}