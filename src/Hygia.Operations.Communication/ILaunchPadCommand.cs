namespace Hygia.Operations.Communication
{
    public interface ILaunchPadCommand
    {
        void Send(object command);
    }
}