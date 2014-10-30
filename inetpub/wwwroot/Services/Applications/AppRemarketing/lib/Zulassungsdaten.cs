using System;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;

namespace AppRemarketing.lib
{
	public class ZulassungsdatenPublic : BankBase
    {
        #region "Declarations"

        DataTable m_tblVermieter;
        DataTable m_tblFehler;
        String m_strAVNr;
        String m_strSelectionType;
        String m_strDatumVon;
        String m_strDatumBis;
        DataTable m_tblUpload;
        DataTable m_tblError;
        Boolean m_Edit;

        #endregion

        #region "Properties"

        public DataTable Vermieter
        {
            get { return m_tblVermieter; }
            set { m_tblVermieter = value; }
        }

        public DataTable Fehler
        {
            get { return m_tblFehler; }
            
        }

        public String AVNr
        {
            get { return m_strAVNr; }
            set { m_strAVNr = value; }
        }

        public String SelectionType
        {
            get { return m_strSelectionType; }
            set { m_strSelectionType = value; }
        }

        public String DatumVon
        {
            get { return m_strDatumVon; }
            set { m_strDatumVon = value; }
        }

        public String DatimBis
        {
            get { return m_strDatumBis; }
            set { m_strDatumBis = value; }
        }

        public DataTable tblUpload
        {
            get { return m_tblUpload; }
            set { m_tblUpload = value; }
        }

        public DataTable tblError
        {
            get { return m_tblError; }
            set { m_tblError = value; }
        }

        public Boolean Edit
        {
            get { return m_Edit; }
            set { m_Edit = value; }
        }

        public string Inventarnr { set; get; }
        public string Kennzeichen { set; get; }
        public string Vertragsjahr { set; get; }

        #endregion

        #region "Methods"

        public ZulassungsdatenPublic(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
                                        : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
	    {
	    }

        public void getVermieter(String strAppID, String strSessionID, System.Web.UI.Page page)
        
        {
            m_strClassAndMethod = "Zulassungsdaten.FILL";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_AUFTR6_001", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNUNG", "VERMIETER");

                myProxy.callBapi();

                m_tblVermieter = myProxy.getExportTable("GT_WEB");

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

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

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "),ref m_tblResult);

            }


        }

        public override void Change()
        {

        }

        public override void Show()
        {

        }

