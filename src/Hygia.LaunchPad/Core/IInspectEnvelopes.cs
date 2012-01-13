namespace Hygia.LaunchPad.Core
{
    using System.Collections.Generic;
    using NServiceBus.Unicast.Transport;

    public interface IInspectEnvelopes
    {
        IEnumerable<object> Inspect(TransportMessage transportMessage);
    }
}