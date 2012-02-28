using System;

namespace Hygia.Operations.Communication
{
    public interface ILaunchPadCommand
    {
        Guid EnvironmentId { get; }
    }
}