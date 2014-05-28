using System;
using CKG.Base.Common;
using CKG.Base.Business;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    ///  Laden, speichern, löschen und aktualisieren von Lantexten in SAP.
    /// </summary>
    public class LongStringToSap
    {
        CKG.Base.Kernel.Security.User obj_User;
        CKG.Base.Kernel.Security.App obj_App; 
        System.Web.UI.Page objPage;

        /// <summary>
        /// Konstruktor LongStringToSap.
        /// </summary>
        /// <param name="objUser">Userobjekt</param>
        /// <param name="objApp">Applikationsobjekt</param>
        /// <param name="page">Aufrufende Seite.</param>
        public LongStringToSap(CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, System.Web.UI.Page page)
        {
            obj_User = objUser;
            obj_App = objApp;
            objPage = page;
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
        /// <param name="Text">Langtext</param>
        /// <param name="sLTextNr">Langtextnummer</param>
        /// <param name="sUName">Username (max. 12 Zeichen)</param>
        public void UpdateString( String Text, String sLTextNr, String sUName)
        {
            LText = Text;        
            LTextNr = sLTextNr;       
            E_MESSAGE = "";

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_BC_LTEXT_UPDATE", ref obj_App, ref obj_User, ref objPage);


                    myProxy.setImportParameter("I_LTEXT_NR", LTextNr);
                    myProxy.setImportParameter("I_STRING",LText );
                    myProxy.setImportParameter("I_UNAME", sUName);

                    myProxy.callBapi();
                    
                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    E_SUBRC = subrc.ToString();
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    E_MESSAGE = sapMessage;
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            E_SUBRC = "-9999";
                            E_MESSAGE =  "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
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
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_BC_LTEXT_DELETE", ref obj_App, ref obj_User, ref objPage);


                myProxy.setImportParameter("I_LTEXT_NR", LTextNr);

                myProxy.callBapi();

                Int32 subrc;
                Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                E_SUBRC = subrc.ToString();
                String sapMessage;
                sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                E_MESSAGE = sapMessage;
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        E_SUBRC = "-9999";
                        E_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
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
            LText="";
            E_MESSAGE = "";

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_BC_LTEXT_READ", ref obj_App, ref obj_User, ref objPage);

                myProxy.setImportParameter("I_LTEXT_NR", LTextNr);

                myProxy.callBapi();

                LText = myProxy.getExportParameter("E_STRING").ToString();
                LTextID = myProxy.getExportParameter("LTEXT_ID").ToString();

                Int32 subrc;
                Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                E_SUBRC = subrc.ToString();
                String sapMessage;
                sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                E_MESSAGE = sapMessage;
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        E_SUBRC = "-9999";
                        E_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }
            }
            return LText;
        }

        /// <summary>
        /// Fügt einen neuen Eintrag in die SAP-Langtext Tabelle ein und liefert die ID des Eintrags zurück.
        /// Bapi: Z_BC_LTEXT_INSERT
        /// </summary>        
        /// <param name="Text">Langtext</param>
        /// <param name="sUName">Username (max. 12 Zeichen)</param>
        /// <returns>Langtextnummer</returns>
        public String InsertString(String Text, String sUName)
        {
            LText = Text;
            E_MESSAGE = "";

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_BC_LTEXT_INSERT", ref obj_App, ref obj_User, ref objPage);

                myProxy.setImportParameter("I_LTEXT_ID", "UMLT");
                myProxy.setImportParameter("I_STRING", LText);
                myProxy.setImportParameter("I_UNAME", sUName);

                myProxy.callBapi();

                LTextNr = myProxy.getExportParameter("E_LTEXT_NR").ToString();

                Int32 subrc;
                Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                E_SUBRC = subrc.ToString();
                String sapMessage;
                sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                E_MESSAGE = sapMessage;
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        E_SUBRC = "-9999";
                        E_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }
            }
            return LTextNr;
        }
    }
    
}
