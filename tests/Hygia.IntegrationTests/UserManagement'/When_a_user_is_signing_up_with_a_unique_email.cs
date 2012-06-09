namespace Hygia.IntegrationTests.Operations.Faults
{
    using System;
    using Core;
    using Machine.Specifications;
    using RestSharp;

    [Subject("Api")]
    public class When_a_user_is_signing_up_with_a_unique_email : ApiContext
    {
        static string email = Guid.NewGuid() +  "@watchr.se";

        Establish context = () =>
        {
            request = new RestRequest("api/usermanagement/useraccounts", Method.POST) { RequestFormat = DataFormat.Json };

            request.AddBody(new {email});
            
        };

        It should_send_a_verification_email = () => VerifyMessageSent("SendEmailRequest");

        It should_create_a_account = () => VerifyStore("UserAccounts/" + email.ToGuid());
    }
}

