using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Kernel;
using CKG.Base.Business;
using CKG.Base.Common;
using CKG.Base;
using System.Data;
using System.Text.RegularExpressions;
using CKG.Base.Kernel.Common;

namespace Leasing.lib
{
    public class LP_02 : CKG.Base.Business.DatenimportBase
    {

        #region " Declarations"
        String m_strBriefnummer;
        DateTime m_datEingangsdatumVon;
        DateTime m_datEingangsdatumBis;
        String m_strFahrgestellnummer;
        String m_strHaendlerID;
        DataTable m_tblHistory;
        DataTable m_tableGrund;
        DataTable m_tblResultModelle;
        Int32 m_intResultCount;
        String m_strExpress;
        String m_auftragsgrund;
        String m_strBeauftragungKlartext;
        String m_strWunschkennzeichen;
        String m_equ;
        String m_Kreis;
        String m_ReserviertAuf;
        String m_Versicherungstraeger;
        String m_DurchfuehrungsDatum;
        String m_Bemerkung;
        String evbNummer;
        String m_strHalterName1;
        String m_strHalterName2;
        String m_strHalterOrt;
        String m_strHalterPLZ;
        String m_strHalterStrasse;
        String m_strHalterHausnr;
        String m_strStandortName1;
        String m_strStandortName2;
        String m_strStandortOrt;
        String m_strStandortPLZ;
        String m_strStandortStrasse;
        String m_strStandortHausnr;
        String m_strEmpfaengerName1;
        String m_strEmpfaengerName2;
        String m_strEmpfaengerOrt;
        String m_strEmpfaengerPLZ;
        String m_strEmpfaengerStrasse;
        String m_strEmpfaengerHausnr;
        String strAuftragsstatus;
        String strAuftragsnummer;
        String m_strSucheFahrgestellNr;
        String m_strSucheKennzeichen;
        String m_strSucheLeasingvertragsNr;
        String m_strSucheNummerZB2;
        DataTable m_laenderPlz;

        #endregion

        #region "Properties"
        public String Equimpent
        {
            get { return m_equ; }
            set { m_equ = value; }
        }

        public String Auftragsstatus
        {
            get { return strAuftragsstatus; }
            set { strAuftragsstatus = value; }
        }
        public String Auftragsnummer { get; set; }
        public String BeauftragungKlartext
        {
            get { return m_strBeauftragungKlartext; }
            set { m_strBeauftragungKlartext = value; }
        }
        public DataTable History
        {
            get { return m_tblHistory; }
            set { m_tblHistory = value; }
        }
        public DataTable ResultModelle
        {
            get { return m_tblResultModelle; }
            set { m_tblResultModelle = value; }
        }

        public DataTable Fahrzeuge
        {
            get { return m_tblResult; }
            set { m_tblResult = value; }
        }

        public Int32 ResultCount
        {
            get { return m_intResultCount; }
            set { m_intResultCount = value; }
        }
        public String SucheFahrgestellNr
        {
            get { return m_strSucheFahrgestellNr; }
            set { m_strSucheFahrgestellNr = value; }
        }
        public String Fahrgestellnummer
        {
            get { return m_strFahrgestellnummer; }
            set { m_strFahrgestellnummer = value; }
        }
        public String SucheKennzeichen
        {
            get { return m_strSucheKennzeichen; }
            set { m_strSucheKennzeichen = value; }
        }

        public String SucheLeasingvertragsNr
        {
            get { return m_strSucheLeasingvertragsNr; }
            set { m_strSucheLeasingvertragsNr = value; }
        }
        public String SucheNummerZB2
        {
            get { return m_strSucheNummerZB2; }
            set { m_strSucheNummerZB2 = value; }
        }

        public String HalterName1
        {
            get { return m_strHalterName1; }
            set { m_strHalterName1 = value; }
        }
        public String HalterName2
        {
            get { return m_strHalterName2; }
            set { m_strHalterName2 = value; }
        }

        public String HalterOrt
        {
            get { return m_strHalterOrt; }
            set { m_strHalterOrt = value; }
        }

