using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace AgisWebService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            var zielsystem = ConfigurationManager.AppSettings["Agis_Zielsystem"];

            switch (zielsystem)
            {
                case "TUI":
                    Common.AgisUrl = "https://tui-agis-services.audi.de/agis/adapter/ws";
                    Common.AgisCert = new X509Certificate2(Resources.TAMCDOE, "76WBLQMK");
                    break;

                case "PRELIVE":
                    Common.AgisUrl = "https://pre-agis-services.audi.de/agis/adapter/ws";
                    Common.AgisCert = new X509Certificate2(Resources.TAMCDOQ, "0Q5ET8N2");
                    break;

                case "LIVE":
                    Common.AgisUrl = "https://agis-services.audi.de/agis/adapter/ws";
                    Common.AgisCert = new X509Certificate2(Resources.TAMCDOP, "SNCJIIZA");
                    break;
            }

            Common.LogTable = ConfigurationManager.AppSettings["LogTableName"];
        }

        protected void Session_Start(object sender, EventArgs e)
        {

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

        private static string GetString(Stream stream)
        {
            var bytes = new byte[stream.Length - 1];
            stream.Read(bytes, 0, bytes.Length);
            stream.Position = 0;
            return Encoding.UTF8.GetString(bytes);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Wird ausgelöst, wenn ein Fehler auftritt.
            var LastError = Server.GetLastError();

            var logService = new GeneralTools.Services.LogService(String.Empty, String.Empty);
            logService.LogElmahError(LastError, null);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}