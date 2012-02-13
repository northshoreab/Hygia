namespace Hygia.Backend.SLA.Domain
{
    public class SLA
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IRule Rule { get; set; }
    }
}