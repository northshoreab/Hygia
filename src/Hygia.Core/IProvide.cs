namespace Hygia
{
    public interface IProvide
    {
        dynamic ProvideFor(dynamic parameters);
    }

    public interface IInvokeProviders
    {
        dynamic Invoke<T>(dynamic parameters) where T : IProvide;
    }
}