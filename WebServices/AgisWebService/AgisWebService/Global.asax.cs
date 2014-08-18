using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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