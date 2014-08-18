using System;
using System.IO;
using System.Text;
using System.Web;

namespace AgisWebService
{
    public class RequestLoggingModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += App_BeginRequest;
            context.EndRequest += App_EndRequest;
        }

        public void Dispose()
        {
        }

        protected void App_BeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var context = app.Context;

            if (context.Request.ContentLength == 0)
                return;

            var request = context.Request;
            var response = context.Response;

            response.Filter = new ResponseFilterStream(response.Filter);

            var logService = new GeneralTools.Services.LogService(String.Empty, String.Empty);
            logService.LogWebServiceTraffic("Request", GetString(request.InputStream), Common.LogTable);
        }

        protected void App_EndRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var context = app.Context;

            if (context.Request.ContentLength == 0)
                return;

            var filter = context.Response.Filter as ResponseFilterStream;

            var logService = new GeneralTools.Services.LogService(String.Empty, String.Empty);
            logService.LogWebServiceTraffic("Response", filter.ReadStream(), Common.LogTable);
        }

        private string GetString(Stream stream)
        {
            var bytes = new byte[stream.Length - 1];
            stream.Read(bytes, 0, bytes.Length);
            stream.Position = 0;
            return Encoding.UTF8.GetString(bytes);
        }
    }
}