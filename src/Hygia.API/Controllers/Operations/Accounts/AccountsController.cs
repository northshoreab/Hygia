namespace Hygia.API.Controllers.Operations.Accounts
{
    using System;
    using System.Linq;
    using Events;
    using FubuMVC.Core;

    public class AccountsController:WebController
    {
        [JsonEndpoint]
        public Account post_accounts_create(CreateAccountModel model)
        {
            var account = Session.Query<Account>().FirstOrDefault(t=>t.Name == model.Name);

            if (account != null)
                throw new InvalidOperationException("A Account with that name already exists");

            account = new Account
                                  {
                                      Id = model.AccountId,
                                      Name = model.Name
                                  };



            Session.Store(account);
           
            Bus.Publish<AccountCreated>(m=>
                                            {
                                                m.AccountId = account.Id;
                                            });
            return account;
        }


        public class CreateAccountModel
        {
            public Guid AccountId { get; set; }
            public string Name { get; set; }
        }
    }
}