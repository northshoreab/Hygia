using Hygia.Operations.Communications.LaunchPad;

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

            var key = ConfigurationManager.AppSettings["watchr.apikey"];

            if (string.IsNullOrEmpty(key))
                OnPremiseMode();
            else
                CloudMode(key);

            Configure.Instance.Configurer.ConfigureComponent<LaunchPadCommandPersister>(DependencyLifecycle.SingleInstance);
            Configure.Instance.Configurer.ConfigureComponent<TransportFactory>(DependencyLifecycle.SingleInstance);
        }

        void CloudMode(string apikey)
        {
            var apiUrl = ConfigurationManager.AppSettings["watchr.apiurl"];

            if (string.IsNullOrEmpty(apiUrl))
                apiUrl = "http://api.watchr.se";


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