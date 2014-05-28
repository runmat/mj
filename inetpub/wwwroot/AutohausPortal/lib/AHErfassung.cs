using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Common;
using System.Data;
using CKG.Base.Business;
using System.Linq.Expressions;
using System.Globalization;
using System.Configuration;
using System.Data.Linq;
namespace AutohausPortal.lib
{   
    /// <summary>
    /// Klasse die verschiedenste SAP- bzw. Datenbankzugriffe herstellt. 
    /// Sammel, sichern, Löschen und editieren von Eingabedaten in SQL oder SAP.
    /// </summary>
    public class AHErfassung : CKG.Base.Business.DatenimportBase
    {
        #region Declarations

        Int64 id_Kopf;
        
        #endregion

        #region Properties

        public DataTable tblEingabeListe { get; set; }
        public DataTable KopfTabelle { get; set; }
        public DataTable Positionen { get; set; }
        public DataTable tblFehler { get; set; }
        public DataTable tblPrint { get; set; }

        public String VKORG { get; set; }
        public String VKBUR { get; set; }


        public Int64 id_sap { get; set; }
        public Int64 KopfID { get; set; }

        public Boolean Abgerechnet { get; set; }
        public String Kundenname { get; set; }
        public String Kunnr { get; set; }
        public String Ref1 { get; set; }

        public String Ref2 { get; set; }
        public String Ref3 { get; set; }
        public String Ref4 { get; set; }
        public String KreisKennz { get; set; }
        public String Kreis { get; set; }
        public Boolean WunschKenn { get; set; }
        public Boolean Reserviert { get; set; }
        public String ReserviertKennz { get; set; }
        public Boolean Feinstaub { get; set; }
        public String ZulDate { get; set; }
        public String Kennzeichen { get; set; }
        public string WunschKZ2 { get; set; }
        public string WunschKZ3 { get; set; }
        public String Kennztyp { get; set; }
        public String KennzForm { get; set; }
        public Boolean EinKennz { get; set; }
        public String EVB { get; set; }
        public String Fahrzeugart { get; set; }

        public Boolean EC { get; set; }
        public Boolean Bar { get; set; }
        public Boolean saved { get; set; }
        public Boolean bearbeitet { get; set; }
        public String Vorgang { get; set; }
        public Int16 toSave { get; set; }
        public String toDelete { get; set; }
        public String KundennrWE { get; set; }
        public String Partnerrolle { get; set; }
        public String Name1 { get; set; }
        public String Name2 { get; set; }
        public String PLZ { get; set; }
        public String Ort { get; set; }
        public String Strasse { get; set; }
        public String SWIFT { get; set; }
        public String IBAN { get; set; }
        public String Bankkey { get; set; }
        public String Kontonr { get; set; }
        public String Inhaber { get; set; }
        public String Geldinstitut { get; set; }
        public Boolean EinzugErm { get; set; }
        public Boolean Rechnung { get; set; }
        public Boolean Barzahlung { get; set; }
        public DataTable BestLieferanten { get; set; }
        public String Altkenn { get; set; }
        public bool ZBII_ALT_NEU { get; set; }
        public bool VorhKennzReserv { get; set; }
        public String Lieferant_ZLD { get; set; }
        public String FrachtNrHin { get; set; }
        public String FrachtNrBack { get; set; }
        public String Vorfuehr{ get; set; }
        
        public Boolean Barkunde { get; set; }

        public String IsZLD { get; set; }

        public String NeueKundenNr { get; set; }

        public String NeueKundenName { get; set; }
        public String KennzTeil1 { get; set; }
        public String IDCount{get;set;}
        public String Name1Hin { get; set; }
        public String Name2Hin { get; set; }
        public String StrasseHin { get; set; }
        public String PLZHin { get; set; }
        public String OrtHin { get; set; }
        public String DocRueck1 { get; set; }
        public String NameRueck1 { get; set; }
        public String NameRueck2 { get; set; }
        public String StrasseRueck { get; set; }
        public String PLZRueck { get; set; }

        public String OrtRueck { get; set; }
        public String Doc2Rueck { get; set; }
        public String Name1Rueck2 { get; set; }
        public String Name2Rueck2 { get; set; }
        public String Strasse2Rueck { get; set; }
        public String PLZ2Rueck { get; set; }

        public String Ort2Rueck {get;set;}

        public String Bemerkung { get; set; }
        public String Notiz { get; set; }
        public String InternRef { get; set; }
        public String VkKurz { get; set; }

        public String StillDate { get; set; }
        public String TuvAu { get; set; }
        public String ZollVers { get; set; }
        public String ZollVersDauer { get; set; }

        public Boolean KennzUebernahme { get; set; }
        public Boolean Serie { get; set; }
        public Boolean Saison { get; set; }
        public Boolean ZusatzKZ { get; set; }
        public Boolean MussReserviert { get; set; }
        public Boolean KennzVorhanden { get; set; }
        public String KurzZeitKennz { get; set; }
        public String NrLangText { get; set; }
        public String LangText { get; set; }

        public String SaisonBeg { get; set; }
        public String SaisonEnd { get; set; }

        public String NrMaterial { get; set; }
        public String Material { get; set; }

        public String AppID { get; set; }
        public String Haltedauer { get; set; }

        public DataTable Kundenadresse { get; set; }

        public DataTable Abmeldedaten { get; set; }

        public bool OhneGruenenVersSchein { get; set; }

        private bool SendForAll { get { return m_objUser.Organization.OrganizationName.ToUpper().Contains(("SENDFORALL")); } }

        public byte[] KundenformularPDF { get; set; }

        #endregion
        
        #region Contructor
        /// <summary>
        /// Kontruktor
        /// </summary>
        /// <param name="objUser">Userobjekt</param>
        /// <param name="objApp">Anwendungsobjekt</param>
        /// <param name="sVorgang">Vorgang z.B. Kennzeichenbestellung</param>
        public AHErfassung(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, String sVorgang)
            : base(ref objUser, objApp, "")
        {

            Vorgang = sVorgang;
            CreatePosTable();
        } 
        #endregion

        #region Methods

