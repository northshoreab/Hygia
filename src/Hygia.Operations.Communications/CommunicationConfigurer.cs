namespace Hygia.Operations.Communications
{
    using System.Configuration;
    using Cloud;
    using NServiceBus;
    using NServiceBus.Config;

    public class CommunicationConfigurer:INeedInitialization
    {
        public void Init()
        {

            var key = ConfigurationManager.AppSettings["hygia.apikey"];

            if (string.IsNullOrEmpty(key))
                OnPremiseMode();
            else
                CloudMode(key);

           
        }

        void CloudMode(string apikey)
        {
            var apiUrl = ConfigurationManager.AppSettings["hygia.apiurl"];

            if (string.IsNullOrEmpty(apiUrl))
                throw new ConfigurationErrorsException("Hygia.apiurl is required");


            Configure.Instance.Configurer.ConfigureComponent<CloudUploader>(DependencyLifecycle.SingleInstance)
                .ConfigureProperty(p => p.ApiUrl, apiUrl)
                .ConfigureProperty(p => p.ApiKey, apikey);
        }

        void OnPremiseMode()
        {
            throw new System.NotImplementedException();
        }
    }
}