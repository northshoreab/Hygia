using System;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.Operations.Communication.Domain;
using Raven.Client;

namespace Hygia.API.Controllers.Operations.LaunchPad
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/operations/launchpad/error")]
    public class ErrorController : ApiController
    {
        private readonly IDocumentSession _session;

        public ErrorController(IDocumentSession session)
        {
            _session = session;
        }

        public ErrorInputModel GetAll()
        {
            return new ErrorInputModel();
        }

        public string Post(ErrorInputModel model)
        {
            _session.Store(new LaunchPadError
                               {
                                   TimeOfReport = DateTime.UtcNow,
                                   StackTrace = model.StackTrace,
                                   Message = model.Message,
                                   InnerException = model.InnerException
                               });

            return "ok";
        }
    }

    public class ErrorInputModel
    {
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public Exception InnerException { get; set; }
    }
}