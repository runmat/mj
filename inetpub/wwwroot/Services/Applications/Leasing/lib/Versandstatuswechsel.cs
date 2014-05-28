using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using CKG.Base.Business;
using CKG.Base.Common;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;

namespace Leasing.lib
{
    public class Versandstatuswechsel : BankBase
    {
        public Versandstatuswechsel(ref User objUser, ref App objApp, string strAppID)
            : base(ref objUser, ref objApp, strAppID, HttpContext.Current.Session.SessionID, string.Empty)
        {
        }

        public void GetData(Page page, string fahrgestellnummer, string kennzeichen, string vertragsnummer,
                            string objektnummer, DateTime? von, DateTime? bis)
        {
            try
            {
                m_intStatus = 0;
                m_strMessage = string.Empty;
                m_tblResult = null;

                var proxy = DynSapProxy.getProxy("Z_DPM_TEMP_VERSENDUNGEN_01", ref m_objApp, ref m_objUser, ref page);
                proxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                if (!string.IsNullOrEmpty(fahrgestellnummer))
                    proxy.setImportParameter("I_CHASSIS_NUM", fahrgestellnummer);
                if (!string.IsNullOrEmpty(kennzeichen)) proxy.setImportParameter("I_LICENSE_NUM", kennzeichen);
                if (!string.IsNullOrEmpty(vertragsnummer)) proxy.setImportParameter("I_LIZNR", vertragsnummer);
                if (!string.IsNullOrEmpty(objektnummer)) proxy.setImportParameter("I_ZZREFERENZ1", objektnummer);
                if (von.HasValue) proxy.setImportParameter("I_ZZTMPDT_VON", von.Value.ToShortDateString());
                if (bis.HasValue) proxy.setImportParameter("I_ZZTMPDT_BIS", bis.Value.ToShortDateString());

                proxy.callBapi();

                var tmpResult = proxy.getExportTable("GT_OUT");
                var statusCol = new DataColumn { ColumnName = "Status", DataType = typeof(string), DefaultValue = "-" };
                tmpResult.Columns.Add(statusCol);
                tmpResult.AcceptChanges();

                m_tblResult = tmpResult;
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_intStatus = -3000;
                        m_strMessage = "Keine temporär versendeten Briefe gefunden.";
                        break;
                    default:
                        m_intStatus = -9999;
                        m_strMessage = string.Format("Beim Erstellen des Reports ist ein Fehler aufgetreten ({0}).", ex.Message);
                        break;
                }
            }
        }

        public void EndVersand(Page page)
        {
            try
            {
                m_intStatus = 0;
                m_strMessage = string.Empty;

                var proxy = DynSapProxy.getProxy("Z_DPM_TEMP_END_SPERR_01", ref m_objApp, ref m_objUser, ref page);
                proxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                var fzg = proxy.getImportTable("GT_FZG");
                var equnrs = Result.Select("Status='X'").Select(r => (string) r["EQUNR"]).ToList();
                equnrs.ForEach(e =>
                                   {
                                       var r = fzg.NewRow();
                                       r["EQUNR"] = e;
                                       fzg.Rows.Add(r);
                                   });
                fzg.AcceptChanges();

                proxy.callBapi();

                var tmpResult = proxy.getExportTable("GT_FZG");
                foreach (DataRow r in tmpResult.Rows)
                {
                    var equnr = (string) r["EQUNR"];
                    var r2 = Result.Select(string.Format("EQUNR='{0}'", equnr)).FirstOrDefault();
                    if (r2 != null)
                        r2["Status"] = r["BEM"] as string ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_intStatus = -3000;
                        m_strMessage = "Keine temporär versendeten Briefe gefunden.";
                        break;
                    default:
                        m_intStatus = -9999;
                        m_strMessage = string.Format("Beim Erstellen des Reports ist ein Fehler aufgetreten ({0}).", ex.Message);
                        break;
                }
            }
        }

        public override void Show()
        {
            throw new NotImplementedException();
        }

        public override void Change()
        {
            throw new NotImplementedException();
        }
    }
}