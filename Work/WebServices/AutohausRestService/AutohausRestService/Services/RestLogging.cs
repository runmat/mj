using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GeneralTools.Services;

namespace AutohausRestService.Services
{
    public class RestLoggingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                      CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(task =>
            {
                var logService = new LogService(String.Empty, String.Empty);

                logService.LogWebServiceTraffic("Request", request.ToString(), ConfigurationService.LogTableName);

                var response = task.Result;

                logService.LogWebServiceTraffic("Response", response.ToString(), ConfigurationService.LogTableName);

                return response;
            });
        }
    }
}