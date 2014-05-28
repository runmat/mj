using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using CKG.Base.Business;
using CKG.Base.Common;

namespace AppRemarketing.lib
{
    public class Belastungsanzeigen : BankBase
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
        public String Rechnungsnummer { set; get; }
        public DataTable Belastungen { set; get; }
        public String GutschriftBetrag { set; get; }
        public String GutschriftBemerkung { set; get; }
        public String Gutachter { set; get; }
        public String Empfaenger { set; get; }
        public String MerkantilerMinderwert { set; get; }
        public String Belegart { set; get; }
        public String Inventarnummer { set; get; }
        public String Sachbearbeiter { set; get; }
        public String Telefon { set; get; }
        public String Mail { set; get; }
        public String Vertragsjahr { set; get; }
        public String Freibetrag { set; get; }
        public DataTable TableBemerkungen { set; get; }

        #endregion


        public Belastungsanzeigen(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
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
            m_strClassAndMethod = "Belastungsanzeigen.Show";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_BELASTUNGSANZEIGE", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KFZKZ", Kennzeichen);
                myProxy.setImportParameter("I_FIN", Fahrgestellnummer);
                myProxy.setImportParameter("I_INVENTAR", Inventarnummer);
                myProxy.setImportParameter("I_RENNR", Rechnungsnummer);
                myProxy.setImportParameter("I_NO_NULL", Freibetrag);
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
                //Result.Columns.Add("Reklamationstext", typeof(String));

