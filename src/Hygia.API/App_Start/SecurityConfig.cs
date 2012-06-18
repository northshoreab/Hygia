using System.Web.Http;
using Hygia.API.Infrastructure.Authentication;
using Thinktecture.IdentityModel.Tokens.Http;

namespace Hygia.API.App_Start
{
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

            config.AddBasicAuthentication(AuthenticationHelper.ValidateUser);

            config.AddAccessKey(AuthenticationHelper.GetApiKeyIdentity, AuthenticationOptions.ForHeader("apikey"));

            config.AddJsonWebToken(
                issuer: "http://watchr.se",
                audience: Constants.Realm,
                signingKey: Constants.JWTKeyEncoded,
                options: AuthenticationOptions.ForAuthorizationHeader("JWT"));

            return config;
        }
    }
}