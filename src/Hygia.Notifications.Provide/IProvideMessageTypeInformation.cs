namespace Hygia.Notifications.Provide
{
    using System;

    public interface IProvideMessageTypeInformation : IProvide
    {
        dynamic ProvideFor(Guid messageTypeId);
    }
}