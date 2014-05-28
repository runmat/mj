using System;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;

namespace AppRemarketing.lib
{
    public class Schadensgutachten : BankBase
    {

        #region "Declarations"

        protected DataTable m_tblUploads;
        private string m_svArt;
        private string m_svDatum;

        #endregion

        #region "Properties"

        public DataTable tblUploads
        {
            get { return m_tblUploads; }
            set { m_tblUploads = value; }
        }

        public string SvArt
        {
            get { return m_svArt; }
            set { m_svArt = value; }
        }

        public string SvDatum
        {
            get { return m_svDatum; }
            set { m_svDatum = value; }
        }

        #endregion

        #region "Methods"

        public Schadensgutachten(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
                                        : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
        {
            InitUploadTable();
        }

        private void InitUploadTable()
        {
            m_tblUploads = new DataTable();
            m_tblUploads.Columns.Add("FAHRGESTELLNUMMER");
            m_tblUploads.Columns.Add("STATUS");
            m_tblUploads.Columns.Add("DATUM");
            m_tblUploads.AcceptChanges();
        }

        public string getUploaddatum(String strAppID, String strSessionID, System.Web.UI.Page page, string fahrgestellnummer)
        {
            string erg = "";

            m_strClassAndMethod = "Schadensgutachten.getUploaddatum";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SET_SCHADENDAT_PDF", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_READ", "X");

                DataTable tblTemp = myProxy.getImportTable("GT_IN");
                DataRow newRow = tblTemp.NewRow();
                newRow["FIN"] = fahrgestellnummer;
                tblTemp.Rows.Add(newRow);
                tblTemp.AcceptChanges();

                myProxy.callBapi();

                tblTemp = myProxy.getExportTable("GT_OUT");
                if (tblTemp.Rows.Count > 0)
                {
                    erg = tblTemp.Rows[0]["SDPDF_DAT"].ToString();
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

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "),ref m_tblResult);

            }

            return erg;
        }

        public void setUploaddatum(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Schadensgutachten.setUploaddatum";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SET_SCHADENDAT_PDF", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_ART", SvArt);
                myProxy.setImportParameter("I_SVMDAT", SvDatum);

                DataTable tblTemp = myProxy.getImportTable("GT_IN");
                foreach (DataRow uRow in tblUploads.Rows)
                {
                    if (uRow["STATUS"].ToString() == "OK")
                    {
                        DataRow newRow = tblTemp.NewRow();
                        newRow["FIN"] = uRow["FAHRGESTELLNUMMER"];
                        tblTemp.Rows.Add(newRow);
                    }
                }
                tblTemp.AcceptChanges();

                myProxy.callBapi();

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

        public override void Change()
        {

        }

		public override void Show()
		{

		}

        #endregion

    }
}
