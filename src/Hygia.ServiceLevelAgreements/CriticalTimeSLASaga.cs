namespace Hygia.ServiceLevelAgreements
{
    using System;
    using Commands;
    using Domain;
    using Events;
    using NServiceBus.Saga;
    using Raven.Client;

    public class CriticalTimeSLASaga : Saga<CriticalTimeSLASagaData>,
                                       IAmStartedByMessages<VerifySLA>,
                                       IHandleTimeouts<SLAGracePeriodOver>
    {
        public IDocumentSession Session { get; set; }

        public void Handle(VerifySLA message)
        {
            Data.MessageTypeId = message.MessageTypeId;

            var criticalTimeSLA = GetMessageTypeSLA();

            if (message.CriticalTime <= criticalTimeSLA)
                return;


            Data.SLABreachedAt = message.ProcessedAt;

            if (Data.SLABreached)
                return;

            Data.SLABreached = true;

            Bus.Publish(new CriticalTimeSLAForMessageTypeViolated
                            {
                                MessageTypeId = Data.MessageTypeId,
                                ActiveSLA = criticalTimeSLA,
                                ActualCriticalTime = message.CriticalTime,
                                TimeOfSLABreach = message.ProcessedAt
                            });

            //todo - add a signature to nsb
            RequestGracePeriodTimeout();

        }

        void RequestGracePeriodTimeout()
        {
            RequestUtcTimeout(TimeSpan.FromMinutes(SLAGracePeriodMinutes), new SLAGracePeriodOver
                                                           {
                                                               TimeOfBreach = Data.SLABreachedAt
                                                           });
        }

        public void Timeout(SLAGracePeriodOver state)
        {
            //did we break the sla while we where sleeping?
            if (Data.SLABreachedAt != state.TimeOfBreach)
            {
                RequestGracePeriodTimeout();
                return;
            }

            //sla hasn't been breaked since we went to sleep
            Data.SLABreached = false;
            Bus.Publish(new CriticalTimeSLAForMessageTypeRestored
                            {
                                MessageTypeId = Data.MessageTypeId
                            });
        }

        TimeSpan GetMessageTypeSLA()
        {
            //todo - move this out to a config setting to avoid calling the db all the time
            var environmentSLA = Session.Load<EnvironmentSLA>("Environment/ServiceLevelAgreement");

            if (environmentSLA == null)
                return TimeSpan.MaxValue;

            var criticalTimeSLA = environmentSLA.DefaultCriticalTimeSLA;

            //todo - do this be subscribing to a event when NSB adds support for multi event sagas
            var messageSpecificSLA = Session.Load<MessageTypeSLA>(Data.MessageTypeId);

            if (messageSpecificSLA != null)
                criticalTimeSLA = messageSpecificSLA.CriticalTimeSLA;
            return criticalTimeSLA;
        }

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<VerifySLA>(s => s.MessageTypeId, m => m.MessageTypeId);
        }

        public static int SLAGracePeriodMinutes { get; set; }

        static CriticalTimeSLASaga()
        {
            SLAGracePeriodMinutes = 5;
        }
    }
}