namespace Hygia.LaunchPad
{
    using System;
    using NServiceBus;
    using Operations.AuditUploads.Messages;

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server,IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                .HygiaMessageConventions()
                .StructureMapBuilder()
                .XmlSerializer()
                .RunGatewayWithInMemoryPersistence();
        }
    }

}