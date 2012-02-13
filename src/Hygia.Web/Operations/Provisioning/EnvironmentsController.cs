namespace Hygia.Web.Operations.Provisioning
{
    using System;
    using FubuMVC.Core;
    using Raven.Client;

    public class EnvironmentsController
    {
        readonly IDocumentSession _session;

        public EnvironmentsController(IDocumentSession session)
        {
            _session = session;
        }

        [JsonEndpoint]
        public Environment post_provisionenvironment(ProvisionModel model)
        {
            var tennant = _session.Load<Tennant>(model.TennantId.ToString());

            if (tennant == null)
                throw new InvalidOperationException("Tennant not found");

            var environmentId = Guid.NewGuid();

            var environment = new Environment
                                  {
                                      Id = environmentId,
                                      Name = model.Name,
                                      ApiKey = environmentId
                                  };
            tennant.Environments.Add(environment);
            _session.Store(tennant);

            return environment;
        }
    }



    public class ProvisionModel
    {
        public Guid TennantId { get; set; }
        public string Name { get; set; }
    }
}