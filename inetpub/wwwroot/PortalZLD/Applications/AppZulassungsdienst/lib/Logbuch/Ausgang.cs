using CKG.Base.Common;
using System;
using CKG.Base.Business;

namespace AppZulassungsdienst.lib.Logbuch
{
	public class Ausgang : LogbuchEntry
	{
		private string ERLDAT;

		public string ErledigtAm 
        {
			get { return ERLDAT; }
		}

		public Ausgang(string vorgid, string lfdnr, DateTime erfassungszeit, string an, string vertr, string betreff, string ltxnr, string antw_lfdnr,
            EntryStatus objstatus, EmpfängerStatus statuse, string vgart, string zerldat, string erldat, ref CKG.Base.Kernel.Security.User objUser,
            CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
            : base(vorgid, lfdnr, erfassungszeit, vertr, betreff, ltxnr, antw_lfdnr, objstatus, statuse, vgart, zerldat, an, ref objUser, 
            objApp, strAppID, strSessionID, strFilename)
		{
			this.ERLDAT = erldat;
		}

        public void EintragAbschliessen(string strAppID, string strSessionID, System.Web.UI.Page page, EntryStatus status)
		{
            m_strClassAndMethod = "Ausgang.EintragAbschliessen";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_MC_SAVE_STATUS_OUT", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VORGID", VORGID);
                    myProxy.setImportParameter("I_LFDNR", LFDNR);
                    myProxy.setImportParameter("I_BD_NR", m_objUser.UserName.ToUpper());
                    myProxy.setImportParameter("I_STATUS", TranslateEntryStatus(status));

                    myProxy.callBapi();

                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    m_intStatus = subrc;
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    m_strMessage = sapMessage;
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            m_intStatus = -5555;
                            m_strMessage = "Keine Daten gefunden.";
                            break;
                        default:
                            m_intStatus = -9999;
                            m_strMessage = m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
		}
	}
}
