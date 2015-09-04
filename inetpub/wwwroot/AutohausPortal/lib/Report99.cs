using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Common;
using System.Data;
using CKG.Base.Business;

namespace AutohausPortal.lib
{
    /// <summary>
    /// Klasse für die Dokumentenanforderung der Zulassungstellen
    /// </summary>
    public class Report99 : CKG.Base.Business.DatenimportBase
    {
        #region "Declarations"
        String strKennzeichen;
        #endregion

        #region "Properties"
        /// <summary>
        /// Selektierte StVa zur Übergabe an das Bapi.
        /// </summary>
        public String PKennzeichen
        {
            get { return strKennzeichen; }
            set { strKennzeichen = value; }
        }

        #endregion

        #region "Methods"
        /// <summary>
        /// Kontruktor
        /// </summary>
        /// <param name="objUser">User-Object</param>
        /// <param name="objApp">Applikations-Object</param>
        /// <param name="strFilename">Empty</param>
        public Report99(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
        { }
        /// <summary>
        ///  Datenselektion für die Ausgabe der Dokumentenanforderung(Z_M_ZGBS_BEN_ZULASSUNGSUNT).
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Dokumentenanforderung.aspx.cs</param>
        public void Fill(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "Report99ZLD.Fill";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_ZGBS_BEN_ZULASSUNGSUNT", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_ZKBA1", "");
                    myProxy.setImportParameter("I_ZKBA2", "");
                    myProxy.setImportParameter("I_ZKFZKZ", strKennzeichen);
                    myProxy.setImportParameter("I_AUSWAHL", "");

                    myProxy.callBapi();

                    DataTable m_tblResultRaw = new DataTable();
                    m_tblResult = myProxy.getExportTable("GT_WEB");
                    WriteLogEntry(true, "I_ZKFZKZ=" + strKennzeichen, ref m_tblResult, false);
                }

                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            m_intStatus = -5555;
                            m_strMessage = "Keine Daten gefunden.";
                            WriteLogEntry(false, "I_ZKFZKZ=" + strKennzeichen + ", " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message), ref m_tblResult, false);
                            break;
                        default:
                            m_intStatus = -9999;
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            WriteLogEntry(false, "I_ZKFZKZ=" + strKennzeichen + ", " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message), ref m_tblResult, false);
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }
        #endregion
    }
}