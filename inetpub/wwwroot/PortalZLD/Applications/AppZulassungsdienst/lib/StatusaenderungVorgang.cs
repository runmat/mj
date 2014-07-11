using System;
using System.Data;
using CKG.Base.Common;
using CKG.Base.Business;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Statusänderung Vorgang
    /// </summary>
    public class StatusaenderungVorgang : DatenimportBase
    {
        #region "Properties"

        public string IDSuche { get; set; }

        public string ID { get; set; }

        public string Belegtyp { get; set; }

        public string Zulassungsdatum { get; set; }

        public string Kundennummer { get; set; }

        public string Kreis { get; set; }

        public string Kennzeichen { get; set; }

        public string BEBStatus { get; set; }

        public string BEBStatusNeu { get; set; }

        public DataTable tblBEBStatusWerte { get; private set; }

        #endregion

        #region "Methods"

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="objUser">Webuserobjekt</param>
        /// <param name="objApp">Applikationsobjekt</param>
        /// <param name="strFilename">Filename</param>
        public StatusaenderungVorgang(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
        {}

        /// <summary>
        /// Werte für BEB-Status laden. Bapi: Z_ZLD_DOMAENEN_WERTE
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">...aspx</param>
        public void LoadStatuswerte(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "StatusaenderungVorgang.LeseStatuswerte";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_DOMAENEN_WERTE", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_DOMNAME", "ZZLD_BEB_STATUS");

                    myProxy.callBapi();

                    tblBEBStatusWerte = myProxy.getExportTable("GT_WERTE");

                    Int32 subrc;
                    if (Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out subrc))
                    {
                        m_intStatus = subrc;
                    }
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");
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
        }

        /// <summary>
        /// Vorgang zur ID laden. Bapi: Z_ZLD_MOB_GET_VG_FOR_UPD
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">...aspx</param>
        public void LoadVorgang(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "StatusaenderungVorgang.LeseVorgang";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_MOB_GET_VG_FOR_UPD", ref m_objApp, ref m_objUser, ref page);
                  
                    myProxy.setImportParameter("I_ZULBELN", IDSuche.PadLeft(10, '0'));
                    myProxy.setImportParameter("I_MOBUSER", m_objUser.UserName);

                    myProxy.callBapi();

                    var tmpTable = myProxy.getExportTable("GT_VG_STAT");

                    Int32 subrc;
                    if (Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out subrc))
                    {
                        m_intStatus = subrc;
                    }
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");

                    if (m_intStatus == 0)
                    {
                        var row = tmpTable.Rows[0];
                        ID = row["ZULBELN"].ToString();
                        Belegtyp = row["BLTYP"].ToString();
                        Zulassungsdatum = row["ZZZLDAT"].ToString();
                        Kundennummer = row["KUNNR"].ToString();
                        Kreis = row["KREISKZ"].ToString();
                        Kennzeichen = row["ZZKENN"].ToString();
                        BEBStatus = row["BEB_STATUS"].ToString();
                        if (tblBEBStatusWerte != null)
                        {
                            var statusRows = tblBEBStatusWerte.Select("DOMVALUE_L = '" + BEBStatus + "'");
                            if (statusRows.Length > 0)
                            {
                                BEBStatus = statusRows[0]["DDTEXT"].ToString();
                            }
                        }
                    }
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
        }

        /// <summary>
        /// Vorgangs-Status speichern. Bapi: Z_ZLD_MOB_SET_VG_STATUS
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">...aspx</param>
        public void SaveVorgang(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "StatusaenderungVorgang.SaveVorgang";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_MOB_SET_VG_STATUS", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_ZULBELN", ID.PadLeft(10, '0'));
                    myProxy.setImportParameter("I_STATUS", BEBStatusNeu);

                    myProxy.callBapi();

                    Int32 subrc;
                    if (Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out subrc))
                    {
                        m_intStatus = subrc;
                    }
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");

                    if (m_intStatus == 0)
                    {
                        BEBStatus = BEBStatusNeu;
                    }
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
        }

        #endregion
    }
}