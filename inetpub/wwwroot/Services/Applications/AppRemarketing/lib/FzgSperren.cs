using System;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;

namespace AppRemarketing.lib
{
    public class FzgSperren : BankBase
    {
        #region "Declaclarations"

        String m_strFilename2;
        String m_strFahrgestellnummer;
        String m_selHaendler;
        String m_debitor;
        String m_haendler;
        DataTable m_tblFahrzeuge;
        DataTable m_tblFehlerFahrzeuge;
        DataTable m_tblUpload;

        String mE_SUBRC;
        String mE_MESSAGE;

        #endregion

        #region "Properties"

        public String E_SUBRC
        {
            get { return mE_SUBRC; }
            set { mE_SUBRC = value; }
        }

        public String E_Message
        {
            get { return mE_MESSAGE; }
            set { mE_MESSAGE = value; }
        }

        public DataTable tblUpload
        {
            get { return m_tblUpload; }
            set { m_tblUpload = value; }
        }

        public DataTable tblFahrzeuge
        {
            get { return m_tblFahrzeuge; }
            set { m_tblFahrzeuge = value; }
        }

        public DataTable tblFehlerFahrzeuge
        {
            get { return m_tblFehlerFahrzeuge; }
            set { m_tblFehlerFahrzeuge = value; }
        }

        public String Fahrgestellnummer
        {
            get { return m_strFahrgestellnummer; }
            set { m_strFahrgestellnummer = value; }
        }

        public String SelHaendler
        {
            get { return m_selHaendler; }
            set { m_selHaendler = value; }
        }

        public String Debitor
        {
            get { return m_debitor; }
            set { m_debitor = value; }
        }

        public String Haendler
        {
            get { return m_haendler; }
            set { m_haendler = value; }
        }

        #endregion

