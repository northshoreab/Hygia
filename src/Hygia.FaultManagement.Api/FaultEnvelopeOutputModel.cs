namespace Hygia.FaultManagement.Api
{
    using System;

    public class FaultEnvelopeOutputModel
    {
        public Guid FaultId { get; set; }
        public long FaultNumber { get; set; }
        public string Title 
        {
            get
            {
                if (!string.IsNullOrEmpty(ExceptionMessage))
                {
                    if (ExceptionMessage.Length > 30)
                    {
                        return ExceptionMessage.Substring(0, 27) + "...";
                    }

                    return ExceptionMessage;
                }

                return string.Empty;
            }
        }
        public string ExceptionMessage { get; set; }
        public string TimeSent { get; set; }        
        public int Retries { get; set; }
        public string EnclosedMessageTypes { get; set; }                
        public string BusinessService { get; set; }
    }
}