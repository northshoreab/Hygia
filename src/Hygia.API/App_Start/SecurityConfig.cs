using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Hygia.API.Authentication;
using Microsoft.IdentityModel.Claims;
using Newtonsoft.Json;
using Raven.Client;
using Raven.Client.Linq;
using StructureMap;
using Thinktecture.IdentityModel.Claims;
using Thinktecture.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens.Http;

namespace Hygia.API.App_Start
{
    public class UserDatabaseAccess
    {
        public bool ReadOnly { get; set; }
        public bool Admin { get; set; }
        public string Name { get; set; }
    }

    public class AuthenticationUser
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public bool Admin { get; set; }
        public string[] AllowedDatabases { get; set; }

        public UserDatabaseAccess[] Databases { get; set; }

        protected string HashedPassword { get; private set; }

        private Guid passwordSalt;

        protected Guid PasswordSalt
        {
            get
            {
                if (passwordSalt == Guid.Empty)
                    passwordSalt = Guid.NewGuid();
                return passwordSalt;
            }
            set { passwordSalt = value; }
        }


        public AuthenticationUser SetPassword(string pwd)
        {
            HashedPassword = GetHashedPassword(pwd);
            return this;
        }

        private string GetHashedPassword(string pwd)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = PasswordSalt.ToByteArray().Concat(Encoding.Unicode.GetBytes(pwd)).ToArray();

                return Convert.ToBase64String(sha.ComputeHash(bytes));
            }
        }

        public bool ValidatePassword(string maybePwd)
        {
            return HashedPassword == GetHashedPassword(maybePwd);
        }

    }

    public static class SecurityConfig
    {
        public static AuthenticationConfiguration AuthenticationConfiguration { get; set; } 

        static SecurityConfig()
        {
            AuthenticationConfiguration = ConfigureAuthentication();
        }

        public static void ConfigureGlobal(HttpConfiguration globalConfig)
        {
            globalConfig.MessageHandlers.Add(new AuthenticationHandler(AuthenticationConfiguration));
            //globalConfig.SetAuthorizationManager(new AuthorizationManager());
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

            config.AddBasicAuthentication(ValidateUser);

            #endregion

            var handler = new SimpleSecurityTokenHandler(Constants.GithubScheme, token =>
                                                                                     {
                                                                                         var githubToken = JsonConvert.DeserializeObject<GithubLoginToken>(token);
                                                                                         if (githubToken.LoginKey == Constants.GithubLoginKey)
                                                                                         {
                                                                                             return IdentityFactory.Create("Github",
                                                                                                                           new Claim(Constants.ClaimTypes.GithubAccessToken, githubToken.AccessToken),
                                                                                                                           new Claim(ClaimTypes.Name, githubToken.UserName),
                                                                                                                           AuthenticationInstantClaim.Now);
                                                                                        }

                                                                                         return null;
                                                                                     });

            config.AddAccessKey(handler, AuthenticationOptions.ForAuthorizationHeader("github"));

            //#region IdSrv Simple Web Tokens
            //config.Handler.AddSimpleWebToken(
            //    "IdSrv",
            //    "http://identity.thinktecture.com/trust",
            //    Constants.Realm,
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