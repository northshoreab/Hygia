using System;
using System.Web.Http;

namespace Hygia.API.Controllers
{
    using Microsoft.IdentityModel.Claims;

    public abstract class AccountController : WatchRApiController
    {
        public Guid Account { get; set; }
        public Guid System { get; set; }
    }


    public class UserContext
    {
        public Guid UserId { get; set; }

        public IClaimsIdentity ClaimsIdentity { get; set; }
    }
}