using System.Data;

namespace AppZulassungsdienst.lib
{
    public abstract class ErrorHandlingClass
    {
        protected string E_MESSAGE = "";
        protected string E_SUBRC = "";
        protected DataTable GT_MESSAGE;
        protected bool bError = false;

        public string ErrorMessage
        {
            get { return E_MESSAGE; }
        }

        public string ErrorCode
        {
            get { return E_SUBRC; }
        }

        public DataTable ErrorTable
        {
            get { return GT_MESSAGE; }
        }

        public bool ErrorOccured
        {
            get { return bError; }
        }

        /// <summary>
        /// Löst einen Fehler aus und füllt die entsprechenden Statuswerte
        /// </summary>
        /// <param name="subrc">Subrc - Fehlernummer</param>
        /// <param name="message">Fehlertext</param>
        /// <remarks></remarks>
        public void RaiseError(string subrc, string message)
        {
            bError = true;
            E_SUBRC = subrc;
            E_MESSAGE = message;
            GT_MESSAGE = null;
        }

        /// <summary>
        /// Löst einen Fehler aus und füllt die entsprechenden Statuswerte
        /// </summary>
        /// <param name="subrc">Subrc - Fehlernummer</param>
        /// <param name="message">Fehlertext</param>
        /// <param name="errortable">Fehlertabelle</param>
        /// <remarks></remarks>
        public void RaiseError(string subrc, string message, DataTable errortable)
        {
            bError = true;
            E_SUBRC = subrc;
            E_MESSAGE = message;
            GT_MESSAGE = errortable;
        }

        /// <summary>
        /// Setzt den Fehlerstatus komplett zurück
        /// </summary>
        /// <remarks></remarks>
        public void ClearErrorState()
        {
            E_MESSAGE = "";
            E_SUBRC = "0";
            GT_MESSAGE = null;
            bError = false;
        }
    }
}
