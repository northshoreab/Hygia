using System;
using Hygia.PhysicalMonitoring.Domain;

namespace Hygia.Backend.SLA.Domain
{
    public class Rule : IRule
    {
        public Func<bool> RuleExpression { get; protected set; }

        public Envelope Envelope { get; set; }
        
        public bool Execute(Envelope envelope)
        {
            Envelope = envelope;
            return RuleExpression.Invoke();
        }
    }
}