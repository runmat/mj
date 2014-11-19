using System;
using System.Linq;
using CKG.Base.Common;
using System.Data;
using CKG.Base.Business;
using System.Configuration;
using System.Data.SqlClient;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Klasse für die Nacherfassung, Nacherfassung beauftragter und durchzuführende Versandzulassungen.
    /// </summary>
    public class NacherfZLD : DatenimportBase
    {
        #region "Declarations"

        DataTable tblListe;
        DataTable tblErrors;

        DataTable KopfTabelle;
        DataTable Bankverbindung;
        DataTable Kundenadresse;

        String strVKORG;
        String strVKBUR;

        // Kopftabelle
        Int32 id_Kopf;
        Int32 id_sap;
        String kundenname;
        String kundennr;
        String Referenz1;
        String Referenz2;
        String KreisKZ;
        String KreisBez;
        Boolean WunschKenn;
        Boolean mReserviert;
        String mReserviertKennz;
        Boolean mFeinstaub;
        String mZulDate;
        String mKennzeichen;
        String mKennztyp;
        String mKennzForm;
        Int32 mKennzAnz;
        Boolean mEinKennz;
        String mBemerkung;
        Boolean mEC;
        Boolean mBar;
        String mPauschal;
        // Positionstabelle

        DataTable tblPositionen;

        // Filter
        String mSelMatnr;
        String mSelID;
        String mSelDatum;
        String mSelKunde;
        String mSelKreis;

        #endregion

        #region "Properties"
        /// <summary>
        /// Tabelle zur Anzeige der erfassten Daten
        /// </summary>
        public DataTable tblEingabeListe
        {
            get { return tblListe; }
            set { tblListe = value; }
        }
        /// <summary>
        /// Fehlertabelle SAP
        /// </summary>
        public DataTable tblFehler
        {
            get { return tblErrors; }
            set { tblErrors = value; }
        }
        /// <summary>
        /// Verkaufsorganisation
        /// </summary>
        public String VKORG
        {
            get { return strVKORG; }
            set { strVKORG = value; }
        }
        /// <summary>
        /// Verkaufsbüro
        /// </summary>
        public String VKBUR
        {
            get { return strVKBUR; }
            set { strVKBUR = value; }
        }
        /// <summary>
        /// Positionstabelle
        /// </summary>
        public DataTable Positionen
        {
            get { return tblPositionen; }
            set { tblPositionen = value; }
        }
        /// <summary>
        /// ZUBELN
        /// </summary>
        public Int32 SapID
        {
            get { return id_sap; }
            set { id_sap = value; }
        }
        /// <summary>
        /// KopfID SQL
        /// </summary>
        public Int32 KopfID
        {
            get { return id_Kopf; }
            set { id_Kopf = value; }
        }
        /// <summary>
        /// Flag abgerechnet SQL
        /// </summary>
        public Boolean abgerechnet
        {
            get;
            set;
        }
        /// <summary>
        /// Name des Kunden
        /// </summary>
        public String Kundenname
        {
            get { return kundenname; }
            set { kundenname = value; }
        }
        /// <summary>
        /// Kundennummer
        /// </summary>
        public String Kunnr
        {
            get { return kundennr; }
            set { kundennr = value; }
        }
        /// <summary>
        /// Kundennummer für die Preisfindung
        /// </summary>
        public String KunnrPreis
        {
            get;
            set;
        }
        /// <summary>
        /// Referenz1
        /// </summary>
        public String Ref1
        {
            get { return Referenz1; }
            set { Referenz1 = value; }
        }
        /// <summary>
        /// Refernenz2
        /// </summary>
        public String Ref2
        {
            get { return Referenz2; }
            set { Referenz2 = value; }
        }
        /// <summary>
        /// Kreiskennzeichen/StVa
        /// </summary>
        public String KreisKennz
        {
            get { return KreisKZ; }
            set { KreisKZ = value; }
        }
        /// <summary>
        /// Kreisbezeichnung
        /// </summary>
        public String Kreis
        {
            get { return KreisBez; }
            set { KreisBez = value; }
        }
        /// <summary>
        /// Wunschkennzeichen erwünscht!?
        /// </summary>
        public Boolean WunschKennz
        {
            get { return WunschKenn; }
            set { WunschKenn = value; }
        }
        /// <summary>
        /// Wunschkennzeichen reserviert!?
        /// </summary>
        public Boolean Reserviert
        {
            get { return mReserviert; }
            set { mReserviert = value; }
        }
        /// <summary>
        /// Reservierungsnummer
        /// </summary>
        public String ReserviertKennz
        {
            get { return mReserviertKennz; }
            set { mReserviertKennz = value; }
        }
        /// <summary>
        /// Feinstaubplakette ja/nein
        /// </summary>
        public Boolean Feinstaub
        {
            get { return mFeinstaub; }
            set { mFeinstaub = value; }
        }
        /// <summary>
        /// Zulassungsdatum
        /// </summary>
        public String ZulDate
        {
            get { return mZulDate; }
            set { mZulDate = value; }
        }
        /// <summary>
        /// Kennzeichen komplett
        /// </summary>
        public String Kennzeichen
        {
            get { return mKennzeichen; }
            set { mKennzeichen = value; }
        }
        /// <summary>
        /// Kennzeichentyp(EURO/Fun etc.)
        /// </summary>
        public String Kennztyp
        {
            get { return mKennztyp; }
            set { mKennztyp = value; }
        }
        /// <summary>
        /// Kennzeichengröße
        /// </summary>
        public String KennzForm
        {
            get { return mKennzForm; }
            set { mKennzForm = value; }
        }
        /// <summary>
        /// Anzahl der Kennzeichen
        /// </summary>
        public Int32 KennzAnzahl
        {
            get { return mKennzAnz; }
            set { mKennzAnz = value; }
        }
        /// <summary>
        /// nur ein Kennzeichen erwünscht!?
        /// </summary>
        public Boolean EinKennz
        {
            get { return mEinKennz; }
            set { mEinKennz = value; }
        }
        /// <summary>
        /// Bemerkung
        /// </summary>
        public String Bemerkung
        {
            get { return mBemerkung; }
            set { mBemerkung = value; }
        }
        /// <summary>
        /// Barcode
        /// </summary>
        public String Barcode
        {
            get;
            set;
        }
        /// <summary>
        /// EC-Zahlung
        /// </summary>
        public Boolean EC
        {
            get { return mEC; }
            set { mEC = value; }
        }
        /// <summary>
        /// Barzahlung
        /// </summary>
        public Boolean Bar
        {
            get { return mBar; }
            set { mBar = value; }
        }
        /// <summary>
        /// per Rechnung
        /// </summary>
        public Boolean RE
        {
            get;
            set;
        }
        /// <summary>
        /// Datensatz bereits angelegt
        /// </summary>
        public Boolean saved
        {
            get;
            set;
        }

        /// <summary>
        /// Datensatz wurde bearbeitet
        /// </summary>
        public Boolean bearbeitet
        {
            get;
            set;
        }
        /// <summary>
        /// Vorgang z.B. Vorerfassung(VE), Nacherfassung(NZ), Versandzulassung(VZ)
        /// </summary>
        public String Vorgang
        {
            get;
            set;
        }
        /// <summary>
        /// Datensatz zum Speichern markiert
        /// </summary>
        public Int16 toSave
        {
            get;
            set;
        }

        /// <summary>
        /// Datensatz zum Löschen markiert
        /// </summary>
        public String toDelete
        {
            get;
            set;
        }
        /// <summary>
        /// zu leistende Steuer
        /// </summary>
        public Decimal Steuer
        {
            get;
            set;
        }
        /// <summary>
        /// Kennzeichenpreis
        /// </summary>
        public Decimal PreisKennz
        {
            get;
            set;
        }
        /// <summary>
        /// Materialnummer zur Selektion Nacherfassungsdaten
        /// </summary>
        public String SelMatnr
        {
            get { return mSelMatnr; }
            set { mSelMatnr = value; }
        }
        /// <summary>
        /// Zulassungsdatum zur Selektion Nacherfassungsdaten
        /// </summary>
        public String SelDatum
        {
            get { return mSelDatum; }
            set { mSelDatum = value; }
        }
        /// <summary>
        /// SAPID/ZUBELN zur Selektion Nacherfassungsdaten
        /// </summary>
        public String SelID
        {
            get { return mSelID; }
            set { mSelID = value; }
        }
        /// <summary>
        /// Kundennummer zur Selektion Nacherfassungsdaten
        /// </summary>
        public String SelKunde
        {
            get { return mSelKunde; }
            set { mSelKunde = value; }
        }
        /// <summary>
        /// Zulassungskreis zur Selektion Nacherfassungsdaten
        /// </summary>
        public String SelKreis
        {
            get { return mSelKreis; }
            set { mSelKreis = value; }
        }
        /// <summary>
        /// Lieferantennummer zur Selektion Nacherfassungsdaten
        /// </summary>
        public String SelLief
        {
            get;
            set;
        }
        /// <summary>
        /// Kunde ist Pauschalkunde
        /// </summary>
        public String PauschalKunde
        {
            get { return mPauschal; }
            set { mPauschal = value; }

        }
        /// <summary>
        /// Kundeaufträge mit/ohne Steuer berechnet
        /// </summary>
        public String OhneSteuer
        {
            get;
            set;

        }
        /// <summary>
        /// zuständige Zulassungsdienste Versandzulassung
        /// </summary>
        public DataTable BestLieferanten
        {
            get;
            set;
        }
        /// <summary>
        /// zuständiger Zulassungsdienst Versandzulassung
        /// </summary>
        public String Lieferant_ZLD
        {
            get;
            set;
        }
        /// <summary>
        /// Frachtnummer Hinsendung Versandzulassung
        /// </summary>
        public String FrachtNrHin
        {
            get;
            set;
        }
        /// <summary>
        /// Frachtnummer Rücksendung Versandzulassung
        /// </summary>
        public String FrachtNrBack
        {
            get;
            set;
        }
        /// <summary>
        /// Belegtyp SAP
        /// </summary>
        public String SelStatus
        {
            get;
            set;
        }
        /// <summary>
        /// zuständige Verkaufsbüro durchzuführende Versandzulassung
        /// </summary>
        public String DZVKBUR
        {
            get;
            set;
        }
        /// <summary>
        /// Kunde ist Barkunde ?!
        /// </summary>
        public Boolean Barkunde
        {
            get;
            set;
        }
        /// <summary>
        /// Positionstabelle für die Preisfindung(neu Positionen)
        /// </summary>
        public DataTable NewPosPreise
        {
            get;
            set;
        }
        /// <summary>
        /// Kundennummer Warenempfänger
        /// </summary>
        public String KundennrWE
        {
            get;
            set;
        }
        /// <summary>
        /// Partnerrolle (WE=Warenempfänger, AG = Auftrtaggeber)
        /// </summary>
        public String Partnerrolle
        {
            get;
            set;
        }
        /// <summary>
        /// Name des Kunden(abweichende Adressdaten)
        /// </summary>
        public String Name1
        {
            get;
            set;
        }
        /// <summary>
        /// Name des Kunden(abweichende Adressdaten)
        /// </summary>
        public String Name2
        {
            get;
            set;
        }
        /// <summary>
        /// Postleitzahl des Kunden(abweichende Adressdaten)
        /// </summary>
        public String PLZ
        {
            get;
            set;
        }
        /// <summary>
        /// Ort des Kunden(abweichende Adressdaten)
        /// </summary>
        public String Ort
        {
            get;
            set;
        }
        /// <summary>
        /// Strasse des Kunden(abweichende Adressdaten)
        /// </summary>
        public String Strasse
        {
            get;
            set;
        }
        /// <summary>
        /// SWIFT-BIC SAP
        /// </summary>
        public String SWIFT
        {
            get;
            set;
        }
        /// <summary>
        /// IBAN des Kunden
        /// </summary>
        public String IBAN
        {
            get;
            set;
        }
        /// <summary>
        /// Bankschlüssel SAP
        /// </summary>
        public String BankKey
        {
            get;
            set;
        }
        /// <summary>
        /// Kontonummer des Kunden
        /// </summary>
        public String Kontonr
        {
            get;
            set;
        }
        /// <summary>
        /// Inhaber der Firma(Kunde)
        /// </summary>
        public String Inhaber
        {
            get;
            set;
        }
        /// <summary>
        /// Geldinstitut des Kunden
        /// </summary>
        public String Geldinstitut
        {
            get;
            set;
        }
        /// <summary>
        /// Kunde gibt Einzugsermächtigung!?
        /// </summary>
        public Boolean EinzugErm
        {
            get;
            set;
        }
        /// <summary>
        /// Kunde zahlt per Rechnung
        /// </summary>
        public Boolean Rechnung
        {
            get;
            set;
        }
        /// <summary>
        /// erfasste Dienstleistung geändert(Hinweis Preisfindung)
        /// </summary>
        public Boolean ChangeMatnr
        {
            get;
            set;
        }
        /// <summary>
        /// Resulttabele SAP Nacherfassungsdaten
        /// </summary>
        public DataTable SapResultTable
        {
            get;
            set;
        }
        /// <summary>
        /// Amtsgebühr pflegen ja/nein(Filiale)
        /// </summary>
        public Boolean ShowGebAmt
        {
            get;
            set;
        }
        /// <summary>
        /// Anzahl anzuzeigener Datensätze in der Übersicht(Gridview)
        /// </summary>
        public int ListePageSize
        {
            get;
            set;

        }
        /// <summary>
        /// ausgewählte Seite(Gridnavigation) in der Übersicht
        /// </summary>
        public int ListePageIndex
        {
            get;
            set;

        }
        /// <summary>
        /// Index PageSize in der Übersicht (Gridview)
        /// </summary>
        public int ListePageSizeIndex
        {
            get;
            set;

        }
        /// <summary>
        /// ausgewählte Sortierung in der Übersicht
        /// </summary>
        public string GridSort
        {
            get;
            set;

        }
        /// <summary>
        /// Rückgabetabelle SAP mit Pfaden der Barquittungen
        /// </summary>
        public DataTable tblBarquittungen
        {
            get;
            set;
        }
        /// <summary>
        /// Verzeichnis der Sofortabrechnungen
        /// </summary>
        public string SofortabrechnungVerzeichnis
        {
            get;
            set;
        }
        /// <summary>
        /// Vorgänge ohne bestimmtes Zul. bzw. Durchführungsdatum
        /// </summary>
        public bool bFlieger
        {
            get;
            set;

        }
        /// <summary>
        /// Selektion Vorgänge ohne bestimmtes Zul. bzw. Durchführungsdatum 
        /// </summary>
        public bool SelFlieger
        {
            get;
            set;

        }
        /// <summary>
        /// Selektion bzw. Modus "Neue AH-Vorgänge" 
        /// </summary>
        public bool SelAnnahmeAH
        {
            get;
            set;
        }
        /// <summary>
        /// Selektion bzw. Modus "Sofortabrechnung" 
        /// </summary>
        public bool SelSofortabrechnung
        {
            get;
            set;
        }
        /// <summary>
        /// Selektion bzw. Modus "Nacherfassung durchzuf. Vers.zul. -> Bearbeitung" 
        /// </summary>
        public bool SelEditDurchzufVersZul
        {
            get;
            set;
        }
        /// <summary>
        /// Gruppen-/Tour-ID
        /// </summary>
        public string SelGroupTourID
        {
            get;
            set;
        }
        /// <summary>
        /// Langtextnummer SAP 
        /// </summary>
        public String NrLangText
        {
            get;
            set;

        }
        /// <summary>
        /// Langtext SAP 
        /// </summary>
        public String LangText
        {
            get;
            set;

        }
        /// <summary>
        /// Rückgabetabelle Versandzulassungen erfasst durch Autohaus
        /// </summary>
        public DataTable AHVersandListe
        {
            get;
            set;
        }
        /// <summary>
        /// Kopftabelle Versandzulassungen erfasst durch Autohaus
        /// </summary>
        public DataTable AHVersandKopf
        {
            get;
            set;
        }
        /// <summary>
        /// Positionstabelle Versandzulassungen erfasst durch Autohaus
        /// </summary>
        public DataTable AHVersandPos
        {
            get;
            set;
        }
        /// <summary>
        /// Bankdaten Versandzulassungen erfasst durch Autohaus
        /// </summary>
        public DataTable AHVersandBank
        {
            get;
            set;
        }
        /// <summary>
        /// abweichende Versandadresse Versandzulassungen erfasst durch Autohaus
        /// </summary>
        public DataTable AHVersandAdresse
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Hinsendung v. Dokumenten
        /// </summary>
        public String Name1Hin
        {
            get;
            set;
        }

        /// <summary>
        /// Adressdaten für die Hinsendung v. Dokumenten
        /// </summary>
        public String Name2Hin
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Hinsendung v. Dokumenten
        /// </summary>
        public String StrasseHin
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Hinsendung v. Dokumenten
        /// </summary>
        public String PLZHin
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Hinsendung v. Dokumenten
        /// </summary>
        public String OrtHin
        {
            get;
            set;
        }
        /// <summary>
        /// Dokumente für die Hinsendung
        /// </summary>
        public String DocRueck1
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Rücksendung v. Dokumenten
        /// </summary>
        public String NameRueck1
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Rücksendung v. Dokumenten
        /// </summary>
        public String NameRueck2
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Rücksendung v. Dokumenten
        /// </summary>
        public String StrasseRueck
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Rücksendung v. Dokumenten
        /// </summary>
        public String PLZRueck
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Rücksendung v. Dokumenten
        /// </summary>
        public String OrtRueck
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Rücksendung v. Dokumenten
        /// </summary>
        public String Doc2Rueck
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Rücksendung v. Dokumenten
        /// </summary>
        public String Name1Rueck2
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Rücksendung v. Dokumenten
        /// </summary>
        public String Name2Rueck2
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Rücksendung v. Dokumenten
        /// </summary>
        public String Strasse2Rueck
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Rücksendung v. Dokumenten
        /// </summary>
        public String PLZ2Rueck
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten für die Rücksendung v. Dokumenten
        /// </summary>
        public String Ort2Rueck
        {
            get;
            set;
        }
        /// <summary>
        /// Ist der ausgwählte Lieferant ein ZLD (Versandzulassung)?!
        /// </summary>
        public String IsZLD
        {
            get;
            set;
        }
        /// <summary>
        /// Material dem ZLD nicht zugwiesen(Fehlernummer).
        /// </summary>
        public int MatError
        {
            get;
            set;
        }
        /// <summary>
        /// Material dem ZLD nicht zugwiesen(Fehlertext).
        /// </summary>
        public String MatErrorText
        {
            get;
            set;
        }

        public Boolean ZusatzKZ
        {
            get;
            set;
        }
        public string WunschKZ2
        {
            get;
            set;
        }
        public string WunschKZ3
        {
            get;
            set;
        }
        public bool OhneGruenenVersSchein
        {
            get;
            set;
        }
        public bool SofortabrechnungErledigt
        {
            get;
            set;
        }
        public string SofortabrechnungPfad
        {
            get; 
            set;
        }
        public string Infotext
        {
            get;
            set;
        }
        public bool Nachbearbeiten
        {
            get;
            set;
        }
        public string Mobuser { get; set; }
        public string Poolnr { get; set; }
        public bool Onlinevorgang { get; set; }

        #endregion

        #region "Methods"

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="objUser">User -Objekt</param>
        /// <param name="objApp">Applikations-Objekt</param>
        /// <param name="sVorgang">Vorgangsart z.B. NZ, VZ</param>
        public NacherfZLD(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, String sVorgang)
            : base(ref objUser, objApp, "")
        {
            ListePageIndex = 0;
            ListePageSize = 20;
            ListePageSizeIndex = 1;
            Vorgang = sVorgang;
            GridSort = "";
            CreatePosTable();
        }

        /// <summary>
        /// Daten für die Nacherfassung aus SAP laden
        /// Prüfen der Vorgänge ob schon in der SQL-DB
        /// Aufruf der Speicherfunktion der Daten in SQL-DB
        /// Nur Daten aus SAP anzeigen. Bapi: Z_ZLD_EXPORT_NACHERF
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ChangeZLDSelect.aspx, ChangeZLDNachVersand.aspx, ChangeSelectVersand.aspx</param>
        /// <param name="tblKundenStamm">Kundentabellen</param>
        /// <param name="tblMaterialStamm">Materialstammtabelle</param>
        /// <returns></returns>
        /// 
        public Int32 getSAPDatenNacherf(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblKundenStamm, DataTable tblMaterialStamm)
        {
            Int32 returnvalue = 0;

            m_strClassAndMethod = "NacherfZLD.getSAPDatenNacherf";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            ClearError();

            MatError = 0;
            MatErrorText = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    var bapiName = "Z_ZLD_EXPORT_NACHERF";

                    if (SelSofortabrechnung)
                        bapiName = "Z_ZLD_EXPORT_SOFORT_ABRECHNUNG";

                    DynSapProxyObj myProxy = DynSapProxy.getProxy(bapiName, ref m_objApp, ref m_objUser, ref page);
                    
                    myProxy.setImportParameter("I_KUNNR", mSelKunde);
                    myProxy.setImportParameter("I_VKORG", VKORG);
                    myProxy.setImportParameter("I_VKBUR", VKBUR);
                    myProxy.setImportParameter("I_ZZZLDAT", mSelDatum);

                    if (mSelID.Length > 0)
                    {
                        myProxy.setImportParameter("I_ZULBELN", mSelID.PadLeft(10, '0'));
                    }
                    else
                    {
                        myProxy.setImportParameter("I_ZULBELN", "");
                    }

                    myProxy.setImportParameter("I_FLIEGER", ZLDCommon.BoolToX(SelFlieger));

                    if (!SelSofortabrechnung)
                    {
                        myProxy.setImportParameter("I_KREISKZ", mSelKreis);
                        myProxy.setImportParameter("I_MATNR", mSelMatnr);
                        myProxy.setImportParameter("I_BLTYP", SelStatus);
                        myProxy.setImportParameter("I_DZLD_VKBUR", DZVKBUR);

                        if (SelLief != "0" && SelLief != "")
                        {
                            myProxy.setImportParameter("I_LIFNR", SelLief);
                        }

                        myProxy.setImportParameter("I_AH_ANNAHME", ZLDCommon.BoolToX(SelAnnahmeAH));

                        if (!String.IsNullOrEmpty(SelGroupTourID))
                        {
                            myProxy.setImportParameter("I_GRUPPE", SelGroupTourID.PadLeft(10, '0'));
                        }
                    }

                    myProxy.callBapi();

                    DataTable tblAuftrag = myProxy.getExportTable("GT_EX_BAK");
                    DataTable tblAuftragPos = myProxy.getExportTable("GT_EX_POS");
                    DataTable tblAuftragKunden = myProxy.getExportTable("GT_EX_KUNDE");
                    DataTable tblAuftragBank = myProxy.getExportTable("GT_EX_BANK");
                    DataTable tblAuftragAdress = myProxy.getExportTable("GT_EX_ADRS");

                    if (tblAuftrag.Rows.Count == 0)
                    {
                        returnvalue = 1;
                    }
                    else
                    {
                        for (int i = tblAuftrag.Rows.Count - 1; i >= 0; i--)
                        {
                            String ZULBELN = tblAuftrag.Rows[i]["ZULBELN"].ToString();
                            // Auftrag ohne Hauptpositionen ?
                            if (tblAuftragPos.Select("ZULBELN = '" + ZULBELN + "' AND ZULPOSNR = '000010'").Length == 0)
                            {
                                tblAuftrag.Rows.RemoveAt(i);
                            }
                            else
                            {   // Material einer Versandzulassung dem durchzuführenden ZLD zugewiesen?
                                DataRow MatnRow = tblAuftragPos.Select("ZULBELN = '" + ZULBELN + "' AND ZULPOSNR = '000010'")[0];
                                if (tblMaterialStamm.Select("MATNR='" + MatnRow["MATNR"].ToString().TrimStart('0') + "'").Length == 0)
                                {
                                    MatError = -4444;
                                    MatErrorText += "ID " + ZULBELN.TrimStart('0') + ": Material " +
                                                    MatnRow["MATNR"].ToString().TrimStart('0') + " nicht freigeschaltet. \r\n";
                                    tblAuftrag.Rows.RemoveAt(i);
                                }
                            }
                        }

                        // Gebührenmat. / Kennzeichenmat./ Preis_Amt / Gebühren /Kennzeichenpreis in die Kopftabelle übernehmen
                        // wichtig für die Übersichtsanzeige der Vorgänge
                        tblAuftragPos.Columns.Add("GebMatnr", typeof(String));
                        tblAuftragPos.Columns.Add("GebMatbez", typeof(String));
                        tblAuftragPos.Columns.Add("GebPreis", typeof(String));
                        tblAuftragPos.Columns.Add("Preis_Amt", typeof(String));
                        tblAuftragPos.Columns.Add("GebMatnrSt", typeof(String));
                        tblAuftragPos.Columns.Add("GebMatBezSt", typeof(String));
                        tblAuftragPos.Columns.Add("Kennzmat", typeof(String));
                        tblAuftragPos.Columns.Add("KennzMatPreis", typeof(String));

                        foreach (DataRow itemRow in tblAuftragPos.Rows)
                        {
                            if (itemRow["WEBMTART"].ToString() == "D")
                            {
                                DataRow[] SelRow = tblAuftragPos.Select("ZULBELN = '" + itemRow["ZULBELN"].ToString() +
                                                                         "' AND UEPOS = '" + itemRow["ZULPOSNR"].ToString() +
                                                                         "' AND WEBMTART = 'G'");
                                if (SelRow.Length == 1)
                                {
                                    itemRow["GebMatnr"] = SelRow[0]["MATNR"].ToString();
                                    itemRow["GebMatbez"] = SelRow[0]["MAKTX"].ToString();
                                    itemRow["GebPreis"] = SelRow[0]["PREIS"].ToString();
                                    itemRow["Preis_Amt"] = SelRow[0]["GEB_AMT"].ToString();
                                    itemRow["GBPAK"] = SelRow[0]["GBPAK"].ToString();
                                    itemRow["Kennzmat"] = "";
                                    itemRow["KennzMatPreis"] = "";

                                }

                                SelRow = tblAuftragPos.Select("ZULBELN = '" + itemRow["ZULBELN"].ToString() +
                                                                         "' AND UEPOS = '" + itemRow["ZULPOSNR"].ToString() +
                                                                         "' AND WEBMTART = 'K'");

                                if (SelRow.Length == 1)
                                {
                                    itemRow["Kennzmat"] = SelRow[0]["MATNR"].ToString();
                                    itemRow["KennzMatPreis"] = SelRow[0]["PREIS"].ToString();
                                }
                            }
                        }

                        SapResultTable = tblAuftrag;

                        // Bereits in der SQL-Tabelle vorhandene Vorgänge nicht wieder einfügen
                        cleanSapTable(ref tblAuftrag);

                        InsertFromSapDB_ZLD(strAppID, strSessionID, page, tblAuftrag, tblAuftragPos, tblAuftragKunden, tblAuftragBank,
                            tblAuftragAdress, tblKundenStamm, tblMaterialStamm);

                    }

                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            RaiseError("-5555", "Keine Daten gefunden!");
                            break;
                        default:
                            RaiseError("-9999", "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" +
                                HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
                            break;
                    }
                }
                finally { m_blnGestartet = false; }

            }
            return returnvalue;
        }

        /// <summary>
        /// Läd die Daten für Versandzulassungen , die von Autohäusern erfasst wurden
        /// Nur für die tabellarische Übersicht (AHVersandListe.aspx). Bapi: Z_ZLD_AH_EX_VSZUL
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page"></param>
        /// <param name="tblKundenStamm">Kundentabellen</param>
        /// <param name="tblMaterialStamm">Materialstammtabelle</param>
        /// <returns></returns>
        public Int32 getSAPAHVersand(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblKundenStamm,
                                    DataTable tblMaterialStamm)
        {
            Int32 returnvalue = 0;

            m_strClassAndMethod = "NacherfZLD.getSAPAHVersand";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            ClearError();

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_EX_VSZUL", ref m_objApp, ref m_objUser, ref page);

                    if (mSelKunde.Length > 0) mSelKunde = mSelKunde.PadLeft(10, '0');
                    myProxy.setImportParameter("I_KUNNR", mSelKunde);
                    myProxy.setImportParameter("I_VKBUR", VKBUR);
                    myProxy.setImportParameter("I_VKORG", VKORG);
                    myProxy.setImportParameter("I_ZZZLDAT", mSelDatum);
                    myProxy.setImportParameter("I_KREISKZ", mSelKreis);

                    if (mSelID.Length > 0)
                    {
                        myProxy.setImportParameter("I_ZULBELN", mSelID.PadLeft(10, '0'));
                    }
                    else
                    {
                        myProxy.setImportParameter("I_ZULBELN", "");
                    }

                    myProxy.callBapi();

                    AHVersandListe = myProxy.getExportTable("GT_OUT");
                    AHVersandListe.Columns.Add("toDelete", typeof(String));

                    foreach (DataRow itemRow in AHVersandListe.Rows)
                    {
                        itemRow["ZULBELN"] = itemRow["ZULBELN"].ToString().TrimStart('0');
                        itemRow["KUNNR"] = itemRow["KUNNR"].ToString().TrimStart('0');
                        itemRow["ZULPOSNR"] = itemRow["ZULPOSNR"].ToString().TrimStart('0');
                        itemRow["toDelete"] = String.Empty;
                    }

                    foreach (DataRow itemRow in AHVersandListe.Rows)
                    {
                        itemRow["ZULBELN"] = itemRow["ZULBELN"].ToString().TrimStart('0');
                        itemRow["KUNNR"] = itemRow["KUNNR"].ToString().TrimStart('0');
                        itemRow["toDelete"] = String.Empty;
                    }

                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out m_intStatus);

                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            RaiseError("-5555", "Keine Daten gefunden!");
                            break;
                        default:
                            RaiseError("-9999", "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" +
                                HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
                            break;
                    }
                }
                finally { m_blnGestartet = false; }

            }
            return returnvalue;
        }

        /// <summary>
        /// Läd die Daten für Versandzulassungen , die von Autohäusern erfasst wurden
        /// Detaildaten für die weitere Verarbeitung analog Vorerfassung Versandzulassung ZLD
        /// (AHVersandChange.aspx). Bapi: Z_ZLD_GET_ORDER
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">AHVersandChange.aspx</param>
        /// <param name="ID">ZULBELN</param>
        /// <returns>Fehler</returns>
        public Int32 getSAPAHVersandDetail(String strAppID, String strSessionID, System.Web.UI.Page page, String ID)
        {
            Int32 returnvalue = 0;

            m_strClassAndMethod = "NacherfZLD.getSAPAHVersandDetail";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            ClearError();

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_GET_ORDER", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_ZULBELN", ID.PadLeft(10, '0'));

                    myProxy.callBapi();

                    AHVersandKopf = myProxy.getExportTable("GS_BAK");
                    AHVersandPos = myProxy.getExportTable("GT_POS_S01");
                    AHVersandBank = myProxy.getExportTable("GT_BANK");
                    AHVersandAdresse = myProxy.getExportTable("GT_ADRS");

                    foreach (DataRow itemRow in AHVersandPos.Rows)
                    {
                        itemRow["ZULPOSNR"] = itemRow["ZULPOSNR"].ToString().TrimStart('0');
                    }


                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out m_intStatus);
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE");
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            RaiseError("-5555", "Keine Daten gefunden!");
                            break;
                        default:
                            RaiseError("-9999", "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" +
                                                HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
                            break;
                    }
                }
                finally { m_blnGestartet = false; }

            }
            return returnvalue;
        }

        /// <summary>
        /// Löscht den Vorgang der Versandzulassung Autohaus 
        /// aus der Übersicht (AHVersandListe.aspx). Bapi: Z_ZLD_SET_LOEKZ
        /// </summary>
        /// <param name="strAppID"></param>
        /// <param name="strSessionID"></param>
        /// <param name="page"></param>
        public void setSAPLOEKZ(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "NacherfZLD.setSAPLOEKZ";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            ClearError();

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    foreach (DataRow SaveRow in AHVersandListe.Rows)
                    {
                        if (SaveRow["toDelete"].ToString() == "L")
                        {
                            DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_SET_LOEKZ", ref m_objApp, ref m_objUser, ref page);

                            myProxy.setImportParameter("I_ZULBELN", SaveRow["ZULBELN"].ToString().PadLeft(10, '0'));

                            myProxy.callBapi();

                            Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out m_intStatus);
                            m_strMessage = myProxy.getExportParameter("E_MESSAGE");
                        }
                    }

                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            RaiseError("-9999", "Es ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
                            break;
                    }
                }
            }

        }

        /// <summary>
        /// Bereinigen der SAP-Daten wenn gleiche Vorgänge schon In SQL-DB
        /// </summary>
        /// <param name="Table"></param>
        private void cleanSapTable(ref DataTable Table)
        {
            var tmp = Table.Copy();
            var ZLD_DataContext = new ZLDTableClassesDataContext();

            try
            {
                bool blnVersandzulassungen = (SelStatus.Contains("VZ") || SelStatus.Contains("VE") || SelStatus.Contains("AV") || SelStatus.Contains("AX"));

                foreach (DataRow row in Table.Rows)
                {
                    Int32 idRecord;
                    Int32.TryParse(row["ZULBELN"].ToString(), out idRecord);

                    int KopfCount;
                    if (SelSofortabrechnung)
                    {
                        KopfCount = (from k in ZLD_DataContext.ZLDKopfTabelle
                                     where k.id_sap == idRecord && k.Vorgang == row["BLTYP"].ToString()
                                     select k).Count();
                    }
                    else if (blnVersandzulassungen) // Versandzulassungen
                    {
                        KopfCount = (from k in ZLD_DataContext.ZLDKopfTabelle
                                     where k.id_sap == idRecord && (k.Vorgang == "VZ" || k.Vorgang == "VE" || k.Vorgang == "AV" || k.Vorgang == "AX")
                                     select k).Count();
                    }
                    else if (Vorgang == "ON") // Onlinevorgänge
                    {
                        KopfCount = (from k in ZLD_DataContext.ZLDKopfTabelle
                                     where k.id_sap == idRecord && (k.Vorgang == "ON" || k.Vorgang == "OA")
                                     select k).Count();
                    }
                    else if (Vorgang == "A") // alle Autohausvorgänge
                    {
                        KopfCount = (from k in ZLD_DataContext.ZLDKopfTabelle
                                     where k.id_sap == idRecord && k.Vorgang.StartsWith(Vorgang)
                                     select k).Count();
                    }
                    else if (Vorgang == "ANZ") // alle Autohausvorgänge und normale Nacherfassung
                    {
                        KopfCount = (from k in ZLD_DataContext.ZLDKopfTabelle
                                     where k.id_sap == idRecord && (k.Vorgang.StartsWith("A") || k.Vorgang == "NZ")
                                     select k).Count();
                    }
                    else //normale Nacherfassung
                    {
                        KopfCount = (from k in ZLD_DataContext.ZLDKopfTabelle
                                     where k.id_sap == idRecord && k.Vorgang == Vorgang
                                     select k).Count();
                    }

                    if (KopfCount > 0)
                    {
                        DataRow[] rowDel = tmp.Select("ZULBELN=" + idRecord);
                        tmp.Rows.Remove(rowDel[0]);
                    }
                }
                Table = tmp.Copy();

            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Positionstabelle aufbauen
        /// </summary>
        private void CreatePosTable()
        {
            tblPositionen = new DataTable();
            tblPositionen.Columns.Add("id_Kopf", typeof(Int32));
            tblPositionen.Columns.Add("id_pos", typeof(Int32));
            tblPositionen.Columns.Add("Menge", typeof(String));
            tblPositionen.Columns.Add("Matnr", typeof(String));
            tblPositionen.Columns.Add("Matbez", typeof(String));
            tblPositionen.Columns.Add("Preis", typeof(String));
            tblPositionen.Columns.Add("WebMTArt", typeof(String));
            tblPositionen.Columns.Add("SDRelevant", typeof(String));
            tblPositionen.Columns.Add("UPreis", typeof(String));
            tblPositionen.Columns.Add("Differrenz", typeof(String));
            tblPositionen.Columns.Add("Konditionstab", typeof(String));
            tblPositionen.Columns.Add("Konditionsart", typeof(String));
            tblPositionen.Columns.Add("CALCDAT", typeof(String));
            tblPositionen.Columns.Add("GebPak", typeof(String));
        }

        /// <summary>
        /// SAP-Daten in die Sql-Tabellen speichern(ZLDKopfTabelle,ZLDPositionsTabelle, ZLDBankverbindung, ZLDKundenadresse )
        /// </summary>
        /// <param name="strAppID"></param>
        /// <param name="strSessionID"></param>
        /// <param name="page"></param>
        /// <param name="tblSap"></param>
        /// <param name="tblPos"></param>
        /// <param name="tblAuftragKunden"></param>
        /// <param name="tblAuftragBank"></param>
        /// <param name="tblAuftragAdress"></param>
        /// <param name="tblKundenStamm"></param>
        /// <param name="tblMaterialStamm"></param>
        public void InsertFromSapDB_ZLD(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblSap,
                                        DataTable tblPos, DataTable tblAuftragKunden, DataTable tblAuftragBank, DataTable tblAuftragAdress, DataTable tblKundenStamm, DataTable tblMaterialStamm)
        {
            ClearError();

            var ZLD_DataContext = new ZLDTableClassesDataContext();

            try
            {
                foreach (DataRow dRow in tblSap.Rows)
                {
                    String ohneSt;
                    var tblKopf = new ZLDKopfTabelle();

                    Int32.TryParse(dRow["ZULBELN"].ToString(), out id_sap);
                    tblKopf.id_sap = id_sap;

                    tblKopf.id_user = m_objUser.UserID;
                    tblKopf.id_session = strSessionID;
                    tblKopf.abgerechnet = false;
                    tblKopf.username = m_objUser.UserName;
                    tblKopf.Vorgang = dRow["BLTYP"].ToString();
                    tblKopf.kundennr = dRow["KUNNR"].ToString().TrimStart('0');

                    tblKopf.Barkunde = ZLDCommon.XToBool(dRow["KUNDEBAR_JN"].ToString());

                    DataRow[] KundeRow = tblKundenStamm.Select("KUNNR='" + tblKopf.kundennr + "'");

                    if (KundeRow.Length == 1)
                    {
                        tblKopf.OhneSteuer = KundeRow[0]["OHNEUST"].ToString();
                        tblKopf.PauschalKunde = KundeRow[0]["ZZPAUSCHAL"].ToString();
                        tblKopf.KreisKZ_Direkt = KundeRow[0]["KREISKZ_DIREKT"].ToString();
                        if (!tblKopf.Barkunde.Value)
                        {
                            tblKopf.Barkunde = ZLDCommon.XToBool(KundeRow[0]["BARKUNDE"].ToString());
                        }
                    }
                    else if (tblKopf.Vorgang == "VZ" || tblKopf.Vorgang == "AV")
                    {
                        tblKopf.OhneSteuer = "";
                        tblKopf.PauschalKunde = "";
                        tblKopf.KreisKZ_Direkt = "";
                    }

                    KundeRow = tblAuftragKunden.Select("KUNNR='" + dRow["KUNNR"].ToString() + "'");
                    if (KundeRow.Length == 1)
                    {
                        tblKopf.kundenname = KundeRow[0]["NAME1"].ToString();
                        if (KundeRow[0]["EXTENSION1"].ToString().Length > 0)
                        { tblKopf.kundenname += " / " + KundeRow[0]["EXTENSION1"].ToString(); }

                    }
                    tblKopf.referenz1 = dRow["ZZREFNR1"].ToString();
                    tblKopf.referenz2 = dRow["ZZREFNR2"].ToString();
                    tblKopf.KreisKZ = dRow["KREISKZ"].ToString();
                    tblKopf.KreisBez = dRow["KREISBEZ"].ToString();
                    tblKopf.WunschKenn = ZLDCommon.XToBool(dRow["WUNSCHKENN_JN"].ToString());
                    tblKopf.ZusatzKZ = dRow["ZUSKENNZ"].ToString();
                    tblKopf.WunschKZ2 = dRow["WU_KENNZ2"].ToString();
                    tblKopf.WunschKZ3 = dRow["WU_KENNZ3"].ToString();
                    tblKopf.OhneGruenenVersSchein = dRow["O_G_VERSSCHEIN"].ToString();
                    tblKopf.SofortabrechnungErledigt = ZLDCommon.XToBool(dRow["SOFORT_ABR_ERL"].ToString());
                    tblKopf.SofortabrechnungPfad = dRow["SA_PFAD"].ToString();
                    tblKopf.Reserviert = ZLDCommon.XToBool(dRow["RESERVKENN_JN"].ToString());
                    tblKopf.ReserviertKennz = dRow["RESERVKENN"].ToString();
                    tblKopf.Feinstaub = ZLDCommon.XToBool(dRow["FEINSTAUBAMT"].ToString());
                    tblKopf.toDelete = dRow["LOEKZ"].ToString();

                    if (dRow["ZZZLDAT"].ToString().Length > 0)
                    {
                        DateTime tmpDate;
                        DateTime.TryParse(dRow["ZZZLDAT"].ToString(), out tmpDate);
                        tblKopf.Zulassungsdatum = tmpDate;
                    }
                    else
                    { tblKopf.Zulassungsdatum = null; }

                    tblKopf.Kennzeichen = dRow["ZZKENN"].ToString();

                    String sKennz = dRow["ZZKENN"].ToString();
                    if (sKennz.Contains("-"))// möglicherweise ein Fun-/Parkschild?
                    {
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
                        tblKopf.KennKZ = sKennzKZ;
                        tblKopf.KennABC = sKennzABC;
                    }
                    else
                    {
                        tblKopf.KennKZ = tblKopf.Kennzeichen.Substring(0, 3);
                        tblKopf.KennABC = tblKopf.Kennzeichen.Substring(3);
                    }

                    tblKopf.Kennztyp = dRow["KENNZTYP"].ToString();
                    tblKopf.KennzForm = dRow["KENNZFORM"].ToString();
                    tblKopf.EinKennz = ZLDCommon.XToBool(dRow["EINKENN_JN"].ToString());
                    tblKopf.Bemerkung = dRow["BEMERKUNG"].ToString();
                    tblKopf.Infotext = dRow["INFO_TEXT"].ToString();
                    tblKopf.Nachbearbeiten = ZLDCommon.XToBool(dRow["NACHBEARBEITEN"].ToString());
                    tblKopf.ONLINE_VG = ZLDCommon.XToBool(dRow["ONLINE_VG"].ToString());
                    tblKopf.Mobuser = dRow["MOBUSER"].ToString();
                    tblKopf.POOLNR = dRow["POOLNR"].ToString();
                    tblKopf.NrFrachtHin = dRow["ZL_RL_FRBNR_HIN"].ToString();
                    tblKopf.NrFrachtZu = dRow["ZL_RL_FRBNR_ZUR"].ToString();

                    int iAnz;
                    int.TryParse(dRow["KENNZANZ"].ToString(), out iAnz);
                    tblKopf.KennzAnz = iAnz;

                    tblKopf.saved = saved;
                    tblKopf.toDelete = toDelete;
                    tblKopf.bearbeitet = false;
                    tblKopf.Barcode = dRow["BARCODE"].ToString();
                    tblKopf.Filiale = dRow["VKORG"].ToString() + dRow["VKBUR"].ToString();

                    // Bei Versandzulassungen wird VersandVKBUR gefüllt mit VKORG + VZD_VKBUR
                    if (tblKopf.Vorgang == "VZ")
                    {
                        tblKopf.VersandVKBUR = dRow["VKORG"].ToString() + dRow["VZD_VKBUR"].ToString();
                    }
                    else if (tblKopf.Vorgang == "AV")
                    {
                        tblKopf.VersandVKBUR = dRow["VKORG"].ToString() + dRow["VZD_VKBUR"].ToString();
                    }
                    else
                    {
                        tblKopf.VersandVKBUR = "";
                    }

                    tblKopf.EC = ZLDCommon.XToBool(dRow["EC_JN"].ToString());
                    tblKopf.Bar = ZLDCommon.XToBool(dRow["BAR_JN"].ToString());
                    tblKopf.RE = ZLDCommon.XToBool(dRow["RE_JN"].ToString());

                    // Zahlungsarten-Variablen sind Nullable<bool>, deshalb die null-Abfragen
                    if ((tblKopf.EC == null || tblKopf.EC == false)
                        && (tblKopf.Bar == null || tblKopf.Bar == false)
                        && (tblKopf.RE == null || tblKopf.RE == false))
                    {
                        // wenn keine Zahlungsart gesetzt -> Default: EC
                        tblKopf.EC = true;
                    }

                    tblKopf.testuser = m_objUser.IsTestUser;
                    tblKopf.KunnrLF = dRow["ZL_LIFNR"].ToString();
                    tblKopf.FLAG = dRow["FLAG"].ToString();
                    tblKopf.VZB_STATUS = dRow["VZB_STATUS"].ToString();
                    tblKopf.STATUS_Versand = dRow["STATUS"].ToString();
                    tblKopf.Zahlungsart = dRow["ZAHLART"].ToString();
                    tblKopf.ZBII_ALT_NEU = ZLDCommon.XToBool(dRow["ZBII_ALT_NEU"].ToString());
                    tblKopf.VorhKennzReserv = ZLDCommon.XToBool(dRow["VH_KENNZ_RES"].ToString());
                    tblKopf.KennzVH = ZLDCommon.XToBool(dRow["KENNZ_VH"].ToString());
                    tblKopf.VKKurz = dRow["VK_KUERZEL"].ToString();
                    tblKopf.interneRef = dRow["KUNDEN_REF"].ToString();
                    tblKopf.KundenNotiz = dRow["KUNDEN_NOTIZ"].ToString();
                    tblKopf.KennzAlt = dRow["ALT_KENNZ"].ToString();
                    tblKopf.Vorerfasser = dRow["VE_ERNAM"].ToString();

                    if (dRow["KSTATUS"].ToString() == "B")
                    {
                        tblKopf.bearbeitet = true;
                    }

                    if (dRow["VE_ERDAT"].ToString().Length > 0)
                    {
                        DateTime tmpDate;
                        DateTime.TryParse(dRow["VE_ERDAT"].ToString(), out tmpDate);
                        tblKopf.VorerfDatum = tmpDate;
                    }
                    else
                    { tblKopf.VorerfDatum = null; }

                    tblKopf.VorerfUhrzeit = dRow["VE_ERZEIT"].ToString();

                    if (dRow["STILL_DAT"].ToString().Length > 0)
                    {
                        DateTime tmpDate;
                        DateTime.TryParse(dRow["STILL_DAT"].ToString(), out tmpDate);
                        tblKopf.Still_Datum = tmpDate;
                    }
                    else
                    { tblKopf.Still_Datum = null; }

                    tblKopf.Prali_Print = ZLDCommon.XToBool(dRow["PRALI_PRINT"].ToString());
                    tblKopf.Flieger = ZLDCommon.XToBool(dRow["FLIEGER"].ToString());

                    tblKopf.UMBU = ZLDCommon.XToBool(dRow["UMBU"].ToString());
                    tblKopf.ABGESAGT = ZLDCommon.XToBool(dRow["ABGESAGT"].ToString());
                    tblKopf.RUECKBU = ZLDCommon.XToBool(dRow["RUECKBU"].ToString());
                    tblKopf.EVB = dRow["ZZEVB"].ToString();
                    tblKopf.OBJECT_ID = dRow["OBJECT_ID"].ToString();
                    tblKopf.FAHRZ_ART = dRow["FAHRZ_ART"].ToString();

                    tblKopf.MNRESW = ZLDCommon.XToBool(dRow["MNRESW"].ToString());
                    tblKopf.SERIE = ZLDCommon.XToBool(dRow["SERIE"].ToString());
                    tblKopf.SAISON_KNZ = ZLDCommon.XToBool(dRow["SAISON_KNZ"].ToString());
                    tblKopf.SAISON_BEG = dRow["SAISON_BEG"].ToString();
                    tblKopf.SAISON_END = dRow["SAISON_END"].ToString();
                    tblKopf.TUEV_AU = dRow["TUEV_AU"].ToString();
                    tblKopf.KURZZEITVS = dRow["KURZZEITVS"].ToString();
                    tblKopf.ZOLLVERS = dRow["ZOLLVERS"].ToString();

                    tblKopf.ZOLLVERS_DAUER = dRow["ZOLLVERS_DAUER"].ToString();
                    tblKopf.VORFUEHR = ZLDCommon.XToBool(dRow["VORFUEHR"].ToString());

                    if (dRow["HALTE_DAUER"].ToString().Length > 0)
                    {
                        DateTime tmpDate;
                        DateTime.TryParse(dRow["HALTE_DAUER"].ToString(), out tmpDate);
                        tblKopf.HALTE_DAUER = tmpDate;
                    }
                    else
                    { tblKopf.HALTE_DAUER = null; }

                    tblKopf.LTEXT_NR = dRow["LTEXT_NR"].ToString();
                    tblKopf.BEB_STATUS = dRow["BEB_STATUS"].ToString();
                    tblKopf.ZZREFNR3 = dRow["ZZREFNR3"].ToString();
                    tblKopf.ZZREFNR4 = dRow["ZZREFNR4"].ToString();
                    tblKopf.ZZREFNR5 = dRow["ZZREFNR5"].ToString();
                    tblKopf.ZZREFNR6 = dRow["ZZREFNR6"].ToString();
                    tblKopf.ZZREFNR7 = dRow["ZZREFNR7"].ToString();
                    tblKopf.ZZREFNR8 = dRow["ZZREFNR8"].ToString();
                    tblKopf.ZZREFNR9 = dRow["ZZREFNR9"].ToString();
                    tblKopf.ZZREFNR10 = dRow["ZZREFNR10"].ToString();
                    tblKopf.AH_DOKNAME = dRow["AH_DOKNAME"].ToString();

                    ZLD_DataContext.Connection.Open();
                    ZLD_DataContext.ZLDKopfTabelle.InsertOnSubmit(tblKopf);
                    ZLD_DataContext.SubmitChanges();
                    id_Kopf = tblKopf.id;
                    ohneSt = tblKopf.OhneSteuer;

                    ZLD_DataContext.Connection.Close();

                    ZLD_DataContext = new ZLDTableClassesDataContext();
                    if (tblPos.Rows.Count > 0)
                    {
                        foreach (DataRow drow in tblPos.Rows)
                        {
                            Int32 id_sapPos = 0;
                            if (ZLDCommon.IsNumeric(drow["ZULBELN"].ToString()))
                            {
                                Int32.TryParse(drow["ZULBELN"].ToString(), out id_sapPos);
                            }


                            if (id_sapPos == id_sap)
                            {
                                var tmpPos = new ZLDPositionsTabelle { id_Kopf = id_Kopf };

                                int dPosID;
                                int.TryParse(drow["ZULPOSNR"].ToString(), out dPosID);

                                tmpPos.id_pos = dPosID;
                                tmpPos.Menge = drow["MENGE"].ToString();
                                tmpPos.Matnr = drow["MATNR"].ToString();
                                tmpPos.Matbez = drow["MAKTX"].ToString();

                                Decimal dPreis;
                                Decimal.TryParse(drow["PREIS"].ToString(), out dPreis);
                                tmpPos.Preis = dPreis;

                                Decimal.TryParse(drow["PREIS_AMT"].ToString(), out dPreis);
                                tmpPos.Preis_Amt = dPreis;

                                tmpPos.PosLoesch = "";
                                if (ohneSt == "X")
                                {
                                    tmpPos.GebMatnr = drow["GebMatnr"].ToString();
                                    tmpPos.GebMatbez = drow["GebMatbez"].ToString();
                                }
                                else
                                {
                                    tmpPos.GebMatnrSt = drow["GebMatnr"].ToString();
                                    tmpPos.GebMatBezSt = drow["GebMatbez"].ToString();
                                }

                                tmpPos.Kennzmat = drow["Kennzmat"].ToString();

                                if (drow["LOEKZ"].ToString() == "X" && tblKopf.bearbeitet == true)
                                {
                                    tmpPos.PosLoesch = "L";
                                }
                                else if (drow["LOEKZ"].ToString() == "" && tblKopf.bearbeitet == true)
                                {
                                    tmpPos.PosLoesch = "O";
                                }
                                else
                                {
                                    tmpPos.PosLoesch = "";
                                }

                                Decimal.TryParse(drow["GebPreis"].ToString(), out dPreis);
                                tmpPos.GebPreis = dPreis;

                                Decimal.TryParse(drow["KennzMatPreis"].ToString(), out dPreis);
                                tmpPos.PreisKZ = dPreis;

                                int iUEPOS;
                                int.TryParse(drow["UEPOS"].ToString(), out iUEPOS);
                                tmpPos.UEPOS = iUEPOS;

                                tmpPos.SDRelevant = drow["SD_REL"].ToString();
                                tmpPos.WebMTArt = drow["WEBMTART"].ToString();

                                if (tmpPos.WebMTArt == "G")
                                {
                                    Decimal.TryParse(drow["GEB_AMT"].ToString(), out dPreis);
                                    tmpPos.Preis_Amt = dPreis;

                                    Decimal.TryParse(drow["GEB_AMT_ADD"].ToString(), out dPreis);
                                    tmpPos.Preis_Amt_Add = dPreis;
                                }

                                tmpPos.GebMatPflicht = "";
                                tmpPos.UPreis = drow["UPREIS"].ToString();
                                tmpPos.Differrenz = drow["DIFF"].ToString();
                                tmpPos.Konditionstab = drow["KONDTAB"].ToString();
                                tmpPos.Konditionsart = drow["KSCHL"].ToString();
                                tmpPos.GebPak = drow["GBPAK"].ToString();

                                if (drow["CALCDAT"].ToString().Length > 0)
                                {
                                    DateTime tmpDate;
                                    DateTime.TryParse(drow["CALCDAT"].ToString(), out tmpDate);
                                    tmpPos.CalcDat = tmpDate;
                                }
                                else
                                { tmpPos.CalcDat = null; }

                                DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + tmpPos.Matnr.TrimStart('0') + "'");

                                if (MatRow.Length == 1)
                                {
                                    if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                                    {
                                        tmpPos.GebMatPflicht = "X";
                                    }
                                }

                                ZLD_DataContext.Connection.Open();
                                ZLD_DataContext.ZLDPositionsTabelle.InsertOnSubmit(tmpPos);
                                ZLD_DataContext.SubmitChanges();
                                ZLD_DataContext.Connection.Close();
                            }
                        }
                    }

                    var tblBank = new ZLDBankverbindung { id_Kopf = id_Kopf };

                    if (tblAuftragBank.Rows.Count == 0)
                    {
                        tblBank.Inhaber = "";
                        tblBank.IBAN = "";
                        tblBank.Geldinstitut = "";
                        tblBank.SWIFT = "";
                        tblBank.BankKey = "";
                        tblBank.Kontonr = "";
                        tblBank.EinzugErm = false;
                        tblBank.Rechnung = false;
                    }
                    else
                    {
                        DataRow[] drBank = tblAuftragBank.Select("ZULBELN = '" + id_sap.ToString().PadLeft(10, '0') + "'");
                        if (drBank.Length > 0)
                        {
                            tblBank.Inhaber = drBank[0]["KOINH"].ToString();
                            tblBank.IBAN = drBank[0]["IBAN"].ToString();
                            tblBank.Geldinstitut = drBank[0]["EBPP_ACCNAME"].ToString();
                            tblBank.SWIFT = drBank[0]["SWIFT"].ToString();
                            tblBank.BankKey = drBank[0]["BANKL"].ToString();
                            tblBank.Kontonr = drBank[0]["BANKN"].ToString();
                            tblBank.EinzugErm = ZLDCommon.XToBool(drBank[0]["EINZ_JN"].ToString());
                            tblBank.Rechnung = ZLDCommon.XToBool(drBank[0]["RECH_JN"].ToString());
                        }

                    }

                    ZLD_DataContext.Connection.Open();
                    ZLD_DataContext.ZLDBankverbindung.InsertOnSubmit(tblBank);
                    ZLD_DataContext.SubmitChanges();
                    ZLD_DataContext.Connection.Close();

                    var tblKunnadresse = new ZLDKundenadresse { id_Kopf = id_Kopf };

                    if (tblAuftragAdress.Rows.Count == 0)
                    {
                        tblKunnadresse.Kundennr = "";
                        tblKunnadresse.Partnerrolle = "";
                        tblKunnadresse.Name1 = "";
                        tblKunnadresse.Name2 = "";
                        tblKunnadresse.Strasse = "";
                        tblKunnadresse.Ort = "";
                        tblKunnadresse.PLZ = "";
                    }
                    else
                    {
                        DataRow[] drAdresse = tblAuftragAdress.Select("ZULBELN = '" + id_sap.ToString().PadLeft(10, '0') + "'");
                        if (drAdresse.Length > 0)
                        {
                            tblKunnadresse.Kundennr = drAdresse[0]["KUNNR"].ToString();
                            tblKunnadresse.Partnerrolle = drAdresse[0]["PARVW"].ToString();
                            tblKunnadresse.Name1 = drAdresse[0]["LI_NAME1"].ToString();
                            tblKunnadresse.Name2 = drAdresse[0]["LI_NAME2"].ToString();
                            tblKunnadresse.Strasse = drAdresse[0]["LI_STREET"].ToString();
                            tblKunnadresse.Ort = drAdresse[0]["LI_CITY1"].ToString();
                            tblKunnadresse.PLZ = drAdresse[0]["LI_PLZ"].ToString();
                        }
                    }

                    ZLD_DataContext.Connection.Open();
                    ZLD_DataContext.ZLDKundenadresse.InsertOnSubmit(tblKunnadresse);
                    ZLD_DataContext.SubmitChanges();
                    ZLD_DataContext.Connection.Close();
                }

            }
            catch (Exception ex)
            {
                RaiseError("9999", ex.Message);
            }
            finally
            {
                if (ZLD_DataContext.Connection.State == ConnectionState.Open)
                {
                    ZLD_DataContext.Connection.Close();
                }
            }
        }

        /// <summary>
        /// die im Gridview editierten Daten abspeichern
        /// </summary>
        /// <param name="IDRecordset"></param>
        /// <param name="IDPos"></param>
        /// <param name="KennzAbc"></param>
        /// <param name="Griddate"></param>
        /// <param name="Amt"></param>
        public void UpdateDB_GridData(Int32 IDRecordset, Int32 IDPos, String KennzAbc, String Griddate, String Amt)
        {
            var ZLD_DataContext = new ZLDTableClassesDataContext();

            ClearError();

            try
            {
                var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                               where k.id == IDRecordset
                               select k).Single();

                tblKopf.id_user = m_objUser.UserID;
                tblKopf.username = m_objUser.UserName;

                if (IDPos == 10)
                {
                    tblKopf.KennABC = KennzAbc;
                    if (!String.IsNullOrEmpty(Amt))
                    {
                        tblKopf.KreisKZ = Amt.ToUpper();
                    }

                    if (tblKopf.Kennzeichen != tblKopf.KennKZ + "-" + KennzAbc)
                    {
                        tblKopf.Prali_Print = false;
                    }
                    tblKopf.Kennzeichen = tblKopf.KennKZ + "-" + KennzAbc;

                    Griddate = ZLDCommon.toShortDateStr(Griddate);
                    if (ZLDCommon.IsDate(Griddate))
                    {
                        DateTime tmpDate;
                        DateTime.TryParse(Griddate, out tmpDate);
                        tblKopf.Zulassungsdatum = tmpDate;
                    }
                }

                ZLD_DataContext.Connection.Open();
                ZLD_DataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                RaiseError("-9999", ex.Message);
            }
            finally
            {
                if (ZLD_DataContext.Connection.State == ConnectionState.Open)
                {
                    ZLD_DataContext.Connection.Close();
                    ZLD_DataContext.Dispose();
                }
            }
        }

        /// <summary>
        /// die im Gridview editierten Daten abspeichern
        /// </summary>
        /// <param name="IDRecordset"></param>
        /// <param name="IDPos"></param>
        /// <param name="Preis"></param>
        /// <param name="Gebuehr"></param>
        /// <param name="PreisKZ"></param>
        /// <param name="KennzAbc"></param>
        /// <param name="Griddate"></param>
        public void UpdateDB_GridData(Int32 IDRecordset, Int32 IDPos, Decimal Preis,
                            Decimal Gebuehr, Decimal PreisKZ, String KennzAbc, String Griddate)
        {
            var ZLD_DataContext = new ZLDTableClassesDataContext();

            ClearError();

            try
            {
                var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                               where k.id == IDRecordset
                               select k).Single();


                tblKopf.id_user = m_objUser.UserID;
                tblKopf.username = m_objUser.UserName;

                if (IDPos == 10)
                {
                    tblKopf.KennABC = KennzAbc;

                    if (tblKopf.Kennzeichen != tblKopf.KennKZ + "-" + KennzAbc)
                    {
                        tblKopf.Prali_Print = false;
                    }
                    tblKopf.Kennzeichen = tblKopf.KennKZ + "-" + KennzAbc;

                    Griddate = ZLDCommon.toShortDateStr(Griddate);
                    if (ZLDCommon.IsDate(Griddate))
                    {
                        DateTime tmpDate;
                        DateTime.TryParse(Griddate, out tmpDate);
                        tblKopf.Zulassungsdatum = tmpDate;
                    }
                }

                ZLD_DataContext.Connection.Open();
                ZLD_DataContext.SubmitChanges();


                var tblPos = (from p in ZLD_DataContext.ZLDPositionsTabelle
                              where p.id_Kopf == IDRecordset && p.id_pos == IDPos
                              select p);
                if (tblPos.Count() == 1)
                {
                    foreach (var PosRow in tblPos)
                    {
                        PosRow.Preis = Preis;
                        PosRow.GebPreis = Gebuehr;

                        PosRow.PreisKZ = PreisKZ;
                    }
                    ZLD_DataContext.SubmitChanges();
                }

                tblPos = (from p in ZLD_DataContext.ZLDPositionsTabelle
                          where p.id_Kopf == IDRecordset && p.UEPOS == IDPos
                          select p);

                foreach (var PosRow in tblPos)
                {
                    if (PosRow.WebMTArt == "G")
                    {
                        PosRow.Preis = Gebuehr;
                    }

                    if (PosRow.WebMTArt == "K")
                    {
                        PosRow.Preis = PreisKZ;
                    }
                }

                ZLD_DataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                RaiseError("-9999", ex.Message);
            }
            finally
            {
                if (ZLD_DataContext.Connection.State == ConnectionState.Open)
                {
                    ZLD_DataContext.Connection.Close();
                    ZLD_DataContext.Dispose();
                }
            }
        }

        /// <summary>
        /// die im Gridview editierten Daten abspeichern
        /// </summary>
        /// <param name="IDRecordset"></param>
        /// <param name="IDPos"></param>
        /// <param name="Preis"></param>
        /// <param name="Gebuehr"></param>
        /// <param name="Steuern"></param>
        /// <param name="PreisKZ"></param>
        /// <param name="KennzAbc"></param>
        /// <param name="blnBar"></param>
        /// <param name="blnEC"></param>
        /// <param name="Griddate"></param>
        /// <param name="GebAmt"></param>
        public void UpdateDB_GridData(Int32 IDRecordset, Int32 IDPos, Decimal Preis,
                            Decimal Gebuehr, Decimal Steuern, Decimal PreisKZ,
                            String KennzAbc, Boolean blnBar, Boolean blnEC, String Griddate, Decimal GebAmt)
        {
            var ZLD_DataContext = new ZLDTableClassesDataContext();

            ClearError();

            try
            {
                var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                               where k.id == IDRecordset
                               select k).Single();


                tblKopf.id_user = m_objUser.UserID;
                tblKopf.username = m_objUser.UserName;

                if (IDPos == 10)
                {
                    tblKopf.Steuer = Steuern;
                    if (tblKopf.Vorgang != "VZ" && tblKopf.Vorgang != "VE")
                    {
                        if (blnBar)
                        {
                            tblKopf.Bar = true;
                            tblKopf.EC = false;
                            tblKopf.RE = false;
                        }
                        else if (blnEC)
                        {
                            tblKopf.EC = true;
                            tblKopf.Bar = false;
                            tblKopf.RE = false;
                        }
                        else
                        {
                            tblKopf.EC = false;
                            tblKopf.Bar = false;
                            tblKopf.RE = true;
                        }
                    }


                    tblKopf.KennABC = KennzAbc;

                    if (tblKopf.Kennzeichen != tblKopf.KennKZ + "-" + KennzAbc)
                    {
                        tblKopf.Prali_Print = false;
                    }
                    tblKopf.Kennzeichen = tblKopf.KennKZ + "-" + KennzAbc;

                    Griddate = ZLDCommon.toShortDateStr(Griddate);
                    if (ZLDCommon.IsDate(Griddate))
                    {
                        DateTime tmpDate;
                        DateTime.TryParse(Griddate, out tmpDate);
                        tblKopf.Zulassungsdatum = tmpDate;
                    }
                }

                ZLD_DataContext.Connection.Open();
                ZLD_DataContext.SubmitChanges();


                var tblPos = (from p in ZLD_DataContext.ZLDPositionsTabelle
                              where p.id_Kopf == IDRecordset && p.id_pos == IDPos
                              select p);
                if (tblPos.Count() == 1)
                {
                    foreach (var PosRow in tblPos)
                    {
                        PosRow.Preis = Preis;
                        PosRow.GebPreis = Gebuehr;

                        if (m_objUser.Groups[0].Authorizationright == 0) { PosRow.Preis_Amt = GebAmt; }

                        PosRow.PreisKZ = PreisKZ;
                    }
                    ZLD_DataContext.SubmitChanges();
                }

                tblPos = (from p in ZLD_DataContext.ZLDPositionsTabelle
                          where p.id_Kopf == IDRecordset && p.UEPOS == IDPos
                          select p);

                foreach (var PosRow in tblPos)
                {
                    if (PosRow.WebMTArt == "G")
                    {
                        PosRow.Preis = Gebuehr;
                        if (m_objUser.Groups[0].Authorizationright == 0) { PosRow.Preis_Amt = GebAmt; }
                    }

                    if (PosRow.WebMTArt == "K")
                    {
                        PosRow.Preis = PreisKZ;
                    }

                    if (PosRow.WebMTArt == "S")
                    {
                        PosRow.Preis = Steuern;
                    }
                }

                ZLD_DataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                RaiseError("-9999", ex.Message);
            }
            finally
            {
                if (ZLD_DataContext.Connection.State == ConnectionState.Open)
                {
                    ZLD_DataContext.Connection.Close();
                    ZLD_DataContext.Dispose();
                }
            }
        }

        /// <summary>
        /// die im Gridview editierbaren Daten abspeichern
        /// nur Nacherfassung durchzuführende Versandzulassungen (ChangeZLDNachVersandListe.aspx)
        /// </summary>
        /// <param name="IDRecordset"></param>
        /// <param name="IDPos"></param>
        /// <param name="Gebuehr"></param>
        /// <param name="KennzAbc"></param>
        /// <param name="blnBar"></param>
        /// <param name="blnEC"></param>
        /// <param name="Griddate"></param>
        /// <param name="DLBezeichnung"></param>
        public void UpdateDB_GridDataGeb(Int32 IDRecordset, Int32 IDPos, Decimal Gebuehr, String KennzAbc, Boolean blnBar, Boolean blnEC, String Griddate, String DLBezeichnung)
        {
            var ZLD_DataContext = new ZLDTableClassesDataContext();

            ClearError();

            try
            {
                var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                               where k.id == IDRecordset
                               select k).Single();


                tblKopf.id_user = m_objUser.UserID;
                tblKopf.username = m_objUser.UserName;

                if (IDPos == 10)
                {
                    if (blnBar)
                    {
                        tblKopf.Bar = true;
                        tblKopf.EC = false;
                        tblKopf.RE = false;
                    }
                    else if (blnEC)
                    {
                        tblKopf.EC = true;
                        tblKopf.Bar = false;
                        tblKopf.RE = false;
                    }
                    else
                    {
                        tblKopf.EC = false;
                        tblKopf.Bar = false;
                        tblKopf.RE = true;
                    }

                    tblKopf.KennABC = KennzAbc;

                    // Wenn das Kennzeichen geändert wird 
                    // muss das Flag für Prägeliste gedruckt zurückgesetzt werden
                    if (tblKopf.Kennzeichen != tblKopf.KennKZ + "-" + KennzAbc)
                    {
                        tblKopf.Prali_Print = false;
                    }
                    tblKopf.Kennzeichen = tblKopf.KennKZ + "-" + KennzAbc;

                    Griddate = ZLDCommon.toShortDateStr(Griddate);
                    if (ZLDCommon.IsDate(Griddate))
                    {
                        DateTime tmpDate;
                        DateTime.TryParse(Griddate, out tmpDate);
                        tblKopf.Zulassungsdatum = tmpDate;
                    }
                }
                tblKopf.bearbeitet = true;

                ZLD_DataContext.Connection.Open();
                ZLD_DataContext.SubmitChanges();


                var tblPos = (from p in ZLD_DataContext.ZLDPositionsTabelle
                              where p.id_Kopf == IDRecordset && p.id_pos == IDPos
                              select p);

                if (tblPos.Count() == 1)
                {
                    foreach (var PosRow in tblPos)
                    {
                        PosRow.Preis_Amt = Gebuehr;
                    }
                    ZLD_DataContext.SubmitChanges();
                }

                tblPos = (from p in ZLD_DataContext.ZLDPositionsTabelle
                          where p.id_Kopf == IDRecordset && p.UEPOS == IDPos
                          select p);

                foreach (var PosRow in tblPos)
                {
                    if (PosRow.WebMTArt == "G")
                    {
                        PosRow.Preis_Amt = Gebuehr;
                    }
                    PosRow.Matbez = DLBezeichnung;
                }

                ZLD_DataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                RaiseError("-9999", ex.Message);
            }
            finally
            {
                if (ZLD_DataContext.Connection.State == ConnectionState.Open)
                {
                    ZLD_DataContext.Connection.Close();
                    ZLD_DataContext.Dispose();
                }
            }
        }

        /// <summary>
        /// aus Gridview Positionen löschen,  in der Datenbank Loeschkennzeichen setzen
        /// wenn Pos_id == 10(Hauptposition) dann Loeschkennzeichen auch im Kopf setzen
        /// </summary>
        /// <param name="IDRecordset">ID SQL</param>
        /// <param name="LoeschKZ">Löschkennzeichen</param>
        /// <param name="PosID">ID der Position</param>
        public void UpdateDB_LoeschKennzeichen(Int32 IDRecordset, String LoeschKZ, Int32 PosID)
        {
            var connection = new SqlConnection();

            try
            {

                connection.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"];
                connection.Open();
                String query = "";

                if (PosID == 10)
                {
                    query = LoeschKZ == "L" ? ", toDelete='X'" : ", toDelete=''";
                }

                String str = "Update ZLDKopfTabelle Set id_user='" + m_objUser.UserID + "', " +
                             " username='" + m_objUser.UserName + "', " +
                             "bearbeitet= 1 " + query +
                             " Where id = " + IDRecordset;

                var command = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = str
                    };
                command.ExecuteNonQuery();


                str = "Update ZLDPositionsTabelle Set Posloesch='" + LoeschKZ + "'";
                if (PosID != 10 && PosID != 0)
                {
                    str += " Where id_Kopf = " + IDRecordset + " AND id_pos = " + PosID;
                }
                else if (LoeschKZ == "O")
                {
                    str += " Where id_Kopf = " + IDRecordset + " AND Not Posloesch ='L'";
                }
                else
                {
                    str += " Where id_Kopf = " + IDRecordset;
                }

                command = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.Text,
                        CommandText = str
                    };
                command.ExecuteNonQuery();

            }
            finally { connection.Close(); }
        }

        /// <summary>
        /// Löschen der der Vorgänge nach dem absenden
        /// Versandszulassungen (ChangeZLDNachVersandListe.aspx), Annahme AH
        /// </summary>
        /// <param name="IDRecordset"></param>
        public void DeleteRecordSet(Int32 IDRecordset)
        {
            ClearError();
            var ZLD_DataContext = new ZLDTableClassesDataContext();

            try
            {
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
                RaiseError("-9999", ex.Message);
            }
            finally
            {
                if (ZLD_DataContext.Connection.State == ConnectionState.Open)
                {
                    ZLD_DataContext.Connection.Close();
                    ZLD_DataContext.Dispose();
                }
            }
        }

        /// <summary>
        /// die Vorgänge die in SAP erfolgreich angelegt wurden auf abgerechnet setzen
        /// damit sie später nicht mehr angezeigt werden
        /// </summary>
        /// <param name="IDRecordset"></param>
        public void SetAbgerechnet(Int32 IDRecordset)
        {
            ClearError();

            var ZLD_DataContext = new ZLDTableClassesDataContext();

            try
            {
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
                RaiseError("-9999", ex.Message);
            }
            finally
            {
                if (ZLD_DataContext.Connection.State == ConnectionState.Open)
                {
                    ZLD_DataContext.Connection.Close();
                    ZLD_DataContext.Dispose();
                }
            }
        }

        /// <summary>
        /// BEB_Status setzen
        /// </summary>
        /// <param name="IDRecordset"></param>
        /// <param name="neuerStatus"/>
        public void SetBEBStatus(Int32 IDRecordset, string neuerStatus)
        {
            ClearError();

            var ZLD_DataContext = new ZLDTableClassesDataContext();

            try
            {
                var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                               where k.id == IDRecordset
                               select k).Single();

                tblKopf.BEB_STATUS = neuerStatus;
                ZLD_DataContext.Connection.Open();
                ZLD_DataContext.SubmitChanges();
                ZLD_DataContext.Connection.Close();
            }
            catch (Exception ex)
            {
                RaiseError("-9999", ex.Message);
            }
            finally
            {
                if (ZLD_DataContext.Connection.State == ConnectionState.Open)
                {
                    ZLD_DataContext.Connection.Close();
                    ZLD_DataContext.Dispose();
                }
            }
        }

        /// <summary>
        /// Daten aus der Eingabemaske speichern bzw. aktualisieren
        /// </summary>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="clsCommon"></param>
        public void UpdateDB_ZLD(String strSessionID, ZLDCommon clsCommon)
        {
            var ZLD_DataContext = new ZLDTableClassesDataContext();

            ClearError();

            try
            {
                var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                               where k.id == id_Kopf
                               select k).Single();

                tblKopf.id_user = m_objUser.UserID;
                tblKopf.id_session = strSessionID;
                tblKopf.abgerechnet = false;
                tblKopf.username = m_objUser.UserName;
                tblKopf.kundenname = Kundenname;

                tblKopf.kundennr = Kunnr;
                DataRow[] KundeRow = clsCommon.tblKundenStamm.Select("KUNNR='" + Kunnr + "'");

                // Kundenrelevante Daten aus dem Kundenstamm laden
                if (KundeRow.Length == 1)
                {
                    tblKopf.OhneSteuer = KundeRow[0]["OHNEUST"].ToString();
                    tblKopf.PauschalKunde = KundeRow[0]["ZZPAUSCHAL"].ToString();
                    tblKopf.KreisKZ_Direkt = KundeRow[0]["KREISKZ_DIREKT"].ToString();
                }

                tblKopf.WunschKenn = WunschKenn;
                tblKopf.ZusatzKZ = ZLDCommon.BoolToX(ZusatzKZ);
                tblKopf.WunschKZ2 = WunschKZ2;
                tblKopf.WunschKZ3 = WunschKZ3;
                tblKopf.OhneGruenenVersSchein = ZLDCommon.BoolToX(OhneGruenenVersSchein);
                tblKopf.SofortabrechnungErledigt = SofortabrechnungErledigt;
                tblKopf.SofortabrechnungPfad = SofortabrechnungPfad;

                tblKopf.Reserviert = mReserviert;
                tblKopf.Feinstaub = mFeinstaub;
                tblKopf.KreisKZ = KreisKennz;

                // Kennzeichen geändert? dann wieder für die 
                // Prägeliste freigeben
                if (tblKopf.Kennzeichen != Kennzeichen)
                {
                    tblKopf.Prali_Print = false;
                    tblKopf.FLAG = "";
                }

                tblKopf.Kennzeichen = Kennzeichen;
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
                tblKopf.KennKZ = sKennzKZ;
                tblKopf.KennABC = sKennzABC;
                tblKopf.KennzForm = KennzForm;

                if (EinKennz == false)
                {
                    tblKopf.KennzAnz = 2;
                }
                else { tblKopf.KennzAnz = 1; }

                tblKopf.EinKennz = EinKennz;
                tblKopf.Bemerkung = Bemerkung;
                tblKopf.Infotext = Infotext;
                tblKopf.Nachbearbeiten = Nachbearbeiten;
                tblKopf.ONLINE_VG = Onlinevorgang;
                tblKopf.Mobuser = Mobuser;
                tblKopf.POOLNR = Poolnr;
                tblKopf.NrFrachtHin = FrachtNrHin;
                tblKopf.NrFrachtZu = FrachtNrBack;
                tblKopf.Barkunde = Barkunde;
                tblKopf.referenz1 = Ref1;
                tblKopf.referenz2 = Ref2;
                tblKopf.Steuer = Steuer;
                tblKopf.saved = saved;
                tblKopf.bearbeitet = bearbeitet;
                tblKopf.toDelete = toDelete;
                DateTime tmpDate;
                Int32 iMenge = 1;
                DateTime.TryParse(mZulDate, out tmpDate);

                if (tblKopf.Zulassungsdatum != tmpDate)
                {
                    tblKopf.FLAG = "";
                }
                tblKopf.Flieger = bFlieger;
                tblKopf.Zulassungsdatum = tmpDate;
                ZLD_DataContext.Connection.Open();
                ZLD_DataContext.SubmitChanges();
                id_Kopf = tblKopf.id;
                ZLD_DataContext.Connection.Close();

                ZLD_DataContext = new ZLDTableClassesDataContext();
                ZLD_DataContext.Connection.Open();
                // Positionen updaten bzw. neu aufbauen
                if (tblPositionen.Rows.Count > 0)
                {
                    var tblPosCount = (from p in ZLD_DataContext.ZLDPositionsTabelle
                                       where p.id_Kopf == id_Kopf
                                       select p);

                    if (tblPosCount.Count() == tblPositionen.Rows.Count || tblPosCount.Count() < tblPositionen.Rows.Count)
                    {
                        foreach (DataRow drow in tblPositionen.Rows)
                        {
                            var idpos = (Int32) drow["id_pos"];

                            var tblPos = (from p in ZLD_DataContext.ZLDPositionsTabelle
                                          where p.id_Kopf == id_Kopf && p.id_pos == idpos
                                          select p);
                            if (tblPos.Any())
                            {
                                foreach (var PosRow in tblPos)
                                {
                                    if (PosRow.id_Kopf == id_Kopf)
                                    {
                                        PosRow.PosLoesch = drow["PosLoesch"].ToString();

                                        Decimal Preis;
                                        PosRow.Preis = Decimal.TryParse(drow["Preis"].ToString(), out Preis) ? Preis : 0;
                                        PosRow.GebPreis = Decimal.TryParse(drow["GebPreis"].ToString(), out Preis) ? Preis : 0;
                                        PosRow.Preis_Amt = Decimal.TryParse(drow["Preis_Amt"].ToString(), out Preis) ? Preis : 0;
                                        PosRow.Preis_Amt_Add = Decimal.TryParse(drow["Preis_Amt_Add"].ToString(), out Preis) ? Preis : 0;


                                        iMenge = 1;
                                        if (ZLDCommon.IsNumeric(drow["Menge"].ToString()) && drow["Menge"].ToString() != "0")
                                        {
                                            Int32.TryParse(drow["Menge"].ToString(), out iMenge);
                                        }
                                        PosRow.Menge = iMenge.ToString("0");

                                        PosRow.WebMTArt = drow["WEBMTART"].ToString();

                                        String strMatbz = drow["Matbez"].ToString();
                                        if (drow["Matbez"].ToString().Length > 0)
                                        {
                                            if ((iMenge > 1) && (PosRow.WebMTArt != "K") && (PosRow.WebMTArt != "G"))
                                            {
                                                strMatbz = CombineBezeichnungMenge(drow["Matbez"].ToString(), iMenge);
                                            }
                                        }
                                        PosRow.Matbez = strMatbz;

                                        PosRow.PreisKZ = PreisKennz;
                                        PosRow.SDRelevant = drow["SDRelevant"].ToString();

                                        if (ZLDCommon.IsDecimal(drow["UPREIS"].ToString()))
                                        {
                                            PosRow.UPreis = drow["UPREIS"].ToString();
                                        }
                                        else { PosRow.UPreis = "0"; }

                                        if (ZLDCommon.IsDecimal(drow["Differrenz"].ToString()))
                                        {
                                            PosRow.Differrenz = drow["Differrenz"].ToString();
                                        }
                                        else { PosRow.Differrenz = "0"; }

                                        PosRow.Konditionstab = drow["Konditionstab"].ToString();
                                        PosRow.Konditionsart = drow["Konditionsart"].ToString();

                                        if (ZLDCommon.IsDate(drow["CALCDAT"].ToString()))
                                        {
                                            DateTime.TryParse(drow["CALCDAT"].ToString(), out tmpDate);
                                            PosRow.CalcDat = tmpDate;
                                        }

                                        PosRow.GebPak = drow["GebPak"].ToString();
                                        PosRow.Matnr = drow["Matnr"].ToString().PadLeft(18, '0');

                                        DataRow[] MatRow = clsCommon.tblMaterialStamm.Select("MATNR='" + PosRow.Matnr.TrimStart('0') + "'");
                                        PosRow.GebMatPflicht = "";

                                        if (MatRow.Length == 1)
                                        {
                                            if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                                            {
                                                PosRow.GebMatPflicht = "X";
                                            }
                                        }
                                    }
                                }
                                ZLD_DataContext.SubmitChanges();
                            }
                            else
                            {
                                var tblPosNew = new ZLDPositionsTabelle
                                    {
                                        id_Kopf = id_Kopf,
                                        id_pos = idpos,
                                        Matnr = drow["Matnr"].ToString().PadLeft(18, '0')
                                    };


                                iMenge = 1;
                                if (ZLDCommon.IsNumeric(drow["Menge"].ToString()) && drow["Menge"].ToString() != "0")
                                {
                                    Int32.TryParse(drow["Menge"].ToString(), out iMenge);
                                }
                                tblPosNew.Menge = iMenge.ToString("0");

                                String strMatbz = drow["Matbez"].ToString();
                                if (drow["Matbez"].ToString().Length > 0 && (iMenge > 1) && (drow["WEBMTART"].ToString() != "K") && (drow["WEBMTART"].ToString() != "G"))
                                {
                                    strMatbz = CombineBezeichnungMenge(drow["Matbez"].ToString(), iMenge);
                                }
                                tblPosNew.Matbez = strMatbz;

                                tblPosNew.PosLoesch = "";

                                Decimal Preis;
                                tblPosNew.Preis = Decimal.TryParse(drow["Preis"].ToString(), out Preis) ? Preis : 0;
                                tblPosNew.GebPreis = Decimal.TryParse(drow["GebPreis"].ToString(), out Preis) ? Preis : 0;
                                tblPosNew.Preis_Amt = Decimal.TryParse(drow["Preis_Amt"].ToString(), out Preis) ? Preis : 0;
                                tblPosNew.Preis_Amt_Add = Decimal.TryParse(drow["Preis_Amt_Add"].ToString(), out Preis) ? Preis : 0;
                                tblPosNew.UPreis = Decimal.TryParse(drow["Differrenz"].ToString(), out Preis) ? drow["UPREIS"].ToString() : "0";
                                tblPosNew.Differrenz = Decimal.TryParse(drow["Differrenz"].ToString(), out Preis) ? drow["Differrenz"].ToString() : "0";

                                tblPosNew.Konditionstab = drow["Konditionstab"].ToString();
                                tblPosNew.Konditionsart = drow["Konditionsart"].ToString();

                                if (DateTime.TryParse(drow["CALCDAT"].ToString(), out tmpDate))
                                {
                                    tblPosNew.CalcDat = tmpDate;
                                }

                                tblPosNew.PreisKZ = PreisKennz;
                                tblPosNew.GebMatbez = drow["GebMatbez"].ToString();
                                tblPosNew.GebMatnr = drow["GebMatnr"].ToString().PadLeft(18, '0');
                                tblPosNew.GebMatnrSt = drow["GebMatnrSt"].ToString().PadLeft(18, '0');
                                tblPosNew.GebMatBezSt = drow["GebMatBezSt"].ToString();
                                tblPosNew.GebMatBezSt = drow["GebMatBezSt"].ToString();
                                tblPosNew.UEPOS = (Int32)drow["UEPOS"];
                                tblPosNew.WebMTArt = drow["WEBMTART"].ToString();
                                tblPosNew.SDRelevant = drow["SDRelevant"].ToString();
                                tblPosNew.GebMatPflicht = "";
                                tblPosNew.PosLoesch = drow["PosLoesch"].ToString();
                                tblPosNew.GebPak = drow["GebPak"].ToString();
                                DataRow[] MatRow = clsCommon.tblMaterialStamm.Select("MATNR='" + tblPosNew.Matnr.TrimStart('0') + "'");

                                if (MatRow.Length == 1)
                                {
                                    if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                                    {
                                        tblPosNew.GebMatPflicht = "X";
                                    }

                                }

                                ZLD_DataContext.ZLDPositionsTabelle.InsertOnSubmit(tblPosNew);
                                ZLD_DataContext.SubmitChanges();
                            }
                        }
                    }
                    else if (tblPosCount.Count() > tblPositionen.Rows.Count)
                    {

                        foreach (var PosRow in tblPosCount)
                        {
                            DataRow[] drow = tblPositionen.Select("id_pos = " + PosRow.id_pos);
                            if (drow.Length == 1)
                            {
                                PosRow.id_Kopf = id_Kopf;
                                PosRow.id_pos = (Int32)drow[0]["id_pos"];
                                PosRow.Matnr = drow[0]["Matnr"].ToString().PadLeft(18, '0');

                                iMenge = 1;
                                if (ZLDCommon.IsNumeric(drow[0]["Menge"].ToString()) && drow[0]["Menge"].ToString() != "0")
                                {
                                    Int32.TryParse(drow[0]["Menge"].ToString(), out iMenge);
                                }
                                PosRow.Menge = iMenge.ToString("0");

                                String strMatbz = drow[0]["Matbez"].ToString();
                                if (drow[0]["Matbez"].ToString().Length > 0 && (iMenge > 1) && (drow[0]["WEBMTART"].ToString() != "K") && (drow[0]["WEBMTART"].ToString() != "G"))
                                {
                                    strMatbz = CombineBezeichnungMenge(drow[0]["Matbez"].ToString(), iMenge);
                                }
                                PosRow.Matbez = strMatbz;

                                PosRow.PosLoesch = drow[0]["PosLoesch"].ToString();

                                Decimal Preis;
                                PosRow.Preis = Decimal.TryParse(drow[0]["Preis"].ToString(), out Preis) ? Preis : 0;
                                PosRow.GebPreis = Decimal.TryParse(drow[0]["GebPreis"].ToString(), out Preis) ? Preis : 0;
                                PosRow.Preis_Amt = Decimal.TryParse(drow[0]["Preis_Amt"].ToString(), out Preis) ? Preis : 0;
                                PosRow.Preis_Amt_Add = Decimal.TryParse(drow[0]["Preis_Amt_Add"].ToString(), out Preis) ? Preis : 0;

                                PosRow.WebMTArt = drow[0]["WEBMTART"].ToString();
                                PosRow.GebMatbez = drow[0]["GebMatbez"].ToString();
                                PosRow.GebMatnr = drow[0]["GebMatnr"].ToString().PadLeft(18, '0');
                                PosRow.GebMatnrSt = drow[0]["GebMatnrSt"].ToString().PadLeft(18, '0');
                                PosRow.GebMatBezSt = drow[0]["GebMatBezSt"].ToString();
                                PosRow.SDRelevant = drow[0]["SDRelevant"].ToString();
                                PosRow.PosLoesch = drow[0]["PosLoesch"].ToString();
                                PosRow.GebPak = drow[0]["GebPak"].ToString();
                                PosRow.PreisKZ = PreisKennz;

                                PosRow.UPreis = Decimal.TryParse(drow[0]["UPREIS"].ToString(), out Preis) ? drow[0]["UPREIS"].ToString() : "0";
                                PosRow.Differrenz = Decimal.TryParse(drow[0]["Differrenz"].ToString(), out Preis) ? drow[0]["Differrenz"].ToString() : "0";

                                PosRow.Konditionstab = drow[0]["Konditionstab"].ToString();
                                PosRow.Konditionsart = drow[0]["Konditionsart"].ToString();
                                if (ZLDCommon.IsDate(drow[0]["CALCDAT"].ToString()))
                                {
                                    DateTime.TryParse(drow[0]["CALCDAT"].ToString(), out tmpDate);
                                    PosRow.CalcDat = tmpDate;
                                }

                                ZLD_DataContext.SubmitChanges();
                            }
                            else
                            {
                                ZLD_DataContext.ZLDPositionsTabelle.DeleteOnSubmit(PosRow);
                                ZLD_DataContext.SubmitChanges();
                            }
                        }
                        ZLD_DataContext.Connection.Close();

                        foreach (DataRow drow in tblPositionen.Rows)
                        {
                            var idpos = (Int32) drow["id_pos"];

                            var tblPos = (from p in ZLD_DataContext.ZLDPositionsTabelle
                                          where p.id_Kopf == id_Kopf && p.id_pos == idpos
                                          select p);

                            if (tblPos.Any())
                            {
                                foreach (var PosRow in tblPos)
                                {
                                    if (PosRow.id_Kopf == id_Kopf)
                                    {
                                        PosRow.id_Kopf = id_Kopf;
                                        PosRow.id_pos = idpos;
                                        PosRow.Matnr = drow["Matnr"].ToString().PadLeft(18, '0');
                                        
                                        iMenge = 1;
                                        if (ZLDCommon.IsNumeric(drow["Menge"].ToString()) && drow["Menge"].ToString() != "0")
                                        {
                                            Int32.TryParse(drow["Menge"].ToString(), out iMenge);
                                        }
                                        PosRow.Menge = iMenge.ToString("0");

                                        String strMatbz = drow["Matbez"].ToString();

                                        if ((iMenge > 1) && (drow["WEBMTART"].ToString() != "K") && (drow["WEBMTART"].ToString() != "G"))
                                        {
                                            strMatbz = CombineBezeichnungMenge(drow["Matbez"].ToString(), iMenge);
                                        }
                                        PosRow.Matbez = strMatbz;

                                        PosRow.PosLoesch = drow["PosLoesch"].ToString();
                                        PosRow.GebMatbez = drow["GebMatbez"].ToString();
                                        PosRow.GebMatnr = drow["GebMatnr"].ToString().PadLeft(18, '0');
                                        PosRow.GebMatnrSt = drow["GebMatnrSt"].ToString().PadLeft(18, '0');
                                        PosRow.GebMatBezSt = drow["GebMatBezSt"].ToString();
                                        PosRow.SDRelevant = drow["SDRelevant"].ToString();
                                        PosRow.PosLoesch = drow["PosLoesch"].ToString();
                                        PosRow.WebMTArt = drow["WEBMTART"].ToString();
                                        PosRow.GebPak = drow["GebPak"].ToString();
                                        PosRow.PreisKZ = PreisKennz;
                                        
                                        Decimal Preis;
                                        PosRow.Preis = Decimal.TryParse(drow["Preis"].ToString(), out Preis) ? Preis : 0;
                                        PosRow.GebPreis = Decimal.TryParse(drow["GebPreis"].ToString(), out Preis) ? Preis : 0;
                                        PosRow.Preis_Amt = Decimal.TryParse(drow["Preis_Amt"].ToString(), out Preis) ? Preis : 0;
                                        PosRow.Preis_Amt_Add = Decimal.TryParse(drow["Preis_Amt_Add"].ToString(), out Preis) ? Preis : 0;
                                    }
                                }

                                ZLD_DataContext.SubmitChanges();
                            }
                            else
                            {


                                var tblPosNew = new ZLDPositionsTabelle
                                    {
                                        id_Kopf = id_Kopf,
                                        id_pos = idpos,
                                        Matnr = drow["Matnr"].ToString().PadLeft(18, '0')
                                    };
                                
                                iMenge = 1;
                                if (ZLDCommon.IsNumeric(drow["Menge"].ToString()) && drow["Menge"].ToString() != "0")
                                {
                                    Int32.TryParse(drow["Menge"].ToString(), out iMenge);
                                }
                                tblPosNew.Menge = iMenge.ToString("0");

                                string strMatbz = drow["Matbez"].ToString();
                                if ((iMenge > 1) && (drow["WEBMTART"].ToString() != "K") && (drow["WEBMTART"].ToString() != "G"))
                                {
                                    strMatbz = CombineBezeichnungMenge(drow["Matbez"].ToString(), iMenge);
                                }
                                tblPosNew.Matbez = strMatbz;

                                tblPosNew.PosLoesch = "";
                                tblPosNew.GebMatbez = drow["GebMatbez"].ToString();
                                tblPosNew.GebMatnr = drow["GebMatnr"].ToString().PadLeft(18, '0');
                                tblPosNew.GebMatnrSt = drow["GebMatnrSt"].ToString().PadLeft(18, '0');
                                tblPosNew.GebMatBezSt = drow["GebMatBezSt"].ToString();
                                tblPosNew.PreisKZ = PreisKennz;
                                tblPosNew.SDRelevant = drow["SDRelevant"].ToString();
                                tblPosNew.PosLoesch = drow["PosLoesch"].ToString();
                                tblPosNew.WebMTArt = drow["WEBMTART"].ToString();
                                tblPosNew.GebPak = drow["GebPak"].ToString();

                                Decimal Preis;
                                tblPosNew.Preis = Decimal.TryParse(drow["Preis"].ToString(), out Preis) ? Preis : 0;
                                tblPosNew.GebPreis = Decimal.TryParse(drow["GebPreis"].ToString(), out Preis) ? Preis : 0;
                                tblPosNew.Preis_Amt = Decimal.TryParse(drow["Preis_Amt"].ToString(), out Preis) ? Preis : 0;
                                tblPosNew.Preis_Amt_Add = Decimal.TryParse(drow["Preis_Amt_Add"].ToString(), out Preis) ? Preis : 0;
                                tblPosNew.UPreis = Decimal.TryParse(drow["Differrenz"].ToString(), out Preis) ? drow["UPREIS"].ToString() : "0";
                                tblPosNew.Differrenz = Decimal.TryParse(drow["Differrenz"].ToString(), out Preis) ? drow["Differrenz"].ToString() : "0";

                                tblPosNew.Konditionstab = drow["Konditionstab"].ToString();
                                tblPosNew.Konditionsart = drow["Konditionsart"].ToString();
                                if (ZLDCommon.IsDate(drow["CALCDAT"].ToString()))
                                {
                                    DateTime.TryParse(drow["CALCDAT"].ToString(), out tmpDate);
                                    tblPosNew.CalcDat = tmpDate;
                                }
                            }
                        }
                    }

                    ZLD_DataContext = new ZLDTableClassesDataContext();

                    var tblBank = (from b in ZLD_DataContext.ZLDBankverbindung
                                   where b.id_Kopf == KopfID
                                   select b).Single();

                    tblBank.id_Kopf = KopfID;
                    tblBank.IBAN = IBAN;
                    tblBank.SWIFT = SWIFT;
                    tblBank.BankKey = BankKey;
                    tblBank.Kontonr = Kontonr;
                    tblBank.Inhaber = Inhaber;
                    tblBank.Geldinstitut = Geldinstitut.Length > 40 ? Geldinstitut.Substring(0, 40):Geldinstitut;
                    tblBank.EinzugErm = EinzugErm;
                    tblBank.Rechnung = Rechnung;

                    ZLD_DataContext.Connection.Open();
                    ZLD_DataContext.SubmitChanges();
                    ZLD_DataContext.Connection.Close();

                    ZLD_DataContext = new ZLDTableClassesDataContext();
                    var tblKunnadresse = (from k in ZLD_DataContext.ZLDKundenadresse
                                          where k.id_Kopf == KopfID
                                          select k).Single();

                    tblKunnadresse.Partnerrolle = "AG";
                    tblKunnadresse.Name1 = Name1;
                    tblKunnadresse.Name2 = Name2;
                    tblKunnadresse.Ort = Ort;
                    tblKunnadresse.PLZ = PLZ;
                    tblKunnadresse.Strasse = Strasse;
                    ZLD_DataContext.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                RaiseError("-9999",ex.Message);
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
        /// Liefert aus SAP den am nahe gelegensten ZLD oder ext. Dienst
        /// nach KreisKZ (Versandzulassunng). Bapi: Z_ZLD_EXPORT_INFOPOOL
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">AHVersandSelect.aspx</param>
        public void getBestLieferant(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "VoerfZLD.getBestLieferant";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
           
            ClearError();

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    BestLieferanten = new DataTable();
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_INFOPOOL", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_KREISKZ", SelKreis);

                    myProxy.callBapi();

                    BestLieferanten = myProxy.getExportTable("GT_EX_ZUSTLIEF");

                    DataRow NewLief = BestLieferanten.NewRow();
                    NewLief["LIFNR"] = "0";
                    NewLief["NAME1"] = "";
                    BestLieferanten.Rows.Add(NewLief);
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            RaiseError("-9999","Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + 
                                HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
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
            ClearError();
            var ds = new DataSet();
            FillDataSet(IDRecordset, ref ds);
            
            try
            {
                DataTable tmpKopf = ds.Tables[0];
                DataTable tmpPos = ds.Tables[1];
                DataTable tmpBank = ds.Tables[2];
                DataTable tmpKunde = ds.Tables[3];

                KopfTabelle = tmpKopf;
                DataRow dRow = KopfTabelle.Rows[0];

                id_Kopf = (Int32)dRow["id"];
                id_sap = (Int32)dRow["id_sap"];
                abgerechnet = (dRow["abgerechnet"] != null && (Boolean)dRow["abgerechnet"]);
                kundenname = dRow["kundenname"].ToString();
                Kunnr = dRow["kundennr"].ToString().TrimStart('0');
                Ref1 = dRow["referenz1"].ToString();
                Ref2 = dRow["referenz2"].ToString();
                KreisKZ = dRow["KreisKZ"].ToString();
                KreisBez = dRow["KreisBez"].ToString();
                WunschKenn = (dRow["WunschKenn"] != null && (Boolean)dRow["WunschKenn"]);
                ZusatzKZ = ZLDCommon.XToBool(dRow["ZusatzKZ"].ToString());
                WunschKZ2 = dRow["WunschKZ2"].ToString();
                WunschKZ3 = dRow["WunschKZ3"].ToString();
                OhneGruenenVersSchein = ZLDCommon.XToBool(dRow["OhneGruenenVersSchein"].ToString());
                SofortabrechnungErledigt = (dRow["SofortabrechnungErledigt"] != null && (Boolean)dRow["SofortabrechnungErledigt"]);
                SofortabrechnungPfad = dRow["SofortabrechnungPfad"].ToString();
                mReserviert = (dRow["Reserviert"] != null && (Boolean)dRow["Reserviert"]);
                mReserviertKennz = dRow["ReserviertKennz"].ToString();
                mFeinstaub = (dRow["Feinstaub"] != null && (Boolean)dRow["Feinstaub"]);
                mZulDate = dRow["Zulassungsdatum"].ToString();
                if (ZLDCommon.IsDate(mZulDate)) { mZulDate = ((DateTime)dRow["Zulassungsdatum"]).ToShortDateString(); }
                mKennzeichen = dRow["Kennzeichen"].ToString();
                Bemerkung = dRow["Bemerkung"].ToString();
                Infotext = dRow["Infotext"].ToString();
                Nachbearbeiten = (dRow["Nachbearbeiten"] != null && (Boolean)dRow["Nachbearbeiten"]);
                Onlinevorgang = (dRow["ONLINE_VG"] != null && (Boolean)dRow["ONLINE_VG"]);
                Mobuser = dRow["Mobuser"].ToString();
                Poolnr = dRow["POOLNR"].ToString();
                FrachtNrHin = dRow["NrFrachtHin"].ToString();
                FrachtNrBack = dRow["NrFrachtZu"].ToString();
                Poolnr = dRow["POOLNR"].ToString();
                KennzForm = dRow["KennzForm"].ToString();
                EinKennz = (dRow["EinKennz"] != null && (Boolean)dRow["EinKennz"]);
                mEC = (dRow["EC"] != null && (Boolean)dRow["EC"]);
                mBar = (dRow["Bar"] != null && (Boolean)dRow["Bar"]);
                RE = (dRow["RE"] != null && (Boolean)dRow["RE"]);
                PauschalKunde = dRow["PauschalKunde"].ToString();
                OhneSteuer = dRow["OhneSteuer"].ToString();
                saved = (dRow["saved"] != null && (Boolean)dRow["saved"]);
                toDelete = dRow["toDelete"].ToString();
                Barkunde = (dRow["Barkunde"] != null && (Boolean)dRow["Barkunde"]);
                bearbeitet = (dRow["bearbeitet"] != null && (Boolean)dRow["bearbeitet"]);
                Vorgang = dRow["Vorgang"].ToString();
                Barcode = dRow["Barcode"].ToString();
                bFlieger = (dRow["FLIEGER"] != null && (Boolean)dRow["FLIEGER"]);
                NrLangText = dRow["LTEXT_NR"].ToString();
                
                Decimal Preis;
                Steuer = Decimal.TryParse(dRow["Steuer"].ToString(), out Preis) ? Preis : 0;
               
                tblPositionen = tmpPos;

                foreach (DataRow PosRow in tblPositionen.Rows)
                {
                    PosRow["Matnr"] = PosRow["Matnr"].ToString().TrimStart('0');
                    if (PosRow["id_pos"].ToString() == "10")
                    {
                        PreisKennz =Decimal.TryParse(PosRow["PreisKZ"].ToString(), out Preis) ? Preis:0;
                    }
                }

                Bankverbindung = tmpBank;

                SWIFT = Bankverbindung.Rows[0]["SWIFT"].ToString();
                IBAN = Bankverbindung.Rows[0]["IBAN"].ToString();
                BankKey = Bankverbindung.Rows[0]["BankKey"].ToString();
                Kontonr = Bankverbindung.Rows[0]["Kontonr"].ToString();
                Inhaber = Bankverbindung.Rows[0]["Inhaber"].ToString();
                Geldinstitut = Bankverbindung.Rows[0]["Geldinstitut"].ToString();
                EinzugErm = (Bankverbindung.Rows[0]["EinzugErm"] != null && (Boolean)Bankverbindung.Rows[0]["EinzugErm"]);
                Rechnung = (Bankverbindung.Rows[0]["Rechnung"] != null && (Boolean)Bankverbindung.Rows[0]["Rechnung"]);


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
            var connection = new SqlConnection();
            try
            {
                connection.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"];
                connection.Open();

                var adapter = new SqlDataAdapter(
                                            "SELECT * FROM ZLDKopfTabelle as KopfTabelle where id=" + RecordID + " AND id_user=" + m_objUser.UserID + " AND Vorgang='" + Vorgang + "';" +

                                            "SELECT * FROM ZLDPositionsTabelle As PositionsTabelle where id_Kopf=" + RecordID + "; " +

                                            "SELECT * FROM ZLDBankverbindung As Bankverbindung where id_Kopf=" + RecordID + ";  " +

                                            "SELECT * FROM ZLDKundenadresse as Kundenadresse where id_Kopf=" + RecordID + ";  ", connection);

                adapter.TableMappings.Add("ZLDKopfTabelle", "KopfTabelle");
                adapter.TableMappings.Add("ZLDPositionsTabelle", "PositionsTabelle");
                adapter.TableMappings.Add("ZLDBankverbindung", "Bankverbindung");
                adapter.TableMappings.Add("ZLDKundenadresse", "Kundenadresse");
                adapter.Fill(ds);
            }
            finally 
            { 
                connection.Close(); 
            }
        }

        /// <summary>
        /// Läd die Daten für Nacherfassung und Nacherfassung beauftragter Versandzulassung
        /// Nur für die tabellarische Übersicht (ChangeZLDNachListe.aspx)
        /// </summary>
        /// <param name="sVorgang">Vorgang</param>
        public void LadeNacherfassungDB_ZLDNew(String sVorgang)
        {
            ClearError();
            var connection = new SqlConnection
                {
                    ConnectionString = ConfigurationManager.AppSettings["Connectionstring"]
                };

            try
            {

                tblListe = new DataTable();
                foreach (DataRow SapRow in SapResultTable.Rows)
                {
                    var tmpTable = new DataTable();
                    var command = new SqlCommand();
                    var adapter = new SqlDataAdapter();

                    command.CommandText = "SELECT dbo.ZLDKopfTabelle.*, dbo.ZLDPositionsTabelle.Matnr, dbo.ZLDPositionsTabelle.Matbez, dbo.ZLDPositionsTabelle.id_pos," +
                                          " dbo.ZLDPositionsTabelle.Preis, dbo.ZLDPositionsTabelle.GebPreis, dbo.ZLDPositionsTabelle.Preis_Amt, dbo.ZLDPositionsTabelle.Preis_Amt_Add, dbo.ZLDPositionsTabelle.PreisKZ," +
                                          " dbo.ZLDPositionsTabelle.PosLoesch, dbo.ZLDPositionsTabelle.GebMatPflicht, dbo.ZLDPositionsTabelle.GebMatnr, " +
                                          "dbo.ZLDPositionsTabelle.GebMatnrSt, dbo.ZLDPositionsTabelle.Menge, dbo.ZLDPositionsTabelle.GebPak" +
                                          " FROM dbo.ZLDKopfTabelle INNER JOIN" +
                                          " dbo.ZLDPositionsTabelle ON dbo.ZLDKopfTabelle.id = dbo.ZLDPositionsTabelle.id_Kopf" +
                                          " WHERE id_sap = @id_sap AND testuser = @testuser" +
                                          " AND WebMTArt = 'D' AND abgerechnet = 0 AND Flieger = @Flieger";

                    command.Parameters.AddWithValue("@testuser", m_objUser.IsTestUser);
                    command.Parameters.AddWithValue("@Flieger", SelFlieger);

                    Int32 tmpID_SAP;
                    Int32.TryParse(SapRow["ZULBELN"].ToString(), out tmpID_SAP);
                    command.Parameters.AddWithValue("@id_sap", tmpID_SAP);

                    command.CommandText += " ORDER BY  dbo.ZLDKopfTabelle.id_sap, dbo.ZLDPositionsTabelle.id_pos,kundenname asc, referenz1 asc, Kennzeichen";
                    
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    adapter.SelectCommand = command;

                    adapter.Fill(tmpTable);

                    tmpTable.Columns.Add("Status", typeof(String));
                    foreach (DataRow rowListe in tmpTable.Rows)
                    {
                        rowListe["Status"] = "";
                    }

                    if (tblListe.Columns.Count == 0)
                    {
                        tblListe = tmpTable.Copy();
                    }
                    else
                    {
                        for (int iRow = 0; iRow < tmpTable.Rows.Count; iRow++)
                        {
                            DataRow RowNew = tblListe.NewRow();
                            for (int i = 0; i < tmpTable.Columns.Count; i++)
                            {
                                RowNew[i] = tmpTable.Rows[iRow][i];
                            }
                            tblListe.Rows.Add(RowNew);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RaiseError("9999","Fehler beim Laden der Eingabeliste: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Übergeben der Daten an SAP aus den SQL-Tabellen
        /// Speichern in SAP(Nacherfassung und Nacherfassung beauftragter Versandzulassung). Bapi: Z_ZLD_IMP_NACHERF
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID"></param>
        /// <param name="page"></param>
        /// <param name="tblStvaStamm"></param>
        /// <param name="tblMaterialStamm"></param>
        public void SaveZLDNacherfassung(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblStvaStamm, DataTable tblMaterialStamm)
        {
            m_strClassAndMethod = "NacherfZLD.SaveZLDNacherfassung";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
          
            ClearError();

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                var ZLD_DataContext = new ZLDTableClassesDataContext();

                try
                {
                    var bapiName = "Z_ZLD_IMP_NACHERF";

                    if (SelSofortabrechnung)
                        bapiName = "Z_ZLD_IMPORT_SOFORT_ABRECHNUNG";

                    DynSapProxyObj myProxy = DynSapProxy.getProxy(bapiName, ref m_objApp, ref m_objUser, ref page);

                    DataTable importAuftrag = myProxy.getImportTable("GT_IMP_BAK");

                    DataTable importPos = myProxy.getImportTable("GT_IMP_POS_S01");
                    DataTable importBank = myProxy.getImportTable("GT_IMP_BANK");
                    DataTable importAdresse = myProxy.getImportTable("GT_IMP_ADRS");
                    Int32 LastID = 0;
                    Int32 OKLoeschCount = 0;
                    
                    foreach (DataRow SaveRow in tblListe.Select(tblListe.DefaultView.RowFilter))
                    {
                        if ((SaveRow["PosLoesch"].ToString() == "O" && SaveRow["id_Pos"].ToString() == "10")
                            || (SaveRow["PosLoesch"].ToString() == "L" && SaveRow["id_Pos"].ToString() == "10"))
                        {
                            var tmpID = (Int32)SaveRow["id"];
                            OKLoeschCount++;
                            if (LastID != tmpID)
                            {

                                var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                                               where k.id == tmpID
                                               select k).Single();
                                DataRow importAuftrRow = importAuftrag.NewRow();

                                var aktion = "Save";

                                if (SelSofortabrechnung)
                                    aktion = "Update";

                                insertDatatoSAPTable(tblKopf, ref importAuftrRow, tblStvaStamm, aktion);

                                DataRow importRow;
                                //----------------
                                var tblPosCount = (from p in ZLD_DataContext.ZLDPositionsTabelle
                                                   where p.id_Kopf == tmpID
                                                   select p);

                                foreach (var PosRow in tblPosCount)
                                {
                                    importRow = importPos.NewRow();
                                    importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                    importRow["ZULPOSNR"] = PosRow.id_pos.ToString().PadLeft(6, '0');
                                    importRow["UEPOS"] = PosRow.UEPOS.ToString().PadLeft(6, '0');
                                    importRow["SD_REL"] = PosRow.SDRelevant;
                                    importRow["GBPAK"] = ""; //nur Gebühren haben das Kennzeichen "Gebührenpaket"
                                    importRow["GEB_AMT_ADD_C"] = PosRow.Preis_Amt_Add;
                                    importRow["MENGE_C"] = PosRow.Menge;
                                    importRow["WEBMTART"] = PosRow.WebMTArt;

                                    DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + PosRow.Matnr.TrimStart('0') + "'");
                                    if (MatRow.Length == 1)
                                    {
                                        if (MatRow[0]["KENNZREL"].ToString() == "X")
                                        {
                                            if (tblKopf.EinKennz == true)
                                            {
                                                importAuftrRow["KENNZANZ"] = "1";
                                            }
                                            else
                                            {
                                                importAuftrRow["KENNZANZ"] = "2";
                                            }
                                        }
                                        importRow["NULLPREIS_OK"] = MatRow[0]["NULLPREIS_OK"].ToString();
                                    }
                                    importRow["MATNR"] = PosRow.Matnr.PadLeft(18, '0');
                                    importRow["MAKTX"] = PosRow.Matbez;
                                    importRow["LOEKZ"] = "";
                                    if (PosRow.UPreis != null)
                                    { importRow["UPREIS_C"] = PosRow.UPreis; }
                                    if (PosRow.Differrenz != null)
                                    { importRow["DIFF_C"] = PosRow.Differrenz; }
                                    if (PosRow.Konditionstab != null)
                                    { importRow["KONDTAB"] = PosRow.Konditionstab; }
                                    if (PosRow.Konditionsart != null)
                                    { importRow["KSCHL"] = PosRow.Konditionsart; }
                                    if (ZLDCommon.IsDate(PosRow.CalcDat.ToString()))
                                    {
                                        importRow["CALCDAT"] = PosRow.CalcDat;
                                    }


                                    if (PosRow.PosLoesch == "L"){ importRow["LOEKZ"] = "X"; }

                                    importRow["PREIS_C"] = PosRow.Preis.ToString();

                                    if (PosRow.id_pos == 10)
                                    {
                                        PreisKennz = (Decimal)PosRow.PreisKZ;
                                    }

                                    importRow["WEBMTART"] = PosRow.WebMTArt;

                                    if (PosRow.WebMTArt == "G")
                                    {
                                        importRow["GEB_AMT_C"] = PosRow.Preis_Amt.ToString();
                                        // falls Flag false soll hier der Preis aus der Gebühr stehen 
                                        if (m_objUser.Groups[0].Authorizationright == 1) { importRow["GEB_AMT_C"] = PosRow.Preis.ToString(); }
                                        importRow["GBPAK"] = PosRow.GebPak; //nur Gebühren haben das Kennzeichen "Gebührenpaket"
                                        if (tblKopf.Vorgang == "VZ" || tblKopf.Vorgang == "AV")
                                        {
                                            importRow["GEB_AMT_C"] = PosRow.Preis_Amt.ToString();
                                        }
                                    }

                                    if (PosRow.UEPOS != 10)
                                    {
                                        if (PosRow.WebMTArt == "S")
                                        {
                                            importRow["LOEKZ"] = "X";
                                        }

                                    }
                                    else if (PosRow.WebMTArt == "S" && PosRow.Preis == 0)
                                    {
                                        importRow["LOEKZ"] = "X";
                                    }
                                    else if (PosRow.WebMTArt == "S" && PosRow.Preis > 0)
                                    {
                                        importRow["LOEKZ"] = "";
                                    }

                                    if (PosRow.WebMTArt == "K" && PosRow.Preis == 0)
                                    {
                                        importRow["LOEKZ"] = "X";
                                    }
                                    else if (PosRow.WebMTArt == "K" && PosRow.Preis > 0)
                                    {
                                        importRow["LOEKZ"] = "";
                                    }

                                    importPos.Rows.Add(importRow);
                                }

                                importAuftrag.Rows.Add(importAuftrRow);
                                var tblBank = (from b in ZLD_DataContext.ZLDBankverbindung
                                               where b.id_Kopf == tmpID
                                               select b).Single();

                                if (tblBank.Inhaber != null)
                                {
                                    importRow = importBank.NewRow();
                                    if (tblBank.Inhaber.Length > 0)
                                    {
                                        importRow["MANDT"] = "010";
                                        importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                        importRow["SWIFT"] = tblBank.SWIFT;
                                        importRow["IBAN"] = tblBank.IBAN;
                                        importRow["BANKL"] = tblBank.BankKey;
                                        importRow["BANKN"] = tblBank.Kontonr;
                                        importRow["EBPP_ACCNAME"] = tblBank.Geldinstitut;
                                        importRow["KOINH"] = tblBank.Inhaber;
                                        importRow["EINZ_JN"] = ZLDCommon.BoolToX(tblBank.EinzugErm);
                                        importRow["RECH_JN"] = ZLDCommon.BoolToX(tblBank.Rechnung);
                                        importRow["LOEKZ"] = "";
                                    }
                                    else
                                    {
                                        importRow["MANDT"] = "010";
                                        importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                        importRow["LOEKZ"] = "X";
                                    }
                                    importBank.Rows.Add(importRow);
                                }

                                var tblKunnadresse = (from k in ZLD_DataContext.ZLDKundenadresse
                                                      where k.id_Kopf == tmpID
                                                      select k).Single();

                                if (tblKunnadresse.Name1 != null)
                                {
                                    importRow = importAdresse.NewRow();
                                    importRow["MANDT"] = "010";
                                    importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                    importRow["KUNNR"] = tblKopf.kundennr.PadLeft(10, '0');
                                    importRow["PARVW"] = "AG";

                                    if (tblKunnadresse.Name1.Length > 0)
                                    {
                                        importRow["LI_NAME1"] = tblKunnadresse.Name1;
                                        importRow["LI_NAME2"] = tblKunnadresse.Name2;
                                        importRow["LI_PLZ"] = tblKunnadresse.PLZ;
                                        importRow["LI_CITY1"] = tblKunnadresse.Ort;
                                        importRow["LI_STREET"] = tblKunnadresse.Strasse;
                                        importRow["LOEKZ"] = "";
                                    }
                                    else
                                    {
                                        importRow["LOEKZ"] = "X";
                                    }

                                    importAdresse.Rows.Add(importRow);
                                }
                                LastID = tmpID;
                            }
                        }
                    }

                    if (OKLoeschCount > 0)
                    {
                        myProxy.callBapi();

                        tblErrors = myProxy.getExportTable("GT_EX_ERRORS");

                        if (tblErrors.Rows.Count > 0)
                        {
                            m_intStatus = -9999;

                            foreach (DataRow rowError in tblErrors.Rows)
                            {
                                Int32 idsap;
                                Int32.TryParse(rowError["ZULBELN"].ToString(), out idsap);
                                DataRow[] rowListe = tblListe.Select("id_sap=" + idsap);
                                if (rowListe.Length > 0)
                                {
                                    rowListe[0]["Status"] = rowError["ERROR_TEXT"].ToString();
                                }
                            }
                        }
                        else if (SelSofortabrechnung)
                        {
                            // Beim Sofortabrechnungs-Bapi erscheinen fehlerfreie Vorgänge nicht in der Fehlertabelle, deshalb hier Status <> 0 setzen
                            m_intStatus = -9999;
                        }

                        if (SelSofortabrechnung)
                        {
                            SofortabrechnungVerzeichnis = myProxy.getExportParameter("G_SA_PFAD");
                        }
                        else
                        {
                            tblBarquittungen = myProxy.getExportTable("GT_BARQ");
                        }
                    }
                    else
                    {
                        RaiseError("-7777","Es sind keine Vorgänge mit \"O\" oder \"L\" markiert");
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            RaiseError("-5555","Es ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
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
        /// Füllt Daten in die SAP-Kopftabelle
        /// </summary>
        /// <param name="tblKopf"></param>
        /// <param name="importAuftrRow"></param>
        /// <param name="tblStvaStamm"></param>
        /// <param name="aktion">Update oder Insert</param>
        private void insertDatatoSAPTable(ZLDKopfTabelle tblKopf, ref DataRow importAuftrRow, DataTable tblStvaStamm, String aktion)
        {
            importAuftrRow["MANDT"] = "010";
            importAuftrRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
            importAuftrRow["VBELN"] = "";
            importAuftrRow["VKORG"] = tblKopf.Filiale.Substring(0, 4);
            importAuftrRow["VKBUR"] = tblKopf.Filiale.Substring(4, 4);
            importAuftrRow["ERNAM"] = m_objUser.UserName.PadLeft(12);
            importAuftrRow["ERDAT"] = DateTime.Now;
            importAuftrRow["FLAG"] = tblKopf.FLAG;
            importAuftrRow["VZB_STATUS"] = "";

            importAuftrRow["STATUS"] = tblKopf.STATUS_Versand;
            importAuftrRow["BARCODE"] = tblKopf.Barcode;
            importAuftrRow["KUNNR"] = tblKopf.kundennr.PadLeft(10, '0');
            importAuftrRow["ZZREFNR1"] = tblKopf.referenz1;
            importAuftrRow["ZZREFNR2"] = tblKopf.referenz2;
            importAuftrRow["ZZREFNR3"] = tblKopf.ZZREFNR3;
            importAuftrRow["ZZREFNR4"] = tblKopf.ZZREFNR4;
            importAuftrRow["ZZREFNR5"] = tblKopf.ZZREFNR5;
            importAuftrRow["ZZREFNR6"] = tblKopf.ZZREFNR6;
            importAuftrRow["ZZREFNR7"] = tblKopf.ZZREFNR7;
            importAuftrRow["ZZREFNR8"] = tblKopf.ZZREFNR8;
            importAuftrRow["ZZREFNR9"] = tblKopf.ZZREFNR9;
            importAuftrRow["ZZREFNR10"] = tblKopf.ZZREFNR10;
            importAuftrRow["KREISKZ"] = tblKopf.KreisKZ;
            DataRow[] RowStva = tblStvaStamm.Select("KREISKZ='" + tblKopf.KreisKZ + "'");
            if (RowStva.Length == 1)
            {
                importAuftrRow["KREISBEZ"] = RowStva[0]["KREISBEZ"];
            }
            else
            {
                importAuftrRow["KREISBEZ"] = tblKopf.KreisBez;
            }
            importAuftrRow["WUNSCHKENN_JN"] = ZLDCommon.BoolToX(tblKopf.WunschKenn);
            importAuftrRow["ZUSKENNZ"] = tblKopf.ZusatzKZ;
            importAuftrRow["WU_KENNZ2"] = tblKopf.WunschKZ2;
            importAuftrRow["WU_KENNZ3"] = tblKopf.WunschKZ3;
            importAuftrRow["O_G_VERSSCHEIN"] = tblKopf.OhneGruenenVersSchein;
            importAuftrRow["SOFORT_ABR_ERL"] = ZLDCommon.BoolToX(tblKopf.SofortabrechnungErledigt);
            importAuftrRow["SA_PFAD"] = tblKopf.SofortabrechnungPfad;

            importAuftrRow["RESERVKENN_JN"] = ZLDCommon.BoolToX(tblKopf.Reserviert);
            importAuftrRow["RESERVKENN"] = tblKopf.ReserviertKennz;
            importAuftrRow["FEINSTAUBAMT"] = ZLDCommon.BoolToX(tblKopf.Feinstaub);
            
            if (tblKopf.Zulassungsdatum != null)
            {
                importAuftrRow["ZZZLDAT"] = tblKopf.Zulassungsdatum;
            }

            importAuftrRow["ZZKENN"] = tblKopf.Kennzeichen;
            importAuftrRow["BLTYP"] = tblKopf.Vorgang;
            importAuftrRow["KENNZTYP"] = tblKopf.Kennztyp;
            importAuftrRow["KENNZFORM"] = tblKopf.KennzForm;
            importAuftrRow["KENNZANZ"] = "0";
            importAuftrRow["EINKENN_JN"] = ZLDCommon.BoolToX(tblKopf.EinKennz);
            importAuftrRow["BEMERKUNG"] = tblKopf.Bemerkung;
            importAuftrRow["INFO_TEXT"] = tblKopf.Infotext;
            importAuftrRow["NACHBEARBEITEN"] = ZLDCommon.BoolToX(tblKopf.Nachbearbeiten);
            importAuftrRow["ONLINE_VG"] = ZLDCommon.BoolToX(tblKopf.ONLINE_VG);
            importAuftrRow["MOBUSER"] = tblKopf.Mobuser;
            importAuftrRow["POOLNR"] = tblKopf.POOLNR;
            importAuftrRow["ZL_RL_FRBNR_HIN"] = tblKopf.NrFrachtHin;
            importAuftrRow["ZL_RL_FRBNR_ZUR"] = tblKopf.NrFrachtZu;

            importAuftrRow["EC_JN"] = ZLDCommon.BoolToX(tblKopf.EC);
            importAuftrRow["BAR_JN"] = ZLDCommon.BoolToX(tblKopf.Bar);
            importAuftrRow["RE_JN"] = ZLDCommon.BoolToX(tblKopf.RE);
            importAuftrRow["KUNDEBAR_JN"] = ZLDCommon.BoolToX(tblKopf.Barkunde);
            if (tblKopf.VersandVKBUR.Length > 0)
            {
                importAuftrRow["VZD_VKBUR"] = tblKopf.VersandVKBUR.Substring(4, 4);
            }

            importAuftrRow["ZL_LIFNR"] = tblKopf.KunnrLF;

            if (aktion == "Save")
            {
                if (tblKopf.Vorgang == "VZ" || tblKopf.Vorgang == "VE")
                {
                    importAuftrRow["VZERDAT"] = DateTime.Now.ToShortDateString();
                    importAuftrRow["VZB_STATUS"] = "VD";
                }
                if (tblKopf.Vorgang == "AV" || tblKopf.Vorgang == "AX")
                {
                    importAuftrRow["VZERDAT"] = DateTime.Now.ToShortDateString();
                    importAuftrRow["VZB_STATUS"] = "VD";
                }
            }
            else if (aktion == "Update")
            {
                if (tblKopf.Vorgang == "VZ" || tblKopf.Vorgang == "VE")
                {
                    importAuftrRow["VZB_STATUS"] = tblKopf.VZB_STATUS;
                }
                if (tblKopf.Vorgang == "AV" || tblKopf.Vorgang == "AX")
                {
                    importAuftrRow["VZB_STATUS"] = tblKopf.VZB_STATUS;
                }
                // Im Modus "AnnahmeAH" den KSTATUS nicht in SAP auf "B" setzen (hier wird nur beim Absenden in SAP gespeichert). 
                // Für die Nacherfassung dieser Vorgänge darf der KSTATUS initial auch nicht B sein, weil der Vorgang dort
                // sonst schon als bearbeitet angezeigt wird
                if (SelAnnahmeAH || SelSofortabrechnung)
                {
                    importAuftrRow["KSTATUS"] = "O";
                }
                else
                {
                    importAuftrRow["KSTATUS"] = "B";
                }
            }


            if (tblKopf.toDelete != null)
            {
                if (tblKopf.toDelete == "X")
                {
                    importAuftrRow["LOEKZ"] = "X";
                }
                else
                {
                    importAuftrRow["LOEKZ"] = "";
                }

            }
            else
            {
                importAuftrRow["LOEKZ"] = "";
            }

            importAuftrRow["ZAHLART"] = tblKopf.Zahlungsart;
            importAuftrRow["PRALI_PRINT"] = ZLDCommon.BoolToX(tblKopf.Prali_Print);

            importAuftrRow["VH_KENNZ_RES"] = ZLDCommon.BoolToX(tblKopf.VorhKennzReserv);
            importAuftrRow["ZBII_ALT_NEU"] = ZLDCommon.BoolToX(tblKopf.ZBII_ALT_NEU);
            importAuftrRow["KENNZ_VH"] = ZLDCommon.BoolToX(tblKopf.KennzVH);
            importAuftrRow["VK_KUERZEL"] = tblKopf.VKKurz;
            importAuftrRow["KUNDEN_REF"] = tblKopf.interneRef;
            importAuftrRow["KUNDEN_NOTIZ"] = tblKopf.KundenNotiz;
            importAuftrRow["ALT_KENNZ"] = tblKopf.KennzAlt;
            importAuftrRow["VE_ERNAM"] = tblKopf.Vorerfasser;

            if (ZLDCommon.IsDate(tblKopf.VorerfDatum.ToString()))
            {
                importAuftrRow["VE_ERDAT"] = tblKopf.VorerfDatum;
            }

            if (!String.IsNullOrEmpty(tblKopf.VorerfUhrzeit))
            {
                importAuftrRow["VE_ERZEIT"] = tblKopf.VorerfUhrzeit;
            }

            if (ZLDCommon.IsDate(tblKopf.Still_Datum.ToString()))
            {
                importAuftrRow["STILL_DAT"] = tblKopf.Still_Datum;
            }

            importAuftrRow["FLIEGER"] = ZLDCommon.BoolToX(tblKopf.Flieger);
            importAuftrRow["UMBU"] = ZLDCommon.BoolToX(tblKopf.UMBU);
            importAuftrRow["ABGESAGT"] = ZLDCommon.BoolToX(tblKopf.ABGESAGT);
            importAuftrRow["RUECKBU"] = ZLDCommon.BoolToX(tblKopf.RUECKBU);
            importAuftrRow["ZZEVB"] = tblKopf.EVB;
            importAuftrRow["OBJECT_ID"] = tblKopf.OBJECT_ID;
            importAuftrRow["FAHRZ_ART"] = tblKopf.FAHRZ_ART;
            importAuftrRow["MNRESW"] = ZLDCommon.BoolToX(tblKopf.MNRESW);
            importAuftrRow["SERIE"] = ZLDCommon.BoolToX(tblKopf.SERIE);
            importAuftrRow["SAISON_KNZ"] = ZLDCommon.BoolToX(tblKopf.SAISON_KNZ);
            importAuftrRow["SAISON_BEG"] = tblKopf.SAISON_BEG;
            importAuftrRow["SAISON_END"] = tblKopf.SAISON_END;
            importAuftrRow["TUEV_AU"] = tblKopf.TUEV_AU;
            importAuftrRow["KURZZEITVS"] = tblKopf.KURZZEITVS;
            importAuftrRow["ZOLLVERS"] = tblKopf.ZOLLVERS;
            importAuftrRow["ZOLLVERS_DAUER"] = tblKopf.ZOLLVERS_DAUER;
            importAuftrRow["VORFUEHR"] = ZLDCommon.BoolToX(tblKopf.VORFUEHR);

            if (ZLDCommon.IsDate(tblKopf.HALTE_DAUER.ToString()))
            {
                importAuftrRow["HALTE_DAUER"] = tblKopf.HALTE_DAUER;
            }

            importAuftrRow["LTEXT_NR"] = tblKopf.LTEXT_NR;
            importAuftrRow["BEB_STATUS"] = tblKopf.BEB_STATUS;
            importAuftrRow["ZZREFNR3"] = tblKopf.ZZREFNR3;
            importAuftrRow["ZZREFNR4"] = tblKopf.ZZREFNR4;
            importAuftrRow["ZZREFNR5"] = tblKopf.ZZREFNR5;
            importAuftrRow["ZZREFNR6"] = tblKopf.ZZREFNR6;
            importAuftrRow["ZZREFNR7"] = tblKopf.ZZREFNR7;
            importAuftrRow["ZZREFNR8"] = tblKopf.ZZREFNR8;
            importAuftrRow["ZZREFNR9"] = tblKopf.ZZREFNR9;
            importAuftrRow["ZZREFNR10"] = tblKopf.ZZREFNR10;
            importAuftrRow["ZZEVB"] = tblKopf.EVB;
            importAuftrRow["AH_DOKNAME"] = tblKopf.AH_DOKNAME;

        }

        /// <summary>
        /// Übergeben der Daten an SAP aus den SQL-Tabellen
        /// Speichern in SAP(Nacherfassung durchzuführender Versandzulassung). Bapi: Z_ZLD_IMP_NACHERF_DZLD
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID"></param>
        /// <param name="page"></param>
        /// <param name="tblStvaStamm"></param>
        /// <param name="tblMaterialStamm"></param>
        public void SaveDZLDNacherfassung(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblStvaStamm, DataTable tblMaterialStamm)
        {
            m_strClassAndMethod = "NacherfZLD.SaveDZLDNacherfassung";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
           
            ClearError();

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                var ZLD_DataContext = new ZLDTableClassesDataContext();
                
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_IMP_NACHERF_DZLD", ref m_objApp, ref m_objUser, ref page);

                    DataTable importAuftrag = myProxy.getImportTable("GT_IMP_BAK");
                    DataTable importPos = myProxy.getImportTable("GT_IMP_POS_S01");

                    Int32 LastID = 0;
                    Int32 OKLoeschCount = 0;

                    foreach (DataRow SaveRow in tblListe.Select(tblListe.DefaultView.RowFilter))
                    {
                        if ((SaveRow["PosLoesch"].ToString() == "O") || (SaveRow["PosLoesch"].ToString() == "L"))
                        {
                            var tmpID = (Int32)SaveRow["id"];
                            OKLoeschCount++;

                            if (LastID != tmpID)
                            {
                                var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                                               where k.id == tmpID
                                               select k).Single();

                                DataRow importAuftrRow = importAuftrag.NewRow();

                                importAuftrRow["MANDT"] = "010";
                                importAuftrRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                importAuftrRow["VBELN"] = "";
                                importAuftrRow["VKORG"] = tblKopf.Filiale.Substring(0, 4);
                                importAuftrRow["VKBUR"] = tblKopf.Filiale.Substring(4, 4);
                                importAuftrRow["ERNAM"] = m_objUser.UserName.PadLeft(12);
                                importAuftrRow["ERDAT"] = DateTime.Now;
                                importAuftrRow["FLAG"] = tblKopf.FLAG;

                                if (tblKopf.Vorgang == "VZ" || tblKopf.Vorgang == "VE" || tblKopf.Vorgang == "AV" || tblKopf.Vorgang == "AX")
                                {
                                    importAuftrRow["VZERDAT"] = DateTime.Now.ToShortDateString();
                                }

                                importAuftrRow["STATUS"] = tblKopf.STATUS_Versand;
                                importAuftrRow["BARCODE"] = tblKopf.Barcode;
                                importAuftrRow["KUNNR"] = tblKopf.kundennr.PadLeft(10, '0');
                                importAuftrRow["ZZREFNR1"] = tblKopf.referenz1;
                                importAuftrRow["ZZREFNR2"] = tblKopf.referenz2;
                                importAuftrRow["ZZREFNR3"] = tblKopf.ZZREFNR3;
                                importAuftrRow["ZZREFNR4"] = tblKopf.ZZREFNR4;
                                importAuftrRow["ZZREFNR5"] = tblKopf.ZZREFNR5;
                                importAuftrRow["ZZREFNR6"] = tblKopf.ZZREFNR6;
                                importAuftrRow["ZZREFNR7"] = tblKopf.ZZREFNR7;
                                importAuftrRow["ZZREFNR8"] = tblKopf.ZZREFNR8;
                                importAuftrRow["ZZREFNR9"] = tblKopf.ZZREFNR9;
                                importAuftrRow["ZZREFNR10"] = tblKopf.ZZREFNR10;

                                importAuftrRow["KREISKZ"] = tblKopf.KreisKZ;
                                DataRow[] RowStva = tblStvaStamm.Select("KREISKZ='" + tblKopf.KreisKZ + "'");
                                if (RowStva.Length == 1)
                                {
                                    importAuftrRow["KREISBEZ"] = RowStva[0]["KREISBEZ"];
                                }
                                else
                                {
                                    importAuftrRow["KREISBEZ"] = tblKopf.KreisBez;
                                }

                                importAuftrRow["WUNSCHKENN_JN"] = ZLDCommon.BoolToX(tblKopf.WunschKenn);
                                importAuftrRow["ZUSKENNZ"] = tblKopf.ZusatzKZ;
                                importAuftrRow["WU_KENNZ2"] = tblKopf.WunschKZ2;
                                importAuftrRow["WU_KENNZ3"] = tblKopf.WunschKZ3;
                                importAuftrRow["O_G_VERSSCHEIN"] = tblKopf.OhneGruenenVersSchein;
                                importAuftrRow["SOFORT_ABR_ERL"] = ZLDCommon.BoolToX(tblKopf.SofortabrechnungErledigt);
                                importAuftrRow["SA_PFAD"] = tblKopf.SofortabrechnungPfad;

                                importAuftrRow["RESERVKENN_JN"] = ZLDCommon.BoolToX(tblKopf.Reserviert);
                                importAuftrRow["RESERVKENN"] = tblKopf.ReserviertKennz;
                                importAuftrRow["FEINSTAUBAMT"] = ZLDCommon.BoolToX(tblKopf.Feinstaub);
                                
                                if (tblKopf.Zulassungsdatum != null)
                                {
                                    importAuftrRow["ZZZLDAT"] = tblKopf.Zulassungsdatum;
                                }

                                importAuftrRow["ZZKENN"] = tblKopf.Kennzeichen;
                                importAuftrRow["KENNZFORM"] = tblKopf.KennzForm;
                                importAuftrRow["KENNZANZ"] = "0";
                                importAuftrRow["EINKENN_JN"] = ZLDCommon.BoolToX(tblKopf.EinKennz);
                                importAuftrRow["BEMERKUNG"] = tblKopf.Bemerkung;
                                importAuftrRow["INFO_TEXT"] = tblKopf.Infotext;
                                importAuftrRow["NACHBEARBEITEN"] = ZLDCommon.BoolToX(tblKopf.Nachbearbeiten);
                                importAuftrRow["ONLINE_VG"] = ZLDCommon.BoolToX(tblKopf.ONLINE_VG);
                                importAuftrRow["MOBUSER"] = tblKopf.Mobuser;
                                importAuftrRow["POOLNR"] = tblKopf.POOLNR;
                                importAuftrRow["ZL_RL_FRBNR_HIN"] = tblKopf.NrFrachtHin;
                                importAuftrRow["ZL_RL_FRBNR_ZUR"] = tblKopf.NrFrachtZu;
                                importAuftrRow["EC_JN"] = ZLDCommon.BoolToX(tblKopf.EC);
                                importAuftrRow["BAR_JN"] = ZLDCommon.BoolToX(tblKopf.Bar);
                                importAuftrRow["RE_JN"] = ZLDCommon.BoolToX(tblKopf.RE);
                                importAuftrRow["KUNDEBAR_JN"] = ZLDCommon.BoolToX(tblKopf.Barkunde);
                                importAuftrRow["BLTYP"] = tblKopf.Vorgang;
                                importAuftrRow["ZL_LIFNR"] = tblKopf.KunnrLF;
                                importAuftrRow["KENNZTYP"] = tblKopf.Kennztyp;
                                importAuftrRow["VZD_VKBUR"] = tblKopf.VersandVKBUR.Substring(4, 4);
                                importAuftrRow["LOEKZ"] = tblKopf.toDelete;
                                importAuftrRow["ZAHLART"] = tblKopf.Zahlungsart;
                                importAuftrRow["VH_KENNZ_RES"] = ZLDCommon.BoolToX(tblKopf.VorhKennzReserv);
                                importAuftrRow["ZBII_ALT_NEU"] = ZLDCommon.BoolToX(tblKopf.ZBII_ALT_NEU);
                                importAuftrRow["KENNZ_VH"] = ZLDCommon.BoolToX(tblKopf.KennzVH);
                                importAuftrRow["VK_KUERZEL"] = tblKopf.VKKurz;
                                importAuftrRow["KUNDEN_REF"] = tblKopf.interneRef;
                                importAuftrRow["KUNDEN_NOTIZ"] = tblKopf.KundenNotiz;
                                importAuftrRow["ALT_KENNZ"] = tblKopf.KennzAlt;
                                importAuftrRow["VE_ERNAM"] = tblKopf.Vorerfasser;

                                if (ZLDCommon.IsDate(tblKopf.VorerfDatum.ToString()))
                                {
                                    importAuftrRow["VE_ERDAT"] = tblKopf.VorerfDatum;
                                }

                                if (!String.IsNullOrEmpty(tblKopf.VorerfUhrzeit))
                                {
                                    importAuftrRow["VE_ERZEIT"] = tblKopf.VorerfUhrzeit;
                                }

                                if (ZLDCommon.IsDate(tblKopf.Still_Datum.ToString()))
                                {
                                    importAuftrRow["STILL_DAT"] = tblKopf.Still_Datum;
                                }

                                importAuftrRow["PRALI_PRINT"] = ZLDCommon.BoolToX(tblKopf.Prali_Print);
                                importAuftrRow["FLIEGER"] = ZLDCommon.BoolToX(tblKopf.Flieger);
                                importAuftrRow["UMBU"] = ZLDCommon.BoolToX(tblKopf.UMBU);
                                importAuftrRow["ABGESAGT"] = ZLDCommon.BoolToX(tblKopf.ABGESAGT);
                                importAuftrRow["RUECKBU"] = ZLDCommon.BoolToX(tblKopf.RUECKBU);
                                importAuftrRow["ZZEVB"] = tblKopf.EVB;
                                importAuftrRow["OBJECT_ID"] = tblKopf.OBJECT_ID;
                                importAuftrRow["FAHRZ_ART"] = tblKopf.FAHRZ_ART;
                                importAuftrRow["MNRESW"] = ZLDCommon.BoolToX(tblKopf.MNRESW);
                                importAuftrRow["SERIE"] = ZLDCommon.BoolToX(tblKopf.SERIE);
                                importAuftrRow["SAISON_KNZ"] = ZLDCommon.BoolToX(tblKopf.SAISON_KNZ);
                                importAuftrRow["SAISON_BEG"] = tblKopf.SAISON_BEG;
                                importAuftrRow["SAISON_END"] = tblKopf.SAISON_END;
                                importAuftrRow["TUEV_AU"] = tblKopf.TUEV_AU;
                                importAuftrRow["KURZZEITVS"] = tblKopf.KURZZEITVS;
                                importAuftrRow["ZOLLVERS"] = tblKopf.ZOLLVERS;
                                importAuftrRow["ZOLLVERS_DAUER"] = tblKopf.ZOLLVERS_DAUER;
                                importAuftrRow["VORFUEHR"] = ZLDCommon.BoolToX(tblKopf.VORFUEHR);
                                
                                if (ZLDCommon.IsDate(tblKopf.HALTE_DAUER.ToString()))
                                {
                                    importAuftrRow["HALTE_DAUER"] = tblKopf.HALTE_DAUER;
                                }

                                importAuftrRow["LTEXT_NR"] = tblKopf.LTEXT_NR;
                                importAuftrRow["BEB_STATUS"] = tblKopf.BEB_STATUS;
                                importAuftrRow["AH_DOKNAME"] = tblKopf.AH_DOKNAME;

                                //----------------
                                var tblPosCount = (from p in ZLD_DataContext.ZLDPositionsTabelle
                                                   where p.id_Kopf == tmpID
                                                   select p);

                                foreach (var PosRow in tblPosCount)
                                {
                                    DataRow importRow = importPos.NewRow();

                                    importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                    importRow["ZULPOSNR"] = PosRow.id_pos.ToString().PadLeft(6, '0');
                                    importRow["UEPOS"] = PosRow.UEPOS.ToString().PadLeft(6, '0');
                                    importRow["SD_REL"] = PosRow.SDRelevant;
                                    importRow["GBPAK"] = "";
                                    importRow["GEB_AMT_ADD_C"] = PosRow.Preis_Amt_Add;

                                    importRow["MENGE_C"] = PosRow.Menge;
                                    importRow["WEBMTART"] = PosRow.WebMTArt;

                                    DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + PosRow.Matnr.TrimStart('0') + "'");
                                    if (MatRow.Length == 1)
                                    {
                                        if (MatRow[0]["KENNZREL"].ToString() == "X")
                                        {
                                            importAuftrRow["KENNZANZ"] = tblKopf.EinKennz == true ? "1" : "2";
                                        }
                                    }
                                    importRow["MATNR"] = PosRow.Matnr.PadLeft(18, '0');
                                    importRow["MAKTX"] = PosRow.Matbez;
                                    importRow["LOEKZ"] = "";
                                    if (PosRow.PosLoesch == "L"){ importRow["LOEKZ"] = "X"; }
                                    if (PosRow.UPreis != null){ importRow["UPREIS_C"] = PosRow.UPreis; }
                                    if (PosRow.Differrenz != null){ importRow["DIFF_C"] = PosRow.Differrenz; }
                                    if (PosRow.Konditionstab != null){ importRow["KONDTAB"] = PosRow.Konditionstab; }
                                    if (PosRow.Konditionsart != null){ importRow["KSCHL"] = PosRow.Konditionsart; }
                                    
                                    if (ZLDCommon.IsDate(PosRow.CalcDat.ToString()))
                                    {
                                        importRow["CALCDAT"] = PosRow.CalcDat;
                                    }

                                    importRow["PREIS_C"] = PosRow.Preis.ToString();
                                   
                                    importRow["WEBMTART"] = PosRow.WebMTArt;
                                    if (PosRow.UEPOS != 10)
                                    {
                                        if (PosRow.WebMTArt == "S")
                                        {
                                            importRow["LOEKZ"] = "X";
                                        }
                                    }
                                    else if (PosRow.WebMTArt == "S" && PosRow.Preis == 0)
                                    {
                                        importRow["LOEKZ"] = "X";
                                    }

                                    if (PosRow.WebMTArt == "G")
                                    {
                                        importRow["GBPAK"] = PosRow.GebPak; //nur Gebühren haben das Kennzeichen "Gebührenpaket"
                                        if (PosRow.GebPak == "")
                                        {
                                            importRow["PREIS_C"] = PosRow.Preis_Amt.ToString();
                                            importRow["GEB_AMT_C"] = PosRow.Preis_Amt.ToString();
                                        }
                                        else
                                        {
                                            importRow["GEB_AMT_C"] = PosRow.Preis_Amt.ToString();
                                        }
                                    }
                                    
                                    importPos.Rows.Add(importRow);
                                }

                                importAuftrag.Rows.Add(importAuftrRow);

                                LastID = tmpID;
                            }
                        }
                    }

                    if (OKLoeschCount > 0)
                    {
                        myProxy.callBapi();

                        tblErrors = myProxy.getExportTable("GT_EX_ERRORS");

                        if (tblErrors.Rows.Count > 0)
                        {
                            RaiseError("-9999","Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden");
                           
                            foreach (DataRow rowError in tblErrors.Rows)
                            {
                                Int32 idsap;
                                Int32.TryParse(rowError["ZULBELN"].ToString(), out idsap);
                                DataRow[] rowListe = tblListe.Select("id_sap=" + idsap);
                                if (rowListe.Length > 0)
                                {
                                    rowListe[0]["Status"] = rowError["ERROR_TEXT"].ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        RaiseError("-7777","Es sind keine Vorgänge mit \"O\" oder \"L\" markiert");
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            RaiseError("-5555","Es ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
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
        /// Übergeben der Daten an SAP aus den SQL-Tabellen
        /// Zwischenspeichern in SAP(Nacherfassung und Nacherfassung beauftragter Versandzulassung). Bapi: Z_ZLD_SAVE_DATA
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID"></param>
        /// <param name="page"></param>
        /// <param name="tblStvaStamm"></param>
        /// <param name="tblMaterialStamm"></param>
        public void UpdateZLDNacherfassung(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblStvaStamm, DataTable tblMaterialStamm)
        {
            m_strClassAndMethod = "NacherfZLD.UpdateZLDNacherfassung";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
           
            ClearError();

            Int32 LastID = 0;
            Int32 tmpID = 0;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                var ZLD_DataContext = new ZLDTableClassesDataContext();
                
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_SAVE_DATA", ref m_objApp, ref m_objUser, ref page);

                    if (SelAnnahmeAH)
                    {
                        myProxy.setImportParameter("I_AH_ANNAHME", "X");
                    }

                    if (SelSofortabrechnung)
                    {
                        myProxy.setImportParameter("I_SOFORTABRECHNUNG", "X");
                    }

                    DataTable importAuftrag = myProxy.getImportTable("GT_IMP_BAK");

                    DataTable importPos = myProxy.getImportTable("GT_IMP_POS_S01");
                    DataTable importBank = myProxy.getImportTable("GT_IMP_BANK");
                    DataTable importAdresse = myProxy.getImportTable("GT_IMP_ADRS");
                   

                    foreach (DataRow SaveRow in tblListe.Select(tblListe.DefaultView.RowFilter))
                    {
                        tmpID = (Int32)SaveRow["id"];
                        if (LastID != tmpID)
                        {

                            var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                                           where k.id == tmpID
                                           select k).Single();
                            DataRow importAuftrRow = importAuftrag.NewRow();

                            // für "neue AH-Vorgänge" den beb_status aktualisieren
                            if (SelAnnahmeAH)
                            {
                                if (SaveRow["PosLoesch"].ToString() == "A")
                                {
                                    tblKopf.BEB_STATUS = "A";
                                }
                                else if (SaveRow["PosLoesch"].ToString() == "L")
                                {
                                    tblKopf.BEB_STATUS = "L";
                                }
                                else
                                {
                                    tblKopf.BEB_STATUS = "1";
                                }
                            }

                            // Nachbearbeitete fehlgeschlagene (Flieger) wieder auf "Angenommen" setzen, wenn Flieger-Flag raus ist
                            if ((tblKopf.BEB_STATUS == "F") && ((tblKopf.Flieger == null) || (tblKopf.Flieger == false)))
                            {
                                tblKopf.BEB_STATUS = "A";
                                tblKopf.Mobuser = "";
                            }

                            insertDatatoSAPTable(tblKopf, ref importAuftrRow, tblStvaStamm, "Update");


                            String KennMaterial = "";
                            DataRow importRow;
                            //----------------
                            var tblPosCount = (from p in ZLD_DataContext.ZLDPositionsTabelle
                                               where p.id_Kopf == tmpID
                                               select p);

                            foreach (var PosRow in tblPosCount)
                            {
                                importRow = importPos.NewRow();

                                importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                importRow["ZULPOSNR"] = PosRow.id_pos.ToString().PadLeft(6, '0');
                                importRow["UEPOS"] = PosRow.UEPOS.ToString().PadLeft(6, '0');
                                importRow["SD_REL"] = PosRow.SDRelevant;
                                importRow["GBPAK"] = "";
                                importRow["GEB_AMT_ADD_C"] = PosRow.Preis_Amt_Add;
                                importRow["MENGE_C"] = PosRow.Menge;
                                importRow["WEBMTART"] = PosRow.WebMTArt;

                                DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + PosRow.Matnr.TrimStart('0') + "'");
                                if (MatRow.Length == 1)
                                {
                                    if (MatRow[0]["KENNZREL"].ToString() == "X")
                                    {
                                        if (tblKopf.EinKennz == true)
                                        {
                                            importAuftrRow["KENNZANZ"] = "1";
                                        }
                                        else
                                        {
                                            importAuftrRow["KENNZANZ"] = "2";
                                        }
                                    }
                                    importRow["NULLPREIS_OK"] = MatRow[0]["NULLPREIS_OK"].ToString();
                                }
                                importRow["MATNR"] = PosRow.Matnr.PadLeft(18, '0');
                                importRow["MAKTX"] = PosRow.Matbez;
                                importRow["LOEKZ"] = "";

                                if (PosRow.PosLoesch == "L"){ importRow["LOEKZ"] = "X"; }
                                
                                importRow["PREIS_C"] = PosRow.Preis.ToString();

                                if (PosRow.id_pos == 10)
                                {
                                    PreisKennz = (Decimal)PosRow.PreisKZ;
                                    if (PosRow.Kennzmat.Trim(' ') != ""){ KennMaterial = PosRow.Kennzmat.Trim(' '); }
                                }
                                importRow["WEBMTART"] = PosRow.WebMTArt;

                                if (PosRow.WebMTArt == "G")
                                {
                                    importRow["GEB_AMT_C"] = PosRow.Preis_Amt.ToString();
                                    // falls Flag false soll hier der Preis aus der Gebühr stehen 
                                    if (m_objUser.Groups[0].Authorizationright == 1) { importRow["GEB_AMT_C"] = PosRow.Preis.ToString(); }
                                    importRow["GBPAK"] = PosRow.GebPak; //nur Gebühren haben das Kennzeichen "Gebührenpaket"

                                    if (tblKopf.Vorgang == "VZ" || tblKopf.Vorgang == "AV")
                                    {
                                        if (tblKopf.VZB_STATUS != "NB" && PosRow.GebPak == "")
                                        {
                                            importRow["PREIS_C"] = PosRow.Preis_Amt.ToString();
                                            importRow["GEB_AMT_C"] = PosRow.Preis_Amt.ToString();
                                        }
                                        else
                                        {
                                            importRow["GEB_AMT_C"] = PosRow.Preis_Amt.ToString();
                                        }
                                    }
                                }

                                if (PosRow.UEPOS != 10)
                                {
                                    if (PosRow.WebMTArt == "S")
                                    {
                                        importRow["LOEKZ"] = "X";
                                    }
                                }

                                if (PosRow.UPreis != null){ importRow["UPREIS_C"] = PosRow.UPreis; }
                                if (PosRow.Differrenz != null){ importRow["DIFF_C"] = PosRow.Differrenz; }
                                if (PosRow.Konditionstab != null){ importRow["KONDTAB"] = PosRow.Konditionstab; }
                                if (PosRow.Konditionsart != null){ importRow["KSCHL"] = PosRow.Konditionsart; }
                                
                                if (ZLDCommon.IsDate(PosRow.CalcDat.ToString()))
                                {
                                    importRow["CALCDAT"] = PosRow.CalcDat;
                                }

                                importPos.Rows.Add(importRow);
                            }

                            if (PauschalKunde != "X")
                            {
                                if (importPos.Select("WEBMTART = 'K' AND ZULBELN = " + tblKopf.id_sap.ToString().PadLeft(10, '0') + 
                                    " AND UEPOS = '000010'").Length == 0 && PreisKennz > 0)
                                {
                                    if (KennMaterial != "")
                                    {
                                        importRow = importPos.NewRow();
                                        importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                        importRow["UEPOS"] = 10.ToString().PadLeft(6, '0');
                                        importRow["ZULPOSNR"] = ((importPos.Rows.Count + 1) * 10).ToString().PadLeft(6, '0');
                                        
                                        importRow["MENGE_C"] = "1";
                                        if (EinKennz == false)
                                        {
                                            importRow["MENGE_C"] = "2";
                                        }
                                        
                                        importRow["MATNR"] = 1.ToString().PadLeft(18, '0');
                                        importRow["PREIS_C"] = PreisKennz;
                                        importRow["MAKTX"] = "";
                                        importRow["WEBMTART"] = "K";
                                        importRow["SD_REL"] = "X";
                                        importPos.Rows.Add(importRow);
                                    }
                                }
                            }

                            importAuftrag.Rows.Add(importAuftrRow);
                            var tblBank = (from b in ZLD_DataContext.ZLDBankverbindung
                                           where b.id_Kopf == tmpID
                                           select b).Single();
                            if (tblBank.Inhaber != null)
                            {
                                importRow = importBank.NewRow();
                                if (tblBank.Inhaber.Length > 0)
                                {
                                    importRow["MANDT"] = "010";
                                    importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                    importRow["SWIFT"] = tblBank.SWIFT;
                                    importRow["IBAN"] = tblBank.IBAN;
                                    importRow["BANKL"] = tblBank.BankKey;
                                    importRow["BANKN"] = tblBank.Kontonr;
                                    importRow["EBPP_ACCNAME"] = tblBank.Geldinstitut;
                                    importRow["KOINH"] = tblBank.Inhaber;
                                    importRow["EINZ_JN"] = ZLDCommon.BoolToX(tblBank.EinzugErm);
                                    importRow["RECH_JN"] = ZLDCommon.BoolToX(tblBank.Rechnung);
                                    importRow["LOEKZ"] = "";
                                }
                                else
                                {
                                    importRow["MANDT"] = "010";
                                    importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                    importRow["LOEKZ"] = "X";
                                }
                                importBank.Rows.Add(importRow);
                            }

                            var tblKunnadresse = (from k in ZLD_DataContext.ZLDKundenadresse
                                                  where k.id_Kopf == tmpID
                                                  select k).Single();
                            if (tblKunnadresse.Name1 != null)
                            {
                                importRow = importAdresse.NewRow();

                                importRow["MANDT"] = "010";
                                importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                importRow["KUNNR"] = tblKopf.kundennr.PadLeft(10, '0');
                                importRow["PARVW"] = "AG";
                                importRow["LI_NAME1"] = tblKunnadresse.Name1;
                                importRow["LI_NAME2"] = tblKunnadresse.Name2;
                                importRow["LI_PLZ"] = tblKunnadresse.PLZ;
                                importRow["LI_CITY1"] = tblKunnadresse.Ort;
                                importRow["LI_STREET"] = tblKunnadresse.Strasse;

                                if (tblKunnadresse.Name1.Length > 0)
                                {
                                    importRow["LOEKZ"] = "";
                                }
                                else
                                {
                                    importRow["LOEKZ"] = "X";
                                }

                                importAdresse.Rows.Add(importRow);
                            }
                            LastID = tmpID;
                        }
                    }

                    myProxy.callBapi();

                    tblErrors = myProxy.getExportTable("GT_EX_ERRORS");
                    if (tblErrors.Rows.Count > 0)
                    {
                        RaiseError("-9999","Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden");
                       
                        foreach (DataRow rowError in tblErrors.Rows)
                        {
                            Int32 idsap;
                            Int32.TryParse(rowError["ZULBELN"].ToString(), out idsap);
                            Int32 id_Pos;
                            Int32.TryParse(rowError["ZULPOSNR"].ToString(), out id_Pos);
                            DataRow[] rowListe = tblListe.Select("id_sap=" + idsap);
                            if (rowListe.Length > 0)
                            {
                                rowListe[0]["Status"] = rowError["ERROR_TEXT"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            RaiseError("-5555","Es ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
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
        /// Übergeben der Daten an SAP aus den SQL-Tabellen
        /// Zwischenspeichern eines Vorganges in SAP(Nacherfassung und Nacherfassung beauftragter Versandzulassung). Bapi: Z_ZLD_SAVE_DATA
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID"></param>
        /// <param name="page"></param>
        /// <param name="tblStvaStamm"></param>
        /// <param name="tblMaterialStamm"></param>  
        /// <param name="SaveRow"></param>     
        public void UpdateZLDNacherfassungRow(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblStvaStamm, DataTable tblMaterialStamm, DataRow SaveRow)
        {
            m_strClassAndMethod = "NacherfZLD.UpdateZLDNacherfassungRow";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
           
            ClearError();

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                var ZLD_DataContext = new ZLDTableClassesDataContext();

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_SAVE_DATA", ref m_objApp, ref m_objUser, ref page);

                    if (SelAnnahmeAH)
                    {
                        myProxy.setImportParameter("I_AH_ANNAHME", "X");
                    }

                    if (SelSofortabrechnung)
                    {
                        myProxy.setImportParameter("I_SOFORTABRECHNUNG", "X");
                    }

                    DataTable importAuftrag = myProxy.getImportTable("GT_IMP_BAK");
                    DataTable importPos = myProxy.getImportTable("GT_IMP_POS_S01");
                    DataTable importBank = myProxy.getImportTable("GT_IMP_BANK");
                    DataTable importAdresse = myProxy.getImportTable("GT_IMP_ADRS");
                 
                    Int32 tmpID = (Int32)SaveRow["id"];
                    
                    var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                                   where k.id == tmpID
                                   select k).Single();
                    DataRow importAuftrRow = importAuftrag.NewRow();

                    // für "neue AH-Vorgänge" den beb_status aktualisieren
                    if (SelAnnahmeAH)
                    {
                        switch (SaveRow["PosLoesch"].ToString())
                        {
                            case "A":
                                tblKopf.BEB_STATUS = "A";
                                break;
                            case"L":
                                 tblKopf.BEB_STATUS = "L";
                                break;
                            default:
                                tblKopf.BEB_STATUS = "1";
                                break;
                        }
                    }

                    // Nachbearbeitete fehlgeschlagene (Flieger) wieder auf "Angenommen" setzen, wenn Flieger-Flag raus ist
                    if ((tblKopf.BEB_STATUS == "F") && ((tblKopf.Flieger == null) || (tblKopf.Flieger == false)))
                    {
                        tblKopf.BEB_STATUS = "A";
                        tblKopf.Mobuser = "";
                    }

                    insertDatatoSAPTable(tblKopf, ref importAuftrRow, tblStvaStamm, "Update");

                    DataRow importRow;
                    //----------------
                    var tblPosCount = (from p in ZLD_DataContext.ZLDPositionsTabelle
                                       where p.id_Kopf == tmpID
                                       select p);

                    foreach (var PosRow in tblPosCount)
                    {
                        importRow = importPos.NewRow();
                        importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                        importRow["ZULPOSNR"] = PosRow.id_pos.ToString().PadLeft(6, '0');
                        importRow["UEPOS"] = PosRow.UEPOS.ToString().PadLeft(6, '0');
                        importRow["SD_REL"] = PosRow.SDRelevant;
                        importRow["GBPAK"] = "";
                        importRow["GEB_AMT_ADD_C"] = PosRow.Preis_Amt_Add;
                        importRow["MENGE_C"] = PosRow.Menge;
                        importRow["WEBMTART"] = PosRow.WebMTArt;

                        DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + PosRow.Matnr.TrimStart('0') + "'");
                        if (MatRow.Length == 1)
                        {
                            if (MatRow[0]["KENNZREL"].ToString() == "X")
                            {
                                importAuftrRow["KENNZANZ"] = tblKopf.EinKennz== true ? "1" : "2";
                            }

                            importRow["NULLPREIS_OK"] = MatRow[0]["NULLPREIS_OK"].ToString();
                        }

                        importRow["MATNR"] = PosRow.Matnr.PadLeft(18, '0');
                        importRow["MAKTX"] = PosRow.Matbez;
                        importRow["LOEKZ"] = "";

                        if (PosRow.PosLoesch == "L"){ importRow["LOEKZ"] = "X"; }

                        importRow["PREIS_C"] = PosRow.Preis.ToString();
                       
                        if (PosRow.id_pos == 10){PreisKennz = (Decimal)PosRow.PreisKZ;}

                        importRow["WEBMTART"] = PosRow.WebMTArt;

                        if (PosRow.WebMTArt == "G")
                        {
                            importRow["GEB_AMT_C"] = PosRow.Preis_Amt.ToString();
                            // falls Flag false soll hier der Preis aus der Gebühr stehen 
                            if (m_objUser.Groups[0].Authorizationright == 1) { importRow["GEB_AMT_C"] = PosRow.Preis.ToString(); }
                            importRow["GBPAK"] = PosRow.SDRelevant;

                        }

                        if (PosRow.UEPOS != 10)
                        {
                            if (PosRow.WebMTArt == "S")
                            {
                                importRow["LOEKZ"] = "X";
                            }
                        }
                        else if (PosRow.WebMTArt == "S" && PosRow.Preis == 0)
                        {
                            importRow["LOEKZ"] = "X";
                        }
                        else if (PosRow.WebMTArt == "S" && PosRow.Preis > 0)
                        {
                            importRow["LOEKZ"] = "";
                        }

                        if (PosRow.WebMTArt == "K" && PosRow.Preis == 0)
                        {
                            importRow["LOEKZ"] = "X";
                        }
                        else if (PosRow.WebMTArt == "K" && PosRow.Preis > 0)
                        {
                            importRow["LOEKZ"] = "";
                        }

                        if (PosRow.UPreis != null){ importRow["UPREIS_C"] = PosRow.UPreis; }
                        if (PosRow.Differrenz != null){ importRow["DIFF_C"] = PosRow.Differrenz; }
                        if (PosRow.Konditionstab != null){ importRow["KONDTAB"] = PosRow.Konditionstab; }
                        if (PosRow.Konditionsart != null){ importRow["KSCHL"] = PosRow.Konditionsart; }

                        if (ZLDCommon.IsDate(PosRow.CalcDat.ToString()))
                        {
                            importRow["CALCDAT"] = PosRow.CalcDat;
                        }

                        importPos.Rows.Add(importRow);
                    }

                    if (PauschalKunde != "X")
                    {
                        if (importPos.Select("MATNR = 000000000000000001 AND ZULBELN = " + tblKopf.id_sap.ToString().PadLeft(10, '0')).Length == 0
                           && PreisKennz > 0)
                        {
                            importRow = importPos.NewRow();

                            importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                            importRow["UEPOS"] = 10.ToString().PadLeft(6, '0');
                            importRow["ZULPOSNR"] = ((importPos.Rows.Count + 1) * 10).ToString().PadLeft(6, '0');
                            importRow["MENGE_C"] = "1";

                            if (EinKennz == false)
                            {
                                importRow["MENGE_C"] = "2";
                            }
                           
                            importRow["MATNR"] = 1.ToString().PadLeft(18, '0');
                            importRow["PREIS_C"] = PreisKennz;
                            importRow["MAKTX"] = "";
                            importRow["WEBMTART"] = "K";
                            importRow["SD_REL"] = "X";
                            importPos.Rows.Add(importRow);
                        }
                    }

                    importAuftrag.Rows.Add(importAuftrRow);

                    var tblBank = (from b in ZLD_DataContext.ZLDBankverbindung
                                   where b.id_Kopf == tmpID
                                   select b).Single();

                    if (tblBank.Inhaber != null)
                    {
                        importRow = importBank.NewRow();
                        if (tblBank.Inhaber.Length > 0)
                        {
                            importRow["MANDT"] = "010";
                            importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                            importRow["SWIFT"] = tblBank.SWIFT;
                            importRow["IBAN"] = tblBank.IBAN;
                            importRow["BANKL"] = tblBank.BankKey;
                            importRow["BANKN"] = tblBank.Kontonr;
                            importRow["EBPP_ACCNAME"] = tblBank.Geldinstitut;
                            importRow["KOINH"] = tblBank.Inhaber;
                            importRow["EINZ_JN"] = ZLDCommon.BoolToX(tblBank.EinzugErm);
                            importRow["RECH_JN"] = ZLDCommon.BoolToX(tblBank.Rechnung);
                            importRow["LOEKZ"] = "";
                        }
                        else
                        {
                            importRow["MANDT"] = "010";
                            importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                            importRow["LOEKZ"] = "X";
                        }

                        importBank.Rows.Add(importRow);
                    }

                    var tblKunnadresse = (from k in ZLD_DataContext.ZLDKundenadresse
                                          where k.id_Kopf == tmpID
                                          select k).Single();

                    if (tblKunnadresse.Name1 != null)
                    {
                        importRow = importAdresse.NewRow();
                        importRow["MANDT"] = "010";
                        importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                        importRow["KUNNR"] = tblKopf.kundennr.PadLeft(10, '0');
                        importRow["PARVW"] = "AG";

                        if (tblKunnadresse.Name1.Length > 0)
                        {
                            importRow["LI_NAME1"] = tblKunnadresse.Name1;
                            importRow["LI_NAME2"] = tblKunnadresse.Name2;
                            importRow["LI_PLZ"] = tblKunnadresse.PLZ;
                            importRow["LI_CITY1"] = tblKunnadresse.Ort;
                            importRow["LI_STREET"] = tblKunnadresse.Strasse;
                            importRow["LOEKZ"] = "";
                        }
                        else
                        {
                            importRow["LOEKZ"] = "X";
                        }

                        importAdresse.Rows.Add(importRow);
                    }

                    myProxy.callBapi();

                    tblErrors = myProxy.getExportTable("GT_EX_ERRORS");
                    if (tblErrors.Rows.Count > 0)
                    {
                        RaiseError("-9999","Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden");
                     
                        foreach (DataRow rowError in tblErrors.Rows)
                        {
                            Int32 idsap;
                            Int32.TryParse(rowError["ZULBELN"].ToString(), out idsap);
                            Int32 id_Pos;
                            Int32.TryParse(rowError["ZULPOSNR"].ToString(), out id_Pos);
                            DataRow[] rowListe = tblListe.Select("id_sap=" + idsap);
                            if (rowListe.Length > 0)
                            {
                                rowListe[0]["Status"] = rowError["ERROR_TEXT"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            RaiseError("-5555","Es ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
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
        /// Speichern der Daten in SAP für "Versandzulassungen erfasst durch Autohaus". Bapi: Z_ZLD_AH_VZ_SAVE
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">AHVersandChange_2.aspx.cs</param>
        /// <param name="tblStvaStamm">Stammtabelle StVa</param>
        /// <param name="tblMaterialStamm">Stammtabelle Material</param>
        public void SaveZLDAHVersand(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblStvaStamm, DataTable tblMaterialStamm)
        {
            m_strClassAndMethod = "NacherfZLD.SaveZLDAHVersand";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
           
            ClearError();

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                var ZLD_DataContext = new ZLDTableClassesDataContext();
                
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_VZ_SAVE", ref m_objApp, ref m_objUser, ref page);

                    DataTable importAuftrag = myProxy.getImportTable("GT_BAK");
                    DataTable importPos = myProxy.getImportTable("GT_POS_S01");
                    DataTable importBank = myProxy.getImportTable("GT_BANK");
                    DataTable importAdresse = myProxy.getImportTable("GT_ADRS");


                    String sZULBELN = "";
                    String sKUNNR = "";

                    foreach (DataRow SaveRow in AHVersandKopf.Rows)
                    {

                        DataRow importAuftrRow = importAuftrag.NewRow();

                        importAuftrRow["MANDT"] = SaveRow["MANDT"];
                        sZULBELN = SaveRow["ZULBELN"].ToString().PadLeft(10, '0');
                        importAuftrRow["ZULBELN"] = sZULBELN;
                        importAuftrRow["VKORG"] = SaveRow["VKORG"];
                        importAuftrRow["VKBUR"] = SaveRow["VKBUR"];
                        importAuftrRow["VE_ERNAM"] = SaveRow["VE_ERNAM"];
                        importAuftrRow["VE_ERDAT"] = SaveRow["VE_ERDAT"];
                        importAuftrRow["VE_ERZEIT"] = SaveRow["VE_ERZEIT"];
                        importAuftrRow["ERNAM"] = m_objUser.UserName.PadLeft(12);
                        importAuftrRow["ERDAT"] = DateTime.Now;
                        importAuftrRow["FLAG"] = SaveRow["FLAG"];
                        importAuftrRow["VZB_STATUS"] = "LA";

                        if (Lieferant_ZLD.TrimStart('0').Substring(0, 2) == "56" && IsZLD == "X")
                        {
                            importAuftrRow["VZD_VKBUR"] = Lieferant_ZLD.TrimStart('0').Substring(2, 4);
                            importAuftrRow["BLTYP"] = "AV";
                        }
                        else
                        {
                            importAuftrRow["VZD_VKBUR"] = "";
                            importAuftrRow["BLTYP"] = "AX";
                        }

                        if (SaveRow["VZERDAT"].ToString().Length > 0){ importAuftrRow["VZERDAT"] = SaveRow["VZERDAT"]; }

                        importAuftrRow["STATUS"] = SaveRow["STATUS"];
                        importAuftrRow["BARCODE"] = SaveRow["BARCODE"];

                        sKUNNR = SaveRow["KUNNR"].ToString().PadLeft(10, '0');
                        importAuftrRow["KUNNR"] = sKUNNR;

                        importAuftrRow["ZZREFNR1"] = SaveRow["ZZREFNR1"];
                        importAuftrRow["ZZREFNR2"] = SaveRow["ZZREFNR2"];
                        importAuftrRow["ZZREFNR3"] = SaveRow["ZZREFNR3"];
                        importAuftrRow["ZZREFNR4"] = SaveRow["ZZREFNR4"];
                        importAuftrRow["ZZREFNR5"] = SaveRow["ZZREFNR5"];
                        importAuftrRow["ZZREFNR6"] = SaveRow["ZZREFNR6"];
                        importAuftrRow["ZZREFNR7"] = SaveRow["ZZREFNR7"];
                        importAuftrRow["ZZREFNR8"] = SaveRow["ZZREFNR8"];
                        importAuftrRow["ZZREFNR9"] = SaveRow["ZZREFNR9"];
                        importAuftrRow["ZZREFNR10"] = SaveRow["ZZREFNR10"];
                        importAuftrRow["KREISKZ"] = SaveRow["KREISKZ"];

                        DataRow[] RowStva = tblStvaStamm.Select("KREISKZ='" + SaveRow["KREISKZ"].ToString() + "'");
                        if (RowStva.Length == 1)
                        {
                            importAuftrRow["KREISBEZ"] = RowStva[0]["KREISBEZ"];
                        }
                        else
                        {
                            importAuftrRow["KREISBEZ"] = SaveRow["KREISBEZ"];
                        }

                        importAuftrRow["WUNSCHKENN_JN"] = SaveRow["WUNSCHKENN_JN"];
                        importAuftrRow["ZUSKENNZ"] = SaveRow["ZUSKENNZ"];
                        importAuftrRow["WU_KENNZ2"] = SaveRow["WU_KENNZ2"];
                        importAuftrRow["WU_KENNZ3"] = SaveRow["WU_KENNZ3"];
                        importAuftrRow["O_G_VERSSCHEIN"] = SaveRow["O_G_VERSSCHEIN"];
                        importAuftrRow["SOFORT_ABR_ERL"] = SaveRow["SOFORT_ABR_ERL"];
                        importAuftrRow["SA_PFAD"] = SaveRow["SA_PFAD"];
                        importAuftrRow["RESERVKENN_JN"] = SaveRow["RESERVKENN_JN"];
                        importAuftrRow["RESERVKENN"] = SaveRow["RESERVKENN"];
                        importAuftrRow["FEINSTAUBAMT"] = SaveRow["FEINSTAUBAMT"];
                        importAuftrRow["ZZZLDAT"] = SaveRow["ZZZLDAT"];
                        importAuftrRow["ZZKENN"] = SaveRow["ZZKENN"];
                        importAuftrRow["KENNZTYP"] = SaveRow["KENNZTYP"];
                        importAuftrRow["KENNZFORM"] = SaveRow["KENNZFORM"];
                        importAuftrRow["KENNZANZ"] = SaveRow["KENNZANZ"];
                        importAuftrRow["EINKENN_JN"] = SaveRow["EINKENN_JN"];
                        importAuftrRow["BEMERKUNG"] = SaveRow["BEMERKUNG"];
                        importAuftrRow["INFO_TEXT"] = SaveRow["INFO_TEXT"];
                        importAuftrRow["NACHBEARBEITEN"] = SaveRow["NACHBEARBEITEN"];
                        importAuftrRow["ONLINE_VG"] = SaveRow["ONLINE_VG"];

                        importAuftrRow["EC_JN"] = "X";
                        importAuftrRow["BAR_JN"] = "";
                        importAuftrRow["RE_JN"] = "";
                        importAuftrRow["ZL_RL_FRBNR_HIN"] = FrachtNrHin;
                        importAuftrRow["ZL_RL_FRBNR_ZUR"] = FrachtNrBack;
                        importAuftrRow["ZL_LIFNR"] = Lieferant_ZLD;

                        importAuftrRow["KUNDEBAR_JN"] = SaveRow["KUNDEBAR_JN"];
                        importAuftrRow["LOEKZ"] = SaveRow["LOEKZ"];
                        importAuftrRow["ZAHLART"] = SaveRow["ZAHLART"];
                        importAuftrRow["KSTATUS"] = SaveRow["KSTATUS"];
                        importAuftrRow["BARQ_NR"] = SaveRow["BARQ_NR"];
                        importAuftrRow["VK_KUERZEL"] = SaveRow["VK_KUERZEL"];
                        importAuftrRow["KUNDEN_REF"] = SaveRow["KUNDEN_REF"];
                        importAuftrRow["KUNDEN_NOTIZ"] = SaveRow["KUNDEN_NOTIZ"];
                        importAuftrRow["KENNZ_VH"] = SaveRow["KENNZ_VH"];
                        importAuftrRow["ALT_KENNZ"] = SaveRow["ALT_KENNZ"];
                        importAuftrRow["ZBII_ALT_NEU"] = SaveRow["ZBII_ALT_NEU"];
                        importAuftrRow["VH_KENNZ_RES"] = SaveRow["VH_KENNZ_RES"];

                        if (SaveRow["STILL_DAT"].ToString().Length > 0) importAuftrRow["STILL_DAT"] = SaveRow["STILL_DAT"];

                        importAuftrRow["PRALI_PRINT"] = SaveRow["PRALI_PRINT"];
                        importAuftrRow["UMBU"] = SaveRow["UMBU"];
                        importAuftrRow["ABGESAGT"] = SaveRow["ABGESAGT"];
                        importAuftrRow["RUECKBU"] = SaveRow["RUECKBU"];
                        importAuftrRow["ZZEVB"] = SaveRow["ZZEVB"];
                        importAuftrRow["FLIEGER"] = SaveRow["FLIEGER"];
                        importAuftrRow["OBJECT_ID"] = SaveRow["OBJECT_ID"];
                        importAuftrRow["FAHRZ_ART"] = SaveRow["FAHRZ_ART"];
                        importAuftrRow["MNRESW"] = SaveRow["MNRESW"];
                        importAuftrRow["SERIE"] = SaveRow["SERIE"];
                        importAuftrRow["SAISON_KNZ"] = SaveRow["SAISON_KNZ"];
                        importAuftrRow["SAISON_BEG"] = SaveRow["SAISON_BEG"];
                        importAuftrRow["SAISON_END"] = SaveRow["SAISON_END"];

                        importAuftrRow["TUEV_AU"] = SaveRow["TUEV_AU"];
                        importAuftrRow["KURZZEITVS"] = SaveRow["KURZZEITVS"];
                        importAuftrRow["ZOLLVERS"] = SaveRow["ZOLLVERS"];
                        importAuftrRow["ZOLLVERS_DAUER"] = SaveRow["ZOLLVERS_DAUER"];
                        importAuftrRow["VORFUEHR"] = SaveRow["VORFUEHR"];

                        if (SaveRow["HALTE_DAUER"].ToString().Length > 0) importAuftrRow["HALTE_DAUER"] = SaveRow["HALTE_DAUER"];

                        importAuftrRow["LTEXT_NR"] = SaveRow["LTEXT_NR"];
                        importAuftrRow["BEB_STATUS"] = "A";  // -> Status "angenommen"
                        importAuftrRow["AH_DOKNAME"] = SaveRow["AH_DOKNAME"];
                        importAuftrRow["ABM_STATUS"] = SaveRow["ABM_STATUS"];

                        importAuftrag.Rows.Add(importAuftrRow);
                    }

                    DataRow importRow;
                    foreach (DataRow PosRow in AHVersandPos.Rows)
                    {
                        importRow = importPos.NewRow();
                        
                        importRow["ZULBELN"] = PosRow["ZULBELN"];
                        importRow["ZULPOSNR"] = PosRow["ZULPOSNR"];
                        importRow["UEPOS"] = PosRow["UEPOS"];
                        importRow["SD_REL"] = PosRow["SD_REL"];
                        importRow["MENGE_C"] = PosRow["MENGE_C"];
                        importRow["WEBMTART"] = PosRow["WEBMTART"];
                        importRow["PREIS_C"] = PosRow["PREIS_C"];
                        importRow["GEB_AMT_C"] = PosRow["GEB_AMT_C"];
                        importRow["GEB_AMT_ADD_C"] = PosRow["GEB_AMT_ADD_C"];
                        importRow["MATNR"] = PosRow["MATNR"];
                        importRow["MAKTX"] = PosRow["MAKTX"];
                        importRow["LOEKZ"] = PosRow["LOEKZ"];
                        importRow["UPREIS_C"] = PosRow["UPREIS_C"];
                        importRow["DIFF_C"] = PosRow["DIFF_C"];
                        importRow["KONDTAB"] = PosRow["KONDTAB"];
                        importRow["KSCHL"] = PosRow["KSCHL"];
                        importRow["CALCDAT"] = PosRow["CALCDAT"];
                        importRow["NULLPREIS_OK"] = PosRow["NULLPREIS_OK"];
                        importRow["GBPAK"] = PosRow["GBPAK"];

                        importPos.Rows.Add(importRow);
                    }

                    foreach (DataRow BankRow in AHVersandBank.Rows)
                    {
                        importRow = importBank.NewRow();

                        importRow["MANDT"] = BankRow["MANDT"];
                        importRow["ZULBELN"] = sZULBELN;
                        importRow["IBAN"] = BankRow["IBAN"];
                        importRow["SWIFT"] = BankRow["SWIFT"];
                        importRow["BANKL"] = BankRow["BANKL"];
                        importRow["BANKN"] = BankRow["BANKN"];
                        importRow["EBPP_ACCNAME"] = BankRow["EBPP_ACCNAME"];
                        importRow["KOINH"] = BankRow["KOINH"];
                        importRow["EINZ_JN"] = BankRow["EINZ_JN"];
                        importRow["RECH_JN"] = BankRow["RECH_JN"];

                        importBank.Rows.Add(importRow);
                    }

                    foreach (DataRow AdrRow in AHVersandAdresse.Rows)
                    {
                        importRow = importAdresse.NewRow();
                        importRow["MANDT"] = AdrRow["MANDT"];
                        importRow["ZULBELN"] = sZULBELN;
                        importRow["KUNNR"] = AdrRow["KUNNR"];
                        importRow["PARVW"] = AdrRow["PARVW"];
                        importRow["LI_NAME1"] = AdrRow["LI_NAME1"];
                        importRow["LI_NAME2"] = AdrRow["LI_NAME2"];
                        importRow["LI_PLZ"] = AdrRow["LI_PLZ"];
                        importRow["LI_CITY1"] = AdrRow["LI_CITY1"];
                        importRow["LI_STREET"] = AdrRow["LI_STREET"];

                        importAdresse.Rows.Add(importRow);
                    }

                    if (DocRueck1.Length > 0)
                    {
                        importRow = importAdresse.NewRow();

                        importRow["MANDT"] = "010";
                        importRow["ZULBELN"] = sZULBELN;
                        importRow["KUNNR"] = sKUNNR.PadLeft(10, '0');
                        importRow["PARVW"] = "ZE";
                        importRow["LI_NAME1"] = NameRueck1;
                        importRow["LI_NAME2"] = NameRueck2;
                        importRow["LI_PLZ"] = PLZRueck;
                        importRow["LI_CITY1"] = OrtRueck;
                        importRow["LI_STREET"] = StrasseRueck;
                        importRow["BEMERKUNG"] = DocRueck1;

                        importAdresse.Rows.Add(importRow);
                    }

                    if (Doc2Rueck.Length > 0)
                    {
                        importRow = importAdresse.NewRow();

                        importRow["MANDT"] = "010";
                        importRow["ZULBELN"] = sZULBELN;
                        importRow["KUNNR"] = sKUNNR.PadLeft(10, '0');
                        importRow["PARVW"] = "ZS";
                        importRow["LI_NAME1"] = Name1Rueck2;
                        importRow["LI_NAME2"] = Name2Rueck2;
                        importRow["LI_PLZ"] = PLZ2Rueck;
                        importRow["LI_CITY1"] = Ort2Rueck;
                        importRow["LI_STREET"] = Strasse2Rueck;
                        importRow["BEMERKUNG"] = Doc2Rueck;

                        importAdresse.Rows.Add(importRow);
                    }

                    myProxy.callBapi();

                    tblErrors = myProxy.getExportTable("GT_EX_ERRORS");

                    if (tblErrors.Rows.Count > 0)
                    {
                        RaiseError("-9999","Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden");
                      
                        foreach (DataRow rowError in tblErrors.Rows)
                        {
                            Int32 idsap;
                            Int32.TryParse(rowError["ZULBELN"].ToString(), out idsap);
                            DataRow[] rowListe = tblListe.Select("id_sap=" + idsap);
                            if (rowListe.Length > 0)
                            {
                                rowListe[0]["Status"] = rowError["ERROR_TEXT"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            RaiseError("-5555","Es ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
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
        /// Führt für alle Vorgänge eine Preisfindung durch und aktualisiert deren Preise
        /// </summary>
        public void DoPreisfindung(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblStvaStamm, DataTable tblMaterialStamm)
        {
            Int32 LastID = 0;

            var ZLD_DataContext = new ZLDTableClassesDataContext();

            foreach (DataRow dRow in tblListe.Select(tblListe.DefaultView.RowFilter))
            {
                var tmpID = (Int32)dRow["id"];
                if (LastID != tmpID)
                {
                    var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                                   where k.id == tmpID
                                   select k).Single();

                    // aktuellen Vorgang laden
                    Vorgang = tblKopf.Vorgang;
                    LoadDB_ZLDRecordset(tblKopf.id);
                    KunnrPreis = Kunnr;

                    // Preisfindung durchführen
                    GetPreise(strAppID, strSessionID, page, tblStvaStamm, tblMaterialStamm);

                    // Preise übernehmen
                    foreach (DataRow itemRow in NewPosPreise.Rows)
                    {
                        var tblPos = (from p in ZLD_DataContext.ZLDPositionsTabelle
                                      where p.id_Kopf == tmpID && p.id_pos == Int32.Parse(itemRow["ZULPOSNR"].ToString())
                                      select p).Single();

                        tblPos.SDRelevant = itemRow["SD_REL"].ToString();
                        tblPos.WebMTArt = itemRow["WEBMTART"].ToString();
                        tblPos.GebPak = itemRow["GBPAK"].ToString();

                        decimal iPreisAmtAdd = 0;
                        if (ZLDCommon.IsDecimal(itemRow["GEB_AMT_ADD_C"].ToString().Trim()))
                        {
                            Decimal.TryParse(itemRow["GEB_AMT_ADD_C"].ToString(), out iPreisAmtAdd);
                        }
                        tblPos.Preis_Amt_Add = iPreisAmtAdd;

                        Decimal iMenge = 1;
                        if (ZLDCommon.IsDecimal(itemRow["Menge_C"].ToString().Trim()))
                        {
                            Decimal.TryParse(itemRow["Menge_C"].ToString(), out iMenge);
                        }
                        tblPos.Menge = iMenge.ToString("0");

                        tblPos.PosLoesch = "";

                        if (itemRow["LOEKZ"].ToString() == "X")tblPos.PosLoesch = "L";

                        decimal dPreis;
                        tblPos.Preis = decimal.TryParse(itemRow["PREIS_C"].ToString(), out dPreis) ? dPreis : 0;
                        
                        DataRow[] SelRow = NewPosPreise.Select("ZULBELN = '" + itemRow["ZULBELN"].ToString() +
                                        "' AND UEPOS = '" + itemRow["ZULPOSNR"].ToString() +
                                        "' AND WEBMTART = 'G'");
                        if (SelRow.Length == 1)
                        {
                            decimal dGebPreis;
                            decimal dGebAmtPreis;
                            tblPos.GebPreis = decimal.TryParse(SelRow[0]["PREIS_C"].ToString(), out dGebPreis) ? dGebPreis : 0;
                            tblPos.Preis_Amt = decimal.TryParse(SelRow[0]["GEB_AMT_C"].ToString(), out dGebAmtPreis) ? dGebAmtPreis : 0;
                            
                            tblPos.GebPak = SelRow[0]["GBPAK"].ToString();
                        }
                        else if (tblPos.WebMTArt == "G")
                        {
                            decimal dGebAmtPreis;
                            if (decimal.TryParse(itemRow["GEB_AMT_C"].ToString(), out dGebAmtPreis))
                            {
                                tblPos.GebPreis = 0;
                                tblPos.Preis_Amt = dGebAmtPreis;
                            }
                            tblPos.GebPak = itemRow["GBPAK"].ToString();
                        }
                        else
                        {
                            tblPos.GebPreis = 0;
                            tblPos.Preis_Amt = 0;
                        }

                        SelRow = NewPosPreise.Select("ZULBELN = '" + itemRow["ZULBELN"].ToString() +
                                                                    "' AND UEPOS = '" + itemRow["ZULPOSNR"].ToString() +
                                                                    "' AND WEBMTART = 'K'");
                        if (SelRow.Length == 1)
                        {
                            Decimal Preis;
                            tblPos.PreisKZ = Decimal.TryParse(SelRow[0]["PREIS_C"].ToString(), out Preis) ? Preis : 0;
                        }

                        DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + tblPos.Matnr.TrimStart('0') + "'");

                        if (MatRow.Length == 1)
                        {
                            if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                            {
                                tblPos.GebMatPflicht = "X";
                            }
                        }

                        tblPos.UPreis = itemRow["UPREIS_C"].ToString();
                        tblPos.Differrenz = itemRow["DIFF_C"].ToString();
                        tblPos.Konditionstab = itemRow["KONDTAB"].ToString();
                        tblPos.Konditionsart = itemRow["KSCHL"].ToString();

                        if (ZLDCommon.IsDate(itemRow["CALCDAT"].ToString()))
                        {
                            tblPos.CalcDat = DateTime.Parse(itemRow["CALCDAT"].ToString());
                        }
                    }

                    LastID = tmpID;
                }
            }

            ZLD_DataContext.Connection.Open();
            ZLD_DataContext.SubmitChanges();
            ZLD_DataContext.Connection.Close();
        }

        /// <summary>
        /// Preise bereits vorhandener Positionen(in der Tabelle Positionen)
        /// aus SAP ziehen, später in internen 
        /// Tabellen aktualisieren und anzeigen. Bapi: Z_ZLD_PREISFINDUNG
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ChangeZLDNach.aspx</param>
        /// <param name="tblStvaStamm">Stammtabelle StVa</param>
        /// <param name="tblMaterialStamm">Stammtabelle Material</param>
        public void GetPreise(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblStvaStamm, DataTable tblMaterialStamm)
        {
            m_strClassAndMethod = "NacherfZLD.GetPreise";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
          
            ClearError();

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                var ZLD_DataContext = new ZLDTableClassesDataContext();

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_PREISFINDUNG", ref m_objApp, ref m_objUser, ref page);

                    DataTable importAuftrag = myProxy.getImportTable("GT_IMP_BAK");
                    DataTable importPos = myProxy.getImportTable("GT_IMP_POS_S01");

                    DataRow importAuftrRow = importAuftrag.NewRow();
                    importAuftrRow["MANDT"] = "010";
                    importAuftrRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
                    importAuftrRow["VBELN"] = "";
                    importAuftrRow["VKORG"] = strVKORG;
                    importAuftrRow["VKBUR"] = strVKBUR;
                    importAuftrRow["ERNAM"] = m_objUser.UserName.PadLeft(12);
                    importAuftrRow["ERDAT"] = DateTime.Now;
                    importAuftrRow["FLAG"] = "";
                    importAuftrRow["BARCODE"] = Barcode;
                    importAuftrRow["KUNNR"] = KunnrPreis.PadLeft(10, '0');
                    importAuftrRow["ZZREFNR1"] = Referenz1;
                    importAuftrRow["ZZREFNR2"] = Referenz2;
                    importAuftrRow["KREISKZ"] = KreisKZ;
                    DataRow[] RowStva = tblStvaStamm.Select("KREISKZ='" + KreisKZ + "'");
                    
                    if (RowStva.Length == 1)
                    {
                        importAuftrRow["KREISBEZ"] = RowStva[0]["KREISBEZ"];
                    }
                    else
                    {
                        importAuftrRow["KREISBEZ"] = KreisBez;
                    }

                    importAuftrRow["WUNSCHKENN_JN"] = ZLDCommon.BoolToX(WunschKenn);
                    importAuftrRow["ZUSKENNZ"] = ZLDCommon.BoolToX(ZusatzKZ);
                    importAuftrRow["WU_KENNZ2"] = WunschKZ2;
                    importAuftrRow["WU_KENNZ3"] = WunschKZ3;
                    importAuftrRow["O_G_VERSSCHEIN"] = ZLDCommon.BoolToX(OhneGruenenVersSchein);
                    importAuftrRow["SOFORT_ABR_ERL"] = ZLDCommon.BoolToX(SofortabrechnungErledigt);
                    importAuftrRow["SA_PFAD"] = SofortabrechnungPfad;
                    importAuftrRow["RESERVKENN_JN"] = ZLDCommon.BoolToX(Reserviert);
                    importAuftrRow["RESERVKENN"] = ReserviertKennz;
                    importAuftrRow["FEINSTAUBAMT"] = ZLDCommon.BoolToX(Feinstaub);
                    importAuftrRow["ZZZLDAT"] = DateTime.Now.ToShortDateString();
                    importAuftrRow["ZZKENN"] = Kennzeichen;
                    importAuftrRow["KENNZFORM"] = KennzForm;
                    importAuftrRow["KENNZANZ"] = "0";
                    importAuftrRow["EINKENN_JN"] = ZLDCommon.BoolToX(EinKennz);
                    importAuftrRow["BEMERKUNG"] = Bemerkung;
                    importAuftrRow["INFO_TEXT"] = Infotext;
                    importAuftrRow["NACHBEARBEITEN"] = ZLDCommon.BoolToX(Nachbearbeiten);
                    importAuftrRow["ONLINE_VG"] = ZLDCommon.BoolToX(Onlinevorgang);
                    importAuftrRow["MOBUSER"] = Mobuser;
                    importAuftrRow["POOLNR"] = Poolnr;
                    importAuftrRow["ZL_RL_FRBNR_HIN"] = FrachtNrHin;
                    importAuftrRow["ZL_RL_FRBNR_ZUR"] = FrachtNrBack;
                    importAuftrRow["EC_JN"] = ZLDCommon.BoolToX(EC);
                    importAuftrRow["BAR_JN"] = ZLDCommon.BoolToX(Bar);
                    importAuftrRow["KUNDEBAR_JN"] = ZLDCommon.BoolToX(Barkunde);

                    DataRow[] tblPosCount = Positionen.Select("id_Kopf = " + KopfID);
                    String kennzMat = "";

                    foreach (DataRow PosRow in tblPosCount)
                    {
                        DataRow importRow = importPos.NewRow();
                        importRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
                        importRow["ZULPOSNR"] = PosRow["id_pos"].ToString().PadLeft(6, '0');
                        importRow["UEPOS"] = PosRow["UEPOS"].ToString().PadLeft(6, '0');
                        importRow["MENGE_C"] = PosRow["Menge"].ToString() != "" && PosRow["Menge"].ToString() != "0" ? importRow["MENGE_C"] = PosRow["Menge"].ToString() : importRow["MENGE_C"] = "1";
                        importRow["WEBMTART"] = PosRow["WebMTArt"].ToString();
                        importRow["GEB_AMT_ADD_C"] = PosRow["Preis_Amt_Add"].ToString();

                        DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + PosRow["Matnr"].ToString().TrimStart('0') + "'");
                        if (MatRow.Length == 1)
                        {
                            if (MatRow[0]["KENNZREL"].ToString() == "X")
                            {
                                if (EinKennz)
                                {
                                    importAuftrRow["KENNZANZ"] = "1";
                                }
                                else
                                {
                                    importAuftrRow["KENNZANZ"] = "2";
                                }
                                if (PosRow["id_pos"].ToString() == "10")
                                {
                                    kennzMat = MatRow[0]["KENNZMAT"].ToString().Trim(' ');
                                }
                            }
                        }

                        importRow["MATNR"] = PosRow["MATNR"].ToString().PadLeft(18, '0');
                        importRow["MAKTX"] = PosRow["MATBEZ"].ToString().TrimEnd(' ');
                        
                        if (PosRow["PosLoesch"].ToString() != "L")
                        {
                            importRow["LOEKZ"] = "";
                        }
                        else
                        {
                            importRow["LOEKZ"] = "X";
                        }

                        importRow["Preis_C"] = PosRow["Preis"];

                        if (PosRow["WEBMTART"].ToString() == "G")
                        {
                            // Bei Annahme "Neue AH-Vorgänge" soll Gebühr_Amt immer durch Preisfindung neu ermittelt werden
                            if (!SelAnnahmeAH)
                            {
                                importRow["Geb_Amt_C"] = PosRow["Preis_Amt"];
                            }
                        }
                        else if (PosRow["WEBMTART"].ToString() == "K")
                        {
                            if (kennzMat != "")
                            { importRow["LOEKZ"] = ""; }
                            else
                            {
                                importRow["LOEKZ"] = "X";
                            }
                            importRow["Preis_C"] = "0";
                        }

                        importRow["WEBMTART"] = PosRow["WEBMTART"];
                        if (PosRow["UEPOS"].ToString() != "10")
                        {
                            if (PosRow["WEBMTART"].ToString() == "S")
                            {
                                importRow["LOEKZ"] = "X";
                            }
                        }

                        importPos.Rows.Add(importRow);
                    }

                    importAuftrag.Rows.Add(importAuftrRow);

                    myProxy.callBapi();

                    NewPosPreise = myProxy.getExportTable("GT_IMP_POS_S01");
                    NewPosPreise.Columns.Add("GebMatPflicht", typeof(String));

                    foreach (DataRow itemRow in NewPosPreise.Rows)
                    {
                        DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + itemRow["MATNR"].ToString().TrimStart('0') + "'");

                        if (MatRow.Length == 1)
                        {
                            if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                            {
                                itemRow["GebMatPflicht"] = "X";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            RaiseError("-5555","Es ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
                            break;
                    }
                }
                finally
                {
                    m_blnGestartet = false;

                    if (ZLD_DataContext.Connection.State == ConnectionState.Open)
                    {
                        ZLD_DataContext.Connection.Close();
                        ZLD_DataContext.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Preise neuer hinzugefügter Positionen
        /// aus SAP ziehen, später in internen 
        /// Tabellen aktualisieren und anzeigen. Bapi: Z_ZLD_PREISFINDUNG
        /// </summary>
        /// <param name="strAppId">AppID</param>
        /// <param name="strSessionId">SessionID</param>
        /// <param name="tblPos">Neue Positionen</param>
        /// <param name="page">ChangeZLDNach.aspx</param>
        /// <param name="tblStvaStamm">Stammtabelle StVa</param>
        /// <param name="tblMaterialStamm">Stammtabelle Material</param>
        public void GetPreiseNewPositionen(String strAppId, String strSessionId, System.Web.UI.Page page, DataTable tblPos,
                                                            DataTable tblStvaStamm, DataTable tblMaterialStamm)
        {
            m_strClassAndMethod = "NacherfZLD.GetPreiseNewPositionen";
            m_strAppID = strAppId;
            m_strSessionID = strSessionId;
            
            ClearError();
            
            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                var ZLD_DataContext = new ZLDTableClassesDataContext();

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_PREISFINDUNG", ref m_objApp, ref m_objUser, ref page);

                    DataTable importAuftrag = myProxy.getImportTable("GT_IMP_BAK");
                    DataTable importPos = myProxy.getImportTable("GT_IMP_POS_S01");
                   
                    DataRow importAuftrRow = importAuftrag.NewRow();

                    importAuftrRow["MANDT"] = "010";
                    importAuftrRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                    importAuftrRow["VBELN"] = "";
                    importAuftrRow["VKORG"] = VKORG;
                    importAuftrRow["VKBUR"] = VKBUR;
                    importAuftrRow["KUNNR"] = Kunnr.PadLeft(10, '0');
                    importAuftrRow["KREISKZ"] = KreisKennz;
                    
                    DataRow[] RowStva = tblStvaStamm.Select("KREISKZ='" + KreisKennz + "'");
                    if (RowStva.Length == 1)
                    {
                        importAuftrRow["KREISBEZ"] = RowStva[0]["KREISBEZ"];
                    }
                    else
                    {
                        importAuftrRow["KREISBEZ"] = Kreis;
                    }

                    importAuftrRow["EINKENN_JN"] = ZLDCommon.BoolToX(EinKennz);
                    importAuftrRow["ZZZLDAT"] = DateTime.Now.ToShortDateString();
                    importAuftrRow["WUNSCHKENN_JN"] = ZLDCommon.BoolToX(WunschKennz);
                    importAuftrRow["ZUSKENNZ"] = ZLDCommon.BoolToX(ZusatzKZ);
                    importAuftrRow["WU_KENNZ2"] = WunschKZ2;
                    importAuftrRow["WU_KENNZ3"] = WunschKZ3;
                    importAuftrRow["O_G_VERSSCHEIN"] = ZLDCommon.BoolToX(OhneGruenenVersSchein);
                    importAuftrRow["SOFORT_ABR_ERL"] = ZLDCommon.BoolToX(SofortabrechnungErledigt);
                    importAuftrRow["SA_PFAD"] = SofortabrechnungPfad;

                    importAuftrRow["RESERVKENN_JN"] = ZLDCommon.BoolToX(Reserviert);
                    importAuftrRow["FEINSTAUBAMT"] = ZLDCommon.BoolToX(Feinstaub);
                    
                    DataRow[] tblPosCount = tblPos.Select("id_Kopf = " + KopfID);
                    Int32 ROWCOUNT = 10;
                    int posCount = 1;

                    foreach (DataRow PosRow in tblPosCount)
                    {
                        DataRow importRow = importPos.NewRow();

                        importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                        
                        if (posCount == 1) // erste neue Pos bekommt die mitgegeben posID / die nächsten Neuen werden neu generiert
                        {
                            importRow["ZULPOSNR"] = PosRow["id_pos"].ToString().PadLeft(6, '0');
                            int.TryParse(PosRow["id_pos"].ToString(), out ROWCOUNT);
                        }
                        else
                        {
                            importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
                        }
                        String sUePos = (ROWCOUNT).ToString().PadLeft(6, '0');

                        importRow["UEPOS"] = "000000";
                        importRow["MENGE_C"] = PosRow["Menge"].ToString() != "" && PosRow["Menge"].ToString() != "0" ? PosRow["Menge"].ToString() : "1";

                        importRow["WEBMTART"] = "D";

                        ROWCOUNT += 10;

                        DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + PosRow["Matnr"].ToString() + "'");
                        if (MatRow.Length == 1)
                        {
                            if (MatRow[0]["KENNZREL"].ToString() == "X")
                            {
                                importAuftrRow["KENNZANZ"] = EinKennz ? "1" : "2";
                            }
                        }
                        importRow["MATNR"] = PosRow["Matnr"].ToString().PadLeft(18, '0');
                        importRow["MAKTX"] = PosRow["Matbez"].ToString().TrimEnd(' ');
                        importRow["LOEKZ"] = PosRow["PosLoesch"].ToString() == "L" ? "X" : "";
                        importRow["PREIS_C"] = "";
                       
                        importPos.Rows.Add(importRow);

                        //--- dazu gehörige Gebührenmaterial

                        if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                        {
                            if (OhneSteuer == "X")
                            {
                                importRow = importPos.NewRow();
                               
                                importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                                importRow["UEPOS"] = sUePos;
                                importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
                                importRow["MENGE_C"] = "1";
                                importRow["MATNR"] = PosRow["GebMatnr"].ToString();
                                importRow["MAKTX"] = PosRow["GebMatbez"].ToString();
                                importRow["WEBMTART"] = "G";
                                importRow["PREIS_C"] = "";

                                if (PosRow["PosLoesch"].ToString() == "L"){ importRow["LOEKZ"] = "X"; }
                                
                                ROWCOUNT += 10;
                                importPos.Rows.Add(importRow);
                            }
                            else
                            {
                                importRow = importPos.NewRow();
                               
                                importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                                importRow["UEPOS"] = sUePos;
                                importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
                                importRow["MENGE_C"] = "1";
                                importRow["MATNR"] = PosRow["GebMatnrSt"].ToString();
                                importRow["MAKTX"] = PosRow["GebMatBezSt"].ToString();
                                importRow["PREIS_C"] = "";
                                importRow["WEBMTART"] = "G";

                                if (PosRow["PosLoesch"].ToString() == "L"){ importRow["LOEKZ"] = "X"; }

                                ROWCOUNT += 10;
                                importPos.Rows.Add(importRow);
                            }
                        }

                        if (PauschalKunde != "X")
                        {
                            if (MatRow[0]["Kennzmat"].ToString().Length > 0)
                            {
                                importRow = importPos.NewRow();
                               
                                importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                                importRow["UEPOS"] = sUePos;
                                importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
                                
                                importRow["MENGE_C"] = "1";
                                importRow["MATNR"] = MatRow[0]["Kennzmat"].ToString();
                                importRow["PREIS_C"] = "";
                                importRow["MAKTX"] = "";
                                importRow["WEBMTART"] = "K";

                                if (PosRow["PosLoesch"].ToString() == "L"){ importRow["LOEKZ"] = "X"; }

                                ROWCOUNT += 10;
                                importPos.Rows.Add(importRow);
                            }
                        }

                        if (sUePos == "000010")// Hauptposition mit Steuermaterieal
                        {
                            importRow = importPos.NewRow();
                            
                            importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                            importRow["UEPOS"] = sUePos;
                            importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
                            importRow["MENGE_C"] = "1";
                            importRow["MATNR"] = "591".PadLeft(6, '0');
                            importRow["MAKTX"] = "";
                            importRow["WEBMTART"] = "S";

                            if (PosRow["PosLoesch"].ToString() == "L"){ importRow["LOEKZ"] = "X"; }

                            ROWCOUNT += 10;
                            importPos.Rows.Add(importRow);
                        }
                        posCount++;
                    }

                    importAuftrag.Rows.Add(importAuftrRow);

                    myProxy.callBapi();

                    NewPosPreise = myProxy.getExportTable("GT_IMP_POS_S01");
                    PreisKennz = 0.00m;

                    foreach (DataRow itemRow in NewPosPreise.Rows)
                    {
                        DataRow tblRow = Positionen.NewRow();

                        tblRow["id_Kopf"] = KopfID;
                        tblRow["id_pos"] = itemRow["ZULPOSNR"].ToString();
                        tblRow["uepos"] = itemRow["UEPOS"].ToString();
                        tblRow["Matnr"] = itemRow["MATNR"].ToString().TrimStart('0');
                        tblRow["Matbez"] = itemRow["MAKTX"].ToString();
                        tblRow["SdRelevant"] = itemRow["SD_REL"].ToString();
                        tblRow["WEBMTART"] = itemRow["WEBMTART"].ToString();
                        tblRow["Gebpak"] = itemRow["GBPAK"].ToString();
                        tblRow["Preis_Amt_Add"] = itemRow["GEB_AMT_ADD_C"].ToString();
                        
                        Decimal iMenge = 1;
                        if (ZLDCommon.IsDecimal(itemRow["Menge_C"].ToString().Trim()))
                        {
                            Decimal.TryParse(itemRow["Menge_C"].ToString(), out iMenge);
                        }
                        tblRow["Menge"] = iMenge.ToString("0");

                        tblRow["PosLoesch"] = itemRow["LOEKZ"].ToString() == "X"? "L":"";
                        
                        decimal dPreis;
                        tblRow["Preis"] = decimal.TryParse(itemRow["PREIS_C"].ToString(), out dPreis)? dPreis : 0;
                       
                        DataRow[] SelRow = NewPosPreise.Select("ZULBELN = '" + itemRow["ZULBELN"].ToString() +
                                     "' AND UEPOS = '" + itemRow["ZULPOSNR"].ToString() +
                                     "' AND WEBMTART = 'G'");
                        if (SelRow.Length == 1)
                        {
                            decimal dGebPreis;
                            decimal dGebAmtPreis;

                            tblRow["GebPreis"] = decimal.TryParse(SelRow[0]["PREIS_C"].ToString(), out dGebPreis) ? dGebPreis : 0;
                            tblRow["Preis_Amt"] = decimal.TryParse(SelRow[0]["GEB_AMT_C"].ToString(), out dGebAmtPreis) ? dGebAmtPreis : 0;
                           
                            tblRow["Gebpak"] = SelRow[0]["GBPAK"].ToString();
                        }
                        else if (tblRow["WEBMTART"].ToString() == "G")
                        {
                            decimal dGebAmtPreis;
                            if (decimal.TryParse(itemRow["GEB_AMT_C"].ToString(), out dGebAmtPreis))
                            {
                                tblRow["GebPreis"] = 0;
                                tblRow["Preis_Amt"] = dGebAmtPreis;
                            }
                            tblRow["Gebpak"] = itemRow["GBPAK"].ToString();
                        }
                        else
                        {
                            tblRow["GebPreis"] = 0;
                            tblRow["Preis_Amt"] = 0;
                        }
                        tblRow["ID_POS"] = itemRow["ZULPOSNR"].ToString();

                        SelRow = NewPosPreise.Select("ZULBELN = '" + itemRow["ZULBELN"].ToString() +
                                                                 "' AND UEPOS = '" + itemRow["ZULPOSNR"].ToString() +
                                                                 "' AND WEBMTART = 'K'");
                        if (SelRow.Length == 1)
                        {
                            Decimal Preis;
                            PreisKennz = Decimal.TryParse(SelRow[0]["PREIS_C"].ToString(), out Preis) ? Preis : 0;
                        }
                        DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + tblRow["Matnr"].ToString().TrimStart('0') + "'");

                        if (MatRow.Length == 1)
                        {
                            if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                            {
                                tblRow["GebMatPflicht"] = "X";
                            }
                        }

                        tblRow["UPREIS"] = itemRow["UPREIS_C"].ToString();
                        tblRow["Differrenz"] = itemRow["DIFF_C"].ToString();
                        tblRow["Konditionstab"] = itemRow["KONDTAB"].ToString();
                        tblRow["Konditionsart"] = itemRow["KSCHL"].ToString();

                        if (ZLDCommon.IsDate(itemRow["CALCDAT"].ToString()))
                        {
                            tblRow["CALCDAT"] = itemRow["CALCDAT"].ToString();
                        }

                        Positionen.Rows.Add(tblRow);
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            RaiseError("-5555","Es ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
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
        /// Läd das SDRelevant-Flag eines Gebührenmat. aus der Datenbank
        /// ist der Kunde ein Pauschalkunde,  Gebühr und Gebühr Amt unterschiedlich und 
        /// das Gebührenmaterial nicht SD relevant darf der Vorgang nicht abgesendet werden
        /// Funktion Checkgrid (ChangeZLDNachListe.aspx)
        /// </summary>
        /// <param name="IDRecordset">SQL-ID</param>
        /// <param name="idPos">Positions-ID</param>
        /// <param name="sMatnr">Materialnummer</param>
        /// <returns></returns>
        public String GetSDRelevantsGeb(Int32 IDRecordset, Int32 idPos, String sMatnr)
        {
           
            ClearError();

            var strRel = "";
            var ZLD_DataContext = new ZLDTableClassesDataContext();
            
            try
            {
               var tblPos = (from p in ZLD_DataContext.ZLDPositionsTabelle
                              where p.id_Kopf == IDRecordset && p.UEPOS == idPos && p.Matnr == sMatnr && p.WebMTArt == "G"
                              select p).Single();

                strRel = tblPos.SDRelevant;

                ZLD_DataContext.Connection.Open();
                ZLD_DataContext.SubmitChanges();
                ZLD_DataContext.Connection.Close();

            }
            catch (Exception ex)
            {
               RaiseError("-9999",ex.Message);
            }
            finally
            {
                if (ZLD_DataContext.Connection.State == ConnectionState.Open)
                {
                    ZLD_DataContext.Connection.Close();
                    ZLD_DataContext.Dispose();
                }
            }
            return strRel;
        }

        /// <summary>
        /// Prüft ob es sich bei dem ausgewählten Lieferant um ein
        /// Kroschke Zulassungsdiensthandelt. Bapi: Z_ZLD_CHECK_ZLD
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">AHVersandChange_2.aspx</param>
        public void CheckLieferant(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "NacherfZLD.CheckLieferant";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
          
            ClearError();

            id_sap = 0;
            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_CHECK_ZLD", ref m_objApp, ref m_objUser, ref page);//Z_ZLD_EXPORT_BELNR

                    myProxy.setImportParameter("I_VKORG", "1010");
                    myProxy.setImportParameter("I_VKBUR", Lieferant_ZLD.TrimStart('0').Substring(2, 4));

                    myProxy.callBapi();

                    IsZLD = myProxy.getExportParameter("E_ZLD");
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            RaiseError("-5555","Keine Daten gefunden(Kreiskennzeichen).");
                            break;
                        default:
                            RaiseError("-9999","Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + 
                                HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Kombiniert die Materialbezeichnung mit einem Mengenwert
        /// </summary>
        /// <param name="bezeichnung">Materialbezeichnung</param>
        /// <param name="menge">Menge</param>
        /// <param name="max">Maximale Länge des Strings (default: 40 Zeichen)</param>
        /// <returns>Kombiniertet String</returns>
        private string CombineBezeichnungMenge(string bezeichnung, int menge, int max = 40)
        {
            var strMengeAddon = " x" + menge.ToString();


            int iCut = bezeichnung.LastIndexOf(" x");

            // Alter Werte vorhanden?
            if (iCut != -1)
            {
                var count = bezeichnung.Length - iCut;
                bezeichnung = bezeichnung.Remove(iCut, count);
            }

            if (menge > 1)
            {
                // Gesamtlänge mehr als n Zeichen
                if (bezeichnung.Length + strMengeAddon.Length > max)
                {
                    var idxRemove = (max - 1) - strMengeAddon.Length;
                    var count = bezeichnung.Length - 1 - idxRemove;
                    bezeichnung = bezeichnung.Remove(idxRemove, count);
                }

                bezeichnung += strMengeAddon;
            }

            return bezeichnung;
        }

        /// <summary>
        /// Löscht den Fehlerstatus der Klasse inklusive Fehlertabelle
        /// </summary>
        protected override void ClearError()
        {
            base.ClearError();
            tblErrors = null;
        }

        #endregion
    }
}
