namespace Hygia.Specs
{
    using System;

    internal class FakeProviderInvoker : IInvokeProviders
    {
        readonly Func<dynamic> func;

        public FakeProviderInvoker(Func<dynamic> func)
        {
            this.func = func;
        }

        public dynamic Invoke<T>(dynamic parameters) where T : IProvide
        {
            return func();
        }
    }
}