        public String HalterPLZ
        {
            get { return m_strHalterPLZ; }
            set { m_strHalterPLZ = value; }
        }

        public String HalterStrasse
        {
            get { return m_strHalterStrasse; }
            set { m_strHalterStrasse = value; }
        }

        public String HalterHausnr
        {
            get { return m_strHalterHausnr; }
            set { m_strHalterHausnr = value; }
        }
        public String StandortName1
        {
            get { return m_strStandortName1; }
            set { m_strStandortName1 = value; }
        }
        public String StandortName2
        {
            get { return m_strStandortName2; }
            set { m_strStandortName2 = value; }
        }
        public String StandortOrt
        {
            get { return m_strStandortOrt; }
            set { m_strStandortOrt = value; }
        }
        public String StandortPLZ
        {
            get { return m_strStandortPLZ; }
            set { m_strStandortPLZ = value; }
        }
        public String StandortStrasse
        {
            get { return m_strStandortStrasse; }
            set { m_strStandortStrasse = value; }
        }
        public String StandortHausnr
        {
            get { return m_strStandortHausnr; }
            set { m_strStandortHausnr = value; }
        }
        public String Kreis
        {
            get { return m_Kreis; }
            set { m_Kreis = value; }
        }

        public String Wunschkennzeichen
        {
            get { return m_strWunschkennzeichen; }
            set { m_strWunschkennzeichen = value; }
        }
        public String ReserviertAuf
        {
            get { return m_ReserviertAuf; }
            set { m_ReserviertAuf = value; }
        }
        public String Versicherungstraeger
        {
            get { return m_Versicherungstraeger; }
            set { m_Versicherungstraeger = value; }
        }
        public String EVBNr
        {
            get { return evbNummer; }
            set { evbNummer = value; }
        }

        public String EmpfaengerName1
        {
            get { return m_strEmpfaengerName1; }
            set { m_strEmpfaengerName1 = value; }
        }
        public String EmpfaengerName2
        {
            get { return m_strEmpfaengerName2; }
            set { m_strEmpfaengerName2 = value; }
        }
        public String EmpfaengerOrt
        {
            get { return m_strEmpfaengerOrt; }
            set { m_strEmpfaengerOrt = value; }
        }
        public String EmpfaengerPLZ
        {
            get { return m_strEmpfaengerPLZ; }
            set { m_strEmpfaengerPLZ = value; }
        }
        public String EmpfaengerStrasse
        {
            get { return m_strEmpfaengerStrasse; }
            set { m_strEmpfaengerStrasse = value; }
        }
        public String EmpfaengerHausnr
        {
            get { return m_strEmpfaengerHausnr; }
            set { m_strEmpfaengerHausnr = value; }
        }
        public String DurchfuehrungsDatum
        {
            get { return m_DurchfuehrungsDatum; }
            set { m_DurchfuehrungsDatum = value; }
        }

        public String Bemerkung
        {
            get { return m_Bemerkung; }
            set { m_Bemerkung = value; }
        }
        public String Auftragsgrund
        {
            get { return m_auftragsgrund; }
            set { m_auftragsgrund = value; }
        }

        public string EvbSingle { get; set; }

        public string EvbVon { get; set; }

        public string EvbBis { get; set; }

        public DataTable LaenderPLZ
        {
            get
            {
                if (m_laenderPlz == null)
                {
                    var currentPage = PageHelper.GetCurrentPage();
                    var proxy = DynSapProxy.getProxy("Z_M_Land_Plz_001", ref m_objApp, ref m_objUser, ref currentPage);

                    proxy.callBapi();

                    var result = proxy.getExportTable("GT_WEB");
                    result.Columns.Add("Beschreibung", typeof(string));
                    result.Columns.Add("FullDesc", typeof(string));

                    foreach (DataRow row in result.Rows)
                    {
                        int lnPlz;
                        if (!int.TryParse((string)row["LNPLZ"], out lnPlz)) lnPlz = 0;
                        row["Beschreibung"] = lnPlz > 0 ? string.Format("{0} ({1})", row["Landx"], lnPlz) : (string)row["Landx"];
                        row["FullDesc"] = string.Format("{0} {1}", row["Land1"], row["Beschreibung"]);
                    }
                    result.AcceptChanges();

                    m_laenderPlz = result;
                }

                return m_laenderPlz;
            }
        }
        #endregion

