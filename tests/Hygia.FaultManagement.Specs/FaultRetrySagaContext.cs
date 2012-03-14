namespace Hygia.FaultManagement.Specs
{
    using System;
    using Hygia.Specs;
    using Retries;

    public class FaultRetrySagaContext:WithSaga<FaultRetrySaga, FaultRetrySagaData>
    {
        protected static readonly Guid faultId = Guid.NewGuid();

    }
}