using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Common;
using CKG.Base.Business;

namespace AutohausPortal.lib
{
    /// <summary>
    /// Laden, speichern, löschen und aktualisieren von Lantexten in SAP.
    /// </summary>
    public class LongStringToSap
    {
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
        /// Aktualisieren des gänderten Langtextes in SAP(Z_BC_LTEXT_UPDATE).
        /// </summary>
        /// <param name="objUser">Userobjekt</param>
        /// <param name="objApp">Applikationsobjekt</param>
        /// <param name="page">Sonstiges.aspx.cs</param>
        /// <param name="Text">Langtext</param>
        /// <param name="sLTextNr">Langtextnummer SAP</param>
        /// <param name="sUName">Webbenutzername</param>
        public void UpdateString(CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, System.Web.UI.Page page, String Text, String sLTextNr, String sUName)
        {
            LText = Text;
            LTextNr = sLTextNr;
            E_MESSAGE = "";

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_BC_LTEXT_UPDATE", ref objApp, ref objUser, ref page);


                myProxy.setImportParameter("I_LTEXT_NR", LTextNr);
                myProxy.setImportParameter("I_STRING", LText);
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
                        E_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }
            }
            finally { }
        }
        /// <summary>
        /// Löschen des angelegten Langtextes in SAP(Z_BC_LTEXT_DELETE).
        /// </summary>
        /// <param name="objUser">Userobjekt</param>
        /// <param name="objApp">Applikationsobjekt</param>
        /// <param name="page">Empty</param>
        /// <param name="sLTextNr">Langtextnummer SAP</param>
        public void DeleteString(CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, System.Web.UI.Page page, String sLTextNr)
        {
            LTextNr = sLTextNr;
            E_MESSAGE = "";

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_BC_LTEXT_DELETE", ref objApp, ref objUser, ref page);


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
            finally { }
        }

        /// <summary>
        /// Lesen des Langtextes aus SAP(Z_BC_LTEXT_READ).
        /// </summary>
        /// <param name="objUser">Userobjekt</param>
        /// <param name="objApp">Applikationsobjekt</param>
        /// <param name="page">Sonstiges.aspx.cs</param>
        /// <param name="sLTextNr">Langtextnummer SAP</param>
        /// <returns>Langtext</returns>
        public String ReadString(CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, System.Web.UI.Page page, String sLTextNr)
        {
            LTextNr = sLTextNr;
            LText = "";
            E_MESSAGE = "";

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_BC_LTEXT_READ", ref objApp, ref objUser, ref page);

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
            finally { }
            return LText;
        }
        /// <summary>
        /// Einfügen des neuen Langtextes in SAP(Z_BC_LTEXT_INSERT).
        /// </summary>
        /// <param name="objUser">Userobjekt</param>
        /// <param name="objApp">Applikationsobjekt</param>
        /// <param name="page">Sonstiges.aspx.cs</param>
        /// <param name="Text">Langtext</param>
        /// <param name="sUName">Webbenutzername</param>
        /// <returns>Langtextnummer</returns>
        public String InsertString(CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, System.Web.UI.Page page, String Text, String sUName)
        {
            LText = Text;
            E_MESSAGE = "";

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_BC_LTEXT_INSERT", ref objApp, ref objUser, ref page);

                myProxy.setImportParameter("I_LTEXT_ID", "ZLDBEM");
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
            finally { }
            return LTextNr;
        }
    }
}