namespace Hygia.Specs
{
    using System;
    using Machine.Specifications;

    public class WithSaga<T,TSagaData>:WithHandler<T>
    {
        protected static TSagaData SagaData;


        Establish context = () =>
                                {
                                    SagaData = Activator.CreateInstance<TSagaData>();

                                    typeof(T).GetProperty("Data").SetValue(Handler, SagaData, null);
                                };
    }
}