        public void Show(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Zulassungsdaten.FILL";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_READ_ZUL_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));


                if (m_strAVNr != "00")
                {
                    myProxy.setImportParameter("I_AVNR", m_strAVNr);
                }

                if (m_strSelectionType == "Einzel") { 
                   
                   
                   
                    if (m_strDatumVon != null) { myProxy.setImportParameter("I_ZULDAT_VON", m_strDatumVon); }
                    if (m_strDatumBis != null) { myProxy.setImportParameter("I_ZULDAT_BIS", m_strDatumBis); }
                    if (Kennzeichen != null) { myProxy.setImportParameter("I_KENNZ", Kennzeichen); }
                    if (Inventarnr != null) { myProxy.setImportParameter("I_INVENTAR", Inventarnr); }
                    if (Vertragsjahr != null) { myProxy.setImportParameter("I_VJAHR", Vertragsjahr); } 
                }


                if (m_strSelectionType == "FIN") 
                {

                    DataTable SapTable = myProxy.getImportTable("GT_IN_FIN");
                    DataRow rowUpload = null;
                    foreach (DataRow rowUpload_loopVariable in m_tblUpload.Rows)
                    {
                        rowUpload = rowUpload_loopVariable;
                        DataRow tmpSAPRow = SapTable.NewRow();

                        {
                            tmpSAPRow["CHASSIS_NUM"] = rowUpload["F1"].ToString().Trim();
                        }

                        SapTable.Rows.Add(tmpSAPRow);
                        SapTable.AcceptChanges();
                    }

                }

                
 
                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_DATEN");


                m_tblFehler = myProxy.getExportTable("GT_ERR_OUT");
                Result = tblTemp;
              

                if (((Result.Rows.Count == 0)) && ((m_tblFehler.Rows.Count==0)))
                {
                                   
                    m_intStatus = -9999;
                    m_strMessage = "Keine Dokumente zur Anzeige gefunden.";
                }
                else
                {
                    foreach (DataRow dr in Result.Rows)
                    {

                        dr["POS_TEXT"] = dr["AVNR"] + " " + dr["POS_TEXT"];

                    }
                }

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

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

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }

        }

        public void setZulassungen(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Zulassungsdaten.setZulassungen";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SET_DAT_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                DataTable tblTmp = myProxy.getImportTable("GT_IN");

                DataRow newRow;

                DataTable ImpTable;

                if (Edit==false)
                {
                    ImpTable = tblUpload;
                }
                else
                {
                    ImpTable = tblError;
                }


                foreach (DataRow dr in ImpTable.Rows)
                {
                    newRow = tblTmp.NewRow();

                    newRow["FAHRGNR"] = dr[0];
                    newRow["KENNZ"] = dr[1];
                    newRow["ZULDAT"] = dr[2];
                    newRow["BRIEFNR"] = dr[3];

                    tblTmp.Rows.Add(newRow);

                }


                myProxy.callBapi();

                m_tblError = myProxy.getExportTable("GT_ERR");

                if (m_tblError.Rows.Count > 0)
                {
                    m_tblError.Columns["FAHRGNR"].ColumnName = "Fahrgestellnummer";
                    m_tblError.Columns["KENNZ"].ColumnName = "Kennzeichen";
                    m_tblError.Columns["ZULDAT"].ColumnName = "Zulassungsdatum";
                    m_tblError.Columns["BRIEFNR"].ColumnName = "Briefnummer";
                    m_tblError.Columns.Add("ID", typeof(int));
                    m_tblError.AcceptChanges();
                }

                for (int i = 0; i < m_tblError.Rows.Count; i++)
                {
                    m_tblError.Rows[i]["ID"] = i;
                }




                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

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

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }


        }

        public void setSelbstvermarktung(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Zulassungsdaten.setSelbstvermarktung";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_IMP_SVM_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                DataTable tblTmp = myProxy.getImportTable("GT_IN");

                DataTable ImpTable;

                if (!Edit)
                {
                    ImpTable = tblUpload;
                }
                else
                {
                    ImpTable = tblError;
                }

                foreach (DataRow row in ImpTable.Rows)
                {
                    DataRow newRow = tblTmp.NewRow();

                    newRow["FAHRGNR"] = row["FAHRGNR"];
                    newRow["DAT_ABRECHNUNG"] = row["DAT_ABRECHNUNG"];

                    tblTmp.Rows.Add(newRow);
                }

                myProxy.callBapi();

                m_tblError = myProxy.getExportTable("GT_ERR");

                if (m_tblError.Rows.Count > 0)
                {
                    m_tblError.Columns.Add("DAT_ABRECHNUNG", typeof(string));
                    m_tblError.Columns.Add("ID", typeof(int));
                    m_tblError.AcceptChanges();
                }

                for (int i = 0; i < m_tblError.Rows.Count; i++)
                {
                    m_tblError.Rows[i]["DAT_ABRECHNUNG"] = ImpTable.Select("FAHRGNR='" + m_tblError.Rows[i]["FAHRGNR"] + "'")[0]["DAT_ABRECHNUNG"];
                    m_tblError.Rows[i]["ID"] = i;
                }

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);
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

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }

        public void setVorschaeden(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Zulassungsdaten.setVorschaeden";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_INS_SCHADEN_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                DataTable tblTmp = myProxy.getImportTable("GT_IN");

                DataRow newRow;

                DataTable ImpTable;

                if (!Edit)
                {
                    ImpTable = tblUpload;

                    foreach (DataRow dr in ImpTable.Rows)
                    {
                        newRow = tblTmp.NewRow();

                        newRow["FAHRGNR"] = dr[0];
                        newRow["KENNZ"] = dr[1];
                        newRow["PREIS"] = dr[3];
                        newRow["SCHAD_DAT"] = dr[4];
                        newRow["BESCHREIBUNG"] = dr[2];

                        tblTmp.Rows.Add(newRow);
                    }
                }
                else
                {
                    ImpTable = tblError;

                    foreach (DataRow dr in ImpTable.Rows)
                    {
                        newRow = tblTmp.NewRow();

                        newRow["FAHRGNR"] = dr["Fahrgestellnummer"];
                        newRow["KENNZ"] = dr["Kennzeichen"];
                        newRow["PREIS"] = dr["Betrag"];
                        newRow["SCHAD_DAT"] = dr["Schadensdatum"];
                        newRow["BESCHREIBUNG"] = dr["Beschreibung"];

                        tblTmp.Rows.Add(newRow);
                    }
                }

                myProxy.callBapi();

                m_tblError = myProxy.getExportTable("GT_ERR");

                if (m_tblError.Rows.Count > 0)
                {
                    m_tblError.Columns["FAHRGNR"].ColumnName = "Fahrgestellnummer";
                    m_tblError.Columns["KENNZ"].ColumnName = "Kennzeichen";
                    m_tblError.Columns["PREIS"].ColumnName = "Betrag";
                    m_tblError.Columns["BESCHREIBUNG"].ColumnName = "Beschreibung";
                    m_tblError.Columns.Add("ID", typeof(int));
                    m_tblError.Columns.Add("Schadensdatum", typeof(string));
                    m_tblError.AcceptChanges();
                }

                for (int i = 0; i < m_tblError.Rows.Count; i++)
                {
                    m_tblError.Rows[i]["ID"] = i;
                    DateTime datum;
                    if (m_tblError.Rows[i]["SCHAD_DAT"] != null && DateTime.TryParse(m_tblError.Rows[i]["SCHAD_DAT"].ToString(), out datum))
                    {
                        m_tblError.Rows[i]["Schadensdatum"] = datum.ToShortDateString();
                    }
                    else
                    {
                        m_tblError.Rows[i]["Schadensdatum"] = "";
                    }
                }

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);
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

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }

        #endregion
    }
}
// ************************************************
// $History: Zulassungsdaten.cs $
//
//*****************  Version 6  *****************
//User: Fassbenders  Date: 29.03.11   Time: 10:37
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//
//*****************  Version 5  *****************
//User: Fassbenders  Date: 4.03.11    Time: 10:59
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//
//*****************  Version 4  *****************
//User: Fassbenders  Date: 3.03.11    Time: 21:08
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//
//*****************  Version 3  *****************
//User: Fassbenders  Date: 29.09.10   Time: 11:00
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//
//*****************  Version 2  *****************
//User: Jungj        Date: 24.09.10   Time: 18:45
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4129 testfertig
//
//*****************  Version 1  *****************
//User: Jungj        Date: 24.09.10   Time: 16:48
//Created in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4129 Torso
//
// ************************************************ 
