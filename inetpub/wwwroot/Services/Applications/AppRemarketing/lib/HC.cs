using System;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;

namespace AppRemarketing.lib
{
    public class HC : BankBase
	{
        #region "Declarations"

        DataTable m_tblHC;
        String m_strAVNr;
        String m_strSelectionType;
        String m_strDatumVon;
        String m_strDatumBis;
        DataTable m_tblUpload;
        DataTable m_tblError;
        Boolean m_Edit;

        #endregion

        #region "Properties"

        public DataTable Hereinnahmecenter
        {
            get { return m_tblHC; }
            set { m_tblHC = value; }
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

        #endregion

        #region "Methods"

        public HC(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
            : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
	    {
            //this.m_strFilename2 = strFilename;
		}

        public override void Show()
        {

        }

        public override void Change()
        {

        }

		public void getHC(String strAppID, String strSessionID, System.Web.UI.Page page)
		{
			try
			{
				DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_HC_NR_01", ref m_objApp, ref m_objUser, ref page);

				myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));//KUNNR
				myProxy.setImportParameter("I_KENNUNG", "HC");

				myProxy.callBapi();

				m_tblHC = myProxy.getExportTable("GT_WEB");

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

        public void setHCEingaenge(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "HC.setHCEingaenge";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SET_HC_DAT_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                DataTable tblTmp = myProxy.getImportTable("GT_IN");

                DataRow newRow;

                DataTable ImpTable;

                if (Edit == false)
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
                    newRow["HCEINGDAT"] = dr[2];
                    newRow["HCEINGTIM"] = dr[3].ToString().Replace(":","");
                    newRow["HCORT"] = dr[4];
                    newRow["KMSTAND"] = dr[5];

                    tblTmp.Rows.Add(newRow);
                }

                myProxy.callBapi();

                m_tblError = myProxy.getExportTable("GT_ERR");

                if (m_tblError.Rows.Count > 0)
                {
                    DataTable NewErrorTable = new DataTable();

                    NewErrorTable.Columns.Add("FAHRGNR", typeof(string));
                    NewErrorTable.Columns.Add("KENNZ", typeof(string));
                    NewErrorTable.Columns.Add("HCEINGDAT", typeof(string));
                    NewErrorTable.Columns.Add("HCEINGTIM", typeof(string));
                    NewErrorTable.Columns.Add("HCORT", typeof(string));
                    NewErrorTable.Columns.Add("KMSTAND", typeof(string));
                    NewErrorTable.Columns.Add("ZBEM", typeof(string));
                    NewErrorTable.Columns.Add("ID", typeof(int));

                    NewErrorTable.AcceptChanges();

                    string st;
                    DataRow NewErrorRow;

                    for (int i = 0; i < m_tblError.Rows.Count; i++)
                    {
                        NewErrorRow = NewErrorTable.NewRow();

                        st = m_tblError.Rows[i]["HCEINGTIM"].ToString();

                        NewErrorRow["FAHRGNR"] = m_tblError.Rows[i]["FAHRGNR"].ToString();
                        NewErrorRow["KENNZ"] = m_tblError.Rows[i]["KENNZ"].ToString();
                        NewErrorRow["HCEINGDAT"] = Convert.ToDateTime(m_tblError.Rows[i]["HCEINGDAT"]).ToShortDateString();
                        NewErrorRow["HCEINGTIM"] = st.Substring(0, 2) + ":" + st.Substring(2, 2) + ":" + st.Substring(4, 2);
                        NewErrorRow["HCORT"] = m_tblError.Rows[i]["HCORT"].ToString();
                        NewErrorRow["KMSTAND"] = m_tblError.Rows[i]["KMSTAND"].ToString();
                        NewErrorRow["ZBEM"] = m_tblError.Rows[i]["ZBEM"].ToString();
                        NewErrorRow["ID"] = i;

                        NewErrorTable.Rows.Add(NewErrorRow);
                    }

                    m_tblError = NewErrorTable;
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

        public void setHCAusgaenge(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "HC.setHCAusgaenge";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SET_DAT_HC_AUSG_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                DataTable tblTmp = myProxy.getImportTable("GT_IN");

                DataRow newRow;

                DataTable ImpTable;

                if (Edit == false)
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
                    newRow["DAT_HC_AUSG"] = dr[2];

                    tblTmp.Rows.Add(newRow);
                }

                myProxy.callBapi();

                m_tblError = myProxy.getExportTable("GT_ERR");

                if (m_tblError.Rows.Count > 0)
                {
                    DataTable NewErrorTable = new DataTable();

                    NewErrorTable.Columns.Add("FAHRGNR", typeof(string));
                    NewErrorTable.Columns.Add("KENNZ", typeof(string));
                    NewErrorTable.Columns.Add("DAT_HC_AUSG", typeof(string));
                    NewErrorTable.Columns.Add("ZBEM", typeof(string));
                    NewErrorTable.Columns.Add("ID", typeof(int));

                    NewErrorTable.AcceptChanges();

                    DataRow NewErrorRow;

                    for (int i = 0; i < m_tblError.Rows.Count; i++)
                    {
                        NewErrorRow = NewErrorTable.NewRow();

                        NewErrorRow["FAHRGNR"] = m_tblError.Rows[i]["FAHRGNR"].ToString();
                        NewErrorRow["KENNZ"] = m_tblError.Rows[i]["KENNZ"].ToString();
                        NewErrorRow["DAT_HC_AUSG"] = Convert.ToDateTime(m_tblError.Rows[i]["DAT_HC_AUSG"]).ToShortDateString();
                        NewErrorRow["ZBEM"] = m_tblError.Rows[i]["ZBEM"].ToString();
                        NewErrorRow["ID"] = i;

                        NewErrorTable.Rows.Add(NewErrorRow);
                    }

                    m_tblError = NewErrorTable;
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
