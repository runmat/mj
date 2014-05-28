using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Kernel;
using CKG.Base.Business;
using CKG.Base.Common;
using CKG.Base;
using System.Data;

namespace AppRemarketing.lib
{
    public class Auswertung : BankBase
    {
        #region "Properties"

            public String AVNR { set; get; }
            public String DatumVon { set; get; }
            public String DatumBis { set; get; }
            public String Fahrgestellnummer { set; get; }
            public String Kennzeichen { set; get; }
            public String Inventarnummer { set; get; }
            public String Rechnungsnummer { set; get; }
        #endregion


                public Auswertung(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
            : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
        {
        }

        public override void Show()
        {

        }

        public override void Change()
        {

        }

        public void Show(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Auswertung.Show";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SCHADENBELA_02", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KFZKZ", Kennzeichen);
                myProxy.setImportParameter("I_FIN", Fahrgestellnummer);
                myProxy.setImportParameter("I_INVENTAR", Inventarnummer);
                myProxy.setImportParameter("I_AVNR", AVNR);
                myProxy.setImportParameter("I_ERDAT_VON", DatumVon);
                myProxy.setImportParameter("I_ERDAT_BIS", DatumBis);
                myProxy.setImportParameter("I_RENNR", Rechnungsnummer);

                myProxy.callBapi();

                Result = myProxy.getExportTable("GT_OUT");


                if (Result.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Daten zur Anzeige gefunden.";
                }
                else
                {
                    foreach (DataRow row in Result.Rows)
                    {

                        row["KMSTAND"] = row["KMSTAND"].ToString().TrimStart('0');
                        if (row["KMSTAND"].ToString().Length == 0)
                        {
                            row["KMSTAND"] = "0";
                        }
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

    }
}