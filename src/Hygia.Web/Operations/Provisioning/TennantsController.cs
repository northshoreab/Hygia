namespace Hygia.Web.Operations.Provisioning
{
    using System;
    using System.Linq;
    using FubuMVC.Core;
    using Raven.Client;

    public class TennantsController
    {
        readonly IDocumentSession _session;

        public TennantsController(IDocumentSession session)
        {
            _session = session;
        }

        [JsonEndpoint]
        public Tennant post_provisiontennant(ProvisionTennantModel model)
        {
            var tennant = _session.Query<Tennant>().FirstOrDefault(t=>t.Name == model.Name);

            if (tennant != null)
                throw new InvalidOperationException("A Tennant with that name already exists");

            tennant = new Tennant
                                  {
                                      Id = model.TennantId.ToString(),
                                      Name = model.Name
                                  };



            _session.Store(tennant);

            return tennant;
        }


        public class ProvisionTennantModel
        {
            public Guid TennantId { get; set; }
            public string Name { get; set; }
        }
    }
}