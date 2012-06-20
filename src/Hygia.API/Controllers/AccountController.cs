using System;
using System.Web.Http;
using NServiceBus;
using Raven.Client;

namespace Hygia.API.Controllers
{
    public abstract class AccountController : ApiController
    {
        public IDocumentSession Session { get; set; }
        public IBus Bus { get; set; }
        public Guid Account { get; set; }        
    }
}