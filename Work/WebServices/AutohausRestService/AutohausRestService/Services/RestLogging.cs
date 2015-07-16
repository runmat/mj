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
            var logService = new LogService(String.Empty, String.Empty);

            var requestContent = request.Content.ReadAsStringAsync().Result;

            logService.LogWebServiceTraffic("Request", requestContent, ConfigurationService.LogTableName);

            return base.SendAsync(request, cancellationToken).ContinueWith(task =>
            {
                var response = task.Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;

                logService.LogWebServiceTraffic("Response", responseContent, ConfigurationService.LogTableName);

                return response;
            });
        }
    }
}