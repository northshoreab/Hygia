using System;
using System.Web.Http;
using NServiceBus;
using Raven.Client;

namespace Hygia.API.Controllers
{
    using Microsoft.IdentityModel.Claims;

    public abstract class AccountController : ApiController
    {
        public Guid Account { get; set; }
        public Guid System { get; set; }
    }


    public abstract class ApiController : System.Web.Http.ApiController
    {
        public IDocumentSession Session { get; set; }
        public IBus Bus { get; set; }
        public UserContext CurrentUser { get; set; }
    }

    public class UserContext
    {
        public Guid UserId { get; set; }

        public IClaimsIdentity ClaimsIdentity { get; set; }
    }
}