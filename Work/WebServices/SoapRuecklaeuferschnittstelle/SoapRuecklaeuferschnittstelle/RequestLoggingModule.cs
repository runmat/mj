using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using GeneralTools.Services;

namespace SoapRuecklaeuferschnittstelle
{
    public class RequestLoggingModule : IHttpModule
    {
        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
            context.EndRequest += context_EndRequest;
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var context = app.Context;

            if (context.Request.ContentLength == 0)
                return;

            var request = context.Request;
            var response = context.Response;

            var filter = new ResponseFilterStream(response.Filter);
            response.Filter = filter;

            var logService = new LogService(String.Empty, String.Empty);
            logService.LogWebServiceTraffic("Request", GetString(request.InputStream), ConfigurationManager.AppSettings["LogTableName"]);
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var context = app.Context;

            if (context.Request.ContentLength == 0)
                return;

            var filter = context.Response.Filter as ResponseFilterStream;

            var logService = new LogService(String.Empty, String.Empty);
            logService.LogWebServiceTraffic("Response", filter.ReadStream(), ConfigurationManager.AppSettings["LogTableName"]);
        }

        private string GetString(Stream stream)
        {
            var bytes = new Byte[stream.Length - 1];
            stream.Read(bytes, 0, bytes.Length);
            stream.Position = 0;
            return Encoding.UTF8.GetString(bytes);
        }
    }
}