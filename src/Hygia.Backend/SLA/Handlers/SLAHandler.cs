using System.Collections.Generic;
using Hygia.Backend.SLA.Domain;
using Hygia.Backend.SLA.Events;
using Hygia.PhysicalMonitoring.Commands;
using NServiceBus;
using Raven.Client;

namespace Hygia.Backend.SLA.Handlers
{
    public class SLAHandler : IHandleMessages<EnvelopeRegistered>
    {
        private readonly IBus _bus;
        public IDocumentSession Session { get; set; }

        private readonly IList<Domain.SLA> _slas;

        public SLAHandler(IBus bus)
        {
            _bus = bus;
            _slas = Session.Load<Domain.SLA>();
        }

        public void Handle(EnvelopeRegistered message)
        {
            foreach (var sla in _slas)
            {
                if(sla.Rule.Execute(message.RegisteredEnvelope))
                {
                    Session.Store(new SLAViolation
                                      {
                                          EnvelopeId = message.RegisteredEnvelope.Id,
                                          SLAId = sla.Id
                                      });

                    _bus.Publish(new SLAViolated
                                     {
                                         SLA = sla,
                                         Envelope = message.RegisteredEnvelope
                                     });
                }
            }
        }
    }
}
