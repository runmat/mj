using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using GeneralTools.Contracts;
using GeneralTools.Services;

namespace AutohausRestService.Models
{
    public class RestExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private ILogService _logService;

        public override void OnException(HttpActionExecutedContext context)
        {
            if (_logService == null)
                _logService = new LogService(string.Empty, string.Empty);

            _logService.LogElmahError(context.Exception, null);

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(context.Exception.Message),
                ReasonPhrase = "Exception"
            });
        }
    }
}
