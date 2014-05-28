using System;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;

namespace AppRemarketing.lib
{
    public class Fahrzeugbestand: BankBase
    {
        #region "Declarations"

        DataTable m_tblVermieter;
        String m_strAVNr;
        String m_strArt;
        String m_strAVName;

        #endregion

        #region "Properties"

        public DataTable Vermieter
        {
            get { return m_tblVermieter; }
            set { m_tblVermieter = value; }
        }

        public String AVNr
        {
            get { return m_strAVNr; }
            set { m_strAVNr = value; }
        }

        public String AVName
        {
            get { return m_strAVName; }
            set { m_strAVName = value; }
        }

        public String Art
        {
            get { return m_strArt; }
            set { m_strArt = value; }
        }

        public string StatusArt { set; get; }
        public string Ereignisart { set; get; }

        public string Vertragsjahr { set; get; }
        public string Fahrgestellnummer { set; get; }
        public string Kennzeichen { set; get; }
        public string Inventarnummer { set; get; }
        public string DatumVon { set; get; }
        public string DatumBis { set; get; }
        /// <summary>
        /// A = alle, K = Kauffahrzeuge, M = Mietfahrzeuge
        /// </summary>
        public string Auswahl { get; set; }

        #endregion

        #region "Methods"

        public Fahrzeugbestand(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
                                        : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
	    {
	    }

        public void getVermieter(String strAppID, String strSessionID, System.Web.UI.Page page)
        
        {
            m_strClassAndMethod = "Fahrzeugbestand.getVermieter";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_AUFTR6_001", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNUNG", "VERMIETER");

                myProxy.callBapi();

                m_tblVermieter = myProxy.getExportTable("GT_WEB");

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


        }

        public override void Change()
        {

        }

        public override void Show()
        {

        }

        public void Show(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "FehlendeDaten.FILL";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_REM_004", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_AVNR",m_strAVNr );
                myProxy.setImportParameter("I_LEASING", m_strArt);

 
                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_WEB");

                Result = CreateOutPut(tblTemp, m_strAppID);

                if (Result.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Dokumente zur Anzeige gefunden.";
                }
                else
                {
                    foreach (DataRow row in Result.Rows)
                    {
                        row["Autovermieter"] = AVName;
                        Result.AcceptChanges();
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

        public void ShowVW(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Fahrzeugbestand.ShowVW";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_REM_004", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                
                myProxy.setImportParameter("I_LEASING", m_strArt);
                myProxy.setImportParameter("I_ZULA", StatusArt);
                myProxy.setImportParameter("I_EREIGNIS", Ereignisart);
                myProxy.setImportParameter("I_VJAHR", Vertragsjahr);

                myProxy.setImportParameter("I_FIN", Fahrgestellnummer);
                myProxy.setImportParameter("I_INVENTAR", Inventarnummer);
                myProxy.setImportParameter("I_KENNZ", Kennzeichen);
                myProxy.setImportParameter("I_AUSLDAT_VON", DatumVon);
                myProxy.setImportParameter("I_AUSLDAT_BIS", DatumBis);


                if (m_strAVNr != "00")
                {
                    myProxy.setImportParameter("I_AVNR", m_strAVNr);
                }

                if (!String.IsNullOrEmpty(Auswahl))
                {
                    myProxy.setImportParameter("I_MIETFZG", Auswahl);
                }

                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_WEB");

                Result = tblTemp;

                if (Result.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Dokumente zur Anzeige gefunden.";
                }
                else
                {
                    foreach (DataRow row in Result.Rows)
                    {
                        row["POS_TEXT"] = row["AVNR"] + " " + row["POS_TEXT"];
                       
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

        #endregion
    }
}
