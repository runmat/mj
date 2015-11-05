using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using GeneralTools.Models;

namespace UPSVersandWrapperService
{
    public class Global : HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            var zielsystem = ConfigurationManager.AppSettings["UPS_Zielsystem"];

            Common.UpsUrl = (zielsystem.NotNullOrEmpty().ToUpper() == "PROD" ? "https://onlinetools.ups.com/webservices/Ship" : "https://wwwcie.ups.com/webservices/Ship");
            Common.UpsUsername = ConfigurationManager.AppSettings["UPS_Username"];
            Common.UpsPassword = ConfigurationManager.AppSettings["UPS_Password"];
            Common.UpsAccessKey = ConfigurationManager.AppSettings["UPS_AccessKey"];
            Common.LogTable = ConfigurationManager.AppSettings["LogTableName"];
        }

        void Application_End(object sender, EventArgs e)
        {
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Wird ausgelöst, wenn ein Fehler auftritt.
            var LastError = Server.GetLastError();

            var logService = new GeneralTools.Services.LogService(String.Empty, String.Empty);
            logService.LogElmahError(LastError, null);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
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

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var context = app.Context;

            if (context.Request.ContentLength == 0)
                return;

            var filter = context.Response.Filter as ResponseFilterStream;

            var logService = new GeneralTools.Services.LogService(String.Empty, String.Empty);
            logService.LogWebServiceTraffic("Response", filter.ReadStream(), Common.LogTable);
        }

        void Session_Start(object sender, EventArgs e)
        {
        }

        void Session_End(object sender, EventArgs e)
        {
        }

        private static string GetString(Stream stream)
        {
            var bytes = new byte[stream.Length - 1];
            stream.Read(bytes, 0, bytes.Length);
            stream.Position = 0;
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
