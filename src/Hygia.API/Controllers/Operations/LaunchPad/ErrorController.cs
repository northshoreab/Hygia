using System;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using Hygia.Operations.Communication.Domain;
using Raven.Client;

namespace Hygia.API.Controllers.Operations.LaunchPad
{
    [DefaultHttpRouteConvention]
    [RoutePrefix("api/{environment}/operations/launchpad/error")]
    [Authorize]
    public class ErrorController : EnvironmentController
    {
        public ErrorInputModel GetAll()
        {
            return new ErrorInputModel();
        }

        public string Post(ErrorInputModel model)
        {
            Session.Store(new LaunchPadError
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