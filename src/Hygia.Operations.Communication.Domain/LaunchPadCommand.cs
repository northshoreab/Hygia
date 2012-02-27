namespace Hygia.Operations.Communication.Domain
{
    using System;

    public class LaunchPadCommand
    {
        public Guid Id { get; set; }
        public bool Delivered { get; set; }
    }
}