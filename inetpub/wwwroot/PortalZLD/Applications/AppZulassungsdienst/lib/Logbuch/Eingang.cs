using System;
using CKG.Base.Common;
using CKG.Base.Business;

namespace AppZulassungsdienst.lib.Logbuch
{
	public class Eingang : LogbuchEntry
	{
		private string VON;

		public string Verfasser 
        {
			get { return VON; }
		}

        public Eingang(string vorgid, string lfdnr, DateTime erfassungszeit, string von, string vertr, string betreff, string ltxnr, string antw_lfdnr,
            EntryStatus objstatus, EmpfängerStatus statuse, string vgart, string zerldat, string an, ref CKG.Base.Kernel.Security.User objUser,
            CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
            : base(vorgid, lfdnr, erfassungszeit, vertr, betreff, ltxnr, antw_lfdnr, objstatus, statuse, vgart, zerldat, an, ref objUser,
            objApp, strAppID, strSessionID, strFilename)
		{
			this.VON = von;
		}

        public void EintragBeantworten(string strAppID, string strSessionID, System.Web.UI.Page page, string Betreff, string Text)
		{
            m_strClassAndMethod = "Eingang.EintragBeantworten";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;

                LongStringToSap lsts = new LongStringToSap(m_objUser, m_objApp, page);
                string ltxnr = "";
                if (Text.Trim().Length != 0)
                {
                    ltxnr = lsts.InsertString(Text, "MC");
                    if (lsts.E_SUBRC != "0")
                    {
                        int lstsStatus = 0;
                        Int32.TryParse(lsts.E_SUBRC, out lstsStatus);
                        m_intStatus = lstsStatus;
                        m_strMessage = lsts.E_MESSAGE;
                        return;
                    }
                }

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_MC_SAVE_ANSWER", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VORGID", VORGID);
                    myProxy.setImportParameter("I_LFDNR", LFDNR);
                    myProxy.setImportParameter("I_AN", Verfasser);
                    myProxy.setImportParameter("I_VON", Empfänger);
                    myProxy.setImportParameter("I_BD_NR", m_objUser.UserName.ToUpper());
                    myProxy.setImportParameter("I_LTXNR", ltxnr);

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
                    if (!string.IsNullOrEmpty(ltxnr))
                    {
                        lsts.DeleteString(ltxnr);
                    }
                }
                finally { m_blnGestartet = false; }
            }
		}

        public void Rückfrage(string strAppID, string strSessionID, System.Web.UI.Page page, string Betreff, string Text)
		{
            m_strClassAndMethod = "Eingang.Rückfrage";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;

                LongStringToSap lsts = new LongStringToSap(m_objUser, m_objApp, page);
                string ltxnr = "";
                if (Text.Trim().Length != 0)
                {
                    ltxnr = lsts.InsertString(Text, "MC");
                    if (lsts.E_SUBRC != "0")
                    {
                        int lstsStatus = 0;
                        Int32.TryParse(lsts.E_SUBRC, out lstsStatus);
                        m_intStatus = lstsStatus;
                        m_strMessage = lsts.E_MESSAGE;
                        return;
                    }
                }

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_MC_NEW_VORGANG", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_UNAME", AN);
                    myProxy.setImportParameter("I_BD_NR", m_objUser.UserName.ToUpper());
                    myProxy.setImportParameter("I_AN", Verfasser);
                    myProxy.setImportParameter("I_LTXNR", ltxnr);
                    myProxy.setImportParameter("I_BETREFF", Betreff);
                    myProxy.setImportParameter("I_VGART", "FILL");
                    myProxy.setImportParameter("I_ZERLDAT", "");
                    myProxy.setImportParameter("I_VKBUR", m_objUser.Kostenstelle);

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
                    if (!string.IsNullOrEmpty(ltxnr))
                    {
                        lsts.DeleteString(ltxnr);
                    }
                }
                finally { m_blnGestartet = false; }
            }
		}

        public void EintragBeantworten(string strAppID, string strSessionID, System.Web.UI.Page page, EmpfängerStatus status)
		{
            m_strClassAndMethod = "Eingang.EintragBeantworten";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_MC_SAVE_STATUS_IN", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VORGID", VORGID);
                    myProxy.setImportParameter("I_LFDNR", LFDNR);
                    myProxy.setImportParameter("I_AN", Empfänger);
                    myProxy.setImportParameter("I_BD_NR", m_objUser.UserName.ToUpper());
                    myProxy.setImportParameter("I_STATUSE", LogbuchEntry.TranslateEmpfängerStatus(status));

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
