using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using CKG.Base.Business;
using CKG.Base.Common;
using CKG.Base.Kernel.Common;
using CKG.Base.Kernel.Security;


namespace Leasing.lib
{
    public class Lp02 : DatenimportBase
    {

        #region " Declarations"
        DataTable _history;
        String _auftragsnummer;
        DataTable _laenderPlz;

        #endregion

        #region "Properties"
        public String Equimpent { get; set; }

        public String Auftragsstatus { get; set; }

        public string Auftragsnummer { get; set; }
        public string BeauftragungKlartext { get; set; }

        public DataTable History
        {
            get { return _history; }
            set { _history = value; }
        }
        public DataTable ResultModelle { get; set; }

        public DataTable Fahrzeuge
        {
            get { return m_tblResult; }
            set { m_tblResult = value; }
        }

        public Int32 ResultCount { get; set; }

        public String SucheFahrgestellNr { get; set; }

        public String Fahrgestellnummer { get; set; }

        public String SucheKennzeichen { get; set; }

        public String SucheLeasingvertragsNr { get; set; }

        public String SucheNummerZb2 { get; set; }

        public String HalterName1 { get; set; }

        public String HalterName2 { get; set; }

        public String HalterOrt { get; set; }

        public String HalterPlz { get; set; }

        public String HalterStrasse { get; set; }

        public String HalterHausnr { get; set; }

        public String StandortName1 { get; set; }

        public String StandortName2 { get; set; }

        public String StandortOrt { get; set; }

        public String StandortPlz { get; set; }

        public String StandortStrasse { get; set; }

        public String StandortHausnr { get; set; }

        public String Kreis { get; set; }

        public String Wunschkennzeichen { get; set; }

        public String ReserviertAuf { get; set; }

        public String Versicherungstraeger { get; set; }

        public String EvbNr { get; set; }

        public String EmpfaengerName1 { get; set; }

        public String EmpfaengerName2 { get; set; }

        public String EmpfaengerOrt { get; set; }

        public String EmpfaengerPlz { get; set; }

        public String EmpfaengerStrasse { get; set; }

        public String EmpfaengerHausnr { get; set; }

        public String DurchfuehrungsDatum { get; set; }

        public String Bemerkung { get; set; }

        public String Auftragsgrund { get; set; }

        public string EvbNrSingle { get; set; }

        public DateTime? EvbGueltigVon { get; set; }

        public DateTime? EvbGueltigBis { get; set; }

        public DataTable LaenderPlz
        {
            get
            {
                if (_laenderPlz == null)
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

                    _laenderPlz = result;
                }

                return _laenderPlz;
            }
        }
        #endregion

