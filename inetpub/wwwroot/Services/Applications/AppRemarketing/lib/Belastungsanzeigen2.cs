using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using CKG.Base.Business;
using CKG.Base.Common;

namespace AppRemarketing.lib
{
    public class Belastungsanzeigen2 : BankBase
    {
        #region "Properties"

        public String AVNR { set; get; }
        public String CarportNr { set; get; }
        public String DatumVon { set; get; }
        public String DatumBis { set; get; }
        public String AuswahlStatus { set; get; }
        public String Fahrgestellnummer { set; get; }
        public String Kennzeichen { set; get; }
        public DataTable Vermieter { set; get; }
        public String FinGutachten { set; get; }
        public DataTable Gutachten { set; get; }
        public String Reklamationstext { set; get; }
        public DataTable TableReklamation { set; get; }
        public String Sachbearbeiter { set; get; }
        public String Telefon { set; get; }
        public String Mail { set; get; }
        public String Vertragsjahr { set; get; }

        #endregion

        public Belastungsanzeigen2(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
            : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
        {
        }

        public override void Show()
        {

        }

        public override void Change()
        {

        }

        public void Show(String strAppID, String strSessionID, Page page)
        {
            m_strClassAndMethod = "Belastungsanzeigen2.Show";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_BELASTUNGSANZEIGE_02", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KFZKZ", Kennzeichen);
                myProxy.setImportParameter("I_FIN", Fahrgestellnummer);
                if (Vertragsjahr != null) { myProxy.setImportParameter("I_VJAHR", Vertragsjahr); }
                if (AVNR != "00") { myProxy.setImportParameter("I_AVNR", AVNR); }
                if (CarportNr != "00") { myProxy.setImportParameter("I_HC", CarportNr); }
                myProxy.setImportParameter("I_ERDAT_VON", DatumVon);
                myProxy.setImportParameter("I_ERDAT_BIS", DatumBis);
                myProxy.setImportParameter("I_STATU", AuswahlStatus);

                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_OUT");

                Result = tblTemp;
                Result.Columns.Add("Auswahl", typeof(String));

                if (Result.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Dokumente zur Anzeige gefunden.";
                }
                else
                {
                    foreach (DataRow Row in Result.Rows)
                    {
                        Belastungsanzeigen.MakeShortDate(Row, "HCEINGDAT", "GUTAUFTRAGDAT", "GUTADAT");
                       
                        Row["Auswahl"] = "0";
                        Row["REPKALK"] = Row["REPKALK"].ToString().TrimStart('0');
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

        public void Change(String strAppID, String strSessionID, Page page, string newStatus, string blockText = null) 
        {
            m_strClassAndMethod = "Belastungsanzeigen2.Change";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                var myProxy = DynSapProxy.getProxy("Z_DPM_REM_UPD_STAT_BELAST_02", ref m_objApp, ref m_objUser, ref page);
                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));

                var sapTable = myProxy.getImportTable("GT_IN");

                var dRows = Result.Select("Auswahl = '1'");
                foreach (var gridRow in dRows) 
                {
                    var newRow = sapTable.NewRow();
                    newRow["FAHRGNR"] = gridRow["FAHRGNR"].ToString();
                    newRow["KENNZ"] = gridRow["KENNZ"].ToString();
                    newRow["STATU"] = newStatus;
                    if (newStatus == "9")
                        newRow["BLOCKTEXT"] = blockText;

                    sapTable.Rows.Add(newRow);
                }
                
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

        public void getGutachten(String strAppID, String strSessionID, Page page)
        {
            m_strClassAndMethod = "Belastungsanzeigen2.getGutachten";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_BELASTUNGSA_IBUTT_02", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_FIN", FinGutachten);

                myProxy.callBapi();

                Gutachten = myProxy.getExportTable("GT_OUT");

                if (Gutachten.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Dokumente zur Anzeige gefunden.";
                }
                else
                {
                    foreach (DataRow Row in Gutachten.Rows)
                    {
                        Belastungsanzeigen.MakeShortDate(Row, "INDATUM", "GUTADAT");
                    }
                }
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

        public void setReklamation(String strAppID, String strSessionID, Page page)
        {
            m_strClassAndMethod = "Belastungsanzeigen2.setReklamation";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_BELASTUNGSA_RBUTT_02", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_FIN", FinGutachten);
                myProxy.setImportParameter("I_REKLM", Reklamationstext);
                myProxy.setImportParameter("I_SACHB", Sachbearbeiter);
                myProxy.setImportParameter("I_TELNR", Telefon);
                myProxy.setImportParameter("I_EMAIL", Mail);

                myProxy.callBapi();

                TableReklamation = myProxy.getExportTable("GT_OUT");

                if (TableReklamation.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");
                }
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

        public string ReadReklamationstext(String strAppID, String strSessionID, Page page, string FIN)
        {
            m_strClassAndMethod = "Belastungsanzeigen2.GetReklamationstext";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            
            if (string.IsNullOrEmpty(FIN))
                return null;

            var result = string.Empty;
            try
            {
                var myProxy = DynSapProxy.getProxy("Z_DPM_REM_READ_REKLATEXT", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_FIN", FIN);

                myProxy.callBapi();

                var gtOut = myProxy.getExportTable("GT_OUT");
                var reklms = gtOut.Rows.Cast<DataRow>().Select(row => row["REKLM"] as string).ToArray();
                result = string.Join(" ", reklms);

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref gtOut);
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

            return result;
        }

        public string ReadBlockadetext(string strAppID, string strSessionID, Page page, string FIN)
        {
            m_strClassAndMethod = "Belastungsanzeigen2.ReadBlockadeText";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (string.IsNullOrEmpty(FIN))
                return null;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_READ_BLOCKADETEXT_02", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_FIN", FIN);

                myProxy.callBapi();

                var gtOut = myProxy.getExportTable("GT_OUT");

                if (gtOut.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Blockadetext gefunden.";
                }

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref gtOut);

                var row = gtOut.Rows.Cast<DataRow>().FirstOrDefault();
                if (row != null)
                    return row["BLOCKTEXT"] as string;
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

            return null;
        }

        public void setOpen(String strAppID, String strSessionID, Page page)
        {
            m_strClassAndMethod = "Belastungsanzeigen2.setOpen";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                var myProxy = DynSapProxy.getProxy("Z_DPM_REM_UPD_STATNEU_BELA_01", ref m_objApp, ref m_objUser, ref page);
                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_WEB_USER", m_objUser.UserName);

                var sapTable = myProxy.getImportTable("GT_DAT");

                var dRows = Result.Select("Auswahl = '1'");
                foreach (var gridRow in dRows)
                {
                    var newRow = sapTable.NewRow();
                    newRow["FAHRGNR"] = gridRow["FAHRGNR"].ToString();
                    sapTable.Rows.Add(newRow);
                }

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
    }
}
