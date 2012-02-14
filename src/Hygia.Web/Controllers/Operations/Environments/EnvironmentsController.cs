namespace Hygia.Web.Controllers.Operations.Environments
{
    using System;
    using Accounts;
    using FubuMVC.Core;
    using Environment = Accounts.Environment;

    public class EnvironmentsController:WebController
    {
        [JsonEndpoint]
        public Environment post_environments_create(ProvisionEnvironmentModel environmentModel)
        {
            var account = Session.Load<Account>(environmentModel.AccountId);

            if (account == null)
                throw new InvalidOperationException("Account not found");

            var environmentId = Guid.NewGuid();

            var environment = new Environment
                                  {
                                      Id = environmentId,
                                      Name = environmentModel.Name,
                                      ApiKey = environmentId
                                  };
            account.Environments.Add(environment);
            Session.Store(account);

            return environment;
        }
    }


    public class ProvisionEnvironmentModel
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
    }
}