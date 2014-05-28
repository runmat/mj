using System;
using SapORM.Contracts;

namespace SapORM.Services
{
    /// <summary>
    ///  Laden, speichern, löschen und aktualisieren von Lantexten in SAP.
    /// </summary>
    public class LongStringToSap
    {
        readonly ISapConnection _sapConnection;

        readonly IDynSapProxyFactory _dynSapProxyFactory;

        public LongStringToSap(ISapConnection objApp, IDynSapProxyFactory dynSapProxyFactory)
        {
            _sapConnection = objApp;
            _dynSapProxyFactory = dynSapProxyFactory;
        }
        /// <summary>
        /// Webbenutzername
        /// </summary>
        public String UName
        {
            get;
            set;
        }
        /// <summary>
        /// Langtext
        /// </summary>
        public String LText
        {
            get;
            set;
        }
        /// <summary>
        /// LangtextID - für welche Anwendung(ZLD Bemerkungen,Umlagerungstexte,Bestellpositionstext )
        /// </summary>
        public String LTextID
        {
            get;
            set;
        }
        /// <summary>
        /// Langtextnummer (ID in SAP-Tabelle)
        /// </summary>
        public String LTextNr
        {
            get;
            set;
        }
        /// <summary>
        /// SAP-Fehlernummer
        /// </summary>
        public String E_SUBRC
        {
            get;
            set;
        }
        /// <summary>
        /// SAP-Fehlertext
        /// </summary>
        public String E_MESSAGE
        {
            get;
            set;
        }

        /// <summary>
        /// Aktualisiert den Eintrag in der SAP-Langtext Tabelle mit der angegebenen Langtextnummer. 
        /// Bapi: Z_BC_LTEXT_UPDATE
        /// </summary>
        /// <param name="text">Langtext</param>
        /// <param name="sLTextNr">Langtextnummer</param>
        /// <param name="sUName">Username (max. 12 Zeichen)</param>
        public void UpdateString(String text, String sLTextNr, String sUName)
        {
            LText = text;
            LTextNr = sLTextNr;
            E_MESSAGE = "";

            try
            {
                var myProxy = _dynSapProxyFactory.CreateProxyCache("Z_BC_LTEXT_UPDATE", _sapConnection, _dynSapProxyFactory).GetProxy();


                myProxy.SetImportParameter("I_LTEXT_NR", LTextNr);
                myProxy.SetImportParameter("I_STRING", LText);
                myProxy.SetImportParameter("I_UNAME", sUName);

                myProxy.CallBapi();

                Int32 subrc;
                Int32.TryParse(myProxy.GetExportParameter("E_SUBRC"), out subrc);
                E_SUBRC = subrc.ToString();
                var sapMessage = myProxy.GetExportParameter("E_MESSAGE");
                E_MESSAGE = sapMessage;
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    default:
                        E_SUBRC = "-9999";
                        E_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
                        break;
                }
            }
        }

        /// <summary>
        /// Löscht den Eintrag in der SAP-Langtext Tabelle mit der angegebenen Langtextnummer.
        /// Bapi: Z_BC_LTEXT_DELETE
        /// </summary>       
        /// <param name="sLTextNr">Langtextnummer</param>        
        public void DeleteString(String sLTextNr)
        {
            LTextNr = sLTextNr;
            E_MESSAGE = "";

            try
            {
                var myProxy = _dynSapProxyFactory.CreateProxyCache("Z_BC_LTEXT_DELETE", _sapConnection, _dynSapProxyFactory).GetProxy(); 


                myProxy.SetImportParameter("I_LTEXT_NR", LTextNr);

                myProxy.CallBapi();

                Int32 subrc;
                Int32.TryParse(myProxy.GetExportParameter("E_SUBRC"), out subrc);
                E_SUBRC = subrc.ToString();
                var sapMessage = myProxy.GetExportParameter("E_MESSAGE");
                E_MESSAGE = sapMessage;
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    default:
                        E_SUBRC = "-9999";
                        E_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
                        break;
                }
            }
        }

        /// <summary>
        /// Liest den Eintrag aus der SAP-Langtext Tabelle mit der angegebenen Langtextnummer.
        /// Bapi: Z_BC_LTEXT_READ
        /// </summary>
        /// <param name="sLTextNr">Langtextnummer</param>        
        /// <returns>Langtext</returns>
        public String ReadString(String sLTextNr)
        {
            LTextNr = sLTextNr;
            LText = "";
            E_MESSAGE = "";

            try
            {
                var myProxy = _dynSapProxyFactory.CreateProxyCache("Z_BC_LTEXT_READ", _sapConnection, _dynSapProxyFactory).GetProxy(); 

                myProxy.SetImportParameter("I_LTEXT_NR", LTextNr);

                myProxy.CallBapi();

                LText = myProxy.GetExportParameter("E_STRING");
                LTextID = myProxy.GetExportParameter("LTEXT_ID");

                Int32 subrc;
                Int32.TryParse(myProxy.GetExportParameter("E_SUBRC"), out subrc);
                E_SUBRC = subrc.ToString();
                var sapMessage = myProxy.GetExportParameter("E_MESSAGE");
                E_MESSAGE = sapMessage;
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    default:
                        E_SUBRC = "-9999";
                        E_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
                        break;
                }
            }
            return LText;
        }

        /// <summary>
        /// Fügt einen neuen Eintrag in die SAP-Langtext Tabelle ein und liefert die ID des Eintrags zurück.
        /// Bapi: Z_BC_LTEXT_INSERT
        /// </summary>        
        /// <param name="text">Langtext</param>
        /// <param name="sUName">Username (max. 12 Zeichen)</param>
        /// <returns>Langtextnummer</returns>
        public String InsertString(String text, String sUName)
        {
            LText = text;
            E_MESSAGE = "";

            try
            {
                var myProxy = _dynSapProxyFactory.CreateProxyCache("Z_BC_LTEXT_INSERT", _sapConnection, _dynSapProxyFactory).GetProxy(); 

                myProxy.SetImportParameter("I_LTEXT_ID", "UMLT");
                myProxy.SetImportParameter("I_STRING", LText);
                myProxy.SetImportParameter("I_UNAME", sUName);

                myProxy.CallBapi();

                LTextNr = myProxy.GetExportParameter("E_LTEXT_NR").ToString();

                Int32 subrc;
                Int32.TryParse(myProxy.GetExportParameter("E_SUBRC").ToString(), out subrc);
                E_SUBRC = subrc.ToString();
                var sapMessage = myProxy.GetExportParameter("E_MESSAGE").ToString();
                E_MESSAGE = sapMessage;
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    default:
                        E_SUBRC = "-9999";
                        E_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
                        break;
                }
            }
            return LTextNr;
        }
    }

}