        /// <summary>
        /// Läd die neu generierte Belegnummer aus SAP
        /// </summary>
        /// <param name="strAppID"></param>
        /// <param name="strSessionID"></param>
        /// <param name="page"></param>
        public void GiveSapID(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "VoerfZLD.GiveSapID";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            Int32 tempID = 0;
            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_BELNR", ref m_objApp, ref m_objUser, ref page);

                    myProxy.callBapi();

                    Int32.TryParse(myProxy.getExportParameter("E_BELN").ToString(), out tempID);
                    id_sap = tempID;
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
                            m_strMessage = "Keine Daten gefunden(Kreiskennzeichen).";
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

        /// <summary>
        /// erstellt die Positionstabelle
        /// </summary>
        private void CreatePosTable()
        {
            Positionen = new DataTable();
            Positionen.Columns.Add("id_Kopf", typeof(Int32));
            Positionen.Columns.Add("id_pos", typeof(Int32));
            Positionen.Columns.Add("Menge", typeof(String));
            Positionen.Columns.Add("Matnr", typeof(String));
            Positionen.Columns.Add("Matbez", typeof(String));
        }

        /// <summary>
        /// Speichert die eingebenen Daten in den SQL Tabellen
        /// </summary>
        /// <param name="strAppID"></param>
        /// <param name="strSessionID"></param>
        /// <param name="page"></param>
        /// <param name="tblKundenStamm"></param>
        public void InsertDB_ZLD(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblKundenStamm)
        {
            m_intStatus = 0;
            m_strMessage = "";
            dboZLDAutohausDataContext ZLD_Data = new dboZLDAutohausDataContext();
            try
            {
                m_intStatus = 0;
                m_strMessage = "";
                bool createKopf = false;
                GiveSapID(strAppID, strSessionID, page);
                if (id_sap != 0)
                {
                    ZLD_Data.Connection.Open();

                    {
                        ZLDKopfTabelle tblKopf = new ZLDKopfTabelle();
                        tblKopf.id_sap = id_sap;
                        tblKopf.id_user = m_objUser.UserID;
                        tblKopf.id_session = strSessionID;
                        tblKopf.abgerechnet = false;
                        tblKopf.username = m_objUser.UserName;

                        //Kunden- und Fahrzeugdaten
                        tblKopf.kundenname = Kundenname;
                        tblKopf.kundennr = Kunnr;
                        tblKopf.EVB = EVB;
                        tblKopf.Feinstaub = Feinstaub;
                        DataRow[] KundeRow = tblKundenStamm.Select("KUNNR='" + Kunnr + "'");

                        if (KundeRow.Length == 1)
                        {
                            tblKopf.KunnrLF = KundeRow[0]["KUNNR_LF"].ToString();
                        }
                        //Kunden- und Fahrzeugdaten

                        //Referenzen
                        tblKopf.referenz1 = Ref1;
                        tblKopf.referenz2 = Ref2;
                        tblKopf.referenz3 = Ref3;
                        tblKopf.referenz4 = Ref4;
                        //Referenzen

                        //Zulassungsdaten
                        tblKopf.KreisKZ = KreisKennz;
                        tblKopf.KreisBez = Kreis;
                        tblKopf.WunschKenn = WunschKenn;
                        tblKopf.Reserviert = Reserviert;
                        tblKopf.ReserviertKennz = ReserviertKennz;
                        tblKopf.Fahrz_Art = Fahrzeugart;
                        tblKopf.Kurzzeitkennz = KurzZeitKennz;

                        DateTime tmpDate;
                        DateTime.TryParse(ZulDate, out tmpDate);
                        tblKopf.Zulassungsdatum = tmpDate;
                        tblKopf.Kennzeichen = Kennzeichen;
                        tblKopf.WunschKZ2 = WunschKZ2;
                        tblKopf.WunschKZ3 = WunschKZ3;
                        tblKopf.OhneGruenenVersSchein = BoolToX(OhneGruenenVersSchein);
                        String sKennz = Kennzeichen;
                        String sKennzKZ = "";
                        String sKennzABC = "";
                        String[] sArr = sKennz.Split('-');
                        if (sArr.Length == 1)
                        {
                            sKennzKZ = sArr[0];
                        }
                        if (sArr.Length == 2)
                        {
                            sKennzKZ = sArr[0];
                            sKennzABC = sArr[1];
                        }
                        if (sArr.Length == 3)// Sonderlocke für Behördenfahrzeuge z.B. BWL-4-4444
                        {
                            sKennzKZ = sArr[0];
                            sKennzABC = sArr[1] + "-" + sArr[2];
                        }
                        //tblKopf.KennKZ = sKennzKZ;
                        tblKopf.KennABC = sKennzABC;
                        tblKopf.KennzForm = KennzForm;
                        tblKopf.EinKennz = EinKennz;
                        tblKopf.VorhKennzReserv = VorhKennzReserv;
                        tblKopf.ZBII_ALT_NEU = ZBII_ALT_NEU;
                        tblKopf.KennzVH = KennzVorhanden;
                        tblKopf.KennzAlt = Altkenn;
                        tblKopf.Vorfuehrung = Vorfuehr;
                        tblKopf.KennzUebernahme = BoolToX(KennzUebernahme);
                        tblKopf.Serie = BoolToX(Serie);
                        if (IsDate(StillDate))
                        {
                            DateTime.TryParse(StillDate, out tmpDate);
                            tblKopf.Still_Datum = tmpDate;                        
                        }

                        if (IsDate(Haltedauer))
                        {
                            DateTime.TryParse(Haltedauer, out tmpDate);
                            tblKopf.Haltedauer = tmpDate;
                        }

                        tblKopf.SaisonKZ = BoolToX(Saison);
                        tblKopf.ZusatzKZ = BoolToX(ZusatzKZ);
                        tblKopf.SaisonBeg = SaisonBeg;
                        tblKopf.SaisonEnd = SaisonEnd;
                        tblKopf.Tuev_AU = TuvAu;
                        tblKopf.ZollVers = ZollVers;
                        tblKopf.ZollVers_Dauer = ZollVersDauer;
                        tblKopf.MussResWerden = BoolToX(MussReserviert);
                        tblKopf.KennzVH = KennzVorhanden;
                        //Zulassungsdaten
                        
                        //Langtext
                        tblKopf.Langtextnr = NrLangText;

      
                        // Bemerkung/Notizen
                        tblKopf.Bemerkung = Bemerkung;
                        tblKopf.VKKurz = VkKurz;
                        tblKopf.interneRef = InternRef;
                        tblKopf.KundenNotiz = Notiz;

                        //Intern
                        tblKopf.Vorerfasser = m_objUser.UserName;
                        tblKopf.VorerfDatum = System.DateTime.Now;
                        tblKopf.saved = saved;
                        tblKopf.toDelete = "";
                        tblKopf.bearbeitet = false;
                        tblKopf.Vorgang = Vorgang;
                        tblKopf.testuser = m_objUser.IsTestUser;
                        tblKopf.Filiale = m_objUser.Reference;
                        tblKopf.AppID = AppID;
                        //Intern
                        
                        //Bankdaten
                        tblKopf.Inhaber = Inhaber;
                        tblKopf.IBAN = IBAN;

                        if ((!String.IsNullOrEmpty(Geldinstitut)) && (Geldinstitut.Length > 40))
                        {
                            tblKopf.Geldinstitut = Geldinstitut.Substring(0, 40);
                        }
                        else { tblKopf.Geldinstitut = Geldinstitut; }

                        tblKopf.SWIFT = SWIFT;
                        tblKopf.BankKey = Bankkey;
                        tblKopf.Kontonr = Kontonr;
                        tblKopf.EinzugErm = EinzugErm;
                        tblKopf.Rechnung = Rechnung;
                        tblKopf.Barzahlung = Barzahlung;

                        //Bankdaten

                        ZLD_Data.ZLDKopfTabelle.InsertOnSubmit(tblKopf);
                        ZLD_Data.SubmitChanges();
                        KopfID = tblKopf.id;
                        createKopf = true;
                        try
                        {

                            ZLDPositionsTabelle tblPos = new ZLDPositionsTabelle();
                            tblPos.id_Kopf = KopfID;
                            tblPos.id_pos = 1;
                            tblPos.Menge = "1";
                            tblPos.Matnr = NrMaterial;
                            tblPos.Matbez = Material;
                            ZLD_Data.ZLDPositionsTabelle.InsertOnSubmit(tblPos);


                            ZLDKundenadresse tblKunnadresse = new ZLDKundenadresse();
                            tblKunnadresse.id_Kopf = KopfID;
                            tblKunnadresse.Partnerrolle = Partnerrolle;
                            tblKunnadresse.Name1 = Name1;
                            tblKunnadresse.Name2 = Name2;
                            tblKunnadresse.Strasse = Strasse;
                            tblKunnadresse.Ort = Ort;
                            tblKunnadresse.PLZ = PLZ;
                            ZLD_Data.ZLDKundenadresse.InsertOnSubmit(tblKunnadresse);
                            ZLD_Data.SubmitChanges();

                        }
                        catch (Exception ex)
                        {
                            m_intStatus = 9999;
                            m_strMessage = ex.Message;
                            if (createKopf)
                            {
                                dboZLDAutohausDataContext ZLD_DataKopf = new dboZLDAutohausDataContext();
                                var tblKopftoDel = (from k in ZLD_DataKopf.ZLDKopfTabelle
                                                    where k.id == KopfID
                                                    select k).Single();

                                ZLD_DataKopf.ZLDKopfTabelle.DeleteOnSubmit(tblKopftoDel);
                                ZLD_DataKopf.SubmitChanges();

                            }
                        }
                    }

                }
                else
                {
                    m_intStatus = 9999;
                    m_strMessage = "Fehler beim exportieren der Belegnummer!";
                }

            }
            catch (Exception ex)
            {

                m_intStatus = 9999;
                m_strMessage = ex.Message;
            }
            finally
            {
                if (ZLD_Data.Connection.State == ConnectionState.Open) { ZLD_Data.Connection.Close(); }
            }

        }

        /// <summary>
        /// Speichert die eingebenen Daten in den SQL Tabellen
        /// Mehrere Vorgänge (tblAbmeldung)
        /// </summary>
        /// <param name="strAppID"></param>
        /// <param name="strSessionID"></param>
        /// <param name="page"></param>
        /// <param name="tblKundenStamm"></param>
        /// <param name="tblAbmeldung">Abmeldungen</param>
        public void InsertDB_ZLDAbmeldung(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblKundenStamm, DataTable tblAbmeldung)
        {
            m_intStatus = 0;
            m_strMessage = "";
            dboZLDAutohausDataContext ZLD_Data = new dboZLDAutohausDataContext();
            try
            {
                m_intStatus = 0;
                m_strMessage = "";
                ZLD_Data.Connection.Open();
                foreach (DataRow matRow in tblAbmeldung.Rows)
                {
                    bool createKopf = false;
                    GiveSapID(strAppID, strSessionID, page);
                    if (id_sap != 0)
                    {
                        {
                            ZLDKopfTabelle tblKopf = new ZLDKopfTabelle();
                            tblKopf.id_sap = id_sap;
                            tblKopf.id_user = m_objUser.UserID;
                            tblKopf.id_session = strSessionID;
                            tblKopf.abgerechnet = false;
                            tblKopf.username = m_objUser.UserName;

                            //Kunden- und Fahrzeugdaten
                            tblKopf.kundenname = Kundenname;
                            tblKopf.kundennr = Kunnr;
                            tblKopf.EVB = EVB;
                            tblKopf.Feinstaub = Feinstaub;
                            DataRow[] KundeRow = tblKundenStamm.Select("KUNNR='" + Kunnr + "'");

                            if (KundeRow.Length == 1)
                            {
                                tblKopf.KunnrLF = KundeRow[0]["KUNNR_LF"].ToString();
                            }
                            //Kunden- und Fahrzeugdaten

                            //Referenzen (werden teilweise bei Anzahl >1 je Vorgang erfasst, deshalb die nachfolgende Fallunterscheidung)
                            tblKopf.referenz1 = Ref1;
                            if ((tblAbmeldung.Rows.Count > 1) && (matRow.Table.Columns.Contains("REF2")))
                            {
                                tblKopf.referenz2 = matRow["REF2"].ToString();
                                tblKopf.referenz3 = matRow["REF3"].ToString();
                                tblKopf.referenz4 = matRow["REF4"].ToString();
                            }
                            else
                            {
                                tblKopf.referenz2 = Ref2;
                                tblKopf.referenz3 = Ref3;
                                tblKopf.referenz4 = Ref4;
                            }
                            //Referenzen

                            //Zulassungsdaten
                            tblKopf.KreisKZ = KreisKennz;
                            tblKopf.KreisBez = Kreis;
                            tblKopf.WunschKenn = WunschKenn;
                            tblKopf.Reserviert = Reserviert;
                            tblKopf.ReserviertKennz = ReserviertKennz;
                            tblKopf.Fahrz_Art = Fahrzeugart;

                            DateTime tmpDate;
                            DateTime.TryParse(ZulDate, out tmpDate);
                            tblKopf.Zulassungsdatum = tmpDate;

                            if (tblAbmeldung.Columns.Contains("KennzFun"))
                            {

                                if (matRow["KennzFun"].ToString().Length > 0)
                                { tblKopf.Kennzeichen = matRow["KennzFun"].ToString(); }
                                else
                                {
                                    tblKopf.Kennzeichen = matRow["Kennz1"].ToString() + "-" + matRow["Kennz2"].ToString();
                                    String sKennz = matRow["Kennz1"].ToString() + "-" + matRow["Kennz2"].ToString();
                                    String sKennzKZ = "";
                                    String sKennzABC = "";
                                    String[] sArr = sKennz.Split('-');
                                    if (sArr.Length == 1)
                                    {
                                        sKennzKZ = sArr[0];
                                    }
                                    if (sArr.Length == 2)
                                    {
                                        sKennzKZ = sArr[0];
                                        sKennzABC = sArr[1];
                                    }
                                    if (sArr.Length == 3)// Sonderlocke für Behördenfahrzeuge z.B. BWL-4-4444
                                    {
                                        sKennzKZ = sArr[0];
                                        sKennzABC = sArr[1] + "-" + sArr[2];
                                    }
                                    // tblKopf.KennKZ = sKennzKZ;
                                    tblKopf.KennABC = sKennzABC;
                                }
                            }
                            else 
                            {
                                tblKopf.Kennzeichen = matRow["Kennz1"].ToString() + "-" + matRow["Kennz2"].ToString();
                                String sKennz = matRow["Kennz1"].ToString() + "-" + matRow["Kennz2"].ToString();
                                String sKennzKZ = "";
                                String sKennzABC = "";
                                String[] sArr = sKennz.Split('-');
                                if (sArr.Length == 1)
                                {
                                    sKennzKZ = sArr[0];
                                }
                                if (sArr.Length == 2)
                                {
                                    sKennzKZ = sArr[0];
                                    sKennzABC = sArr[1];
                                }
                                if (sArr.Length == 3)// Sonderlocke für Behördenfahrzeuge z.B. BWL-4-4444
                                {
                                    sKennzKZ = sArr[0];
                                    sKennzABC = sArr[1] + "-" + sArr[2];
                                }
                                // tblKopf.KennKZ = sKennzKZ;
                                tblKopf.KennABC = sKennzABC;                            
                            }

                            tblKopf.KennzForm = KennzForm;
                            if (tblAbmeldung.Columns.Contains("Kennzform")) { tblKopf.KennzForm = matRow["Kennzform"].ToString(); }

                            tblKopf.EinKennz = EinKennz;
                            if (tblAbmeldung.Columns.Contains("EinKennz")) { tblKopf.EinKennz = (Boolean)matRow["EinKennz"]; }

                            tblKopf.VorhKennzReserv = false;
                            if (tblAbmeldung.Columns.Contains("KennzVorhanden")) { tblKopf.VorhKennzReserv = (Boolean)matRow["KennzVorhanden"]; }
                            tblKopf.ZBII_ALT_NEU = ZBII_ALT_NEU;
                            tblKopf.KennzVH = KennzVorhanden;
                            tblKopf.KennzAlt = Altkenn;
                            tblKopf.Vorfuehrung = Vorfuehr;
                            tblKopf.KennzUebernahme = BoolToX(KennzUebernahme);
                            tblKopf.Serie = BoolToX(Serie);
                            if (IsDate(StillDate))
                            {
                                DateTime.TryParse(StillDate, out tmpDate);
                                tblKopf.Still_Datum = tmpDate;
                            }
                            if (IsDate(Haltedauer))
                            {
                                DateTime.TryParse(Haltedauer, out tmpDate);
                                tblKopf.Haltedauer = tmpDate;
                            }
                            tblKopf.SaisonKZ = BoolToX(Saison);
                            tblKopf.ZusatzKZ = BoolToX(ZusatzKZ);
                            tblKopf.SaisonBeg = SaisonBeg;
                            tblKopf.SaisonEnd = SaisonEnd;
                            tblKopf.Tuev_AU = TuvAu;
                            tblKopf.ZollVers = ZollVers;
                            tblKopf.ZollVers_Dauer = ZollVersDauer;
                            tblKopf.MussResWerden = BoolToX(MussReserviert);
                            //Zulassungsdaten
                            tblKopf.Langtextnr = "";

                            // Bemerkung/Notizen
                            tblKopf.Bemerkung = Bemerkung;
                            tblKopf.VKKurz = VkKurz;
                            tblKopf.interneRef = InternRef;
                            tblKopf.KundenNotiz = Notiz;

                            //Intern
                            tblKopf.Vorerfasser = m_objUser.UserName;
                            tblKopf.VorerfDatum = System.DateTime.Now;
                            tblKopf.saved = saved;
                            tblKopf.toDelete = "";
                            tblKopf.bearbeitet = false;
                            tblKopf.Vorgang = Vorgang;
                            tblKopf.testuser = m_objUser.IsTestUser;
                            tblKopf.Filiale = m_objUser.Reference;
                            tblKopf.AppID = AppID;
                            //Intern

                            //Bankdaten
                            tblKopf.Inhaber = Inhaber;
                            tblKopf.IBAN = IBAN;

                            if ((!String.IsNullOrEmpty(Geldinstitut)) && (Geldinstitut.Length > 40))
                            {
                                tblKopf.Geldinstitut = Geldinstitut.Substring(0, 40);
                            }
                            else { tblKopf.Geldinstitut = Geldinstitut; }

                            tblKopf.SWIFT = SWIFT;
                            tblKopf.BankKey = Bankkey;
                            tblKopf.Kontonr = Kontonr;
                            tblKopf.EinzugErm = EinzugErm;
                            tblKopf.Rechnung = Rechnung;
                            tblKopf.Barzahlung = Barzahlung;

                            //Bankdaten

                            ZLD_Data.ZLDKopfTabelle.InsertOnSubmit(tblKopf);
                            ZLD_Data.SubmitChanges();
                            KopfID = tblKopf.id;
                            createKopf = true;
                            try
                            {

                                ZLDPositionsTabelle tblPos = new ZLDPositionsTabelle();
                                tblPos.id_Kopf = KopfID;
                                tblPos.id_pos = 1;
                                tblPos.Menge = "1";
                                tblPos.Matnr = NrMaterial;
                                tblPos.Matbez = Material;
                                ZLD_Data.ZLDPositionsTabelle.InsertOnSubmit(tblPos);


                                ZLDKundenadresse tblKunnadresse = new ZLDKundenadresse();
                                tblKunnadresse.id_Kopf = KopfID;
                                tblKunnadresse.Partnerrolle = Partnerrolle;
                                tblKunnadresse.Name1 = Name1;
                                tblKunnadresse.Name2 = Name2;
                                tblKunnadresse.Strasse = Strasse;
                                tblKunnadresse.Ort = Ort;
                                tblKunnadresse.PLZ = PLZ;
                                ZLD_Data.ZLDKundenadresse.InsertOnSubmit(tblKunnadresse);
                                ZLD_Data.SubmitChanges();

                            }
                            catch (Exception ex)
                            {
                                m_intStatus = 9999;
                                m_strMessage = ex.Message;
                                if (createKopf == true)
                                {
                                    dboZLDAutohausDataContext ZLD_DataKopf = new dboZLDAutohausDataContext();
                                    var tblKopftoDel = (from k in ZLD_DataKopf.ZLDKopfTabelle
                                                        where k.id == KopfID
                                                        select k).Single();

                                    ZLD_DataKopf.ZLDKopfTabelle.DeleteOnSubmit(tblKopftoDel);
                                    ZLD_DataKopf.SubmitChanges();

                                }
                            }
                        }

                    }
                    else
                    {
                        m_intStatus = 9999;
                        m_strMessage = "Fehler beim exportieren der Belegnummer!";
                    }
                }
            }
            catch (Exception ex)
            {

                m_intStatus = 9999;
                m_strMessage = ex.Message;
            }
            finally
            {
                if (ZLD_Data.Connection.State == ConnectionState.Open) { ZLD_Data.Connection.Close(); }
            }

        }

        /// <summary>
        /// setzt das Löschkennzeichen in der Kopf- und Positionstabelle
        /// </summary>
        /// <param name="IDRecordset">ID SQL</param>
        /// <param name="LoeschKZ"></param>
        /// <param name="PosID">nur bei PosID = 10</param>
        public void UpdateDB_LoeschKennzeichen(Int32 IDRecordset, String LoeschKZ, Int32 PosID)
        {
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection();

            try
            {
                connection.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"].ToString();
                connection.Open();
                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
                String query = "";

                if (PosID == 10)
                {
                    if (LoeschKZ == "L")
                    { query = ", toDelete='X'"; }
                    else
                    { query = ", toDelete=''"; }

                }

                String str = "Update ZLDKopfTabelle Set id_user='" + m_objUser.UserID + "', " +
                             " username='" + m_objUser.UserName + "', " +
                             "bearbeitet= 1 " + query +
                             " Where id = " + IDRecordset;

                command = new System.Data.SqlClient.SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = str;
                command.ExecuteNonQuery();


                str = "Update ZLDPositionsTabelle Set Posloesch='" + LoeschKZ + "'";
                if (PosID != 10 && PosID != 0)
                {
                    str += " Where id_Kopf = " + IDRecordset + " AND id_pos = " + PosID;
                }
                else
                {
                    str += " Where id_Kopf = " + IDRecordset;
                }

                command = new System.Data.SqlClient.SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = str;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                m_intStatus = -9999;
                m_strMessage = ex.Message;
            }
            finally
            {
                connection.Close();
            }

        }

        /// <summary>
        /// Setzt den Vorgang auf abgerechnet(SQL-Tabelle)
        /// </summary>
        /// <param name="IDRecordset">ID SQL</param>
        public void SetAbgerechnet(Int32 IDRecordset)
        {
            m_intStatus = 0;
            m_strMessage = "";
            dboZLDAutohausDataContext ZLD_DataContext = new dboZLDAutohausDataContext();
            try
            {
                m_intStatus = 0;
                m_strMessage = "";

                var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                               where k.id == IDRecordset
                               select k).Single();

                tblKopf.abgerechnet = true;
                ZLD_DataContext.Connection.Open();
                ZLD_DataContext.SubmitChanges();
                ZLD_DataContext.Connection.Close();
            }
            catch (Exception ex)
            {
                m_intStatus = -9999;
                m_strMessage = ex.Message;
            }
            finally
            {
                if (ZLD_DataContext != null)
                {
                    if (ZLD_DataContext.Connection.State == ConnectionState.Open)
                    {
                        ZLD_DataContext.Connection.Close();
                        ZLD_DataContext.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///  Löscht den Vorgang in der SQL-Tabelle
        /// </summary>
        /// <param name="IDRecordset">ID SQL</param>
        public void DeleteRecordSet(Int32 IDRecordset)
        {
            m_intStatus = 0;
            m_strMessage = "";
            dboZLDAutohausDataContext ZLD_DataContext = new dboZLDAutohausDataContext();
            try
            {
                m_intStatus = 0;
                m_strMessage = "";

                var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                               where k.id == IDRecordset
                               select k).Single();


                ZLD_DataContext.Connection.Open();
                ZLD_DataContext.ZLDKopfTabelle.DeleteOnSubmit(tblKopf);
                ZLD_DataContext.SubmitChanges();
                ZLD_DataContext.Connection.Close();
            }
            catch (Exception ex)
            {
                m_intStatus = -9999;
                m_strMessage = ex.Message;
            }
            finally
            {
                if (ZLD_DataContext != null)
                {
                    if (ZLD_DataContext.Connection.State == ConnectionState.Open)
                    {
                        ZLD_DataContext.Connection.Close();
                        ZLD_DataContext.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Speicher eines bereits erfassten Vorganges
        /// </summary>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="tblKunde">Kundentabelle</param>
        public void UpdateDB_ZLD(String strSessionID, DataTable tblKunde)
        {
            dboZLDAutohausDataContext ZLD_DataContext = new dboZLDAutohausDataContext();
            try
            {
                m_intStatus = 0;
                m_strMessage = "";
                var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                               where k.id == id_Kopf
                               select k).Single();

                tblKopf.id_sap = id_sap;
                tblKopf.id_user = m_objUser.UserID;
                tblKopf.id_session = strSessionID;
                tblKopf.abgerechnet = false;
                tblKopf.username = m_objUser.UserName;

                //Kunden- und Fahrzeugdaten
                tblKopf.kundenname = Kundenname;
                tblKopf.kundennr = Kunnr;
                tblKopf.EVB = EVB;
                tblKopf.Feinstaub = Feinstaub;
                DataRow[] KundeRow = tblKunde.Select("KUNNR='" + Kunnr + "'");

                if (KundeRow.Length == 1)
                {
                    tblKopf.KunnrLF = KundeRow[0]["KUNNR_LF"].ToString();
                }
                //Kunden- und Fahrzeugdaten

                //Referenzen
                tblKopf.referenz1 = Ref1;
                tblKopf.referenz2 = Ref2;
                tblKopf.referenz3 = Ref3;
                tblKopf.referenz4 = Ref4;
                //Referenzen

                //Zulassungsdaten
                tblKopf.KreisKZ = KreisKennz;
                tblKopf.KreisBez = Kreis;
                tblKopf.WunschKenn = WunschKenn;
                tblKopf.Reserviert = Reserviert;
                tblKopf.ReserviertKennz = ReserviertKennz;
                tblKopf.Fahrz_Art = Fahrzeugart;
                tblKopf.Kurzzeitkennz = KurzZeitKennz;

                DateTime tmpDate;
                DateTime.TryParse(ZulDate, out tmpDate);
                tblKopf.Zulassungsdatum = tmpDate;
                tblKopf.Kennzeichen = Kennzeichen;
                tblKopf.WunschKZ2 = WunschKZ2;
                tblKopf.WunschKZ3 = WunschKZ3;
                tblKopf.OhneGruenenVersSchein = BoolToX(OhneGruenenVersSchein);
                String sKennz = Kennzeichen;
                String sKennzKZ = "";
                String sKennzABC = "";
                String[] sArr = sKennz.Split('-');
                if (sArr.Length == 1)
                {
                    sKennzKZ = sArr[0];
                }
                if (sArr.Length == 2)
                {

                    sKennzKZ = sArr[0];
                    sKennzABC = sArr[1];
                }
                if (sArr.Length == 3)// Sonderlocke für Behördenfahrzeuge z.B. BWL-4-4444
                {
                    sKennzKZ = sArr[0];
                    sKennzABC = sArr[1] + "-" + sArr[2];
                }
                //tblKopf.KennKZ = sKennzKZ;
                tblKopf.KennABC = sKennzABC;
                tblKopf.KennzForm = KennzForm;
                tblKopf.EinKennz = EinKennz;
                tblKopf.VorhKennzReserv = VorhKennzReserv;
                tblKopf.ZBII_ALT_NEU = ZBII_ALT_NEU;
                tblKopf.KennzVH = false;
                tblKopf.KennzAlt = Altkenn;
                tblKopf.Vorfuehrung = Vorfuehr;
                tblKopf.KennzUebernahme = BoolToX(KennzUebernahme);
                tblKopf.Serie = BoolToX(Serie);
                if (IsDate(StillDate))
                {
                    DateTime.TryParse(StillDate, out tmpDate);
                    tblKopf.Still_Datum = tmpDate;
                }
                if (IsDate(Haltedauer))
                {
                    DateTime.TryParse(Haltedauer, out tmpDate);
                    tblKopf.Haltedauer = tmpDate;
                }
                tblKopf.SaisonKZ = BoolToX(Saison);
                tblKopf.ZusatzKZ = BoolToX(ZusatzKZ);
                tblKopf.SaisonBeg = SaisonBeg;
                tblKopf.SaisonEnd = SaisonEnd;
                tblKopf.Tuev_AU = TuvAu;
                tblKopf.ZollVers = ZollVers;
                tblKopf.ZollVers_Dauer = ZollVersDauer;
                tblKopf.MussResWerden = BoolToX(MussReserviert);
                tblKopf.KennzVH = KennzVorhanden;
                //Zulassungsdaten
                tblKopf.Langtextnr = NrLangText;
                // Bemerkung/Notizen
                tblKopf.Bemerkung = Bemerkung;
                tblKopf.VKKurz = VkKurz;
                tblKopf.interneRef = InternRef;
                tblKopf.KundenNotiz = Notiz;

                //Intern
                tblKopf.saved = saved;
                tblKopf.bearbeitet = true;
                //Intern

                //Bankdaten
                tblKopf.Inhaber = Inhaber;
                tblKopf.IBAN = IBAN;

                if ((!String.IsNullOrEmpty(Geldinstitut)) && (Geldinstitut.Length > 40))
                {
                    tblKopf.Geldinstitut = Geldinstitut.Substring(0, 40);
                }
                else { tblKopf.Geldinstitut = Geldinstitut; }

                tblKopf.SWIFT = SWIFT;
                tblKopf.BankKey = Bankkey;
                tblKopf.Kontonr = Kontonr;
                tblKopf.EinzugErm = EinzugErm;
                tblKopf.Rechnung = Rechnung;
                tblKopf.Barzahlung = Barzahlung;

                ZLD_DataContext.Connection.Open();
                ZLD_DataContext.SubmitChanges();
                id_Kopf = tblKopf.id;
                ZLD_DataContext.Connection.Close();

                ZLD_DataContext = new dboZLDAutohausDataContext();
                ZLD_DataContext.Connection.Open();
                
                var tblPos = (from p in ZLD_DataContext.ZLDPositionsTabelle
                                where p.id_Kopf == id_Kopf && p.id_pos == 1
                                select p).Single();

                tblPos.Matnr = NrMaterial;
                tblPos.Matbez = Material;
                ZLD_DataContext.SubmitChanges();
                            
                ZLD_DataContext = new dboZLDAutohausDataContext();
                var tblKunnadresse = (from k in ZLD_DataContext.ZLDKundenadresse
                                        where k.id_Kopf == id_Kopf
                                        select k).Single();

                tblKunnadresse.Partnerrolle = Partnerrolle;
                tblKunnadresse.Name1 = Name1;
                tblKunnadresse.Name2 = Name2;
                tblKunnadresse.Ort = Ort;
                tblKunnadresse.PLZ = PLZ;
                tblKunnadresse.Strasse = Strasse;
                ZLD_DataContext.SubmitChanges();

            }
            catch (Exception ex)
            {
                m_intStatus = 9999;
                m_strMessage = ex.Message;
            }
            finally
            {
                if (ZLD_DataContext != null)
                {
                    if (ZLD_DataContext.Connection.State == ConnectionState.Open)
                    {
                        ZLD_DataContext.Connection.Close();
                        ZLD_DataContext.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Laden eines Vorgange anhand der ID für die Eingabemasken
        /// Aufruf FillDataSet()
        /// Laden der Daten aus den Datasets in die Klasseneigenschaften
        /// </summary>
        /// <param name="IDRecordset">ID SQL</param>
        public void LoadDB_ZLDRecordset(Int32 IDRecordset)
        {
            m_intStatus = 0;
            m_strMessage = "";
            DataSet ds = new DataSet();
            FillDataSet(IDRecordset, ref ds);
            try
            {
                DataTable tmpKopf = ds.Tables[0];
                DataTable tmpPos = ds.Tables[1];
                DataTable tmpKunde = ds.Tables[2];

                KopfTabelle = new DataTable();
                KopfTabelle = tmpKopf;

                id_Kopf = (Int64)KopfTabelle.Rows[0]["id"];
                id_sap = (Int64)KopfTabelle.Rows[0]["id_sap"];
                Abgerechnet = (Boolean)KopfTabelle.Rows[0]["abgerechnet"];
                Kundenname = KopfTabelle.Rows[0]["kundenname"].ToString();
                Kunnr = KopfTabelle.Rows[0]["kundennr"].ToString();
                Ref1 = KopfTabelle.Rows[0]["referenz1"].ToString();
                Ref2 = KopfTabelle.Rows[0]["referenz2"].ToString();
                Ref3 = KopfTabelle.Rows[0]["referenz3"].ToString();
                Ref4 = KopfTabelle.Rows[0]["referenz4"].ToString();
                KreisKennz = KopfTabelle.Rows[0]["KreisKZ"].ToString();
                Kreis = KopfTabelle.Rows[0]["KreisBez"].ToString();
                WunschKenn = (Boolean)KopfTabelle.Rows[0]["WunschKenn"];
                Reserviert = (Boolean)KopfTabelle.Rows[0]["Reserviert"];
                ReserviertKennz = KopfTabelle.Rows[0]["ReserviertKennz"].ToString();
                EVB = KopfTabelle.Rows[0]["EVB"].ToString();

                KennzUebernahme = XToBool(KopfTabelle.Rows[0]["KennzUebernahme"].ToString());
                Serie = XToBool(KopfTabelle.Rows[0]["Serie"].ToString());
                MussReserviert = XToBool(KopfTabelle.Rows[0]["MussResWerden"].ToString());
                KennzVorhanden = (Boolean)KopfTabelle.Rows[0]["KennzVH"];
                Fahrzeugart = KopfTabelle.Rows[0]["Fahrz_Art"].ToString();
                Saison = XToBool(KopfTabelle.Rows[0]["SaisonKZ"].ToString());
                SaisonBeg = KopfTabelle.Rows[0]["SaisonBeg"].ToString();
                SaisonEnd = KopfTabelle.Rows[0]["SaisonEnd"].ToString();
                ZusatzKZ = XToBool(KopfTabelle.Rows[0]["ZusatzKZ"].ToString());
                ZollVers = KopfTabelle.Rows[0]["ZollVers"].ToString();
                ZollVersDauer = KopfTabelle.Rows[0]["ZollVers_Dauer"].ToString();
                EinKennz = (Boolean)KopfTabelle.Rows[0]["EinKennz"];
                Altkenn = KopfTabelle.Rows[0]["KennzAlt"].ToString();
                VorhKennzReserv = (Boolean)KopfTabelle.Rows[0]["VorhKennzReserv"];
                ZBII_ALT_NEU = (Boolean)KopfTabelle.Rows[0]["ZBII_ALT_NEU"];
                Feinstaub = (Boolean)KopfTabelle.Rows[0]["Feinstaub"];
                ZulDate = KopfTabelle.Rows[0]["Zulassungsdatum"].ToString();
                if (IsDate(ZulDate)) { ZulDate = ((DateTime)KopfTabelle.Rows[0]["Zulassungsdatum"]).ToShortDateString(); }
                StillDate = KopfTabelle.Rows[0]["Still_Datum"].ToString();
                if (IsDate(StillDate)) { StillDate = ((DateTime)KopfTabelle.Rows[0]["Still_Datum"]).ToShortDateString(); }
                Kennzeichen = KopfTabelle.Rows[0]["Kennzeichen"].ToString();
                WunschKZ2 = KopfTabelle.Rows[0]["WunschKZ2"].ToString();
                WunschKZ3 = KopfTabelle.Rows[0]["WunschKZ3"].ToString();
                OhneGruenenVersSchein = XToBool(KopfTabelle.Rows[0]["OhneGruenenVersSchein"].ToString());
                Bemerkung = KopfTabelle.Rows[0]["Bemerkung"].ToString();
                KennzForm = KopfTabelle.Rows[0]["KennzForm"].ToString();
                EinKennz = (Boolean)KopfTabelle.Rows[0]["EinKennz"];
                NrLangText = KopfTabelle.Rows[0]["Langtextnr"].ToString();
                LangText = "";
                TuvAu = KopfTabelle.Rows[0]["Tuev_AU"].ToString();
                Haltedauer = KopfTabelle.Rows[0]["Haltedauer"].ToString();
                if (IsDate(Haltedauer)) { Haltedauer = ((DateTime)KopfTabelle.Rows[0]["Haltedauer"]).ToShortDateString(); }
                saved = (Boolean)KopfTabelle.Rows[0]["saved"];
                toDelete = KopfTabelle.Rows[0]["toDelete"].ToString();

                VkKurz = KopfTabelle.Rows[0]["VKKurz"].ToString();
                Notiz = KopfTabelle.Rows[0]["KundenNotiz"].ToString();
                InternRef = KopfTabelle.Rows[0]["interneRef"].ToString();

                bearbeitet = (Boolean)KopfTabelle.Rows[0]["bearbeitet"];
                Vorgang = KopfTabelle.Rows[0]["Vorgang"].ToString();

                SWIFT = KopfTabelle.Rows[0]["SWIFT"].ToString();
                IBAN = KopfTabelle.Rows[0]["IBAN"].ToString();
                Bankkey = KopfTabelle.Rows[0]["Bankkey"].ToString();
                Kontonr = KopfTabelle.Rows[0]["Kontonr"].ToString();
                Inhaber = KopfTabelle.Rows[0]["Inhaber"].ToString();
                Geldinstitut = KopfTabelle.Rows[0]["Geldinstitut"].ToString();
                EinzugErm = (Boolean)KopfTabelle.Rows[0]["EinzugErm"];
                Rechnung = (Boolean)KopfTabelle.Rows[0]["Rechnung"];
                Barzahlung = (Boolean)KopfTabelle.Rows[0]["Barzahlung"];
                
                Positionen = new DataTable();
                Positionen = tmpPos;
                NrMaterial = Positionen.Rows[0]["Matnr"].ToString();
                Material = Positionen.Rows[0]["Matbez"].ToString();
                
                Kundenadresse = new DataTable();
                Kundenadresse = tmpKunde;
                KundennrWE = Kundenadresse.Rows[0]["Kundennr"].ToString();
                Name1 = Kundenadresse.Rows[0]["Name1"].ToString();
                Name2 = Kundenadresse.Rows[0]["Name2"].ToString();
                PLZ = Kundenadresse.Rows[0]["PLZ"].ToString();
                Ort = Kundenadresse.Rows[0]["Ort"].ToString();
                Strasse = Kundenadresse.Rows[0]["Strasse"].ToString();
            }
            catch (Exception ex)
            {
                m_strMessage = ex.Message;
            }         
        }

        /// <summary>
        /// Laden eines Vorgange anhand der ID für die Eingabemasken
        /// Speicherung der Records in einem Dataset
        /// Aufgerufen von LoadDB_ZLDRecordset()
        /// </summary>
        /// <param name="RecordID">ID SQL</param>
        /// <param name="ds">Dataset mit den Tabellen ZLDKopfTabelle, ZLDPositionsTabelle, ZLDKundenadresse</param>
        private void FillDataSet(Int32 RecordID, ref DataSet ds)
        {
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection();

            connection.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"].ToString();
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
            System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();

            connection.Open();

            string selectString = "SELECT * FROM ZLDKopfTabelle as KopfTabelle where id=" + RecordID;
            if (!SendForAll)
            {
                selectString += " AND id_user=" + m_objUser.UserID;
            }
            selectString += "; " + "SELECT * FROM ZLDPositionsTabelle As PositionsTabelle where id_Kopf=" + RecordID +
                            "; " + "SELECT * FROM ZLDKundenadresse as Kundenadresse where id_Kopf=" + RecordID + "; ";

            adapter = new System.Data.SqlClient.SqlDataAdapter(selectString, connection);

            adapter.TableMappings.Add("ZLDKopfTabelle", "KopfTabelle");
            adapter.TableMappings.Add("ZLDPositionsTabelle", "PositionsTabelle");
            adapter.TableMappings.Add("ZLDKundenadresse", "Kundenadresse");
            adapter.Fill(ds);
            DataTable tmpKopf, tmpPos, tmpAdresse;
            tmpKopf = ds.Tables[0];
            tmpPos = ds.Tables[1];
            tmpAdresse = ds.Tables[2];

            connection.Close();

        }

        /// <summary>
        /// Laden der in der SQL-Tabelle angelegten Vorgänge für die Übersicht(Aufträge.aspx)
        /// </summary>
        public void LadeVorerfassungDB_ZLD()
        {
            m_intStatus = 0;
            m_strMessage = "";
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection();
            connection.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"].ToString();
            try
            {
                tblEingabeListe = new DataTable();

                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();


                command.CommandText =
                    " SELECT     dbo.ZLDKopfTabelle.*, dbo.ZLDPositionsTabelle.id_pos, dbo.ZLDPositionsTabelle.Matnr, dbo.ZLDPositionsTabelle.Matbez" +
                    " FROM         dbo.ZLDKopfTabelle " +
                    " INNER JOIN dbo.ZLDPositionsTabelle ON dbo.ZLDKopfTabelle.id = dbo.ZLDPositionsTabelle.id_Kopf " +
                    " INNER JOIN dbo.WebMember ON dbo.ZLDKopfTabelle.id_user = dbo.WebMember.UserID ";
                if (SendForAll)
                {
                    // Für Benutzer, deren Organisation das Tag "SendForAll" enthält, alle erfassten Aufträge des VkBurs bzw. der Gruppe anzeigen
                    command.CommandText += " WHERE Filiale = @filiale AND GroupID = @GroupID AND testuser = @testuser AND abgerechnet = 0";
                    command.Parameters.AddWithValue("@filiale", m_objUser.Reference);
                    command.Parameters.AddWithValue("@GroupID", m_objUser.GroupID);
                }
                else
                {
                    command.CommandText += " WHERE id_User = @id_User AND testuser = @testuser AND abgerechnet = 0";
                    command.Parameters.AddWithValue("@id_User", m_objUser.UserID);
                }
                command.Parameters.AddWithValue("@testuser", m_objUser.IsTestUser);
                command.CommandText += " ORDER BY dbo.ZLDKopfTabelle.id_sap asc,dbo.ZLDKopfTabelle.kundenname, dbo.ZLDPositionsTabelle.id_pos, referenz1 asc, Kennzeichen";
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                adapter.SelectCommand = command;
                adapter.Fill(tblEingabeListe);

                tblEingabeListe.Columns.Add("Status", typeof(String));
                foreach (DataRow rowListe in tblEingabeListe.Rows)
                {
                    rowListe["Status"] = "";
                }
                if (tblEingabeListe.Rows.Count == 0)
                {
                    m_intStatus = 9999;
                    m_strMessage = "Keine Daten gefunden!";
                }
                else
                {
                    command.Parameters.Clear();

                    if (SendForAll)
                    {
                        command.CommandText = "SELECT Count (ID) FROM dbo.ZLDKopfTabelle" +
                            " INNER JOIN dbo.WebMember ON dbo.ZLDKopfTabelle.id_user = dbo.WebMember.UserID " +
                            " WHERE Filiale = @filiale AND GroupID = @GroupID AND testuser = @testuser";
                        command.Parameters.AddWithValue("@filiale", m_objUser.Reference);
                        command.Parameters.AddWithValue("@GroupID", m_objUser.GroupID);
                    }
                    else
                    {
                        command.CommandText = "SELECT Count (ID) FROM dbo.ZLDKopfTabelle" +
                            " WHERE id_User = @id_User AND testuser = @testuser";
                        command.Parameters.AddWithValue("@id_User", m_objUser.UserID);
                    }
                    command.Parameters.AddWithValue("@testuser", m_objUser.IsTestUser);
                    command.CommandType = CommandType.Text;
                    IDCount = command.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                m_intStatus = 9999;
                m_strMessage = "Fehler beim Laden der Eingabeliste: " + ex.Message;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Speichern der markierten Daten(toSave=1) in SAP aus den SQL-Tabellen
        /// Bapi Z_ZLD_AH_IMPORT_ERFASSUNG1
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Aufträge.aspx</param>
        /// <param name="tblStvaStamm">Stvastamm zur Ergänzung der Daten(KREISBEZ)</param>
        public void SaveZLDVorerfassung(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblStvaStamm)
        {
            m_strClassAndMethod = "VoerfZLD.SaveZLDVorerfassung";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            tblFehler = null;
            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                dboZLDAutohausDataContext ZLD_DataContext = new dboZLDAutohausDataContext();
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_IMPORT_ERFASSUNG1", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_TELNR", m_objUser.PhoneEmployee);

                    DataTable importAuftrag = myProxy.getImportTable("GT_BAK_IN");
                    DataTable importPos = myProxy.getImportTable("GT_POS_IN");
                    DataTable importAdresse = myProxy.getImportTable("GT_ADRS_IN");

                    Int64 LastID = 0;

                    foreach (DataRow SaveRow in tblEingabeListe.Rows)
                    {
                        if (SaveRow["toSave"].ToString() == "1")
                        {
                            Int64 tmpID = (Int64)SaveRow["id"];
                            if (LastID != tmpID)
                            {
                                var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                                               where k.id == tmpID
                                               select k).Single();
                                String isCPD = "";
                                DataRow importRowAuftrag = importAuftrag.NewRow();
                                importRowAuftrag["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                importRowAuftrag["VKORG"] = VKORG;
                                importRowAuftrag["VKBUR"] = VKBUR;
                                importRowAuftrag["BLTYP"] = tblKopf.Vorgang;
                                importRowAuftrag["KUNNR"] = tblKopf.kundennr.PadLeft(10, '0');
                                importRowAuftrag["ZZREFNR1"] = tblKopf.referenz1;
                                importRowAuftrag["ZZREFNR2"] = tblKopf.referenz2;
                                importRowAuftrag["ZZREFNR3"] = tblKopf.referenz3;
                                importRowAuftrag["ZZREFNR4"] = tblKopf.referenz4;
                                importRowAuftrag["KREISKZ"] = tblKopf.KreisKZ;
                                DataRow[] RowStva = tblStvaStamm.Select("KREISKZ='" + tblKopf.KreisKZ + "'");
                                if (RowStva.Length == 1)
                                {
                                    importRowAuftrag["KREISBEZ"] = RowStva[0]["KREISBEZ"];
                                }
                                else
                                {
                                    importRowAuftrag["KREISBEZ"] = tblKopf.KreisBez;
                                }
                                importRowAuftrag["WUNSCHKENN_JN"] = BoolToX((Boolean)tblKopf.WunschKenn);
                                importRowAuftrag["RESERVKENN_JN"] = BoolToX((Boolean)tblKopf.Reserviert);
                                importRowAuftrag["RESERVKENN"] = tblKopf.ReserviertKennz;
                                importRowAuftrag["FEINSTAUB"] = BoolToX((Boolean)tblKopf.Feinstaub);
                                importRowAuftrag["ZZZLDAT"] = tblKopf.Zulassungsdatum;
                                importRowAuftrag["ZZKENN"] = tblKopf.Kennzeichen;
                                importRowAuftrag["WU_KENNZ2"] = tblKopf.WunschKZ2;
                                importRowAuftrag["WU_KENNZ3"] = tblKopf.WunschKZ3;
                                importRowAuftrag["O_G_VERSSCHEIN"] = tblKopf.OhneGruenenVersSchein;

                                importRowAuftrag["KENNZTYP"] = "";
                                importRowAuftrag["KENNZFORM"] = tblKopf.KennzForm;
                                importRowAuftrag["EINKENN_JN"] = BoolToX((Boolean)tblKopf.EinKennz);
                                importRowAuftrag["BEMERKUNG"] = tblKopf.Bemerkung;
                                importRowAuftrag["VK_KUERZEL"] = tblKopf.VKKurz;
                                importRowAuftrag["KUNDEN_REF"] = tblKopf.interneRef;
                                importRowAuftrag["KUNDEN_NOTIZ"] = tblKopf.KundenNotiz;
                                importRowAuftrag["KENNZ_VH"] = BoolToX((Boolean)tblKopf.KennzVH);
                               
                                importRowAuftrag["ALT_KENNZ"] = tblKopf.KennzAlt;
                                importRowAuftrag["ZBII_ALT_NEU"] = tblKopf.ZBII_ALT_NEU == true ? importRowAuftrag["ZBII_ALT_NEU"] = "N" : importRowAuftrag["ZBII_ALT_NEU"] = "A";
                                // importRowAuftrag["ZBII_ALT_NEU"] = BoolToX((Boolean)tblKopf.ZBII_ALT_NEU);
                                importRowAuftrag["VH_KENNZ_RES"] = BoolToX((Boolean)tblKopf.VorhKennzReserv);
                                if (IsDate(tblKopf.Still_Datum.ToString()))
                                {
                                    importRowAuftrag["STILL_DAT"] = tblKopf.Still_Datum;
                                }

                                importRowAuftrag["MNRESW"] = tblKopf.MussResWerden;
                                importRowAuftrag["ZZEVB"] = tblKopf.EVB;
                                importRowAuftrag["KENNZ_UEBERNAHME"] = tblKopf.KennzUebernahme;
                                importRowAuftrag["SERIE"] = tblKopf.Serie;
                                importRowAuftrag["SAISON_KNZ"] = tblKopf.SaisonKZ;
                                importRowAuftrag["SAISON_BEG"] = tblKopf.SaisonBeg;
                                importRowAuftrag["SAISON_END"] = tblKopf.SaisonEnd;
                                importRowAuftrag["ZUSKENNZ"] = tblKopf.ZusatzKZ;
                                importRowAuftrag["TUEV_AU"] = tblKopf.Tuev_AU;
                                importRowAuftrag["KURZZEITVS"] = tblKopf.Kurzzeitkennz;
                                importRowAuftrag["ZOLLVERS"] = tblKopf.ZollVers;
                                importRowAuftrag["ZOLLVERS_DAUER"] = tblKopf.ZollVers_Dauer;
                                importRowAuftrag["FAHRZ_ART"] = tblKopf.Fahrz_Art;
                                importRowAuftrag["VORFUEHR"] = tblKopf.Vorfuehrung;
                                importRowAuftrag["LTEXT_NR"] = tblKopf.Langtextnr;
                                //  TODO !!! 
                                if (IsDate(tblKopf.Haltedauer.ToString()))
                                {
                                    importRowAuftrag["HALTE_DAUER"] = tblKopf.Haltedauer;
                                }
                                //  TODO !!! 

                                // ggf. Original-Erfassername erhalten
                                if ((SendForAll) && (!String.IsNullOrEmpty(tblKopf.Vorerfasser)))
                                {
                                    importRowAuftrag["VE_ERNAM"] = tblKopf.Vorerfasser;
                                }
                                else
                                {
                                    importRowAuftrag["VE_ERNAM"] = m_objUser.UserName;
                                }
                                
                                if (tblKopf.SWIFT != null) { importRowAuftrag["SWIFT"] = tblKopf.SWIFT; }
                                if (tblKopf.IBAN != null) { importRowAuftrag["IBAN"] = tblKopf.IBAN; }
                                if (tblKopf.BankKey != null) { importRowAuftrag["BANKL"] = tblKopf.BankKey; }
                                if (tblKopf.Kontonr != null) { importRowAuftrag["BANKN"] = tblKopf.Kontonr; }
                                if (tblKopf.Geldinstitut != null) { importRowAuftrag["EBPP_ACCNAME"] = tblKopf.Geldinstitut; }
                                if (tblKopf.Inhaber != null) { importRowAuftrag["KOINH"] = tblKopf.Inhaber; }
                                if (tblKopf.EinzugErm != null) { importRowAuftrag["EINZ_JN"] = BoolToX((Boolean)tblKopf.EinzugErm); }
                                if (tblKopf.Rechnung != null) { importRowAuftrag["RECH_JN"] = BoolToX((Boolean)tblKopf.Rechnung); }
                                if (tblKopf.Barzahlung != null) { importRowAuftrag["BARZ_JN"] = BoolToX((Boolean)tblKopf.Barzahlung); }

                                importRowAuftrag["BEB_STATUS"] = "1";

                                DataRow importRow;

                                //----------------


                                var tblPosCount = (from p in ZLD_DataContext.ZLDPositionsTabelle
                                                   where p.id_Kopf == tmpID
                                                   select p);

                                Int32 ROWCOUNT = 10;
                                foreach (var PosRow in tblPosCount)
                                {   importRow = importPos.NewRow();
                                    importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                    importRow["LFDNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
                                    importRow["MENGE"] = PosRow.Menge != "" ? importRow["MENGE"] = PosRow.Menge : importRow["MENGE"] = "1";
                                    //if (tblKopf.ZBII_ALT_NEU == true && PosRow.Matnr == "573")
                                    //{
                                    //    importRowAuftrag["KREISKZ"] = "3";
                                    //}
                                    importRow["MATNR"] = PosRow.Matnr.PadLeft(18, '0'); 
                                    importPos.Rows.Add(importRow);
                                    ROWCOUNT += 10;

                                }


                                importAuftrag.Rows.Add(importRowAuftrag);

                                var tblKunnadresse = (from k in ZLD_DataContext.ZLDKundenadresse
                                                      where k.id_Kopf == tmpID
                                                      select k).Single();
                                if (tblKunnadresse.Name1 != null)
                                {
                                    if (tblKunnadresse.Name1.Length > 0)
                                    {
                                        importRow = importAdresse.NewRow();

                                        importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                        if (tblKunnadresse.Name1 != null) { importRow["NAME1"] = tblKunnadresse.Name1; }
                                        if (tblKunnadresse.Name2 != null) { importRow["NAME2"] = tblKunnadresse.Name2; }
                                        if (tblKunnadresse.PLZ != null) { importRow["PLZ"] = tblKunnadresse.PLZ; }
                                        if (tblKunnadresse.Ort != null) { importRow["CITY1"] = tblKunnadresse.Ort; }
                                        if (tblKunnadresse.Strasse != null) { importRow["STREET"] = tblKunnadresse.Strasse; }

                                        importAdresse.Rows.Add(importRow);
                                    }
                                }

                                LastID = tmpID;
                            }
                        }
                    }

                    myProxy.callBapi();


                    tblFehler = myProxy.getExportTable("GT_ERROR");
                    tblPrint = myProxy.getExportTable("GT_FILENAME");

                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    m_strMessage = sapMessage;


                    if (tblFehler.Rows.Count > 0)
                    {

                        m_intStatus = -9999;
                        m_strMessage = "Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden";

                        foreach (DataRow rowError in tblFehler.Rows)
                        {
                            Int32 id_sap;
                            Int32.TryParse(rowError["ZULBELN"].ToString(), out id_sap);
                            Int32 id_Pos;
                            Int32.TryParse(rowError["LFDNR"].ToString(), out id_Pos);
                            DataRow[] rowListe = tblEingabeListe.Select("id_sap=" + id_sap );
                            if (rowListe.Length == 1)
                            {
                                rowListe[0]["Status"] = rowError["MESSAGE"];
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
                            m_strMessage = m_strMessage = "Es ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally
                {
                    m_blnGestartet = false;

                    if (ZLD_DataContext != null)
                    {
                        if (ZLD_DataContext.Connection.State == ConnectionState.Open)
                        {
                            ZLD_DataContext.Connection.Close();
                            ZLD_DataContext.Dispose();
                        }
                    }
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
        /// <param name="VKORG">Verkaufsorganisation</param>
        /// <param name="VKBUR">Verkaufsbüro</param>
        public void GetAbmeldungAH(String strAppID, String strSessionID, System.Web.UI.Page page, String VKORG, String VKBUR)
        {

            m_strClassAndMethod = "ZLD_Suche.GetAbmeldungAH";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_AF_ABM_LISTE", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKORG", VKORG);
                    myProxy.setImportParameter("I_VKBUR", VKBUR);
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
                        default:
                            m_intStatus = -9999;
                            m_strMessage = m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
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
        public void SaveAbmeldungAH(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "ZLD_Suche.GetAbmeldungAH";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
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
                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    m_intStatus = subrc;
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    m_strMessage = sapMessage;
                    if (SAPReturn.Rows.Count > 0)
                    {
                        foreach (DataRow rowError in SAPReturn.Rows)
                        {
                            Int32 id_sap;
                            Int32.TryParse(rowError["ZULBELN"].ToString(), out id_sap);
                            DataRow[] rowListe = Abmeldedaten.Select("ZULBELN=" + id_sap);
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
                            m_strMessage = m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Vorläufigen Vorgang an SAP geben, um dort das Abrechnungsformular für den Endkunden zu generieren
        /// Bapi Z_ZLD_AH_IMPORT_ERFASSUNG1
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Aufträge.aspx</param>
        /// <param name="tblStvaStamm">Stvastamm zur Ergänzung der Daten(KREISBEZ)</param>
        public void CreateKundenformular(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblStvaStamm)
        {
            m_strClassAndMethod = "AHErfassung.CreateKundenformular";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            tblFehler = null;
            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_IMPORT_ERFASSUNG1", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_SIMULATION", "X"); // wichtig!

                    DataTable importAuftrag = myProxy.getImportTable("GT_BAK_IN");
                    DataTable importPos = myProxy.getImportTable("GT_POS_IN");
                    DataTable importAdresse = myProxy.getImportTable("GT_ADRS_IN");

                    DataRow importRowAuftrag = importAuftrag.NewRow();
                    importRowAuftrag["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
                    importRowAuftrag["VKORG"] = VKORG;
                    importRowAuftrag["VKBUR"] = VKBUR;
                    importRowAuftrag["BLTYP"] = Vorgang;
                    importRowAuftrag["KUNNR"] = Kunnr;
                    importRowAuftrag["ZZREFNR1"] = Ref1;
                    importRowAuftrag["ZZREFNR2"] = Ref2;
                    importRowAuftrag["ZZREFNR3"] = Ref3;
                    importRowAuftrag["ZZREFNR4"] = Ref4;
                    importRowAuftrag["KREISKZ"] = KreisKennz;
                    DataRow[] RowStva = tblStvaStamm.Select("KREISKZ='" + KreisKennz + "'");
                    if (RowStva.Length == 1)
                    {
                        importRowAuftrag["KREISBEZ"] = RowStva[0]["KREISBEZ"];
                    }
                    else
                    {
                        importRowAuftrag["KREISBEZ"] = Kreis;
                    }
                    importRowAuftrag["WUNSCHKENN_JN"] = BoolToX(WunschKenn);
                    importRowAuftrag["RESERVKENN_JN"] = BoolToX(Reserviert);
                    importRowAuftrag["RESERVKENN"] = ReserviertKennz;
                    importRowAuftrag["FEINSTAUB"] = BoolToX(Feinstaub);
                    importRowAuftrag["ZZZLDAT"] = ZulDate;
                    importRowAuftrag["ZZKENN"] = Kennzeichen;
                    importRowAuftrag["WU_KENNZ2"] = WunschKZ2;
                    importRowAuftrag["WU_KENNZ3"] = WunschKZ3;
                    importRowAuftrag["O_G_VERSSCHEIN"] = BoolToX(OhneGruenenVersSchein);

                    importRowAuftrag["KENNZTYP"] = "";
                    importRowAuftrag["KENNZFORM"] = KennzForm;
                    importRowAuftrag["EINKENN_JN"] = BoolToX(EinKennz);
                    importRowAuftrag["BEMERKUNG"] = Bemerkung;
                    importRowAuftrag["VK_KUERZEL"] = VkKurz;
                    importRowAuftrag["KUNDEN_REF"] = InternRef;
                    importRowAuftrag["KUNDEN_NOTIZ"] = Notiz;
                    importRowAuftrag["KENNZ_VH"] = BoolToX(KennzVorhanden);

                    importRowAuftrag["ALT_KENNZ"] = Altkenn;
                    importRowAuftrag["ZBII_ALT_NEU"] = (ZBII_ALT_NEU ? "N" : "A");
                    importRowAuftrag["VH_KENNZ_RES"] = BoolToX(VorhKennzReserv);
                    if (IsDate(StillDate))
                    {
                        importRowAuftrag["STILL_DAT"] = StillDate;
                    }

                    importRowAuftrag["MNRESW"] = BoolToX(MussReserviert);
                    importRowAuftrag["ZZEVB"] = EVB;
                    importRowAuftrag["KENNZ_UEBERNAHME"] = BoolToX(KennzUebernahme);
                    importRowAuftrag["SERIE"] = BoolToX(Serie);
                    importRowAuftrag["SAISON_KNZ"] = BoolToX(Saison);
                    importRowAuftrag["SAISON_BEG"] = SaisonBeg;
                    importRowAuftrag["SAISON_END"] = SaisonEnd;
                    importRowAuftrag["ZUSKENNZ"] = BoolToX(ZusatzKZ);
                    importRowAuftrag["TUEV_AU"] = TuvAu;
                    importRowAuftrag["KURZZEITVS"] = KurzZeitKennz;
                    importRowAuftrag["ZOLLVERS"] = ZollVers;
                    importRowAuftrag["ZOLLVERS_DAUER"] = ZollVersDauer;
                    importRowAuftrag["FAHRZ_ART"] = Fahrzeugart;
                    importRowAuftrag["VORFUEHR"] = Vorfuehr;
                    importRowAuftrag["LTEXT_NR"] = NrLangText;

                    if (IsDate(Haltedauer))
                    {
                        importRowAuftrag["HALTE_DAUER"] = Haltedauer;
                    }

                    importRowAuftrag["VE_ERNAM"] = m_objUser.UserName;

                    if (SWIFT != null) { importRowAuftrag["SWIFT"] = SWIFT; }
                    if (IBAN != null) { importRowAuftrag["IBAN"] = IBAN; }
                    if (Bankkey != null) { importRowAuftrag["BANKL"] = Bankkey; }
                    if (Kontonr != null) { importRowAuftrag["BANKN"] = Kontonr; }
                    if (Geldinstitut != null) { importRowAuftrag["EBPP_ACCNAME"] = Geldinstitut; }
                    if (Inhaber != null) { importRowAuftrag["KOINH"] = Inhaber; }
                    importRowAuftrag["EINZ_JN"] = BoolToX(EinzugErm);
                    importRowAuftrag["RECH_JN"] = BoolToX(Rechnung);
                    importRowAuftrag["BARZ_JN"] = BoolToX(Barzahlung);

                    importRowAuftrag["BEB_STATUS"] = "1";

                    importAuftrag.Rows.Add(importRowAuftrag);

                    DataRow importRow;

                    importRow = importPos.NewRow();
                    importRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
                    importRow["LFDNR"] = "1".PadLeft(6, '0');
                    importRow["MENGE"] = "1";
                    importRow["MATNR"] = NrMaterial;
                    importPos.Rows.Add(importRow);

                    if (!String.IsNullOrEmpty(Name1))
                    {
                        importRow = importAdresse.NewRow();

                        importRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
                        if (Name1 != null) { importRow["NAME1"] = Name1; }
                        if (Name2 != null) { importRow["NAME2"] = Name2; }
                        if (PLZ != null) { importRow["PLZ"] = PLZ; }
                        if (Ort != null) { importRow["CITY1"] = Ort; }
                        if (Strasse != null) { importRow["STREET"] = Strasse; }

                        importAdresse.Rows.Add(importRow);
                    }

                    myProxy.callBapi();

                    KundenformularPDF = myProxy.getExportParameterByte("E_PDF");

                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out subrc);
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE");
                    m_strMessage = sapMessage;

                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -5555;
                            m_strMessage = "Beim Erstellen des Kundenformulars ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally
                {
                    m_blnGestartet = false;
                }
            }
        }

        #endregion
        
        #region Helper

        public static string toShortDateStr(string dat)
        {
            DateTime datum = default(DateTime);

            try
            {
                datum = Convert.ToDateTime(dat.Substring(0, 2) + "." + dat.Substring(2, 2) + "." + DateTime.Now.Year.ToString().Substring(0, 2) + dat.Substring(4, 2));
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            return datum.ToShortDateString();
        }

        public static bool IsDate(String inValue)
        {
            bool result;
            DateTime myDT;

            try
            {
                result = DateTime.TryParse(inValue, out myDT);

            }
            catch (FormatException e)
            {
                result = false;
            }

            return result;
        }

        public static bool IsNumeric(string Value)
        {
            try
            {
                Convert.ToInt32(Value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static String BoolToX(Boolean inValue)
        {
            return (inValue ? "X" : "");
        }

        public static Boolean XToBool(String inValue)
        {
            return (inValue == "X");
        } 

        #endregion

    }
}