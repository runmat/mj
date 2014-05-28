using System;
using System.Linq;
using System.Data;
using CKG.Base.Kernel.Security;
using CKG.Base.Common;
using CKG.Base.Business;

namespace MvcAppZulassungsdienst.SAP
{
    public class NacherfassungGekaufteKennzeichen : DatenimportBase
    {
        private System.Web.UI.Page _objPage;

        /// <summary>
        /// Basisklasse zur Nacherfassung gekaufter Kennzeichen
        /// </summary>
        /// <param name="user">User-Objekt</param>
        /// <param name="app">App-Objekt</param> 
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">aktuelle Page</param>
        public NacherfassungGekaufteKennzeichen(ref User user, App app, string strAppID, string strSessionID, System.Web.UI.Page page, string lieferantenNr) 
            : base(ref user, app, "")
        {
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            if (m_objUser.Reference == null || m_objUser.Reference.Length < 8)
                return;

            Vkbur = m_objUser.Reference.Substring(4, 4);

            _objPage = page;

            GetErfassteKennzeichen(lieferantenNr);
        }

        #region "Properties"

        public DataTable TblErfassteKennzeichenKopf { get; private set; }

        public DataTable TblErfassteKennzeichenPositionen { get; private set; }

        public string SapMessage { get { return m_strMessage; } }

        private string Vkbur { get; set; }

        #endregion

        /// <summary>
        /// Liefert eine Liste aller erfassten Kennzeichen zum aktuellen Lieferanten
        /// </summary>
        /// <returns>Lieferantenliste</returns>
        private void GetErfassteKennzeichen(string lieferantenNr)
        {
            m_strClassAndMethod = "NacherfassungGekaufteKennzeichen.GetErfassteKennzeichen";
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    var myProxy = DynSapProxy.getProxy("Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST", ref m_objApp, ref m_objUser,
                                                       ref _objPage);

                    myProxy.setImportParameter("I_KOSTL", Vkbur.PadLeft(10, '0'));
                    myProxy.setImportParameter("I_LIFNR", lieferantenNr);

                    myProxy.callBapi();

                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();

                    switch (subrc)
                    {
                        default:
                            m_strMessage = sapMessage;
                            break;
                    }

                    TblErfassteKennzeichenKopf = myProxy.getExportTable("GT_PO_K");
                    TblErfassteKennzeichenPositionen = myProxy.getExportTable("GT_PO_P");

                    m_strMessage = sapMessage;
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -9999;
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" +
                                           HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally
                {
                    m_blnGestartet = false;
                }
            }
        }


        public DataTable GetLieferanten()
        {
            m_strClassAndMethod = "NacherfassungGekaufteKennzeichen.GetLieferanten";
            m_intStatus = 0;
            m_strMessage = String.Empty;

            DataTable tblLieferanten = null;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    var myProxy = DynSapProxy.getProxy("Z_FIL_EFA_PLATSTAMM", ref m_objApp, ref m_objUser, ref _objPage);

                    var isFil = "";
                    var isZLD = "";

                    if (m_objUser.CustomerName.Contains("ZLD"))
                    {
                        isZLD = "X";
                    }
                    else if (m_objUser.CustomerName.Contains("Kroschke"))
                    {
                        isFil = "X";
                    }

                    myProxy.setImportParameter("I_KOSTL", Vkbur.PadLeft(10, '0'));
                    myProxy.setImportParameter("I_SUPER_USER", "");
                    myProxy.setImportParameter("I_FIL", isFil);
                    myProxy.setImportParameter("I_ZLD", isZLD);

                    myProxy.callBapi();

                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out subrc);
                    var sapMessage = myProxy.getExportParameter("E_MESSAGE");

                    switch (subrc)
                    {
                        case 104:
                            m_strMessage = "KST nicht zulässig! Bitte richtige KST eingeben.";
                            break;
                        default:
                            m_strMessage = sapMessage;
                            break;
                    }

                    tblLieferanten = myProxy.getExportTable("GT_PLATSTAMM");

                    m_strMessage = sapMessage;
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -9999;
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }

            return tblLieferanten;
        }
    }
}