using System;
using System.Data;
using CKG.Base.Common;
using CKG.Base.Business;
using CKG.Base.Kernel.Security;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Nachbearbeitung Vorgang
    /// </summary>
    public class NachbearbeitungAuftrag : DatenimportBase
    {
        #region "Properties"

        public string VkOrg { get; private set; }

        public string VkBur { get; private set; }

        public string SucheId { get; set; }

        public string SucheAuftragsnummer { get; set; }

        public string VorgangId { get; set; }

        public DataTable tblKopfdaten { get; private set; }

        public DataTable tblBankdaten { get; private set; }

        public DataTable tblAdressdaten { get; private set; }

        public DataTable tblPositionsdaten { get; private set; }

        public DataTable tblStornogruende { get; private set; }

        public string Stornogrund { get; set; }

        public string StornoKundennummer { get; set; }

        public string StornoBegruendung { get; set; }

        public string StornoStva { get; set; }

        public string StornoKennzeichen { get; set; }

        /// <summary>
        /// Alle stornierten Datensätze, die noch nicht nachbearbeitet wurden
        /// </summary>
        public DataTable tblOffeneStornos { get; private set; }

        #endregion

        #region "Methods"

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="objUser">Webuserobjekt</param>
        /// <param name="objApp">Applikationsobjekt</param>
        public NachbearbeitungAuftrag(ref User objUser, App objApp)
            : base(ref objUser, objApp, "")
        {
            if ((objUser != null) && (!String.IsNullOrEmpty(objUser.Reference)))
            {
                if (objUser.Reference.Length > 4)
                {
                    VkOrg = objUser.Reference.Substring(0, 4);
                    VkBur = objUser.Reference.Substring(4);
                }
                else
                {
                    VkOrg = objUser.Reference;
                    VkBur = "";
                }
            }
            else
            {
                VkOrg = "";
                VkBur = "";
            }
        }

        /// <summary>
        /// Stornogründe laden. Bapi: Z_ZLD_STO_STORNOGRUENDE
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">...aspx</param>
        public void StornogruendeLaden(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "NachbearbeitungAuftrag.StornogruendeLaden";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_STO_STORNOGRUENDE", ref m_objApp, ref m_objUser, ref page);

                    myProxy.callBapi();

                    tblStornogruende = myProxy.getExportTable("GT_GRUENDE");
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -9999;
                            m_strMessage = "Beim Laden der Stornogründe ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Prüfen, ob Vorgang storniert werden kann. Bapi: Z_ZLD_STO_STORNO_CHECK
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">...aspx</param>
        public void VorgangPruefen(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "NachbearbeitungAuftrag.VorgangPruefen";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_STO_STORNO_CHECK", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKORG", VkOrg);
                    myProxy.setImportParameter("I_VKBUR", VkBur);

                    if (!String.IsNullOrEmpty(SucheAuftragsnummer))
                    {
                        myProxy.setImportParameter("I_VBELN", SucheAuftragsnummer.PadLeft(10, '0'));
                    }
                    if (!String.IsNullOrEmpty(SucheId))
                    {
                        myProxy.setImportParameter("I_ZULBELN", SucheId.PadLeft(10, '0'));
                    }

                    myProxy.callBapi();

                    Int32 subrc;
                    if (Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out subrc))
                    {
                        m_intStatus = subrc;
                    }
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");

                    if (m_intStatus == 0)
                    {
                        VorgangId = myProxy.getExportParameter("E_ZULBELN");
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -9999;
                            m_strMessage = "Beim Prüfen des Vorgangs ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Vorgang zur ID laden. Bapi: Z_ZLD_STO_GET_ORDER
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">...aspx</param>
        public void VorgangLaden(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "NachbearbeitungAuftrag.VorgangLaden";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_STO_GET_ORDER", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_ZULBELN", VorgangId.PadLeft(10, '0'));

                    myProxy.callBapi();

                    Int32 subrc;
                    if (Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out subrc))
                    {
                        m_intStatus = subrc;
                    }
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");

                    if (m_intStatus == 0)
                    {
                        tblKopfdaten = myProxy.getExportTable("ES_BAK");
                        tblBankdaten = myProxy.getExportTable("ES_BANK");
                        tblAdressdaten = myProxy.getExportTable("GT_ADRS");
                        tblPositionsdaten = myProxy.getExportTable("GT_POS_S01");

                        foreach (DataRow row in tblPositionsdaten.Rows)
                        {
                            row["PREIS_C"] = row["PREIS_C"].ToString().Trim();
                            row["GEB_AMT_C"] = row["GEB_AMT_C"].ToString().Trim();
                        }
                        tblPositionsdaten.AcceptChanges();
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -9999;
                            m_strMessage = "Beim Laden des Vorgangs ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Vorgang stornieren. Bapi: Z_ZLD_STO_STORNO_ORDER
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">...aspx</param>
        public void VorgangStornieren(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "NachbearbeitungAuftrag.VorgangStornieren";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_STO_STORNO_ORDER", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_ZULBELN", VorgangId.PadLeft(10, '0'));
                    myProxy.setImportParameter("I_ERNAM", m_objUser.UserName);
                    myProxy.setImportParameter("I_STORNOGRUND", Stornogrund);
                    if (!String.IsNullOrEmpty(StornoKundennummer))
                    {
                        myProxy.setImportParameter("I_KUNNR", StornoKundennummer.PadLeft(10, '0'));
                    }
                    if (!String.IsNullOrEmpty(StornoBegruendung))
                    {
                        myProxy.setImportParameter("I_BEGRUENDUNG", StornoBegruendung);
                    }
                    if (!String.IsNullOrEmpty(StornoStva))
                    {
                        myProxy.setImportParameter("I_KREISKZ", StornoStva);
                    }
                    if (!String.IsNullOrEmpty(StornoKennzeichen))
                    {
                        myProxy.setImportParameter("I_ZZKENN", StornoKennzeichen);
                    }

                    myProxy.callBapi();

                    Int32 subrc;
                    if (Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out subrc))
                    {
                        m_intStatus = subrc;
                    }
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");

                    if (m_intStatus == 0)
                    {
                        VorgangId = myProxy.getExportParameter("E_ZULBELN_NEU");
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -9999;
                            m_strMessage = "Beim Stornieren des Vorgangs ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Vorgang absenden. Bapi: Z_ZLD_STO_STORNO_ORDER
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">...aspx</param>
        public void VorgangAbsenden(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "NachbearbeitungAuftrag.VorgangAbsenden";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_IMP_NACHERF", ref m_objApp, ref m_objUser, ref page);

                    DataTable importAuftrag = myProxy.getImportTable("GT_IMP_BAK");
                    DataTable importPos = myProxy.getImportTable("GT_IMP_POS_S01");
                    DataTable importBank = myProxy.getImportTable("GT_IMP_BANK");
                    DataTable importAdresse = myProxy.getImportTable("GT_IMP_ADRS");

                    FillSapImportTable(tblKopfdaten, ref importAuftrag);
                    FillSapImportTable(tblPositionsdaten, ref importPos);
                    FillSapImportTable(tblBankdaten, ref importBank);
                    FillSapImportTable(tblAdressdaten, ref importAdresse);

                    myProxy.callBapi();

                    var tblErrors = myProxy.getExportTable("GT_EX_ERRORS");

                    if (tblErrors.Rows.Count > 0 && tblErrors.Rows[0]["ERROR_TEXT"].ToString() != "OK")
                    {
                        m_intStatus = -9999;
                        m_strMessage = "Beim Absenden des Vorgangs ist ein Fehler aufgetreten: " + tblErrors.Rows[0]["ERROR_TEXT"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -9999;
                            m_strMessage = "Beim Absenden des Vorgangs ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Offene Stornos laden. Bapi: Z_ZLD_STO_???
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">...aspx</param>
        /// <param name="kundenstamm"></param>
        public void OffeneStornosLaden(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable kundenstamm)
        {
            m_strClassAndMethod = "NachbearbeitungAuftrag.OffeneStornosLaden";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_STO_STORNO_LISTE", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKORG", VkOrg);
                    myProxy.setImportParameter("I_VKBUR", VkBur);

                    myProxy.callBapi();

                    Int32 subrc;
                    if (Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out subrc))
                    {
                        m_intStatus = subrc;
                    }
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");

                    if (m_intStatus == 0)
                    {
                        tblOffeneStornos = myProxy.getExportTable("GT_LISTE");

                        tblOffeneStornos.Columns.Add("Kunde");
                        tblOffeneStornos.Columns.Add("Erfasst");

                        foreach (DataRow row in tblOffeneStornos.Rows)
                        {
                            row["ZULBELN"] = row["ZULBELN"].ToString().TrimStart('0');
                            row["ZULBELN_ALT"] = row["ZULBELN_ALT"].ToString().TrimStart('0');

                            DataRow[] drow = kundenstamm.Select("KUNNR = '" + row["KUNNR"].ToString().TrimStart('0') + "'");
                            if (drow.Length > 0)
                            {
                                row["Kunde"] = drow[0]["NAME1"].ToString();
                            }

                            string strErfasst = "";
                            if (!String.IsNullOrEmpty(row["VE_ERDAT"].ToString()))
                            {
                                DateTime tmpDat;
                                if (DateTime.TryParse(row["VE_ERDAT"].ToString(), out tmpDat))
                                {
                                    strErfasst = tmpDat.ToShortDateString();

                                    string strZeit = row["VE_ERZEIT"].ToString();
                                    if (!String.IsNullOrEmpty(strZeit) && strZeit.Length == 6)
                                    {
                                        strErfasst += " " + strZeit.Substring(0, 2) + ":" + strZeit.Substring(2, 2) + ":" + strZeit.Substring(4, 2);
                                    }
                                }
                            }
                            row["Erfasst"] = strErfasst;
                        }

                        tblOffeneStornos.AcceptChanges();
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -9999;
                            m_strMessage = "Beim Laden der offenen Stornos ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        private void FillSapImportTable(DataTable srcTable, ref DataTable impTable)
        {
            foreach (DataRow srcRow in srcTable.Rows)
            {
                var newRow = impTable.NewRow();

                foreach (DataColumn col in impTable.Columns)
                {
                    // leere DateTime-Werte abfangen
                    if (col.DataType.Name == "DateTime" && String.IsNullOrEmpty(srcRow[col.ColumnName].ToString()))
                    {
                        newRow[col.ColumnName] = DBNull.Value;
                    }
                    else
                    {
                        newRow[col.ColumnName] = srcRow[col.ColumnName];
                    }
                }

                impTable.Rows.Add(newRow);
            }

            impTable.AcceptChanges();
        }

        /// <summary>
        /// Daten "auf Anfang" zurücksetzen
        /// </summary>
        public void ResetData(bool resetSuchparameter)
        {
            if (resetSuchparameter)
            {
                SucheId = "";
                SucheAuftragsnummer = "";
            }
          
            VorgangId = "";

            if (tblKopfdaten != null)
                tblKopfdaten.Clear();
            if (tblBankdaten != null)
                tblBankdaten.Clear();
            if (tblAdressdaten != null)
                tblAdressdaten.Clear();
            if (tblPositionsdaten != null)
                tblPositionsdaten.Clear();

            Stornogrund = "";
            StornoKundennummer = "";
            StornoBegruendung = "";
            StornoStva = "";
            StornoKennzeichen = "";
        }

        #endregion
    }
}