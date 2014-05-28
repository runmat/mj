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
    public class Versandauftraege : BankBase
    {
        #region "Declarations"

        String m_strFilename2;

      

        String m_strSelectedZahlungsart;
        String m_strSelectedBank;
        String m_strSelectedHaendler;

        DataTable m_tblFehler;
        DataTable m_tblUpload;
        DataTable m_tblFahrzeuge;
        DataTable m_tblAdressen;
        DataTable m_tblBankAdressen;
        DataTable m_tblMaterial;
        DataTable m_tblZahlungsarten;
        DataTable m_tblLaender;
        
        String m_strNameHaendler;
        String m_strName2Haendler;
        String m_strStrasseHaendler;
        String m_strNummerHaendler;
        String m_strPlzHaendler;
        String m_strOrtHaendler;
        String m_strLandHaendler;

        String m_strNameBank;
        String m_strName2Bank;
        String m_strStrasseBank;
        String m_strNummerBank;
        String m_strPlzBank;
        String m_strOrtBank;
        String m_strLandBank;
        Boolean m_AbwBriefEmpfaenger;

        #endregion

        #region "Properties"

        public Boolean AbwBriefEmpfaenger
        {
            get { return m_AbwBriefEmpfaenger; }
            set { m_AbwBriefEmpfaenger = value; }
        }
        public String SelectedBank
        {
            get { return m_strSelectedBank; }
            set { m_strSelectedBank = value; }
        }
      

        public String SelectedZahlungsart
        {
            get { return m_strSelectedZahlungsart; }
            set { m_strSelectedZahlungsart = value; }
        }

        public DataTable Fahrzeuge
        {
            get { return m_tblFahrzeuge; }

        }

        public DataTable Zahlungsarten
        {
            get { return m_tblZahlungsarten; }

        }

        public String SelectedHaendler
        {
            get { return m_strSelectedHaendler; }
            set { m_strSelectedHaendler = value; }
        }

        public DataTable Material
        {
            get { return m_tblMaterial; }

        }

        public DataTable Adressen
        {
            get { return m_tblAdressen; }

        }
        public DataTable BankAdressen
        {
            get { return m_tblBankAdressen; }

        }

        public DataTable Fehler
        {
            get { return m_tblFehler; }

        }


        public DataTable tblUpload
        {
            get { return m_tblUpload; }
            set { m_tblUpload = value; }
        }
        public DataTable Laender
        {
            get { return m_tblLaender; }

        }

        public String NameHaendler
        {
            get { return m_strNameHaendler; }
            set { m_strNameHaendler = value; }
        }
        public String Name2Haendler
        {
            get { return m_strName2Haendler; }
            set { m_strName2Haendler = value; }
        }
        public String StrasseHaendler
        {
            get { return m_strStrasseHaendler; }
            set { m_strStrasseHaendler = value; }
        }
        public String NummerHaendler
        {
            get { return m_strNummerHaendler; }
            set { m_strNummerHaendler = value; }
        }
        public String PlzHaendler
        {
            get { return m_strPlzHaendler; }
            set { m_strPlzHaendler = value; }
        }
        public String OrtHaendler
        {
            get { return m_strOrtHaendler; }
            set { m_strOrtHaendler = value; }
        }

        public String LandHaendler
        {
            get { return m_strLandHaendler; }
            set { m_strLandHaendler = value; }
        }


        public String NameBank
        {
            get { return m_strNameBank; }
            set { m_strNameBank = value; }
        }
        public String Name2Bank
        {
            get { return m_strName2Bank; }
            set { m_strName2Bank = value; }
        }
        public String StrasseBank
        {
            get { return m_strStrasseBank; }
            set { m_strStrasseBank = value; }
        }
        public String NummerBank
        {
            get { return m_strNummerBank; }
            set { m_strNummerBank = value; }
        }
        public String PlzBank
        {
            get { return m_strPlzBank; }
            set { m_strPlzBank = value; }
        }
        public String OrtBank
        {
            get { return m_strOrtBank; }
            set { m_strOrtBank = value; }
        }

        public String LandBank
        {
            get { return m_strLandBank; }
            set { m_strLandBank = value; }
        }


        #endregion

        #region "Methods"


        public Versandauftraege(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
            : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
        {
            this.m_strFilename2 = strFilename;

        }


        public override void Change()
        {

        }

        public override void Show()
        {

        }

        public void getBankAdressen(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Versandauftraege.getAdressen";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_ADRESSPOOL_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_EQTYP", "B");
                



                myProxy.callBapi();

                m_tblBankAdressen = myProxy.getExportTable("GT_ADRS");


                DataTable ExportTable = new DataTable();


                m_tblBankAdressen.Columns.Add("Anzeige", string.Empty.GetType());
                m_tblBankAdressen.Columns.Add("Wert", string.Empty.GetType());



                DataRow NewRow = null;


                foreach (DataRow Row in m_tblBankAdressen.Rows)
                {

                    Row["Anzeige"] = Row["Name1"].ToString() + " " + Row["Name2"].ToString() + "  -  " + Row["STREET"].ToString() + ", " + Row["POST_CODE1"].ToString() + " " + Row["CITY1"].ToString();
                    Row["Wert"] = Row["KUNNR"].ToString();



                }

                NewRow = m_tblBankAdressen.NewRow();
                NewRow["Wert"] = "00";
                NewRow["Anzeige"] = "wählen Sie eine Bank";
                m_tblBankAdressen.Rows.InsertAt(NewRow, 0);





                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblBankAdressen);

                

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {

                    case "EXCEPTION NO_DATA RAISED":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Fehler beim Abfragen der Händlerdaten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblBankAdressen);

            }

        }


        public void getAdressen(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Versandauftraege.getAdressen";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_ADRESSPOOL_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_EQTYP", "T");


                myProxy.callBapi();

                m_tblAdressen = myProxy.getExportTable("GT_ADRS");


                DataTable ExportTable = new DataTable();


                m_tblAdressen.Columns.Add("Anzeige", string.Empty.GetType());
                m_tblAdressen.Columns.Add("Wert", string.Empty.GetType());


 
                foreach (DataRow Row in m_tblAdressen.Rows)
                {

                    Row["Anzeige"] = Row["Name1"].ToString() + " " + Row["Name2"].ToString() + "  -  " + Row["STREET"].ToString() + ", " + Row["POST_CODE1"].ToString() + " " + Row["CITY1"].ToString();
                    Row["Wert"] = Row["KUNNR"].ToString();



                }



                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblAdressen);

               
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {

                    case "EXCEPTION NO_DATA RAISED":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Fehler beim Abfragen der Händlerdaten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblAdressen);

            }

        }

        public void GetZahlungsart(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Versandauftraege.GetZahlungsart";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNUNG", "ZAHL");



                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_WEB");

                DataTable ExportTable = new DataTable();

                ExportTable.Columns.Add("Zahlungsnummer", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("Zahlungstext", System.Type.GetType("System.String"));


                DataRow Row = null;
                DataRow NewRow = null;
                ExportTable.Rows.Add(ExportTable.NewRow());
                ExportTable.Rows[0]["Zahlungsnummer"] = "9999";
                ExportTable.Rows[0]["Zahlungstext"] = "-Auswahl-";
                ExportTable.AcceptChanges();
                foreach (DataRow Row_loopVariable in tblTemp.Rows)
                {

                    Row = Row_loopVariable;
                    NewRow = ExportTable.NewRow();

                    NewRow["Zahlungsnummer"] = Row["POS_KURZTEXT"].ToString();
                    NewRow["Zahlungstext"] = Row["POS_KURZTEXT"].ToString() + " " + Row["POS_TEXT"].ToString();
                    ExportTable.Rows.Add(NewRow);
                }

                m_tblZahlungsarten = ExportTable.Copy();
                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblZahlungsarten);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {

                    case "EXCEPTION NO_DATA RAISED":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Fehler beim Abfragen der Händlerdaten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblZahlungsarten);

            }

        }

        public void GetMaterial(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Versandauftraege.GetMaterial";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNUNG", "MATERIAL");



                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_WEB");

                DataTable ExportTable = new DataTable();

                ExportTable.Columns.Add("Matnr", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("Materialtext", System.Type.GetType("System.String"));


                DataRow Row = null;
                DataRow NewRow = null;
                ExportTable.Rows.Add(ExportTable.NewRow());
                ExportTable.Rows[0]["Matnr"] = "9999";
                ExportTable.Rows[0]["Materialtext"] = "-Auswahl-";
                ExportTable.AcceptChanges();
                foreach (DataRow Row_loopVariable in tblTemp.Rows)
                {

                    Row = Row_loopVariable;
                    NewRow = ExportTable.NewRow();

                    NewRow["Matnr"] = Row["POS_KURZTEXT"].ToString();
                    NewRow["Materialtext"] = Row["POS_TEXT"].ToString();
                    ExportTable.Rows.Add(NewRow);
                }

                m_tblMaterial = ExportTable.Copy();
                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblMaterial);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {

                    case "EXCEPTION NO_DATA RAISED":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Fehler beim Abfragen der Händlerdaten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }

        }

       public void GetLaender(System.Web.UI.Page page)
        {

            m_strClassAndMethod = "Versandauftraege.GetLaender";
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNUNG", "LAND");

                myProxy.callBapi();

                m_tblLaender = myProxy.getExportTable("GT_WEB");
                m_tblLaender.Columns.Add("FullDesc", typeof(System.String));

                foreach (DataRow tmpRow in m_tblLaender.Rows)
                {
                    if (tmpRow["PSTLZ"].ToString().Length>0)
                    {
                        if (Convert.ToInt32(tmpRow["PSTLZ"])>0)
                        {
                            tmpRow["FullDesc"] = tmpRow["POS_TEXT"].ToString() + " (" + tmpRow["PSTLZ"].ToString() + ")";
                        }
                        else
                        {
                            tmpRow["FullDesc"] = tmpRow["POS_TEXT"].ToString();
                        }                        
                    }

                }
                
        
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_strMessage = "Keine Daten selektiert!";
                        m_intStatus = -1118;
                        break;
                    default:
                        m_strMessage = "Unbekannter Fehler.";
                        m_intStatus = -9999;
                        break;
                }
            }
        
        }


        public void sendToSap(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Versandauftraege.sendToSap";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            DataTable import;
            DataRow importRow;
            DataRow haendlerRow;
            DataRow bankRow;
            bool haendlerGewaehlt = false;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_IMP_VERS_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_WEB_USER", m_objUser.UserName);
                import = myProxy.getImportTable("GT_IN");
                haendlerRow = null;
                bankRow = null;
                if (SelectedZahlungsart != "7")
                {
                    // wenn Händler gewählt, dessen Adressdaten, sonst die Adressdaten aus den Fahrzeugdaten
                    if (!String.IsNullOrEmpty(m_strSelectedHaendler))
                    {
                        haendlerGewaehlt = true;
                        haendlerRow = Adressen.Select("Wert='" + m_strSelectedHaendler + "'")[0];
                    }
                    bankRow = BankAdressen.Select("Wert='" + m_strSelectedBank + "'")[0];
                }

                foreach (DataRow fzRow in Fahrzeuge.Select("Selected='X'"))
                {
                    importRow = import.NewRow();

                    foreach (DataColumn tmpCol in import.Columns)
                    {
                        switch (tmpCol.ColumnName.ToString())
                        {
                            case "WEB_USER":
                                importRow[tmpCol.ColumnName] = m_objUser.UserName;
                                break;

                            case "DZLART":
                                if (SelectedZahlungsart != "9999")
                                {
                                    importRow[tmpCol.ColumnName] = SelectedZahlungsart;
                                }
                                else
                                {
                                    importRow[tmpCol.ColumnName] = fzRow["DZLART"];
                                }
                                break;

                            case "BETRAG_RE":
                                //importRow[tmpCol.ColumnName] = fzRow[tmpCol.ColumnName].ToString().Replace(".", "").Replace(",", ".").TrimStart('0');
                                //einfach n
                                importRow[tmpCol.ColumnName] = fzRow[tmpCol.ColumnName].ToString();
                                break;
                            //case "VERSANDSPERR":
                            //    importRow[tmpCol.ColumnName] = "X";
                            //    break;

                            case "KUNNR_ZF":
                                if (SelectedZahlungsart== "7")
                                {
                                    importRow[tmpCol.ColumnName] = "";
                                }
                                else if (haendlerGewaehlt)
                                {
                                    importRow[tmpCol.ColumnName] = haendlerRow["KUNNR"];
                                }
                                else
                                {
                                    importRow[tmpCol.ColumnName] = fzRow["KUNNR_ZF"];
                                }
                               
                                break;
                            case "RDEALER":
                                if (SelectedZahlungsart == "7")
                                {
                                    importRow[tmpCol.ColumnName] = "";
                                }
                                else if (haendlerGewaehlt)
                                {
                                    importRow[tmpCol.ColumnName] = haendlerRow["KONZS"];
                                }
                                else
                                {
                                    importRow[tmpCol.ColumnName] = fzRow["RDEALER"];
                                }
                                
                                break;
                            case "NAME1_ZF":
                                if (SelectedZahlungsart == "7")
                                {
                                    importRow[tmpCol.ColumnName] = m_strNameHaendler;
                                }
                                else if (haendlerGewaehlt)
                                {
                                    importRow[tmpCol.ColumnName] = haendlerRow["NAME1"];
                                }
                                else
                                {
                                    importRow[tmpCol.ColumnName] = fzRow["NAME1_ZF"];
                                }

                                break;
                            case "NAME2_ZF":
                                if (SelectedZahlungsart == "7")
                                {
                                    importRow[tmpCol.ColumnName] = m_strName2Haendler;
                                }
                                else if (haendlerGewaehlt)
                                {
                                    importRow[tmpCol.ColumnName] = haendlerRow["NAME2"];
                                }
                                else
                                {
                                    importRow[tmpCol.ColumnName] = fzRow["NAME2_ZF"];
                                }

                                break;
                            case "NAME3_ZF":
                                    importRow[tmpCol.ColumnName] = "";
                                    break;
                            case "STREET_ZF":
                                if (SelectedZahlungsart == "7")
                                {
                                    importRow[tmpCol.ColumnName] = m_strStrasseHaendler;
                                }
                                else if (haendlerGewaehlt)
                                {
                                    importRow[tmpCol.ColumnName] = haendlerRow["STREET"] + " " + haendlerRow["HOUSE_NUM1"];
                                }
                                else
                                {
                                    importRow[tmpCol.ColumnName] = fzRow["STREET_ZF"];
                                }

                                break;
                            case "POST_CODE1_ZF":
                                if (SelectedZahlungsart == "7")
                                {
                                    importRow[tmpCol.ColumnName] = m_strPlzHaendler;
                                }
                                else if (haendlerGewaehlt)
                                {
                                    importRow[tmpCol.ColumnName] = haendlerRow["POST_CODE1"];
                                }
                                else
                                {
                                    importRow[tmpCol.ColumnName] = fzRow["POST_CODE1_ZF"];
                                }

                                break;
                            case "CITY1_ZF":
                                if (SelectedZahlungsart == "7")
                                {
                                    importRow[tmpCol.ColumnName] = m_strOrtHaendler;
                                }
                                else if (haendlerGewaehlt)
                                {
                                    importRow[tmpCol.ColumnName] = haendlerRow["CITY1"];
                                }
                                else
                                {
                                    importRow[tmpCol.ColumnName] = fzRow["CITY1_ZF"];
                                }
                               
                                break;
                            case "LAND_CODE_ZF":
                                if (SelectedZahlungsart == "7")
                                {
                                    importRow[tmpCol.ColumnName] = m_strLandHaendler;
                                }

                                break;
                            case "NAME1_BANK":
                                if (SelectedZahlungsart == "7" && m_AbwBriefEmpfaenger)
                                {
                                    importRow[tmpCol.ColumnName] = m_strNameBank;
                                }
                                else if (SelectedZahlungsart == "7" && m_AbwBriefEmpfaenger == false)
	                            {
                                    importRow[tmpCol.ColumnName] = m_strNameHaendler;
	                            }
                                else if (SelectedZahlungsart != "7")
                                {
                                    importRow[tmpCol.ColumnName] = bankRow["NAME1"];
                                }
                                
                                break;
                            case "NAME2_BANK":
                                if (SelectedZahlungsart == "7" && m_AbwBriefEmpfaenger)
                                {
                                    importRow[tmpCol.ColumnName] = m_strName2Bank;
                                }
                                else if (SelectedZahlungsart == "7" && m_AbwBriefEmpfaenger == false)
                                {
                                    importRow[tmpCol.ColumnName] = m_strName2Haendler;
                                }
                                else if (SelectedZahlungsart != "7")
                                {
                                    importRow[tmpCol.ColumnName] = bankRow["NAME2"];
                                }

                                break;
                            case "NAME3_BANK":
                                importRow[tmpCol.ColumnName] = "";

                                break;
                            case "STREET_BANK":
                                if (SelectedZahlungsart == "7" && m_AbwBriefEmpfaenger)
                                {
                                    importRow[tmpCol.ColumnName] = m_strStrasseBank;
                                }
                                else if (SelectedZahlungsart == "7" && m_AbwBriefEmpfaenger == false)
                                {
                                    importRow[tmpCol.ColumnName] = m_strStrasseHaendler;
                                }
                                else if (SelectedZahlungsart != "7")
                                {
                                    importRow[tmpCol.ColumnName] = bankRow["STREET"] + " " + bankRow["HOUSE_NUM1"];
                                }
                                
                                break;
                            case "POST_CODE1_BANK":
                                if (SelectedZahlungsart == "7" && m_AbwBriefEmpfaenger)
                                {
                                    importRow[tmpCol.ColumnName] = m_strPlzBank;
                                }
                                else if (SelectedZahlungsart == "7" && m_AbwBriefEmpfaenger == false)
                                {
                                    importRow[tmpCol.ColumnName] = m_strPlzHaendler;
                                }
                                else if (SelectedZahlungsart != "7")
                                {
                                    importRow[tmpCol.ColumnName] = bankRow["POST_CODE1"];
                                }
                                
                                break;
                            case "CITY1_BANK":
                                if (SelectedZahlungsart == "7" && m_AbwBriefEmpfaenger)
                                {
                                    importRow[tmpCol.ColumnName] = m_strOrtBank;
                                }
                                else if (SelectedZahlungsart == "7" && m_AbwBriefEmpfaenger == false)
                                {
                                    importRow[tmpCol.ColumnName] = m_strOrtHaendler;
                                }
                                else if (SelectedZahlungsart != "7")
                                {
                                    importRow[tmpCol.ColumnName] = bankRow["CITY1"];
                                }
                                
                                break;

                            case "LAND_CODE_BANK":
                                if (SelectedZahlungsart == "7" && m_AbwBriefEmpfaenger)
                                {
                                    importRow[tmpCol.ColumnName] = m_strLandBank;
                                }
                                else if (SelectedZahlungsart == "7" && m_AbwBriefEmpfaenger == false)
                                {
                                    importRow[tmpCol.ColumnName] = m_strLandHaendler;
                                }
                                
                                break;
                            case "KUNNR_BANK":
                                if (SelectedZahlungsart == "1" || SelectedZahlungsart == "5")
                                {
                                    importRow[tmpCol.ColumnName] = m_strSelectedBank.PadLeft(10,'0');
                                }

                                break;
                            default:
                                if (Fahrzeuge.Columns.Contains(tmpCol.ColumnName))
                                {
                                    importRow[tmpCol.ColumnName] = fzRow[tmpCol.ColumnName];
                                }

                                break;
                        }

                      
                    }
                    import.Rows.Add(importRow);
                }


                import.AcceptChanges();

                myProxy.callBapi();

                m_tblFehler.Clear();
                m_tblFehler = myProxy.getExportTable("GT_ERR_OUT");


                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblFehler);


                if (m_tblFehler.Rows.Count != 0)
                {
                    m_intStatus = -2222;
                    m_strMessage = "Bei Sicherung sind Fehler aufgetreten, siehe Tabelle";
                }

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {

                    case "EXCEPTION NO_DATA RAISED":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Fehler beim Abfragen der Händlerdaten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }

        }

        public void Show(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Versandauftraege.FILL";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_READ_FZG_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                DataTable SapTable = myProxy.getImportTable("GT_FIN_IN");
                DataRow rowUpload = null;
                foreach (DataRow rowUpload_loopVariable in m_tblUpload.Rows)
                {
                    rowUpload = rowUpload_loopVariable;
                    DataRow tmpSAPRow = SapTable.NewRow();

                    tmpSAPRow["CHASSIS_NUM"] = rowUpload["F1"].ToString().Trim().ToUpper();

                    SapTable.Rows.Add(tmpSAPRow);
                    SapTable.AcceptChanges();
                }


                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_OUT");
                m_tblFahrzeuge = tblTemp;

                m_tblFahrzeuge.Columns.Add("Selected", String.Empty.GetType());
                m_tblFahrzeuge.Columns.Add("Adresse", String.Empty.GetType());
                m_tblFahrzeuge.Columns.Add("AdresseBank", String.Empty.GetType());
                m_tblFahrzeuge.Columns.Add("AdresseToolTip", String.Empty.GetType());
                m_tblFahrzeuge.AcceptChanges();

                foreach (DataRow tmpRow in m_tblFahrzeuge.Rows)
                {

                    tmpRow["Adresse"] = tmpRow["NAME1_ZF"] + " " + tmpRow["NAME2_ZF"] + "<br>" + tmpRow["STREET_ZF"] + "<br>" + tmpRow["POST_CODE1_ZF"] + " " + tmpRow["CITY1_ZF"];
                    tmpRow["AdresseBank"] = tmpRow["NAME1_BANK"] + " " + tmpRow["NAME2_BANK"] + "<br>" + tmpRow["STREET_BANK"] + "<br>" + tmpRow["POST_CODE1_BANK"] + " " + tmpRow["CITY1_BANK"];
                    tmpRow["Selected"] = "";

                    if (tmpRow["NAME1_BANK"].ToString() != "")
                    {
                        tmpRow["AdresseToolTip"] = "Bankadresse: \r\n" + tmpRow["NAME1_BANK"] + " " + tmpRow["NAME2_BANK"] + "\r\n" + tmpRow["STREET_BANK"] + "\r\n" + tmpRow["POST_CODE1_BANK"] + " " + tmpRow["CITY1_BANK"];

                        if (tmpRow["NAME1_ZF"].ToString() != "")
                        {
                            tmpRow["AdresseToolTip"] += "\r\n\r\n";
                        }

                    }

                    if (tmpRow["NAME1_ZF"].ToString() != "")
                    {
                        tmpRow["AdresseToolTip"] += "Händleradresse: \r\n" + tmpRow["NAME1_ZF"] + " " + tmpRow["NAME2_ZF"] + "\r\n" + tmpRow["STREET_ZF"] + "\r\n" + tmpRow["POST_CODE1_ZF"] + " " + tmpRow["CITY1_ZF"];
                    }
                }

                m_tblFahrzeuge.AcceptChanges();
                m_tblFehler = myProxy.getExportTable("GT_ERR_OUT");
                DataRow newFahrzeugRow;


                for (int i = 0; i <= m_tblFehler.Rows.Count - 1; i++)
                {
                    if (m_tblFehler.Rows[i]["BEM"].ToString().Trim() == "Fahrgestellnummer nicht vorhanden!")
                    {
                        newFahrzeugRow = m_tblFahrzeuge.NewRow();
                        newFahrzeugRow["CHASSIS_NUM"] = m_tblFehler.Rows[i]["CHASSIS_NUM"];
                        m_tblFahrzeuge.Rows.Add(newFahrzeugRow);
                        m_tblFahrzeuge.AcceptChanges();

                        m_tblFehler.Rows.RemoveAt(i);
                        m_tblFehler.AcceptChanges();
                        i--;
                    }
                }




                if (((m_tblFahrzeuge.Rows.Count == 0)) && ((m_tblFehler.Rows.Count == 0)))
                {

                    m_intStatus = -9999;
                    m_strMessage = "keine Dokumente zur Anzeige gefunden.";
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

// ************************************************
// $History: Versandauftraege.cs $
//
//*****************  Version 19  *****************
//User: Fassbenders  Date: 18.05.11   Time: 13:15
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//
//*****************  Version 18  *****************
//User: Fassbenders  Date: 16.05.11   Time: 20:23
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//
//*****************  Version 17  *****************
//User: Rudolpho     Date: 8.12.10    Time: 10:48
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//
//*****************  Version 16  *****************
//User: Rudolpho     Date: 8.12.10    Time: 10:03
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//
//*****************  Version 15  *****************
//User: Rudolpho     Date: 6.12.10    Time: 14:14
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//
//*****************  Version 14  *****************
//User: Fassbenders  Date: 27.10.10   Time: 17:37
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//
//*****************  Version 13  *****************
//User: Fassbenders  Date: 21.10.10   Time: 15:30
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//
//*****************  Version 12  *****************
//User: Jungj        Date: 22.09.10   Time: 19:57
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4105 nachbesserungen
//
//*****************  Version 11  *****************
//User: Jungj        Date: 21.09.10   Time: 19:43
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4105 unfertig
//
//*****************  Version 10  *****************
//User: Jungj        Date: 20.09.10   Time: 19:48
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4105 unfertig
//
//*****************  Version 9  *****************
//User: Jungj        Date: 20.09.10   Time: 18:07
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4105 unfertig
//
//*****************  Version 8  *****************
//User: Jungj        Date: 19.09.10   Time: 22:04
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4105 unfertig
//
//*****************  Version 7  *****************
//User: Jungj        Date: 19.09.10   Time: 21:03
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4105 unfertig
//
//*****************  Version 6  *****************
//User: Jungj        Date: 19.09.10   Time: 18:11
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4105 unfertig
//
//*****************  Version 5  *****************
//User: Jungj        Date: 18.09.10   Time: 23:58
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4105 unfertig
//
//*****************  Version 4  *****************
//User: Jungj        Date: 18.09.10   Time: 21:36
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4105 unfertig
//
//*****************  Version 3  *****************
//User: Jungj        Date: 17.09.10   Time: 22:08
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4105 unfertig
//
//*****************  Version 2  *****************
//User: Jungj        Date: 17.09.10   Time: 16:54
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4105 unfertig
//
//*****************  Version 1  *****************
//User: Jungj        Date: 17.09.10   Time: 16:05
//Created in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4105 Torso
//
// ************************************************ 
