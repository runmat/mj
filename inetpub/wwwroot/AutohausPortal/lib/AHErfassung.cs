using System;
using System.Collections.Generic;
using System.Web.UI;
using CKG.Base.Common;
using System.Data;
using CKG.Base.Business;
using GeneralTools.Models;

namespace AutohausPortal.lib
{   
    /// <summary>
    /// Klasse die verschiedenste SAP- bzw. Datenbankzugriffe herstellt. 
    /// Sammel, sichern, Löschen und editieren von Eingabedaten in SQL oder SAP.
    /// </summary>
    public class AHErfassung : DatenimportBase
    {
        #region Properties

        public DataTable tblEingabeListe { get; set; }
        public DataTable tblFehler { get; set; }
        public DataTable tblPrint { get; set; }
        public DataTable tblPrintKundenformulare { get; set; }

        public string VKORG { get; set; }
        public string VKBUR { get; set; }

        public long id_sap { get; set; }
        public bool IsNewVorgang { get { return (id_sap == 0); } }

        public string Kundenname { get; set; }
        public string Kunnr { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Ref3 { get; set; }
        public string Ref4 { get; set; }
        public string KreisKennz { get; set; }
        public string Kreis { get; set; }
        public bool WunschKenn { get; set; }
        public bool Reserviert { get; set; }
        public string ReserviertKennz { get; set; }
        public bool Feinstaub { get; set; }
        public string ZulDate { get; set; }
        public string Kennzeichen { get; set; }
        public string WunschKZ2 { get; set; }
        public string WunschKZ3 { get; set; }
        public string Kennztyp { get; set; }
        public string KennzForm { get; set; }
        public bool EinKennz { get; set; }
        public string EVB { get; set; }
        public string Fahrzeugart { get; set; }

        public string Vorgang { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string PLZ { get; set; }
        public string Ort { get; set; }
        public string Strasse { get; set; }
        public string SWIFT { get; set; }
        public string IBAN { get; set; }
        public string Bankkey { get; set; }
        public string Kontonr { get; set; }
        public string Inhaber { get; set; }
        public string Geldinstitut { get; set; }
        public bool EinzugErm { get; set; }
        public bool Rechnung { get; set; }
        public bool Barzahlung { get; set; }
        public DataTable BestLieferanten { get; set; }
        public string Altkenn { get; set; }

        public String IDCount { get; set; }

        public bool ZBII_ALT_NEU { get; set; }
        public bool VorhKennzReserv { get; set; }
        public string Lieferant_ZLD { get; set; }
        public string FrachtNrHin { get; set; }
        public string FrachtNrBack { get; set; }
        public string Vorfuehr { get; set; }

        public string Name1Hin { get; set; }
        public string Name2Hin { get; set; }
        public string StrasseHin { get; set; }
        public string PLZHin { get; set; }
        public string OrtHin { get; set; }
        public string DocRueck1 { get; set; }
        public string NameRueck1 { get; set; }
        public string NameRueck2 { get; set; }
        public string StrasseRueck { get; set; }
        public string PLZRueck { get; set; }
        public string OrtRueck { get; set; }
        public string Doc2Rueck { get; set; }
        public string Name1Rueck2 { get; set; }
        public string Name2Rueck2 { get; set; }
        public string Strasse2Rueck { get; set; }
        public string PLZ2Rueck { get; set; }
        public string Ort2Rueck {get;set;}

        public string Bemerkung { get; set; }
        public string Notiz { get; set; }
        public string InternRef { get; set; }
        public string VkKurz { get; set; }

        public string StillDate { get; set; }
        public string TuvAu { get; set; }
        public string ZollVers { get; set; }
        public string ZollVersDauer { get; set; }

        public bool KennzUebernahme { get; set; }
        public bool Serie { get; set; }
        public bool Saison { get; set; }
        public bool ZusatzKZ { get; set; }
        public bool MussReserviert { get; set; }
        public bool KennzVorhanden { get; set; }
        public string KurzZeitKennz { get; set; }
        public string NrLangText { get; set; }
        public string LangText { get; set; }

        public string SaisonBeg { get; set; }
        public string SaisonEnd { get; set; }

        public string NrMaterial { get; set; }
        public string Material { get; set; }

        public string Haltedauer { get; set; }

        public DataTable Kundenadresse { get; set; }

        public DataTable Abmeldedaten { get; set; }

        public bool OhneGruenenVersSchein { get; set; }

        private bool SendForAll { get { return m_objUser.Organization.OrganizationName.ToUpper().Contains(("SENDFORALL")); } }

        public byte[] KundenformularPDF { get; set; }

        #endregion
        
        #region Constructor

        public AHErfassung(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string sVorgang, string sMaterialNr)
            : base(ref objUser, objApp, "")
        {
            Vorgang = sVorgang;
            NrMaterial = sMaterialNr;
            Material = (ZLDCommon.Materialliste.ContainsKey(NrMaterial) ? ZLDCommon.Materialliste[NrMaterial] : "");
        } 

        #endregion

        #region Methods

        private void Init(string methodName, string appId, string sessionId)
        {
            m_strClassAndMethod = String.Format("AHErfassung.{0}", methodName);
            m_strAppID = appId;
            m_strSessionID = sessionId;
            m_intStatus = 0;
            m_strMessage = String.Empty;
        }

        public void GiveSapID(String strAppID, String strSessionID, Page page)
        {
            Init("GiveSapID", strAppID, strSessionID);

            if (!m_blnGestartet)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_BELNR", ref m_objApp, ref m_objUser, ref page);

                    myProxy.callBapi();

                    id_sap = myProxy.getExportParameter("E_BELN").ToLong(0);
                    m_intStatus = myProxy.getExportParameter("E_SUBRC").ToInt(0);
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            m_intStatus = -5555;
                            m_strMessage = "Keine Daten gefunden(Kreiskennzeichen).";
                            break;
                        default:
                            m_intStatus = -9999;
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        public void LoadVorgaengeFromSap(String strAppID, String strSessionID, Page page, DataTable tblKundenStamm)
        {
            Init("LoadVorgaengeFromSap", strAppID, strSessionID);

            if (!m_blnGestartet)
            {
                m_blnGestartet = true;

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_EXPORT_WARENKORB", ref m_objApp, ref m_objUser, ref page);

                    if (SendForAll)
                    {
                        myProxy.setImportParameter("I_VKBUR", m_objUser.Reference.Substring(4, 4));
                        myProxy.setImportParameter("I_WEBGOUP_ID", m_objUser.GroupID.ToString());
                    }
                    else
                    {
                        myProxy.setImportParameter("I_WEBUSER_ID", m_objUser.UserID.ToString());
                    }

                    myProxy.setImportParameter("I_AUFRUF", "1");

                    myProxy.callBapi();

                    tblEingabeListe = myProxy.getExportTable("GT_BAK");
                    IDCount = tblEingabeListe.Rows.Count.ToString();
                    var tblPos = myProxy.getExportTable("GT_POS");
                    var tblAdrs = myProxy.getExportTable("GT_ADRS");

                    tblEingabeListe.Columns.Add("Status", typeof(String));
                    tblEingabeListe.Columns.Add("toSave", typeof(String));
                    tblEingabeListe.Columns.Add("kundenname", typeof(String));
                    tblEingabeListe.Columns.Add("Matbez", typeof(String));
                    tblEingabeListe.Columns.Add("Matnr", typeof(String));
                    tblEingabeListe.Columns.Add("NAME1", typeof(String));
                    tblEingabeListe.Columns.Add("NAME2", typeof(String));
                    tblEingabeListe.Columns.Add("PLZ", typeof(String));
                    tblEingabeListe.Columns.Add("CITY1", typeof(String));
                    tblEingabeListe.Columns.Add("STREET", typeof(String));

                    foreach (DataRow row in tblEingabeListe.Rows)
                    {
                        row["ZULBELN"] = row["ZULBELN"].ToString().TrimStart('0');
                        row["KUNNR"] = row["KUNNR"].ToString().TrimStart('0');

                        var kundeRows = tblKundenStamm.Select("KUNNR = '" + row["KUNNR"] + "'");
                        if (kundeRows.Length > 0)
                        {
                            row["kundenname"] = kundeRows[0]["NAME1"].ToString();
                        }

                        var posRows = tblPos.Select("ZULBELN = '" + row["ZULBELN"].ToString().PadLeft(10, '0') + "'");
                        if (posRows.Length > 0)
                        {
                            row["Matnr"] = posRows[0]["MATNR"];
                            if (ZLDCommon.Materialliste.ContainsKey(row["Matnr"].ToString().TrimStart('0')))
                                row["Matbez"] = ZLDCommon.Materialliste[row["Matnr"].ToString().TrimStart('0')];
                        }

                        var adrsRows = tblAdrs.Select("ZULBELN = '" + row["ZULBELN"].ToString().PadLeft(10, '0') + "'");
                        if (adrsRows.Length > 0)
                        {
                            row["NAME1"] = adrsRows[0]["NAME1"];
                            row["NAME2"] = adrsRows[0]["NAME2"];
                            row["PLZ"] = adrsRows[0]["PLZ"];
                            row["CITY1"] = adrsRows[0]["CITY1"];
                            row["STREET"] = adrsRows[0]["STREET"];
                        }
                    }

                    if (tblEingabeListe.Rows.Count == 0)
                    {
                        m_intStatus = 9999;
                        m_strMessage = "Keine Daten gefunden!";
                    }

                    m_intStatus = myProxy.getExportParameter("E_SUBRC").ToInt(0);
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -5555;
                            m_strMessage = "Es ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally
                {
                    m_blnGestartet = false;
                }
            }
        }

        public void SelectVorgang(long sapId)
        {
            Init("SelectVorgang", "", "");

            try
            {
                var rows = tblEingabeListe.Select("ZULBELN = '" + sapId.ToString() + "'");

                if (rows.Length == 0)
                {
                    m_intStatus = 9999;
                    m_strMessage = "Fehler beim Laden des Vorgangs";
                    return;
                }

                var row = rows[0];

                id_sap = row["ZULBELN"].ToString().ToLong(0);
                AppID = row["APPID"].ToString();
                Kundenname = row["kundenname"].ToString();
                Kunnr = row["KUNNR"].ToString();
                Ref1 = row["ZZREFNR1"].ToString();
                Ref2 = row["ZZREFNR2"].ToString();
                Ref3 = row["ZZREFNR3"].ToString();
                Ref4 = row["ZZREFNR4"].ToString();
                KreisKennz = row["KREISKZ"].ToString();
                Kreis = row["KREISBEZ"].ToString();
                WunschKenn = row["WUNSCHKENN_JN"].ToString().XToBool();
                Reserviert = row["RESERVKENN_JN"].ToString().XToBool();
                ReserviertKennz = row["RESERVKENN"].ToString();
                EVB = row["ZZEVB"].ToString();
                KennzUebernahme = row["KENNZ_UEBERNAHME"].ToString().XToBool();
                Serie = row["SERIE"].ToString().XToBool();
                MussReserviert = row["MNRESW"].ToString().XToBool();
                KennzVorhanden = row["KENNZ_VH"].ToString().XToBool();
                Fahrzeugart = row["FAHRZ_ART"].ToString();
                Saison = row["SAISON_KNZ"].ToString().XToBool();
                SaisonBeg = row["SAISON_BEG"].ToString().TrimStart('0');
                SaisonEnd = row["SAISON_END"].ToString().TrimStart('0');
                ZusatzKZ = row["ZUSKENNZ"].ToString().XToBool();
                ZollVers = row["ZOLLVERS"].ToString();
                ZollVersDauer = row["ZOLLVERS_DAUER"].ToString();
                EinKennz = row["EINKENN_JN"].ToString().XToBool();
                Altkenn = row["ALT_KENNZ"].ToString();
                VorhKennzReserv = row["VH_KENNZ_RES"].ToString().XToBool();
                ZBII_ALT_NEU = (row["ZBII_ALT_NEU"].ToString() == "N");
                Feinstaub = row["FEINSTAUB"].ToString().XToBool();
                ZulDate = row["ZZZLDAT"].ToString();
                if (ZulDate.IsDate()) { ZulDate = ((DateTime)row["ZZZLDAT"]).ToShortDateString(); }
                StillDate = row["STILL_DAT"].ToString();
                if (StillDate.IsDate()) { StillDate = ((DateTime)row["STILL_DAT"]).ToShortDateString(); }
                Kennzeichen = row["ZZKENN"].ToString();
                WunschKZ2 = row["WU_KENNZ2"].ToString();
                WunschKZ3 = row["WU_KENNZ3"].ToString();
                OhneGruenenVersSchein = row["O_G_VERSSCHEIN"].ToString().XToBool();
                Bemerkung = row["BEMERKUNG"].ToString();
                KennzForm = row["KENNZFORM"].ToString();
                NrLangText = row["LTEXT_NR"].ToString();
                LangText = "";
                TuvAu = row["TUEV_AU"].ToString();
                Haltedauer = row["HALTE_DAUER"].ToString();
                if (Haltedauer.IsDate()) { Haltedauer = ((DateTime)row["HALTE_DAUER"]).ToShortDateString(); }
                VkKurz = row["VK_KUERZEL"].ToString();
                Notiz = row["KUNDEN_NOTIZ"].ToString();
                InternRef = row["KUNDEN_REF"].ToString();
                Vorgang = row["BLTYP"].ToString();
                SWIFT = row["SWIFT"].ToString();
                IBAN = row["IBAN"].ToString();
                Bankkey = row["BANKL"].ToString();
                Kontonr = row["BANKN"].ToString();
                Inhaber = row["KOINH"].ToString();
                Geldinstitut = row["EBPP_ACCNAME"].ToString();
                EinzugErm = row["EINZ_JN"].ToString().XToBool();
                Rechnung = row["RECH_JN"].ToString().XToBool();
                Barzahlung = row["BARZ_JN"].ToString().XToBool();
                NrMaterial = row["Matnr"].ToString();
                Material = row["Matbez"].ToString();
                Kunnr = row["KUNNR"].ToString();
                Name1 = row["NAME1"].ToString();
                Name2 = row["NAME2"].ToString();
                PLZ = row["PLZ"].ToString();
                Ort = row["CITY1"].ToString();
                Strasse = row["STREET"].ToString();
            }
            catch (Exception ex)
            {
                m_intStatus = 9999;
                m_strMessage = ex.Message;
            }
        }

        public void DeleteVorgang(String strAppID, String strSessionID, Page page, long sapId)
        {
            Init("SelectVorgang", "", "");

            if (!m_blnGestartet)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_DELETE_AH_WARENKORB", ref m_objApp, ref m_objUser, ref page);

                    DataTable impBak = myProxy.getImportTable("GT_BAK");

                    var newRow = impBak.NewRow();
                    newRow["ZULBELN"] = sapId.ToString().PadLeft(10, '0');
                    impBak.Rows.Add(newRow);

                    myProxy.callBapi();

                    m_intStatus = myProxy.getExportParameter("E_SUBRC").ToInt(0);
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");
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
                }
                finally { m_blnGestartet = false; }
            }
        }

        public void SaveVorgangToSap(String strAppID, String strSessionID, Page page, bool cpdFormular, bool zusatzFormulare)
        {
            if (IsNewVorgang)
            {
                GiveSapID(strAppID, strSessionID, page);
                if (id_sap == 0)
                {
                    m_intStatus = 9999;
                    m_strMessage = "Fehler beim exportieren der Belegnummer!";
                    return;
                }
            }
            
            Init("SaveVorgangToSap", strAppID, strSessionID);

            tblFehler = null;
            if (!m_blnGestartet)
            {
                m_blnGestartet = true;

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_IMPORT_ERFASSUNG1", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_AUFRUF", "1");
                    myProxy.setImportParameter("I_SPEICHERN", "S");
                    myProxy.setImportParameter("I_TELNR", m_objUser.PhoneEmployee);

                    if (cpdFormular)
                        myProxy.setImportParameter("I_FORMULAR", "X");

                    if (zusatzFormulare)
                        myProxy.setImportParameter("I_ZUSATZFORMULARE", "X");

                    DataTable impBak = myProxy.getImportTable("GT_BAK_IN");
                    DataTable impPos = myProxy.getImportTable("GT_POS_IN");
                    DataTable impAdrs = myProxy.getImportTable("GT_ADRS_IN");

                    var impBakRow = impBak.NewRow();

                    impBakRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
                    impBakRow["APPID"] = AppID;
                    impBakRow["WEBUSER_ID"] = m_objUser.UserID.ToString();
                    impBakRow["WEBGOUP_ID"] = m_objUser.GroupID.ToString();
                    impBakRow["VKORG"] = VKORG;
                    impBakRow["VKBUR"] = VKBUR;
                    impBakRow["BLTYP"] = Vorgang;
                    impBakRow["KUNNR"] = Kunnr.PadLeft(10, '0');
                    impBakRow["ZZREFNR1"] = Ref1;
                    impBakRow["ZZREFNR2"] = Ref2;
                    impBakRow["ZZREFNR3"] = Ref3;
                    impBakRow["ZZREFNR4"] = Ref4;
                    impBakRow["KREISKZ"] = KreisKennz;
                    impBakRow["KREISBEZ"] = Kreis;
                    impBakRow["WUNSCHKENN_JN"] = WunschKenn.BoolToX();
                    impBakRow["RESERVKENN_JN"] = Reserviert.BoolToX();
                    impBakRow["RESERVKENN"] = ReserviertKennz;
                    impBakRow["FEINSTAUB"] = Feinstaub.BoolToX();
                    impBakRow["ZZZLDAT"] = ZulDate.ToDateTimeOrDbNull();
                    impBakRow["ZZKENN"] = Kennzeichen;
                    impBakRow["WU_KENNZ2"] = WunschKZ2;
                    impBakRow["WU_KENNZ3"] = WunschKZ3;
                    impBakRow["O_G_VERSSCHEIN"] = OhneGruenenVersSchein.BoolToX();
                    impBakRow["KENNZTYP"] = "";
                    impBakRow["KENNZFORM"] = KennzForm;
                    impBakRow["EINKENN_JN"] = EinKennz.BoolToX();
                    impBakRow["BEMERKUNG"] = Bemerkung;
                    impBakRow["VK_KUERZEL"] = VkKurz;
                    impBakRow["KUNDEN_REF"] = InternRef;
                    impBakRow["KUNDEN_NOTIZ"] = Notiz;
                    impBakRow["KENNZ_VH"] = KennzVorhanden.BoolToX();
                    impBakRow["ALT_KENNZ"] = Altkenn;
                    impBakRow["ZBII_ALT_NEU"] = (ZBII_ALT_NEU ? "N" : "A");
                    impBakRow["VH_KENNZ_RES"] = VorhKennzReserv.BoolToX();
                    impBakRow["STILL_DAT"] = StillDate.ToDateTimeOrDbNull();
                    impBakRow["MNRESW"] = MussReserviert.BoolToX();
                    impBakRow["ZZEVB"] = EVB;
                    impBakRow["KENNZ_UEBERNAHME"] = KennzUebernahme.BoolToX();
                    impBakRow["SERIE"] = Serie.BoolToX();
                    impBakRow["SAISON_KNZ"] = Saison.BoolToX();
                    impBakRow["SAISON_BEG"] = (String.IsNullOrEmpty(SaisonBeg) ? "" : SaisonBeg.PadLeft(2, '0'));
                    impBakRow["SAISON_END"] = (String.IsNullOrEmpty(SaisonEnd) ? "" : SaisonEnd.PadLeft(2, '0'));
                    impBakRow["ZUSKENNZ"] = ZusatzKZ.BoolToX();
                    impBakRow["TUEV_AU"] = TuvAu;
                    impBakRow["KURZZEITVS"] = KurzZeitKennz;
                    impBakRow["ZOLLVERS"] = ZollVers;
                    impBakRow["ZOLLVERS_DAUER"] = ZollVersDauer;
                    impBakRow["FAHRZ_ART"] = Fahrzeugart;
                    impBakRow["VORFUEHR"] = Vorfuehr;
                    impBakRow["LTEXT_NR"] = NrLangText;
                    impBakRow["HALTE_DAUER"] = Haltedauer.ToDateTimeOrDbNull();
                    impBakRow["VE_ERNAM"] = m_objUser.UserName;
                    impBakRow["SWIFT"] = SWIFT;
                    impBakRow["IBAN"] = IBAN;
                    impBakRow["BANKL"] = Bankkey;
                    impBakRow["BANKN"] = Kontonr;
                    impBakRow["EBPP_ACCNAME"] = Geldinstitut;
                    impBakRow["KOINH"] = Inhaber;
                    impBakRow["EINZ_JN"] = EinzugErm.BoolToX();
                    impBakRow["RECH_JN"] = Rechnung.BoolToX();
                    impBakRow["BARZ_JN"] = Barzahlung.BoolToX();

                    impBak.Rows.Add(impBakRow);

                    var impPosRow = impPos.NewRow();

                    impPosRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
                    impPosRow["LFDNR"] = "000010";
                    impPosRow["MENGE"] = "1";
                    impPosRow["MATNR"] = NrMaterial;

                    impPos.Rows.Add(impPosRow);

                    if (!String.IsNullOrEmpty(Name1))
                    {
                        var impAdrsRow = impAdrs.NewRow();

                        impAdrsRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
                        impAdrsRow["NAME1"] = Name1;
                        impAdrsRow["NAME2"] = Name2;
                        impAdrsRow["PLZ"] = PLZ;
                        impAdrsRow["CITY1"] = Ort;
                        impAdrsRow["STREET"] = Strasse;

                        impAdrs.Rows.Add(impAdrsRow);
                    }

                    myProxy.callBapi();

                    tblFehler = myProxy.getExportTable("GT_ERROR");
                    tblPrint = myProxy.getExportTable("GT_FILENAME");

                    KundenformularPDF = (cpdFormular ? myProxy.getExportParameterByte("E_PDF") : null);

                    tblPrintKundenformulare = (zusatzFormulare ? myProxy.getExportTable("GT_FILENAME") : null);

                    m_intStatus = myProxy.getExportParameter("E_SUBRC").ToInt(0);
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");

                    if (m_intStatus != 0)
                    {
                        if (String.IsNullOrEmpty(m_strMessage))
                            m_strMessage = "Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden";

                        if (tblFehler.Rows.Count > 0)
                            m_strMessage += " (" + tblFehler.Rows[0]["MESSAGE"].ToString() + ")";
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -5555;
                            m_strMessage = "Es ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally
                {
                    m_blnGestartet = false;
                }
            }
        }

        public void SaveVorgaengeToSap(String strAppID, String strSessionID, Page page, DataTable tblVorgaenge, bool cpdFormular, bool zusatzFormulare)
        {
            if (tblVorgaenge == null || tblVorgaenge.Rows.Count == 0)
                return;

            var idList = new Dictionary<int, long>();

            for (var i = 0; i < tblVorgaenge.Rows.Count; i++)
            {
                GiveSapID(strAppID, strSessionID, page);
                if (id_sap == 0)
                {
                    m_intStatus = 9999;
                    m_strMessage = "Fehler beim exportieren der Belegnummer!";
                    return;
                }

                idList.Add(i, id_sap);
            }
            
            Init("SaveVorgangToSap", strAppID, strSessionID);

            tblFehler = null;
            if (!m_blnGestartet)
            {
                m_blnGestartet = true;

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_IMPORT_ERFASSUNG1", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_AUFRUF", "1");
                    myProxy.setImportParameter("I_SPEICHERN", "S");
                    myProxy.setImportParameter("I_TELNR", m_objUser.PhoneEmployee);

                    if (cpdFormular)
                        myProxy.setImportParameter("I_FORMULAR", "X");

                    if (zusatzFormulare)
                        myProxy.setImportParameter("I_ZUSATZFORMULARE", "X");

                    DataTable impBak = myProxy.getImportTable("GT_BAK_IN");
                    DataTable impPos = myProxy.getImportTable("GT_POS_IN");
                    DataTable impAdrs = myProxy.getImportTable("GT_ADRS_IN");

                    foreach (var sapId in idList)
                    {
                        var vRow = tblVorgaenge.Rows[sapId.Key];

                        var impBakRow = impBak.NewRow();

                        impBakRow["ZULBELN"] = sapId.Value.ToString().PadLeft(10, '0');
                        impBakRow["APPID"] = AppID;
                        impBakRow["WEBUSER_ID"] = m_objUser.UserID.ToString();
                        impBakRow["WEBGOUP_ID"] = m_objUser.GroupID.ToString();
                        impBakRow["VKORG"] = VKORG;
                        impBakRow["VKBUR"] = VKBUR;
                        impBakRow["BLTYP"] = Vorgang;
                        impBakRow["KUNNR"] = Kunnr.PadLeft(10, '0');
                        impBakRow["ZZREFNR1"] = Ref1;

                        // Referenzen werden teilweise bei Anzahl >1 je Vorgang erfasst, deshalb die nachfolgende Fallunterscheidung
                        if (tblVorgaenge.Columns.Contains("REF2"))
                        {
                            impBakRow["ZZREFNR2"] = vRow["REF2"].ToString();
                            impBakRow["ZZREFNR3"] = vRow["REF3"].ToString();
                            impBakRow["ZZREFNR4"] = vRow["REF4"].ToString();
                        }
                        else
                        {
                            impBakRow["ZZREFNR2"] = Ref2;
                            impBakRow["ZZREFNR3"] = Ref3;
                            impBakRow["ZZREFNR4"] = Ref4;
                        }

                        impBakRow["KREISKZ"] = KreisKennz;
                        impBakRow["KREISBEZ"] = Kreis;
                        impBakRow["WUNSCHKENN_JN"] = WunschKenn.BoolToX();
                        impBakRow["RESERVKENN_JN"] = Reserviert.BoolToX();
                        impBakRow["RESERVKENN"] = ReserviertKennz;
                        impBakRow["FEINSTAUB"] = Feinstaub.BoolToX();
                        impBakRow["ZZZLDAT"] = ZulDate.ToDateTimeOrDbNull();

                        if (tblVorgaenge.Columns.Contains("KennzFun") && !String.IsNullOrEmpty(vRow["KennzFun"].ToString()))
                        {
                            impBakRow["ZZKENN"] = vRow["KennzFun"].ToString();
                        }
                        else
                        {
                            impBakRow["ZZKENN"] = vRow["Kennz1"].ToString() + "-" + vRow["Kennz2"].ToString();
                        }

                        impBakRow["WU_KENNZ2"] = WunschKZ2;
                        impBakRow["WU_KENNZ3"] = WunschKZ3;
                        impBakRow["O_G_VERSSCHEIN"] = OhneGruenenVersSchein.BoolToX();
                        impBakRow["KENNZTYP"] = "";

                        impBakRow["KENNZFORM"] = (tblVorgaenge.Columns.Contains("Kennzform") ? vRow["Kennzform"].ToString() : KennzForm);

                        impBakRow["EINKENN_JN"] = (tblVorgaenge.Columns.Contains("EinKennz") ? (bool)vRow["EinKennz"] : EinKennz).BoolToX();

                        impBakRow["BEMERKUNG"] = Bemerkung;
                        impBakRow["VK_KUERZEL"] = VkKurz;
                        impBakRow["KUNDEN_REF"] = InternRef;
                        impBakRow["KUNDEN_NOTIZ"] = Notiz;

                        impBakRow["KENNZ_VH"] = KennzVorhanden.BoolToX();
                        impBakRow["ALT_KENNZ"] = Altkenn;
                        impBakRow["ZBII_ALT_NEU"] = (ZBII_ALT_NEU ? "N" : "A");

                        impBakRow["VH_KENNZ_RES"] = (tblVorgaenge.Columns.Contains("KennzVorhanden") ? (bool)vRow["KennzVorhanden"] : VorhKennzReserv).BoolToX();

                        impBakRow["STILL_DAT"] = StillDate.ToDateTimeOrDbNull();
                        impBakRow["MNRESW"] = MussReserviert.BoolToX();
                        impBakRow["ZZEVB"] = EVB;
                        impBakRow["KENNZ_UEBERNAHME"] = KennzUebernahme.BoolToX();
                        impBakRow["SERIE"] = Serie.BoolToX();
                        impBakRow["SAISON_KNZ"] = Saison.BoolToX();
                        impBakRow["SAISON_BEG"] = (String.IsNullOrEmpty(SaisonBeg) ? "" : SaisonBeg.PadLeft(2, '0'));
                        impBakRow["SAISON_END"] = (String.IsNullOrEmpty(SaisonEnd) ? "" : SaisonEnd.PadLeft(2, '0'));
                        impBakRow["ZUSKENNZ"] = ZusatzKZ.BoolToX();
                        impBakRow["TUEV_AU"] = TuvAu;
                        impBakRow["KURZZEITVS"] = KurzZeitKennz;
                        impBakRow["ZOLLVERS"] = ZollVers;
                        impBakRow["ZOLLVERS_DAUER"] = ZollVersDauer;
                        impBakRow["FAHRZ_ART"] = Fahrzeugart;
                        impBakRow["VORFUEHR"] = Vorfuehr;
                        impBakRow["LTEXT_NR"] = NrLangText;
                        impBakRow["HALTE_DAUER"] = Haltedauer.ToDateTimeOrDbNull();
                        impBakRow["VE_ERNAM"] = m_objUser.UserName;
                        impBakRow["SWIFT"] = SWIFT;
                        impBakRow["IBAN"] = IBAN;
                        impBakRow["BANKL"] = Bankkey;
                        impBakRow["BANKN"] = Kontonr;
                        impBakRow["EBPP_ACCNAME"] = Geldinstitut;
                        impBakRow["KOINH"] = Inhaber;
                        impBakRow["EINZ_JN"] = EinzugErm.BoolToX();
                        impBakRow["RECH_JN"] = Rechnung.BoolToX();
                        impBakRow["BARZ_JN"] = Barzahlung.BoolToX();

                        impBak.Rows.Add(impBakRow);

                        var impPosRow = impPos.NewRow();

                        impPosRow["ZULBELN"] = sapId.Value.ToString().PadLeft(10, '0');
                        impPosRow["LFDNR"] = "000010";
                        impPosRow["MENGE"] = "1";
                        impPosRow["MATNR"] = NrMaterial.PadLeft(18, '0');

                        impPos.Rows.Add(impPosRow);

                        if (!String.IsNullOrEmpty(Name1))
                        {
                            var impAdrsRow = impAdrs.NewRow();

                            impAdrsRow["ZULBELN"] = sapId.Value.ToString().PadLeft(10, '0');
                            impAdrsRow["NAME1"] = Name1;
                            impAdrsRow["NAME2"] = Name2;
                            impAdrsRow["PLZ"] = PLZ;
                            impAdrsRow["CITY1"] = Ort;
                            impAdrsRow["STREET"] = Strasse;

                            impAdrs.Rows.Add(impAdrsRow);
                        }
                    }

                    myProxy.callBapi();

                    tblFehler = myProxy.getExportTable("GT_ERROR");
                    tblPrint = myProxy.getExportTable("GT_FILENAME");

                    KundenformularPDF = (cpdFormular ? myProxy.getExportParameterByte("E_PDF") : null);

                    tblPrintKundenformulare = (zusatzFormulare ? myProxy.getExportTable("GT_FILENAME") : null);

                    m_intStatus = myProxy.getExportParameter("E_SUBRC").ToInt(0);
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");

                    if (m_intStatus != 0)
                    {
                        if (String.IsNullOrEmpty(m_strMessage))
                            m_strMessage = "Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden";

                        if (tblFehler.Rows.Count > 0)
                        {
                            foreach (DataRow errRow in tblFehler.Rows)
                            {
                                m_strMessage += " (" + errRow["ZULBELN"].ToString() + ": " + errRow["MESSAGE"].ToString() + ")";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -5555;
                            m_strMessage = "Es ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally
                {
                    m_blnGestartet = false;
                }
            }
        }

        public void SendVorgaengeToSap(String strAppID, String strSessionID, Page page, DataTable tblStvaStamm)
        {
            Init("SendVorgaengeToSap", strAppID, strSessionID);

            tblFehler = null;
            if (!m_blnGestartet)
            {
                m_blnGestartet = true;

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_IMPORT_ERFASSUNG1", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_AUFRUF", "1");
                    myProxy.setImportParameter("I_SPEICHERN", "A");
                    myProxy.setImportParameter("I_TELNR", m_objUser.PhoneEmployee);

                    DataTable impBak = myProxy.getImportTable("GT_BAK_IN");
                    DataTable impPos = myProxy.getImportTable("GT_POS_IN");
                    DataTable impAdrs = myProxy.getImportTable("GT_ADRS_IN");

                    var saveRows = tblEingabeListe.Select("toSave = '1'");

                    foreach (DataRow SaveRow in saveRows)
                    {
                        DataRow impBakRow = impBak.NewRow();

                        if (ModelMapping.Copy(SaveRow, impBakRow))
                        {
                            impBakRow["ZULBELN"] = SaveRow["ZULBELN"].ToString().PadLeft(10, '0');
                            impBakRow["KUNNR"] = SaveRow["KUNNR"].ToString().PadLeft(10, '0');

                            impBak.Rows.Add(impBakRow);

                            var impPosRow = impPos.NewRow();

                            impPosRow["ZULBELN"] = SaveRow["ZULBELN"].ToString().PadLeft(10, '0');
                            impPosRow["LFDNR"] = "000010";
                            impPosRow["MENGE"] = "1";
                            impPosRow["MATNR"] = SaveRow["Matnr"];

                            impPos.Rows.Add(impPosRow);

                            if (!String.IsNullOrEmpty(SaveRow["NAME1"].ToString()))
                            {
                                var impAdrsRow = impAdrs.NewRow();

                                impAdrsRow["ZULBELN"] = SaveRow["ZULBELN"].ToString().PadLeft(10, '0');
                                impAdrsRow["NAME1"] = SaveRow["NAME1"];
                                impAdrsRow["NAME2"] = SaveRow["NAME2"];
                                impAdrsRow["PLZ"] = SaveRow["PLZ"];
                                impAdrsRow["CITY1"] = SaveRow["CITY1"];
                                impAdrsRow["STREET"] = SaveRow["STREET"];

                                impAdrs.Rows.Add(impAdrsRow);
                            }
                        }
                    }

                    myProxy.callBapi();

                    tblFehler = myProxy.getExportTable("GT_ERROR");
                    tblPrint = myProxy.getExportTable("GT_FILENAME");

                    m_intStatus = myProxy.getExportParameter("E_SUBRC").ToInt(0);
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");

                    foreach (DataRow SaveRow in saveRows)
                    {
                        var errRows = tblFehler.Select("ZULBELN = '" + SaveRow["ZULBELN"].ToString().PadLeft(10, '0') + "'");
                        if (errRows.Length > 0)
                        {
                            SaveRow["Status"] = errRows[0]["MESSAGE"].ToString();

                            m_intStatus = -9999;
                            if (String.IsNullOrEmpty(m_strMessage))
                                m_strMessage = "Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden";
                        }
                        else
                        {
                            SaveRow["Status"] = "OK";
                        }
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -5555;
                            m_strMessage = "Es ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally
                {
                    m_blnGestartet = false;
                }
            }
        }

        /// <summary>
        /// Laden der Firmeneigenen Zulassungen aus SAP
        /// diese können abgemeldet oder gelöscht werden
        /// Bapi Z_ZLD_AH_AF_ABM_LISTE
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">AbmeldungAHZul.aspx.cs</param>
        /// <param name="vkOrg">Verkaufsorganisation</param>
        /// <param name="vkBur">Verkaufsbüro</param>
        public void GetAbmeldungAH(String strAppID, String strSessionID, Page page, String vkOrg, String vkBur)
        {
            Init("GetAbmeldungAH", strAppID, strSessionID);

            if (!m_blnGestartet)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_AF_ABM_LISTE", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKORG", vkOrg);
                    myProxy.setImportParameter("I_VKBUR", vkBur);
                    myProxy.setImportParameter("I_GRUPPE", m_objUser.Groups[0].GroupName);
                    myProxy.setImportParameter("I_KUNNR", "");
                    myProxy.callBapi();

                    Abmeldedaten = myProxy.getExportTable("GT_OUT");
                    Abmeldedaten.Columns.Add("Auswahl", typeof(String));
                    Abmeldedaten.Columns.Add("Status", typeof(String));
                    foreach (DataRow aRow in Abmeldedaten.Rows)
                    {
                        aRow["KUNNR"] = aRow["KUNNR"].ToString().TrimStart('0');
                        aRow["ZULBELN"] = aRow["ZULBELN"].ToString().TrimStart('0');
                        aRow["NAME1"] = aRow["NAME1"].ToString() + " ~ " + aRow["KUNNR"].ToString();
                        aRow["Auswahl"] = "";
                    }
                    m_intStatus = myProxy.getExportParameter("E_SUBRC").ToInt(0);
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");
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
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Speichern(Abmelden oder Löschen) von Firmeneigenen Zulassungen in SAP
        /// Bapi Z_ZLD_AH_AF_ABM_SAVE
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">AbmeldungAHZul.aspx.cs</param>
        public void SaveAbmeldungAH(String strAppID, String strSessionID, Page page)
        {
            Init("SaveAbmeldungAH", strAppID, strSessionID);

            if (!m_blnGestartet)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_AF_ABM_SAVE", ref m_objApp, ref m_objUser, ref page);

                    DataTable SAPAbmeldedaten = myProxy.getImportTable("GT_ABM");
                    
                    foreach (DataRow aRow in Abmeldedaten.Rows)
                    {
                        if (aRow["Auswahl"].ToString() != "") 
                        {
                            DataRow RowSap = SAPAbmeldedaten.NewRow();
                            RowSap["ZULBELN"] = aRow["ZULBELN"].ToString().PadLeft(10,'0');
                            RowSap["AUSWAHL"] = aRow["AUSWAHL"];
                            RowSap["VE_ERNAM"] = m_objUser.UserName.PadLeft(12);
                            RowSap["ZZKENN"] = aRow["ZZKENN"].ToString();
                            SAPAbmeldedaten.Rows.Add(RowSap);
                        }
                    }

                    myProxy.callBapi();

                    DataTable SAPReturn = myProxy.getExportTable("GT_ABM");
                    tblPrint = myProxy.getExportTable("GT_FILENAME");
                    m_intStatus = myProxy.getExportParameter("E_SUBRC").ToInt(0);
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");
                    if (SAPReturn.Rows.Count > 0)
                    {
                        foreach (DataRow rowError in SAPReturn.Rows)
                        {
                            Int32 sapId;
                            Int32.TryParse(rowError["ZULBELN"].ToString(), out sapId);
                            DataRow[] rowListe = Abmeldedaten.Select("ZULBELN=" + sapId);
                            if (rowListe.Length == 1)
                            {
                                if (rowError["MESSAGE"].ToString() != "") { rowListe[0]["Status"] = rowError["MESSAGE"]; }
                                else { rowListe[0]["Status"] = rowError["MESSAGE"]; }
                            }
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
                }
                finally { m_blnGestartet = false; }
            }
        }

        public string GetAnzahlAuftraege(String strAppID, String strSessionID, Page page)
        {
            Init("GetAnzahlAuftraege", strAppID, strSessionID);

            var anzahl = "0";

            if (!m_blnGestartet)
            {
                m_blnGestartet = true;

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_EXPORT_WARENKORB", ref m_objApp, ref m_objUser, ref page);

                    if (SendForAll)
                    {
                        myProxy.setImportParameter("I_VKBUR", m_objUser.Reference.Substring(4, 4));
                        myProxy.setImportParameter("I_WEBGOUP_ID", m_objUser.GroupID.ToString());
                    }
                    else
                    {
                        myProxy.setImportParameter("I_WEBUSER_ID", m_objUser.UserID.ToString());
                    }

                    myProxy.callBapi();

                    var tblTmp = myProxy.getExportTable("GT_BAK");
                    anzahl = tblTmp.Rows.Count.ToString();

                    m_intStatus = myProxy.getExportParameter("E_SUBRC").ToInt(0);
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -5555;
                            m_strMessage = "Es ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally
                {
                    m_blnGestartet = false;
                }
            }

            return anzahl;
        }

        #endregion
    }
}