        public FzgSperren(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
                                        : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
	    {
            this.m_strFilename2 = strFilename;

	    }

        public override void Change()
        {

        }

        public override void Show()
        {

        }

        public void Show(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "FzgSperren.FILL";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            mE_SUBRC = "";
            mE_MESSAGE = "";

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_READ_FZG_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                // Selektion nach Händler oder Upload-Tabelle/FIN-Liste
                if (!String.IsNullOrEmpty(m_selHaendler))
                {
                    myProxy.setImportParameter("I_RDEALER", m_selHaendler);
                }
                else if (m_tblUpload != null)
                {
                    DataTable SapTable = myProxy.getImportTable("GT_FIN_IN");
                    DataRow rowUpload = null;
                    foreach (DataRow rowUpload_loopVariable in m_tblUpload.Rows)
                    {
                        rowUpload = rowUpload_loopVariable;
                        DataRow tmpSAPRow = SapTable.NewRow();

                        {
                            tmpSAPRow["CHASSIS_NUM"] = rowUpload["F1"].ToString();
                        }

                        SapTable.Rows.Add(tmpSAPRow);
                        SapTable.AcceptChanges();
                    }
                }

                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_OUT");
                tblTemp.Columns.Add("Adresse", System.Type.GetType("System.String"));
                tblTemp.Columns.Add("AdresseBank", System.Type.GetType("System.String"));
                if (tblTemp.Rows.Count > 0) 
                {
                    foreach (DataRow SapRow in tblTemp.Rows)
                    {
                        String sAdresse;
                        sAdresse = SapRow["NAME1_ZF"].ToString();
                        if (SapRow["NAME2_ZF"].ToString().Length > 0)
                        {
                            sAdresse += " " + SapRow["NAME2_ZF"].ToString();
                        }
                        if (SapRow["NAME3_ZF"].ToString().Length > 0)
                        {
                            sAdresse += " " + SapRow["NAME3_ZF"].ToString();
                        }
                        sAdresse += "<br/>" + SapRow["STREET_ZF"].ToString();
                        sAdresse += "<br/>" + SapRow["POST_CODE1_ZF"].ToString() + " " + SapRow["CITY1_ZF"].ToString();
                        SapRow["Adresse"] = sAdresse;

                        sAdresse = SapRow["NAME1_BANK"].ToString();
                        if (SapRow["NAME2_BANK"].ToString().Length > 0)
                        {
                            sAdresse += " " + SapRow["NAME2_BANK"].ToString();
                        }
                        if (SapRow["NAME3_BANK"].ToString().Length > 0)
                        {
                            sAdresse += " " + SapRow["NAME3_BANK"].ToString();
                        }
                        sAdresse += "<br/>" + SapRow["STREET_BANK"].ToString();
                        sAdresse += "<br/>" + SapRow["POST_CODE1_BANK"].ToString() + " " + SapRow["CITY1_BANK"].ToString();
                        SapRow["AdresseBank"] = sAdresse;
                    
                    }
                
                }
                m_tblFahrzeuge = CreateOutPut(tblTemp, m_strAppID);
                ResultExcel = CreateOutPut(tblTemp, m_strAppID);
                foreach (DataRow SapRow in ResultExcel.Rows)
                {
                    String sAdresse;
                    sAdresse = SapRow["Adresse"].ToString().Replace("<br/>", " ");

                    SapRow["Adresse"] = sAdresse;
                    sAdresse = SapRow["AdresseBank"].ToString().Replace("<br/>", " ");
                    SapRow["AdresseBank"] = sAdresse;

                }

                m_tblFehlerFahrzeuge = myProxy.getExportTable("GT_ERR_OUT");



                mE_SUBRC = myProxy.getExportParameter("E_SUBRC");
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE");
                if (mE_SUBRC != "0")
                {
                    m_intStatus = Convert.ToInt32(mE_SUBRC);
                    m_strMessage = mE_MESSAGE;
                    WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR, ref m_tblUpload);
                }
                else
                {
                    WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblFahrzeuge);
                }
               
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Bei der Suche der Dokumente ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }

        }

        public void ShowAll(String strAppID, String strSessionID, System.Web.UI.Page page, String OhneHaendlerSperre)
        {
            m_strClassAndMethod = "FzgSperren.ShowAll";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            mE_SUBRC = "";
            mE_MESSAGE = "";

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_READ_SP_HAEND_FZG_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("F_KEINE_HAEND_SPERR", OhneHaendlerSperre);



                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_FIN_OUT");
                tblTemp.Columns.Add("Adresse", System.Type.GetType("System.String"));
                tblTemp.Columns.Add("AdresseBank", System.Type.GetType("System.String"));
               
                
                if (tblTemp.Rows.Count > 0)
                {
                    //for (Int32 i = tblTemp.Rows.Count-1; i >= 0; i--)
                    //{
                    //    if (tblTemp.Rows[i]["LICENSE_NUM"].ToString() != "" && tblTemp.Rows[i]["WEB_USER"].ToString() == m_objUser.UserName)
                    //    {
                    //       tblTemp.Rows.RemoveAt(i);
                    //     }
                    //}

                    
                    
                    foreach (DataRow SapRow in tblTemp.Rows)
                    {
                        String sAdresse;
                        sAdresse = SapRow["NAME1_ZF"].ToString();
                        if (SapRow["NAME2_ZF"].ToString().Length > 0)
                        {
                            sAdresse += " " + SapRow["NAME2_ZF"].ToString();
                        }
                        if (SapRow["NAME3_ZF"].ToString().Length > 0)
                        {
                            sAdresse += " " + SapRow["NAME3_ZF"].ToString();
                        }
                        sAdresse += "<br/>" + SapRow["STREET_ZF"].ToString();
                        sAdresse += "<br/>" + SapRow["POST_CODE1_ZF"].ToString() + " " + SapRow["CITY1_ZF"].ToString();
                        SapRow["Adresse"] = sAdresse;

                        sAdresse = SapRow["NAME1_BANK"].ToString();
                        if (SapRow["NAME2_BANK"].ToString().Length > 0)
                        {
                            sAdresse += " " + SapRow["NAME2_BANK"].ToString();
                        }
                        if (SapRow["NAME3_BANK"].ToString().Length > 0)
                        {
                            sAdresse += " " + SapRow["NAME3_BANK"].ToString();
                        }
                        sAdresse += "<br/>" + SapRow["STREET_BANK"].ToString();
                        sAdresse += "<br/>" + SapRow["POST_CODE1_BANK"].ToString() + " " + SapRow["CITY1_BANK"].ToString();
                        SapRow["AdresseBank"] = sAdresse;

                    }

                }



                tblTemp.DefaultView.Sort = "RELDT ASC";

                tblTemp = tblTemp.DefaultView.ToTable();

                m_tblFahrzeuge = CreateOutPut(tblTemp, m_strAppID);
                ResultExcel = CreateOutPut(tblTemp, m_strAppID);
                foreach (DataRow SapRow in ResultExcel.Rows)
                    {
                        String sAdresse;
                        sAdresse=SapRow["Adresse"].ToString().Replace("<br/>"," ");
                       
                        SapRow["Adresse"] = sAdresse;
                        sAdresse=SapRow["AdresseBank"].ToString().Replace("<br/>"," ");
                        SapRow["AdresseBank"] = sAdresse;

                    }

                mE_SUBRC = myProxy.getExportParameter("E_SUBRC");
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE");
                if (mE_SUBRC != "0")
                {
                    m_intStatus = Convert.ToInt32(mE_SUBRC);
                    m_strMessage = mE_MESSAGE;
                    WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR, ref m_tblUpload);
                }
                else
                {
                    WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblFahrzeuge);
                }

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Bei der Suche der Dokumente ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }

        }

        public void Sperren(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "FzgSperren.Sperren";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            mE_SUBRC = "";
            mE_MESSAGE = "";

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SPERR_HAEND_FZG_01", ref m_objApp, ref m_objUser, ref page);

                if (!string.IsNullOrEmpty(m_debitor))
                    myProxy.setImportParameter("I_KUNNR_AG", m_debitor.PadLeft(10, '0'));
                else
                    myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.setImportParameter("I_WEB_USER", m_objUser.UserName.PadLeft(40));

                DataTable SapTable = myProxy.getImportTable("GT_FIN_IN");

                DataRow tmpSAPRow = SapTable.NewRow();

                {
                    tmpSAPRow["CHASSIS_NUM"] = m_strFahrgestellnummer;
                }

                SapTable.Rows.Add(tmpSAPRow);
                SapTable.AcceptChanges();
                
                myProxy.callBapi();
                
                mE_SUBRC = myProxy.getExportParameter("E_SUBRC");
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE");
                if (mE_SUBRC != "0")
                {
                    m_intStatus = Convert.ToInt32(mE_SUBRC);
                    m_strMessage = mE_MESSAGE;
                    WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR, ref m_tblUpload);
                }
                else
                {
                    WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblFahrzeuge);
                }
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Bei der Suche der Dokumente ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }

        public void Entsperren(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "FzgSperren.Sperren";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            mE_SUBRC = "";
            mE_MESSAGE = "";

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_ENTSP_HAEND_FZG_01", ref m_objApp, ref m_objUser, ref page);

                if (!string.IsNullOrEmpty(m_debitor))
                    myProxy.setImportParameter("I_KUNNR_AG", m_debitor.PadLeft(10, '0'));
                else
                    myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                myProxy.setImportParameter("I_WEB_USER", m_objUser.UserName.PadLeft(40));

                DataTable SapTable = myProxy.getImportTable("GT_FIN_IN");

                DataRow tmpSAPRow = SapTable.NewRow();

                {
                    tmpSAPRow["CHASSIS_NUM"] = m_strFahrgestellnummer;
                }

                SapTable.Rows.Add(tmpSAPRow);
                SapTable.AcceptChanges();

                myProxy.callBapi();

                mE_SUBRC = myProxy.getExportParameter("E_SUBRC");
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE");
                if (mE_SUBRC != "0")
                {
                    m_intStatus = Convert.ToInt32(mE_SUBRC);
                    m_strMessage = mE_MESSAGE;
                    WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR, ref m_tblUpload);
                }
                else
                {
                    WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblFahrzeuge);
                }
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Bei der Suche der Dokumente ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }
    }
}
