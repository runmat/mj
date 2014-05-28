using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Business;
using CKG.Base.Kernel.Security;
using System.Web.UI;
using CKG.Base.Common;

namespace AppRemarketing.lib
{
    public class Vermieter : BankBase
    {
        public Vermieter(User user, App app, string appId) :
            base(ref user, ref app, appId, HttpContext.Current.Session.SessionID, string.Empty)
        {
            KUNNR = user.KUNNR;
        }

        const string BAPI_read_auftr6 = "Z_DPM_READ_AUFTR6_001";

        public void LoadData(Page page)
        {
            m_intStatus = 0;

            var mi = System.Reflection.MethodInfo.GetCurrentMethod();
            m_strClassAndMethod = mi.DeclaringType.Name + "." + mi.Name;

            try
            {
                var proxy = DynSapProxy.getProxy(BAPI_read_auftr6, ref m_objApp, ref m_objUser, ref page);

                proxy.setImportParameter("I_KUNNR", KUNNR);
                proxy.setImportParameter("I_KENNUNG", "VERMIETER");

                proxy.callBapi();

                var result = proxy.getExportTable("GT_WEB");

                if (result.Rows.Count == 0)
                {
                    m_intStatus = -10;
                    m_strMessage = "Kein Suchergebnis.";
                }

                m_tblResult = result;

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_intStatus = -12;
                        m_strMessage = "Keine Vermieter gefunden.";
                        break;
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
            throw new NotImplementedException();
        }

        public override void Show()
        {
            throw new NotImplementedException();
        }
    }
}