        public Lp02(ref User objUser, App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
        { }

        public void FillHistory(String appId, String sessionId, String strAmtlKennzeichen, String strFahrgestellnummer, String strBriefnummer, String strOrdernummer, Page page)
        {
            m_strClassAndMethod = "LP_02.FillHistory";
            m_strAppID = appId;
            m_strSessionID = sessionId;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Fahrzeugbriefhistorie", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_ZZKENN", strAmtlKennzeichen.ToUpper());
                myProxy.setImportParameter("I_ZZFAHRG", strFahrgestellnummer.ToUpper());
                myProxy.setImportParameter("I_ZZREF1", strOrdernummer.ToUpper());


                myProxy.callBapi();

                _history = myProxy.getExportTable("GT_WEB");
                ResultCount = Convert.ToInt32(myProxy.getExportParameter("E_COUNTER"));
                if (ResultCount > 1)
                {
                    m_tblResult = myProxy.getExportTable("ET_FAHRG");
                    m_tblResult.Columns.Add("DISPLAY");
                    if (m_tblResult.Rows.Count > 0)
                    {
                        foreach (DataRow row in m_tblResult.Rows)
                        {
                            String strTemp = row["ZZFAHRG"] + ", " + row["LIZNR"] + ", " + row["ZZKENN"];
                            row["DISPLAY"] = strTemp;
                        }
                    }



                }

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR + ", ZZBRIEF=" + strBriefnummer.ToUpper() + ", ZFAHRG=" + strFahrgestellnummer.ToUpper() + ", ZZREF1=" + strOrdernummer.ToUpper() + ", ZZKENN=" + strAmtlKennzeichen.ToUpper() + ", Count=" + ResultCount, ref _history);

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
                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + ", ZZBRIEF=" + strBriefnummer.ToUpper() + ", ZFAHRG=" + strFahrgestellnummer.ToUpper() + ", ZZREF1=" + strOrdernummer.ToUpper() + ", ZZKENN=" + strAmtlKennzeichen.ToUpper() + ", Count=" + ResultCount, ref _history);

            }

        }
        public void GiveCars(String appId, String sessionId, Page page)
        {

            m_strClassAndMethod = "LP_02.GiveCars";
            m_strAppID = appId;
            m_strSessionID = sessionId;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_UNANGEFORDERT_LP", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_LICENSE_NUM", SucheKennzeichen);
                myProxy.setImportParameter("I_CHASSIS_NUM", SucheFahrgestellNr);
                myProxy.setImportParameter("I_LIZNR", SucheLeasingvertragsNr);
                myProxy.setImportParameter("I_TIDNR", SucheNummerZb2);


                myProxy.callBapi();

                m_tblResult = myProxy.getExportTable("GT_WEB");
                m_tblResult.Columns.Add("STATUS", typeof(String));
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

                CreateOutPut(m_tblResult, appId);

                WriteLogEntry(true, "FahrgestellNr=" + SucheFahrgestellNr + ", LVNr.=" + SucheLeasingvertragsNr + ", KfzKz.=" + SucheKennzeichen + ", KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

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
                WriteLogEntry(false, "FahrgestellNr=" + SucheFahrgestellNr + ", LVNr.=" + SucheLeasingvertragsNr + ", KfzKz.=" + SucheKennzeichen + ", KUNNR=" + m_objUser.KUNNR, ref m_tblResult);
            }

        }
        public void Anfordern(String appId, String sessionId, Page page)
        {
            m_strClassAndMethod = "LP_02.Anfordern";
            m_strAppID = appId;
            m_strSessionID = sessionId;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Dezdienstl_001", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_AUGRUND", Auftragsgrund.Split('-')[0].PadLeft(18, '0'));

                myProxy.setImportParameter("I_EQUNR", Equimpent);
                myProxy.setImportParameter("I_CHASSIS_NUM", Fahrgestellnummer);
                myProxy.setImportParameter("I_WUKENNZ", Kreis + "-" + Wunschkennzeichen);

                myProxy.setImportParameter("I_RES_AUF", ReserviertAuf);
                myProxy.setImportParameter("I_VERSTR", Versicherungstraeger);
                myProxy.setImportParameter("I_LIEFDAT", DurchfuehrungsDatum);

                myProxy.setImportParameter("I_BEMERKUNG", Bemerkung);
                myProxy.setImportParameter("I_USER",
                    m_objUser.UserName.Length > 12 ? m_objUser.UserName.Substring(0, 11) : m_objUser.UserName);

                if (!string.IsNullOrEmpty(m_objUser.Store))
                    myProxy.setImportParameter("I_INFO_ZUM_AG", m_objUser.Store);

                myProxy.setImportParameter("I_ZZVSNR", EvbNr);

                DataTable tblPartner = myProxy.getImportTable("T_PARTNERS");

                DataRow objPartner;
                if (HalterName1.Length + HalterName2.Length + HalterStrasse.Length + HalterPlz.Length + HalterOrt.Length > 0)
                {
                    objPartner = tblPartner.NewRow();
                    objPartner["Parvw"] = "ZH";
                    objPartner["Name1"] = HalterName1;
                    objPartner["Name2"] = HalterName2;
                    objPartner["Street"] = HalterStrasse;
                    objPartner["House_Num1"] = HalterHausnr;
                    objPartner["Post_Code1"] = HalterPlz;
                    objPartner["City1"] = HalterOrt;
                    objPartner["State"] = "DE";
                    tblPartner.Rows.Add(objPartner);
                }

                if (EmpfaengerName1.Length + EmpfaengerName2.Length + EmpfaengerStrasse.Length + EmpfaengerPlz.Length + EmpfaengerOrt.Length > 0)
                {
                    objPartner = tblPartner.NewRow();
                    objPartner["Parvw"] = "ZE";
                    objPartner["Name1"] = EmpfaengerName1;
                    objPartner["Name2"] = EmpfaengerName2;
                    objPartner["Street"] = EmpfaengerStrasse;
                    objPartner["House_Num1"] = EmpfaengerHausnr;
                    objPartner["Post_Code1"] = EmpfaengerPlz;
                    objPartner["City1"] = EmpfaengerOrt;
                    objPartner["State"] = "DE";
                    tblPartner.Rows.Add(objPartner);
                }

                myProxy.callBapi();
                _auftragsnummer = myProxy.getExportParameter("O_VBELN").TrimStart('0');
                Auftragsnummer = _auftragsnummer;
                Auftragsstatus = "Vorgang OK";

                if (_auftragsnummer.Length == 0)
                {
                    m_intStatus = -2100;
                    m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden.";
                    Auftragsstatus = "Keine Auftragsnummer erzeugt.";
                }

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        Auftragsstatus = "ZBII zum Kunden nicht vorhanden";
                        m_intStatus = -4411;
                        break;
                    case "ERR_HALTER":
                        m_intStatus = -2222;
                        m_strMessage = "Kein Halter vorhanden.";
                        break;
                    case "ERR_STANDORT":
                        Auftragsstatus = "Fehler bei Standortsuche";
                        m_intStatus = -4413;
                        break;
                    case "ERR_INV_KUNNR":
                        Auftragsstatus = "Unbekannte Kunnr!";
                        m_intStatus = -4415;
                        break;
                    case "ERR_NO_ZULDAT":
                        Auftragsstatus = "Kein Zulassungsdatum angegeben!";
                        m_intStatus = -4416;
                        break;
                    case "ERR_NO_TRANSNAME":
                        Auftragsstatus = "Kein Transaktionsname angegeben!";
                        m_intStatus = -4418;
                        break;
                    case "ERR_INV_FAHRG":
                        Auftragsstatus = "Ungültige Fahrgestellnummer!";
                        m_intStatus = -4419;
                        break;
                    case "ERR_INV_BRIEFNR":
                        Auftragsstatus = "Ungültige Briefnummer!";
                        m_intStatus = -4420;
                        break;
                    case "ERR_NO_LIF":
                        Auftragsstatus = "Kein Zulassungsdienst zu Zulassungsstelle gefunden!";
                        m_intStatus = -4421;
                        break;
                    case "ERR_INV_PARVW":
                        Auftragsstatus = "Ungültige Partnerrolle angegeben!";
                        m_intStatus = -4422;
                        break;
                    case "ERR_INV_ZH":
                        Auftragsstatus = "Ungültige Kundennummer für Halter!";
                        m_intStatus = -4423;
                        break;
                    case "ERR_INV_ZH_ABWADR":
                        Auftragsstatus = "Fehlende Information zu abw. Adresse für Halter!";
                        m_intStatus = -4424;
                        break;
                    case "ERR_INV_ZS":
                        Auftragsstatus = "Ungültige Kundennummer für Empfänger Brief!";
                        m_intStatus = -4425;
                        break;
                    case "ERR_INV_ZS_ABWADR":
                        Auftragsstatus = "Fehlende Information zu abw. Adresse für Empfänger Brief!";
                        m_intStatus = -4426;
                        break;
                    case "ERR_INV_ZE":
                        Auftragsstatus = "Ungültige Kundennummer für Empänger Schein & Schilder!";
                        m_intStatus = -4427;
                        break;
                    case "ERR_INV_ZE_ABWADR":
                        Auftragsstatus = "Fehlende Information zu abw. Adresse für Empfänger Schein & Schilder!";
                        m_intStatus = -4428;
                        break;
                    case "ERR_INV_ZA":
                        Auftragsstatus = "Ungültige Kundennummer für Standort!";
                        m_intStatus = -4429;
                        break;
                    case "ERR_INV_ZA_ABWADR":
                        Auftragsstatus = "Fehlende Information zu abw. Adresse für Standort!";
                        m_intStatus = -4430;
                        break;
                    case "ERR_SAVE":
                        Auftragsstatus = "Fehler beim Speichern!";
                        m_intStatus = -4431;
                        break;
                    case "ERR_INV_VERSART_STR1":
                        Auftragsstatus = "Fehlerhafte Versandart Strecke 1!";
                        m_intStatus = -4432;
                        break;
                    case "ERR_INV_VERSART_STR2":
                        Auftragsstatus = "Fehlerhafte Versandart Strecke 2!";
                        m_intStatus = -4433;
                        break;
                    case "ERR_INV_ZV":
                        Auftragsstatus = "Ungültige Kundennummer für Versicherer!";
                        m_intStatus = -4434;
                        break;
                    case "ERR_INV_ZC":
                        Auftragsstatus = "Ungültige Kundennummer für abw. Versicherungsnehmer!";
                        m_intStatus = -4435;
                        break;
                    case "ERR_INV_ZC_ABWADR":
                        Auftragsstatus = "Fehlende Information zu abw. Adresse für abw. Versicherungsnehmer!";
                        m_intStatus = -4436;
                        break;
                    case "ERR_NO_ZH":
                        Auftragsstatus = "Kein Halter angegeben!";
                        m_intStatus = -4437;
                        break;
                    case "ERR_NO_ZS":
                        Auftragsstatus = "Kein Empfänger Brief angegeben!";
                        m_intStatus = -4438;
                        break;
                    case "ERR_NO_ZE":
                        Auftragsstatus = "Kein Empfänger Schein & Schilder angegeben!";
                        m_intStatus = -4439;
                        break;
                    case "ERR_SONST":
                        Auftragsstatus = "Unbekannter Fehler";
                        m_intStatus = -4440;
                        break;
                    case "ERR_CS_MEL":
                        Auftragsstatus = "Fehler bei anlegen CS-Meldung";
                        m_intStatus = -4441;
                        break;
                    default:
                        Auftragsstatus = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        m_intStatus = -9999;
                        break;
                }

            }

        }


        // ReSharper disable once FunctionComplexityOverflow
        public void AnfordernCustom(String appId, String sessionId, Page page)
        {

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_WEB_SONST_DL_ANF_CKPT_01", ref m_objApp,
                    ref m_objUser, ref page);

                myProxy.setImportParameter("AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("WEB_USER", m_objUser.UserName);
                myProxy.setImportParameter("I_MATNR", Auftragsgrund.Split('-')[0].PadLeft(18, '0'));
                myProxy.setImportParameter("I_NEU", "X");

                DataTable auftragsdaten = myProxy.getImportTable("GT_AUF");

                var rows = Fahrzeuge.Select("MANDT = '99'");
                foreach (var row in rows)
                {
                    var newRow = auftragsdaten.NewRow();

                    newRow["ZZFAHRG"] = row["Fahrgestellnummer"];
                    newRow["EVBNR"] = EvbNrSingle;
                    newRow["INFO_ZUM_AG_1"] = m_objUser.Store;

                    if (EvbGueltigVon.HasValue)
                    {
                        newRow["EVBVONDAT"] = EvbGueltigVon;
                    }
                    if (EvbGueltigBis.HasValue)
                    {
                        newRow["EVBBISDAT"] = EvbGueltigBis;
                    }

                    if (!String.IsNullOrEmpty(DurchfuehrungsDatum))
                    {
                        newRow["ZULDAT"] = DurchfuehrungsDatum;
                    }

                    newRow["ZZBRIEF"] = row["NummerZB2"];
                    newRow["ZZREFNR"] = row["Leasingnummer"];
                    newRow["RES_PIN"] = ReserviertAuf;

                    newRow["WUNSCHKENNZ"] = Kreis + "-" + Wunschkennzeichen;
                    newRow["VERSICHERUNG"] = Versicherungstraeger;
                    newRow["EQUNR"] = row["EQUNR"].ToString();
                    newRow["SFV_FZG"] = Bemerkung;

                    auftragsdaten.Rows.Add(newRow);
                }


                DataTable partner = myProxy.getImportTable("GT_PARTNER");

                var partnerRow = partner.NewRow();
                partnerRow["PARTN_ROLE"] = "ZS";
                partnerRow["PARTN_NUMB"] = "1";
                partnerRow["NAME"] = "DAD Deutscher Auto Dienst GmbH";
                partnerRow["STREET"] = "Ladestraße 1";
                partnerRow["POSTL_CODE"] = "22926";
                partnerRow["CITY"] = "Ahrensburg";
                partnerRow["COUNTRY"] = "DE";
                partner.Rows.Add(partnerRow);



                if (HalterName1.Length + HalterName2.Length + HalterStrasse.Length +
                    HalterPlz.Length + HalterOrt.Length > 0)
                {
                    partnerRow = partner.NewRow();
                    partnerRow["PARTN_ROLE"] = "ZH";
                    partnerRow["NAME"] = HalterName1;
                    partnerRow["NAME_2"] = HalterName2;
                    partnerRow["STREET"] = HalterStrasse + SetSpaceInHouseNumber(HalterHausnr);
                    partnerRow["POSTL_CODE"] = HalterPlz;
                    partnerRow["CITY"] = HalterOrt;
                    partnerRow["COUNTRY"] = "DE";
                    partner.Rows.Add(partnerRow);
                }

                if (EmpfaengerName1.Length + EmpfaengerName2.Length + EmpfaengerStrasse.Length +
                    EmpfaengerPlz.Length + EmpfaengerOrt.Length > 0)
                {
                    partnerRow = partner.NewRow();
                    partnerRow["PARTN_ROLE"] = "ZE";
                    partnerRow["NAME"] = EmpfaengerName1;
                    partnerRow["NAME_2"] = EmpfaengerName2;
                    partnerRow["STREET"] = EmpfaengerStrasse + SetSpaceInHouseNumber(EmpfaengerHausnr);
                    partnerRow["POSTL_CODE"] = EmpfaengerPlz;
                    partnerRow["CITY"] = EmpfaengerOrt;
                    partnerRow["COUNTRY"] = "DE";
                    partner.Rows.Add(partnerRow);
                }

                myProxy.callBapi();

                DataTable returnTable = myProxy.getExportTable("GT_RETURN");

                foreach (DataRow dRow in returnTable.Rows)
                {
                    var fahrzeugeRow = Fahrzeuge.Select("Fahrgestellnummer='" + dRow["ZZFAHRG"] + "'")[0];

                    if (!string.IsNullOrEmpty(dRow["VBELN"].ToString()))
                    {
                        Auftragsnummer = dRow["VBELN"].ToString();
                        fahrzeugeRow["Status"] = "OK";
                    }
                    else
                    {
                        fahrzeugeRow["Status"] = dRow["MESSAGE"];
                    }
                    
                    
                    

                    
                }
            }
            catch (Exception ex)
            {
                Auftragsstatus = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                m_intStatus = -9999;
            }
            

        }


        static String SetSpaceInHouseNumber(String houseNumber)
        {
            if (!String.IsNullOrEmpty(houseNumber))
            {
                houseNumber = " " + houseNumber;
            }

            return houseNumber;
        }


        public DataTable GiveResultStructure(Page page)
        {
            var tblTemp = new DataTable();

            tblTemp.Columns.Add("EQUNR", typeof(String));
            tblTemp.Columns.Add("MANDT", typeof(String));
            tblTemp.Columns.Add("Fahrgestellnummer", typeof(String));
            tblTemp.Columns.Add("Leasingnummer", typeof(String));
            tblTemp.Columns.Add("NummerZB2", typeof(String));
            tblTemp.Columns.Add("Kennzeichen", typeof(String));
            tblTemp.Columns.Add("Ordernummer", typeof(String));
            tblTemp.Columns.Add("Abmeldedatum", typeof(String));
            tblTemp.Columns.Add("CoC", typeof(String));
            tblTemp.Columns.Add("STATUS", typeof(String));
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
  
        public bool IsAlphaNumeric(String str)
        {
            Regex regexAlphaNum = new Regex("[^a-zA-Z0-9]");
            return !regexAlphaNum.IsMatch(str);
        }

        public string KreisSuche(String appId, String sessionId, Page page, String plz)
        {
            string kreiskennzeichen = "";

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_GET_ZULST_BY_PLZ", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_PLZ", plz);
                myProxy.setImportParameter("I_ORT", "");

                myProxy.callBapi();

                var kreise = myProxy.getExportTable("T_ZULST");

                if (kreise.Rows.Count > 0)
                {
                    kreiskennzeichen = kreise.Rows[0]["ZKFZKZ"].ToString();
                }


            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "ERR_INV_PLZ":
                        m_intStatus = -1118;
                        m_strMessage = "Ungültige Postleitzahl.";
                        break;
                    default:
                        m_intStatus = -9999;
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }
                
            }


            return kreiskennzeichen;

        }


    }
}