                if (Result.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Dokumente zur Anzeige gefunden.";
                }
                else
                {
                    foreach (DataRow Row in Result.Rows)
                    {
                        MakeShortDate(Row, "HCEINGDAT", "GUTAUFTRAGDAT", "GUTADAT");
                       
                        Row["Auswahl"] = "0";
                        //if (Row["DDTEXT"] == "Widersprochen")
                        //{
                        //    Row["Reklamationstext"] = GetReklamationstext(m_strAppID, m_strSessionID, page: page, FIN: Row["FAHRGNR"].ToString());
                        //}
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
            m_strClassAndMethod = "Belastungsanzeigen.Change";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                var myProxy = DynSapProxy.getProxy("Z_DPM_REM_UPD_STATUS_BELASTUNG", ref m_objApp, ref m_objUser, ref page);
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

                var tblError = myProxy.getExportTable("GT_ERR");

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

        public void ShowRechnung(String strAppID, String strSessionID, Page page)
        {
            m_strClassAndMethod = "Belastungsanzeigen.ShowRechnung";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SCHADENRG_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNZ", Kennzeichen);
                myProxy.setImportParameter("I_FIN", Fahrgestellnummer);
                myProxy.setImportParameter("I_INVENTAR", Inventarnummer);
                myProxy.setImportParameter("I_RENNR", Rechnungsnummer);
                if (Vertragsjahr != null) { myProxy.setImportParameter("I_VJAHR", Vertragsjahr); }
                if (CarportNr != "00") { myProxy.setImportParameter("I_HCORT", CarportNr); }
                if (AVNR != "00") { myProxy.setImportParameter("I_AVNR", AVNR); }
                myProxy.setImportParameter("I_REDAT_VON", DatumVon);
                myProxy.setImportParameter("I_REDAT_BIS", DatumBis);
                myProxy.setImportParameter("I_STATU", AuswahlStatus);

                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_OUT");

                Result = tblTemp;

                if (Result.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Rechnungen zur Anzeige gefunden.";
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


        public void ShowRechnungMietfahrzeuge(String strAppID, String strSessionID, Page page)
        {
            m_strClassAndMethod = "Belastungsanzeigen.ShowRechnungMietfahrzeuge";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_RUECKK_RG_01 ", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNZ", Kennzeichen);
                myProxy.setImportParameter("I_FIN", Fahrgestellnummer);
                myProxy.setImportParameter("I_INVENTAR", Inventarnummer);
                myProxy.setImportParameter("I_RENNR", Rechnungsnummer);
                if (Vertragsjahr != null) { myProxy.setImportParameter("I_VJAHR", Vertragsjahr); }
                if (AVNR != "00") { myProxy.setImportParameter("I_AVNR", AVNR); }
                myProxy.setImportParameter("I_REDAT_VON", DatumVon);
                myProxy.setImportParameter("I_REDAT_BIS", DatumBis);
                myProxy.setImportParameter("I_STATU", AuswahlStatus);

                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_OUT");

                Result = tblTemp;

                if (Result.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Rechnungen zur Anzeige gefunden.";
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



        public void ShowBelastungsanzeigen(String strAppID, String strSessionID, Page page)
        {
            m_strClassAndMethod = "Belastungsanzeigen.ShowBelastungsanzeigen";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SCHADENBELA_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_RENNR", Rechnungsnummer);
 
                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_OUT");

                foreach (DataRow drow in tblTemp.Rows)
                {

                    drow["REDAT"] = drow["REDAT"].ToString().Substring(0, 10);


                }

                Belastungen = tblTemp;

                if (Belastungen.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Belastungsanzeigen zur Anzeige gefunden.";
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

        public void ShowNachbelastung(String strAppID, String strSessionID, Page page)
        {
            m_strClassAndMethod = "Belastungsanzeigen.ShowNachbelastung";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_NACHBELA_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNZEICHEN", Kennzeichen);
                myProxy.setImportParameter("I_FIN", Fahrgestellnummer);
                myProxy.setImportParameter("I_INVENTAR", Inventarnummer);


                if (AVNR != "00")
                {
                    myProxy.setImportParameter("I_AVNR", AVNR);
                }

                if (CarportNr != "00")
                {
                    myProxy.setImportParameter("I_HCORT", CarportNr);
                }

                if (Gutachter != "")
                {
                    myProxy.setImportParameter("I_GUTAID", Gutachter);
                }

                myProxy.setImportParameter("I_RENNR", Rechnungsnummer);
                myProxy.setImportParameter("I_ERDAT_VON", DatumVon);
                myProxy.setImportParameter("I_ERDAT_BIS", DatumBis);

                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_OUT");

                Result = tblTemp;

                //Result = CreateOutPut(tblTemp, m_strAppID);

                if (Result.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Dokumente zur Anzeige gefunden.";
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

        public void SetGutschrift(String strAppID, String strSessionID, Page page)
        {
            m_strClassAndMethod = "Belastungsanzeigen.SetGutschrift";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SCHADENGUT_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_RENNR", Rechnungsnummer);
                myProxy.setImportParameter("I_NETWR", GutschriftBetrag);
                myProxy.setImportParameter("I_KTEXT", GutschriftBemerkung);
                myProxy.setImportParameter("I_REFIN", Fahrgestellnummer);
                myProxy.setImportParameter("I_BELART", Belegart);
                myProxy.setImportParameter("I_EMPF", Empfaenger);
                myProxy.setImportParameter("I_INPUT", MerkantilerMinderwert);

                myProxy.callBapi();

                m_intStatus = Convert.ToInt16(myProxy.getExportParameter("E_SUBRC"));
                m_strMessage = myProxy.getExportParameter("E_MESSAGE");


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

        private void MakeShortDate(DataRow row, params string[] columns)
        {
            foreach (var col in columns)
            {
                var colValue = row[col];

                if (!(colValue is DBNull))
                {
                    var date = Helper.GetDate(colValue);
                    if (date.HasValue)
                    {
                        row[col] = date.Value.ToShortDateString();
                    }
                    else
                    {
                        row[col] = null;
                    }
                }
            }
        }

        public void getGutachten(String strAppID, String strSessionID, Page page)
        {
            m_strClassAndMethod = "Belastungsanzeigen.getGutachten";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_BELASTUNGSA_IBUTT", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_FIN", FinGutachten);

                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_OUT");

                Gutachten = tblTemp;

                if (Gutachten.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Dokumente zur Anzeige gefunden.";
                }
                else
                {
                    foreach (DataRow Row in Gutachten.Rows)
                    {
                        MakeShortDate(Row, "INDATUM", "GUTADAT");
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
            m_strClassAndMethod = "Belastungsanzeigen.setReklamation";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_BELASTUNGSA_RBUTT", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_FIN", FinGutachten);
                myProxy.setImportParameter("I_REKLM", Reklamationstext);
                myProxy.setImportParameter("I_SACHB", Sachbearbeiter);
                myProxy.setImportParameter("I_TELNR", Telefon);
                myProxy.setImportParameter("I_EMAIL", Mail);

                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_OUT");

                TableReklamation = tblTemp;

                if (TableReklamation.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    //m_strMessage = "Fehler beim Anlegen der Reklamation.";
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

        public void ShowBemerkung(String strAppID, String strSessionID, Page page)
        {
            m_strClassAndMethod = "Belastungsanzeigen.ShowBemerkung";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_READ_KTEXT_RG", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_RENNR", Rechnungsnummer);

                myProxy.callBapi();

                TableBemerkungen = myProxy.getExportTable("GT_OUT");

                


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

        public string ReadReklamationstext(String strAppID, String strSessionID, Page page, string FIN)
        {
            var bapiName = "Z_DPM_REM_READ_REKLATEXT";
            
            m_strClassAndMethod = "Belastungsanzeigen.GetReklamationstext";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            
            if (string.IsNullOrEmpty(FIN))
                return null;

            var result = string.Empty;
            try
            {
                var myProxy = DynSapProxy.getProxy(bapiName, ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_FIN", FIN);

                myProxy.callBapi();

                var gtOut = myProxy.getExportTable("GT_OUT");
                var reklms = gtOut.Rows.Cast<DataRow>().Select(row => row["REKLM"] as string).ToArray(); ;
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
            var bapiName = "Z_DPM_REM_READ_BLOCKADETEXT";

            m_strClassAndMethod = "Belastungsanzeigen.ReadBlockadeText";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (string.IsNullOrEmpty(FIN))
                return null;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy(bapiName, ref m_objApp, ref m_objUser, ref page);

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
    }
}
