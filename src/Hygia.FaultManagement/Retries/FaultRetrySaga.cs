namespace Hygia.FaultManagement.Retries
{
    using System;
    using Hygia.FaultManagement.Commands;
    using Hygia.Core;
    using Hygia.FaultManagement.Events;
    using NServiceBus;
    using NServiceBus.Saga;
    using Hygia.Operations.Events;

    public class FaultRetrySaga : Saga<FaultRetrySagaData>, 
        IAmStartedByMessages<FaultRegistered>,
        IHandleMessages<AuditMessageReceived>
    {
        public void Handle(FaultRegistered message)
        {
            Data.FaultId = message.FaultId;
        }

        public void Handle(AuditMessageReceived message)
        {
            Data.RetriedSuccessFully = true;

            //todo - perhaps move this down to OPS?
            var resolvedTime =  message.Headers.ContainsKey("NServiceBus.SentTime") ? message.Headers["NServiceBus.SentTime"].ToUtcDateTime() : DateTime.UtcNow;
            Bus.Send<MarkFaultAsResolved>(m =>
                                              {
                                                  m.FaultId = Data.FaultId;
                                                  m.ResolvedAt = resolvedTime;
                                              });
        }

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<FaultRegistered>(s => s.FaultId, m => m.FaultId);
            ConfigureMapping<AuditMessageReceived>(s => s.FaultId, m => m.MessageId.ToGuid());
        }
    }
}