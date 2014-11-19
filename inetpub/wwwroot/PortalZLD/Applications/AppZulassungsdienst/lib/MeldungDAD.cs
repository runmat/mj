using System;
using CKG.Base.Common;
using CKG.Base.Business;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Taggleiche Meldung DAD
    /// </summary>
    public class MeldungDAD : DatenimportBase
    {
        #region "Properties"

        public string VkBur { get; set; }

        public string IDSuche { get; set; }

        public string FahrgestellnummerSuche { get; set; }

        public string BriefnummerSuche { get; set; }

        public string ID { get; set; }

        public string Bestellnummer { get; set; }

        public string Frachtbriefnummer { get; set; }

        public string Fahrgestellnummer { get; set; }

        public string Briefnummer { get; set; }

        public string Zulassungsdatum { get; set; }

        public string Kennzeichen { get; set; }

        public string Gebuehr { get; set; }

        public bool Auslieferung { get; set; }

        #endregion

        #region "Methods"

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="objUser">Webuserobjekt</param>
        /// <param name="objApp">Applikationsobjekt</param>
        /// <param name="strFilename">Filename</param>
        public MeldungDAD(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp,
                                      string strFilename)
            : base(ref objUser, objApp, strFilename)
        {
            if ((objUser != null) && (!String.IsNullOrEmpty(objUser.Reference)))
            {
                if (objUser.Reference.Length > 4)
                {
                    VkBur = objUser.Reference.Substring(4);
                }
                else
                {
                    VkBur = "";
                }
            }
            else
            {
                VkBur = "";
            }
        }

        /// <summary>
        /// Vorgang zur ID laden. Bapi: Z_ZLD_FIND_DAD_SD_ORDER
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">...aspx</param>
        public void LoadVorgang(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "MeldungDAD.LoadVorgang";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_FIND_DAD_SD_ORDER", ref m_objApp, ref m_objUser, ref page);
                  
                    myProxy.setImportParameter("I_VKORG", "1510");
                    myProxy.setImportParameter("I_VKBUR", VkBur);

                    if (!String.IsNullOrEmpty(IDSuche))
                    {
                        myProxy.setImportParameter("I_VBELN", IDSuche.PadLeft(10, '0'));
                    }
                    if (!String.IsNullOrEmpty(FahrgestellnummerSuche))
                    {
                        myProxy.setImportParameter("I_FAHRG", FahrgestellnummerSuche);
                    }
                    if (!String.IsNullOrEmpty(BriefnummerSuche))
                    {
                        myProxy.setImportParameter("I_BRIEF", BriefnummerSuche);
                    }

                    myProxy.callBapi();

                    var tmpTable = myProxy.getExportTable("E_VBAK");

                    Int32 subrc;
                    if (Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out subrc))
                    {
                        m_intStatus = subrc;
                    }
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");

                    if (m_intStatus == 0)
                    {
                        var row = tmpTable.Rows[0];
                        ID = row["VBELN"].ToString();
                        Bestellnummer = row["EBELN"].ToString();
                        Frachtbriefnummer = row["ZZSEND2"].ToString();
                        Fahrgestellnummer = row["ZZFAHRG"].ToString();
                        Briefnummer = row["ZZBRIEF"].ToString();
                        Zulassungsdatum = row["VDATU"].ToString();
                        Kennzeichen = row["ZZKENN"].ToString();
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

        public void ClearFields()
        {
            ID = "";
            Bestellnummer = "";
            Frachtbriefnummer = "";
            Fahrgestellnummer = "";
            Briefnummer = "";
            Zulassungsdatum = "";
            Kennzeichen = "";
        }

        /// <summary>
        /// Vorgang speichern. Bapi: Z_ZLD_SAVE_TAGGLEICHE_MELDUNG
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">...aspx</param>
        public void SaveVorgang(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "MeldungDAD.SaveVorgang";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_SAVE_TAGGLEICHE_MELDUNG", ref m_objApp, ref m_objUser, ref page);

                    var tmpTable = myProxy.getImportTable("IS_TG_MELDUNG");

                    var newRow = tmpTable.NewRow();
                    newRow["VKORG"] = "1510";
                    newRow["VBELN"] = ID.PadLeft(10, '0');
                    newRow["EBELN"] = Bestellnummer;
                    newRow["FAHRG"] = Fahrgestellnummer;
                    newRow["BRIEF"] = Briefnummer;
                    if (ZLDCommon.IsDate(Zulassungsdatum)) { newRow["ZZZLDAT"] = Zulassungsdatum; }
                    newRow["ZZKENN"] = Kennzeichen;
                    newRow["GEB_AMT"] = Gebuehr;
                    newRow["AUSLIEF"] = ZLDCommon.BoolToX(Auslieferung);
                    newRow["ZZSEND2"] = Frachtbriefnummer;
                    newRow["ERNAM"] = m_objUser.UserName;
                    newRow["SAVE_STATUS"] = "A";
                    tmpTable.Rows.Add(newRow);

                    myProxy.callBapi();

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

        #endregion
    }
}