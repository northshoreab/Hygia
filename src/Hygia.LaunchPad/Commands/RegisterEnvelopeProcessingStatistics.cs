namespace Hygia.LaunchPad.Commands
{
    using System;

    public class RegisterEnvelopeProcessingStatistics
    {
        public double ProcessingTime { get; set; }
   
        public Guid EnvelopeId { get; set; }

        public double CriticalTime { get; set; }

        public override string ToString()
        {
            return string.Format("Registering statistics for envelope {0}, CriticalTime:{1}(s), ProcessingTime{2}",
                                 EnvelopeId, CriticalTime, ProcessingTime);
        }
    }
}