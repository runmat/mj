using System;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;

namespace AppRemarketing.lib
{
    public class Reifenversand : BankBase
	{
        #region "Declarations"

        DataTable m_tblUpload;
        Boolean m_Edit;

        #endregion

        #region "Properties"

        public DataTable tblUpload
        {
            get { return m_tblUpload; }
            set { m_tblUpload = value; }
        }

        public Boolean Edit
        {
            get { return m_Edit; }
            set { m_Edit = value; }
        }

        #endregion

        #region "Methods"

        public Reifenversand(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
            : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
	    {
		}

        public override void Show()
        {

        }

        public override void Change()
        {

        }

        public void setVersanddatum(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Reifenversand.setVersanddatum";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SET_REIFVERS_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                DataTable tblSap = myProxy.getImportTable("GT_DATEN");

                foreach (DataRow dr in tblUpload.Rows)
                {
                    DataRow newRow = tblSap.NewRow();

                    newRow["FAHRGNR"] = dr[0];
                    newRow["DAT_REIFVERS"] = dr[1];

                    tblSap.Rows.Add(newRow);
                }

                myProxy.callBapi();

                tblUpload = myProxy.getExportTable("GT_DATEN");

                var rowsOk = tblUpload.Select("RET is null OR RET = ''");
                foreach (var drOk in rowsOk)
                {
                    tblUpload.Rows.Remove(drOk);
                }

                tblUpload.Columns.Add("ID");
                for (int i = 0; i < tblUpload.Rows.Count; i++)
                {
                    tblUpload.Rows[i]["ID"] = i.ToString();
                }
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Speichern der Daten ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }
            }
        }

        #endregion
	}
}
