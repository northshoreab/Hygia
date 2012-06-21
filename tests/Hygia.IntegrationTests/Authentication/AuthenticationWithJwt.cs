using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Hygia.API.Infrastructure.Authentication;
using Machine.Specifications;

namespace Hygia.IntegrationTests.Authentication
{
    [Subject("ApiAuthenticationJWT")]
    public class AuthenticationWithJwt
    {
        private static HttpResponseMessage response;
        private Establish context = () => { };

        private Because of = () =>
                                 {

                                     var jwt = AuthenticationHelper.CreateJsonWebToken("test", "test");
                                     var client = new HttpClient { BaseAddress = new Uri("http://localhost:38105/") };
                                     client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", jwt);

                                     response = client.GetAsync("api/faultmanagement").Result;
                                     
                                 };

        It should_be_successfull = () => response.EnsureSuccessStatusCode();
    }
}