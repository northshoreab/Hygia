using System.Linq;
using System.Web.Http;
using Hygia.API.Authentication;
using Raven.Bundles.Authentication;
using Raven.Client;
using Raven.Client.Linq;
using StructureMap;
using Thinktecture.IdentityModel.Http;

namespace Hygia.API.App_Start
{
    public static class SecurityConfig
    {
        public static void ConfigureGlobal(HttpConfiguration globalConfig)
        {
            globalConfig.MessageHandlers.Add(new AuthenticationHandler(ConfigureAuthentication()));
            globalConfig.SetAuthorizationManager(new AuthorizationManager());
        }

        public static bool ValidateUser(string userName, string password)
        {
            var session = ObjectFactory.GetInstance<IDocumentStore>().OpenSession();

            return session.Query<AuthenticationUser>().Where(x => x.Name == userName).Single().ValidatePassword(password);
        }

        public static AuthenticationConfiguration ConfigureAuthentication()
        {
            var config = new AuthenticationConfiguration
                             {
                                 // sample claims transformation for consultants sample, comment out to see raw claims
                                 ClaimsAuthenticationManager = new ClaimsTransformer(),

                                 // value of the www-authenticate header, if not set, the first scheme added to the handler collection is used
                                 DefaultAuthenticationScheme = "Basic"
                             };

            #region Basic Authentication

            config.Handler.AddBasicAuthentication(ValidateUser);
            #endregion


            //#region IdSrv Simple Web Tokens
            //config.Handler.AddSimpleWebToken(
            //    "IdSrv",
            //    "http://identity.thinktecture.com/trust",
            //    Constants.Realm,
                                     437cb66acfaf3e854d3342e8562b0fe9
            //    "Dc9Mpi3jbooUpBQpB/4R7XtUsa3D/ALSjTVvK8IUZbg=");
            //#endregion

            //#region ACS Simple Web Tokens
            //config.Handler.AddSimpleWebToken(
            //    "ACS",
            //    "https://" + Constants.ACS + "/",
            //    Constants.Realm,
            //    "yFvxu8Xkmo/xBSSPrzqZLSAiB4lgjR4PIi0Bn1RsUDI=");
            //#endregion

            //#region ADFS SAML tokens
            //// SAML via ADFS
            //var registry = new ConfigurationBasedIssuerNameRegistry();
            //registry.AddTrustedIssuer("8EC7F962CC083FF7C5997D8A4D5ED64B12E4C174", "ADFS");

            //var adfsConfig = new SecurityTokenHandlerConfiguration();
            //adfsConfig.AudienceRestriction.AllowedAudienceUris.Add(new Uri(Constants.Realm));
            //adfsConfig.IssuerNameRegistry = registry;
            //adfsConfig.CertificateValidator = X509CertificateValidator.None;

            //// token decryption (read from configuration section)
            //adfsConfig.ServiceTokenResolver = FederatedAuthentication.ServiceConfiguration.CreateAggregateTokenResolver();

            //config.Handler.AddSaml11SecurityToken("AdfsSaml", adfsConfig);
            ////manager.AddSaml2SecurityTokenHandler("AdfsSaml", adfsConfig);

            //#endregion

            //#region IdSrv SAML tokens
            //// SAML via IdSrv
            //var idsrvRegistry = new ConfigurationBasedIssuerNameRegistry();
            //idsrvRegistry.AddTrustedIssuer("A1EED7897E55388FCE60FEF1A1EED81FF1CBAEC6", "Thinktecture IdSrv");

            //var idsrvConfig = new SecurityTokenHandlerConfiguration();
            //idsrvConfig.AudienceRestriction.AllowedAudienceUris.Add(new Uri(Constants.Realm));
            //idsrvConfig.IssuerNameRegistry = idsrvRegistry;
            //idsrvConfig.CertificateValidator = X509CertificateValidator.None;

            //// token decryption (read from configuration section)
            //idsrvConfig.ServiceTokenResolver = FederatedAuthentication.ServiceConfiguration.CreateAggregateTokenResolver();

            //config.Handler.AddSaml2SecurityToken("IdSrvSaml", idsrvConfig);

            //#endregion

            return config;
        }
    }
}