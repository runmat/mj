using System;
using GeneralTools.Services;

namespace SoapRuecklaeuferschnittstelle
{
    public class Global : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code, der beim Starten der Anwendung ausgeführt wird.

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code, der beim Herunterfahren der Anwendung ausgeführt wird.

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code, der beim Starten einer neuen Sitzung ausgeführt wird.

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code, der am Ende einer Sitzung ausgeführt wird. 
            // Hinweis: Das Session_End-Ereignis wird nur ausgelöst, wenn der sessionstate-Modus
            // in der Datei "Web.config" auf InProc festgelegt wird. Wenn der Sitzungsmodus auf StateServer festgelegt wird
            // oder auf SQLServer, wird das Ereignis nicht ausgelöst.

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            var logService = new LogService(String.Empty, String.Empty);
            logService.LogElmahError(exception, null);
        }
    }
}
