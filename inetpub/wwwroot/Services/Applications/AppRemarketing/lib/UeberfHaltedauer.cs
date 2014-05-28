using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using CKG.Base.Kernel.Security;
using System.Web.UI;
using CKG.Base.Common;
using CKG.Base.Business;

namespace AppRemarketing.lib
{
    public class UeberfHaltedauer
    {
        const string BapiName = "Z_DPM_REM_CHECK_HALTEDAUER";

        public DataTable Result { get; private set; }

        public void Search(User user, App app, Page page, string fin, string kennz, string invent, string avnr, DateTime von, DateTime bis)
        {
            DataTable result;
            try
            {
                var localUser = user;
                var localApp = app;

                var myProxy = DynSapProxy.getProxy(BapiName, ref localApp, ref localUser, ref page);

                myProxy.setImportParameter("I_KUNNR", localUser.KUNNR.PadLeft(10, '0'));//KUNNR
                if (!string.IsNullOrEmpty(fin)) myProxy.setImportParameter("I_FIN", fin);
                if (!string.IsNullOrEmpty(kennz)) myProxy.setImportParameter("I_KENNZ", kennz);
                if (!string.IsNullOrEmpty(invent)) myProxy.setImportParameter("I_INVENT", invent);
                if (!string.IsNullOrEmpty(avnr) && avnr!="00") myProxy.setImportParameter("I_AVNR", avnr);
                myProxy.setImportParameter("I_DAT_VON", von.ToShortDateString());
                myProxy.setImportParameter("I_DAT_BIS", bis.ToShortDateString());
                //if (hceingang)
                //{
                //    myProxy.setImportParameter("I_HCEINGANG", "X");
                //}
                //else
                //{
                //    myProxy.setImportParameter("I_HCEINGANG", "");
                //}

                myProxy.callBapi();

                result = myProxy.getExportTable("GT_OUT");

                foreach (DataRow dRow in result.Rows)
                {
                    dRow["TAGE_UEBERF"] = dRow["TAGE_UEBERF"].ToString().Trim().PadLeft(6, '0');
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Beim Erstellen des Reportes ist ein Fehler aufgetreten.\n(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
                //WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }

            Result = result;
        }
    }
}