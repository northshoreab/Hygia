namespace Hygia.FaultManagement.LaunchPad
{
    using System;
    using Commands;
    using NServiceBus;
    using NServiceBus.Config;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class FaultCommandConverter : JsonCreationConverter<RetryFault>,INeedInitialization
    {
        protected override RetryFault Create(Type objectType, JObject jObject)
        {
            //todo - add more commands or figure out how to make the json parser do it for us
            return new RetryFault();
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }

        public void Init()
        {
            Configure.Instance.Configurer.RegisterSingleton<JsonConverter>(new FaultCommandConverter());
        }
    }
}