        public LP_02(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
        { }

        public void FillHistory(String strAppID, String strSessionID, String strAmtlKennzeichen, String strFahrgestellnummer, String strBriefnummer, String strOrdernummer, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "LP_02.FillHistory";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            DataTable tblFahrzeugeSap = new DataTable();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Fahrzeugbriefhistorie", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_ZZKENN", strAmtlKennzeichen.ToUpper());
                myProxy.setImportParameter("I_ZZFAHRG", strFahrgestellnummer.ToUpper());
                myProxy.setImportParameter("I_ZZREF1", strOrdernummer.ToUpper());


                myProxy.callBapi();

                m_tblHistory = myProxy.getExportTable("GT_WEB");
                m_intResultCount = Convert.ToInt32(myProxy.getExportParameter("E_COUNTER"));
                if (m_intResultCount > 1)
                {
                    m_tblResult = myProxy.getExportTable("ET_FAHRG");
                    m_tblResult.Columns.Add("DISPLAY");
                    if (m_tblResult.Rows.Count > 0)
                    {
                        foreach (DataRow row in m_tblResult.Rows)
                        {
                            String strTemp = row["ZZFAHRG"].ToString() + ", " + row["LIZNR"].ToString() + ", " + row["ZZKENN"].ToString();
                            row["DISPLAY"] = strTemp;
                        }
                    }



                }

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR + ", ZZBRIEF=" + strBriefnummer.ToUpper() + ", ZFAHRG=" + strFahrgestellnummer.ToUpper() + ", ZZREF1=" + strOrdernummer.ToUpper() + ", ZZKENN=" + strAmtlKennzeichen.ToUpper() + ", Count=" + m_intResultCount.ToString(), ref m_tblHistory, false);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Informationen gefunden.";
                        break;
                    case "NO_WEB":
                        m_intStatus = -2222;
                        m_strMessage = "Keine Web-Tabelle erstellt.";
                        break;
                    default:
                        m_intStatus = -9999;
                        break;
                }
                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + ", ZZBRIEF=" + strBriefnummer.ToUpper() + ", ZFAHRG=" + strFahrgestellnummer.ToUpper() + ", ZZREF1=" + strOrdernummer.ToUpper() + ", ZZKENN=" + strAmtlKennzeichen.ToUpper() + ", Count=" + m_intResultCount.ToString(), ref m_tblHistory, false);

            }

        }
        public void GiveCars(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            DataTable tableGrund = new DataTable();
            DataTable tableFahrzeuge = new DataTable();

            m_strClassAndMethod = "LP_02.GiveCars";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            DataTable tblFahrzeugeSap = new DataTable();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_UNANGEFORDERT_LP", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_LICENSE_NUM", m_strSucheKennzeichen);
                myProxy.setImportParameter("I_CHASSIS_NUM", m_strSucheFahrgestellNr);
                myProxy.setImportParameter("I_LIZNR", m_strSucheLeasingvertragsNr);
                myProxy.setImportParameter("I_TIDNR", m_strSucheNummerZB2);


                myProxy.callBapi();

                m_tableGrund = myProxy.getExportTable("GT_GRU");
                m_tblResult = myProxy.getExportTable("GT_WEB");
                m_tblResult.Columns.Add("STATUS", System.Type.GetType("System.String"));
                m_intStatus = 0;

                foreach (DataRow row in m_tblResult.Rows)
                { row["STATUS"] = String.Empty; }

                if (m_tblResult == null)
                {
                    m_intStatus = -3331;
                    m_strMessage = "Keine Informationen gefunden.";
                }
                else if (m_tblResult.Rows.Count == 0)
                {
                    m_intStatus = -3331;
                    m_strMessage = "Keine Informationen gefunden.";
                }

                CreateOutPut(m_tblResult, strAppID);

                WriteLogEntry(true, "FahrgestellNr=" + m_strSucheFahrgestellNr + ", LVNr.=" + m_strSucheLeasingvertragsNr + ", KfzKz.=" + m_strSucheKennzeichen + ", KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Informationen gefunden.";
                        break;
                    case "NO_HAENDLER":
                        m_intStatus = -2222;
                        m_strMessage = "Keine Informationen gefunden.";
                        break;
                    default:
                        m_intStatus = -9999;
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }
                WriteLogEntry(false, "FahrgestellNr=" + m_strSucheFahrgestellNr + ", LVNr.=" + m_strSucheLeasingvertragsNr + ", KfzKz.=" + m_strSucheKennzeichen + ", KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
            }

        }
        public void Anfordern(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "LP_02.Anfordern";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            DataTable tblFahrzeugeSap = new DataTable();

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Dezdienstl_001", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_AUGRUND", m_auftragsgrund.Split('-')[0].PadLeft(18, '0'));
                myProxy.setImportParameter("I_EXPRESS", m_strExpress);

                myProxy.setImportParameter("I_EQUNR", m_equ);
                myProxy.setImportParameter("I_CHASSIS_NUM", m_strFahrgestellnummer);
                myProxy.setImportParameter("I_WUKENNZ", m_Kreis + "-" + m_strWunschkennzeichen);

                myProxy.setImportParameter("I_RES_AUF", m_ReserviertAuf);
                myProxy.setImportParameter("I_VERSTR", m_Versicherungstraeger);
                myProxy.setImportParameter("I_LIEFDAT", m_DurchfuehrungsDatum);

                myProxy.setImportParameter("I_BEMERKUNG", m_Bemerkung);
                if (m_objUser.UserName.Length > 12)
                { myProxy.setImportParameter("I_USER", m_objUser.UserName.Substring(0, 11)); }
                else
                { myProxy.setImportParameter("I_USER", m_objUser.UserName); }

                if (!string.IsNullOrEmpty(m_objUser.Store))
                    myProxy.setImportParameter("I_INFO_ZUM_AG", m_objUser.Store);

                myProxy.setImportParameter("I_ZZVSNR", evbNummer);

                DataTable tblPartner = myProxy.getImportTable("T_PARTNERS");

                DataRow objPartner;
                if (m_strHalterName1.Length + m_strHalterName2.Length + m_strHalterStrasse.Length + m_strHalterPLZ.Length + m_strHalterOrt.Length > 0)
                {
                    objPartner = tblPartner.NewRow();
                    objPartner["Parvw"] = "ZH";
                    objPartner["Name1"] = m_strHalterName1;
                    objPartner["Name2"] = m_strHalterName2;
                    objPartner["Street"] = m_strHalterStrasse;
                    objPartner["House_Num1"] = m_strHalterHausnr;
                    objPartner["Post_Code1"] = m_strHalterPLZ;
                    objPartner["City1"] = m_strHalterOrt;
                    objPartner["State"] = "DE";
                    tblPartner.Rows.Add(objPartner);
                }

                if (m_strEmpfaengerName1.Length + m_strEmpfaengerName2.Length + m_strEmpfaengerStrasse.Length + m_strEmpfaengerPLZ.Length + m_strEmpfaengerOrt.Length > 0)
                {
                    objPartner = tblPartner.NewRow();
                    objPartner["Parvw"] = "ZE";
                    objPartner["Name1"] = m_strEmpfaengerName1;
                    objPartner["Name2"] = m_strEmpfaengerName2;
                    objPartner["Street"] = m_strEmpfaengerStrasse;
                    objPartner["House_Num1"] = m_strEmpfaengerHausnr;
                    objPartner["Post_Code1"] = m_strEmpfaengerPLZ;
                    objPartner["City1"] = m_strEmpfaengerOrt;
                    objPartner["State"] = "DE";
                    tblPartner.Rows.Add(objPartner);
                }

                myProxy.callBapi();
                strAuftragsnummer = myProxy.getExportParameter("O_VBELN").TrimStart('0');
                Auftragsnummer = strAuftragsnummer;
                strAuftragsstatus = "Vorgang OK";

                if (strAuftragsnummer.Length == 0)
                {
                    m_intStatus = -2100;
                    m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden.";
                    strAuftragsstatus = "Keine Auftragsnummer erzeugt.";
                }

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        strAuftragsstatus = "ZBII zum Kunden nicht vorhanden";
                        m_intStatus = -4411;
                        break;
                    case "ERR_HALTER":
                        m_intStatus = -2222;
                        m_strMessage = "Kein Halter vorhanden.";
                        break;
                    case "ERR_STANDORT":
                        strAuftragsstatus = "Fehler bei Standortsuche";
                        m_intStatus = -4413;
                        break;
                    case "ERR_INV_KUNNR":
                        strAuftragsstatus = "Unbekannte Kunnr!";
                        m_intStatus = -4415;
                        break;
                    case "ERR_NO_ZULDAT":
                        strAuftragsstatus = "Kein Zulassungsdatum angegeben!";
                        m_intStatus = -4416;
                        break;
                    case "ERR_NO_TRANSNAME":
                        strAuftragsstatus = "Kein Transaktionsname angegeben!";
                        m_intStatus = -4418;
                        break;
                    case "ERR_INV_FAHRG":
                        strAuftragsstatus = "Ungültige Fahrgestellnummer!";
                        m_intStatus = -4419;
                        break;
                    case "ERR_INV_BRIEFNR":
                        strAuftragsstatus = "Ungültige Briefnummer!";
                        m_intStatus = -4420;
                        break;
                    case "ERR_NO_LIF":
                        strAuftragsstatus = "Kein Zulassungsdienst zu Zulassungsstelle gefunden!";
                        m_intStatus = -4421;
                        break;
                    case "ERR_INV_PARVW":
                        strAuftragsstatus = "Ungültige Partnerrolle angegeben!";
                        m_intStatus = -4422;
                        break;
                    case "ERR_INV_ZH":
                        strAuftragsstatus = "Ungültige Kundennummer für Halter!";
                        m_intStatus = -4423;
                        break;
                    case "ERR_INV_ZH_ABWADR":
                        strAuftragsstatus = "Fehlende Information zu abw. Adresse für Halter!";
                        m_intStatus = -4424;
                        break;
                    case "ERR_INV_ZS":
                        strAuftragsstatus = "Ungültige Kundennummer für Empfänger Brief!";
                        m_intStatus = -4425;
                        break;
                    case "ERR_INV_ZS_ABWADR":
                        strAuftragsstatus = "Fehlende Information zu abw. Adresse für Empfänger Brief!";
                        m_intStatus = -4426;
                        break;
                    case "ERR_INV_ZE":
                        strAuftragsstatus = "Ungültige Kundennummer für Empänger Schein & Schilder!";
                        m_intStatus = -4427;
                        break;
                    case "ERR_INV_ZE_ABWADR":
                        strAuftragsstatus = "Fehlende Information zu abw. Adresse für Empfänger Schein & Schilder!";
                        m_intStatus = -4428;
                        break;
                    case "ERR_INV_ZA":
                        strAuftragsstatus = "Ungültige Kundennummer für Standort!";
                        m_intStatus = -4429;
                        break;
                    case "ERR_INV_ZA_ABWADR":
                        strAuftragsstatus = "Fehlende Information zu abw. Adresse für Standort!";
                        m_intStatus = -4430;
                        break;
                    case "ERR_SAVE":
                        strAuftragsstatus = "Fehler beim Speichern!";
                        m_intStatus = -4431;
                        break;
                    case "ERR_INV_VERSART_STR1":
                        strAuftragsstatus = "Fehlerhafte Versandart Strecke 1!";
                        m_intStatus = -4432;
                        break;
                    case "ERR_INV_VERSART_STR2":
                        strAuftragsstatus = "Fehlerhafte Versandart Strecke 2!";
                        m_intStatus = -4433;
                        break;
                    case "ERR_INV_ZV":
                        strAuftragsstatus = "Ungültige Kundennummer für Versicherer!";
                        m_intStatus = -4434;
                        break;
                    case "ERR_INV_ZC":
                        strAuftragsstatus = "Ungültige Kundennummer für abw. Versicherungsnehmer!";
                        m_intStatus = -4435;
                        break;
                    case "ERR_INV_ZC_ABWADR":
                        strAuftragsstatus = "Fehlende Information zu abw. Adresse für abw. Versicherungsnehmer!";
                        m_intStatus = -4436;
                        break;
                    case "ERR_NO_ZH":
                        strAuftragsstatus = "Kein Halter angegeben!";
                        m_intStatus = -4437;
                        break;
                    case "ERR_NO_ZS":
                        strAuftragsstatus = "Kein Empfänger Brief angegeben!";
                        m_intStatus = -4438;
                        break;
                    case "ERR_NO_ZE":
                        strAuftragsstatus = "Kein Empfänger Schein & Schilder angegeben!";
                        m_intStatus = -4439;
                        break;
                    case "ERR_SONST":
                        strAuftragsstatus = "Unbekannter Fehler";
                        m_intStatus = -4440;
                        break;
                    case "ERR_CS_MEL":
                        strAuftragsstatus = "Fehler bei anlegen CS-Meldung";
                        m_intStatus = -4441;
                        break;
                    default:
                        strAuftragsstatus = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        m_intStatus = -9999;
                        break;
                }

            }

        }


        public void AnfordernCustom(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_WEB_SONST_DL_ANF_CKPT_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("WEB_USER", m_objUser.UserName);
                myProxy.setImportParameter("I_MATNR", m_auftragsgrund.Split('-')[0].PadLeft(18, '0'));
                myProxy.setImportParameter("I_NEU", "X");

                DataTable auftragsdaten = myProxy.getImportTable("GT_AUF");

                var rows = Fahrzeuge.Select("MANDT = '99'");
                foreach (var row in rows)
                {
                    var newRow = auftragsdaten.NewRow();

                    newRow["ZZFAHRG"] = row["Fahrgestellnummer"];
                    newRow["EVBNR"] = EVBNr;
                    newRow["WUNSCHKENNZ"] = Wunschkennzeichen;
                    newRow["VERSICHERUNG"] = Versicherungstraeger;
                    newRow["EQUNR"] = row["EQUNR"].ToString();
                    newRow["SFV_FZG"] = Bemerkung;
                }
            }
            catch (Exception)
            {
                    
                throw;
            }

        }



        public DataTable GiveResultStructure(System.Web.UI.Page page)
        {
            DataTable tblTemp = new DataTable();

            tblTemp.Columns.Add("EQUNR", typeof(System.String));
            tblTemp.Columns.Add("MANDT", typeof(System.String));
            tblTemp.Columns.Add("Fahrgestellnummer", typeof(System.String));
            tblTemp.Columns.Add("Leasingnummer", typeof(System.String));
            tblTemp.Columns.Add("NummerZB2", typeof(System.String));
            tblTemp.Columns.Add("Kennzeichen", typeof(System.String));
            tblTemp.Columns.Add("Ordernummer", typeof(System.String));
            tblTemp.Columns.Add("Abmeldedatum", typeof(System.String));
            tblTemp.Columns.Add("CoC", typeof(System.String));
            tblTemp.Columns.Add("STATUS", typeof(System.String));
            return tblTemp;

        }

        public DateTime SuggestionDay()
        {
            DateTime datTemp = DateTime.Now;
            Int32 intAddDays = 0;
            do
            {
                datTemp = datTemp.AddDays(1);
                if (datTemp.DayOfWeek == DayOfWeek.Saturday || datTemp.DayOfWeek == DayOfWeek.Sunday)
                {
                    intAddDays += 1;
                }

            } while (datTemp.DayOfWeek == DayOfWeek.Saturday || datTemp.DayOfWeek == DayOfWeek.Sunday || intAddDays < 3);

            return datTemp;

        }
        public bool IsDate(string strDate)
        {
            if (strDate == null)
            {
                strDate = "";
            }
            if (strDate.Length > 0)
            {
                DateTime dummyDate;
                try
                {
                    dummyDate = DateTime.Parse(strDate);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsAlphaNumeric(String str)
        {
            Regex regexAlphaNum = new Regex("[^a-zA-Z0-9]");
            return !regexAlphaNum.IsMatch(str);
        }
    }
}