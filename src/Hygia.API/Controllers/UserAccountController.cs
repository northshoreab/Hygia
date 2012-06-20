using System;
using System.Web.Http;
using NServiceBus;
using Raven.Client;

namespace Hygia.API.Controllers
{
    public abstract class UserAccountController : ApiController
    {
        public IDocumentSession Session { get; set; }
        public IBus Bus { get; set; }
        public Guid UserAccountId { get; set; }        
    }
}