namespace Hygia.FaultManagement.Api
{
    using System.Linq;
    using Domain;
    using FubuMVC.Core;
    using Raven.Client;

    public class FaultsController
    {
        public IDocumentSession Session { get; set; }
       
        [JsonEndpoint]
        public dynamic get_faults()
        {
            return Session.Query<Fault>()
                .Where(f=>f.Status != FaultStatus.Archived)
                .ToList();
        }
    }
}