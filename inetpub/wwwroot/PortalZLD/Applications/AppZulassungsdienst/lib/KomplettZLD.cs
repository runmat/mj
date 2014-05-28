using System;
using System.Data.SqlClient;
using System.Linq;
using CKG.Base.Common;
using System.Data;
using CKG.Base.Business;
using System.Configuration;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Klasse für die Kompletterfassung.
    /// </summary>
    public class KomplettZLD : CKG.Base.Business.DatenimportBase
    {
        DataTable _kopfTabelle;
        DataTable _bankverbindung;
        DataTable _kundenadresse;

        #region "Properties"

        /// <summary>
        /// Tabelle zur Anzeige der erfassten Daten
        /// </summary>
        public DataTable tblEingabeListe
        {
            get;
            set;
        }
        /// <summary>
        /// Fehlertabelle SAP
        /// </summary>
        public DataTable tblFehler
        {
            get;
            set;
        }
        /// <summary>
        /// Benutzertabelle SQL
        /// </summary>
        public DataTable tblUser
        {
            get;
            set;
        }
        /// <summary>
        /// Verkaufsorganisation
        /// </summary>
        public String VKORG
        {
            get;
            set;
        }
        /// <summary>
        /// Verkaufsbüro
        /// </summary>
        public String VKBUR
        {
            get;
            set;
        }
        /// <summary>
        /// Positionstabelle
        /// </summary>
        public DataTable Positionen
        {
            get;
            set;
        }
        /// <summary>
        /// ZUBELN
        /// </summary>
        public Int32 SapID
        {
            get;
            set;
        }
        /// <summary>
        /// KopfID SQL
        /// </summary>
        public Int32 KopfID
        {
            get;
            set;
        }
        /// <summary>
        /// Flag Abgerechnet SQL
        /// </summary>
        public Boolean Abgerechnet
        {
            get;
            set;
        }
        /// <summary>
        /// Name des Kunden
        /// </summary>
        public String Kundenname
        {
            get;
            set;
        }
        /// <summary>
        /// Kundennummer
        /// </summary>
        public String Kunnr
        {
            get;
            set;
        }
        /// <summary>
        /// Referenz 1
        /// </summary>
        public String Ref1
        {
            get;
            set;
        }
        /// <summary>
        /// Referenz 2
        /// </summary>
        public String Ref2
        {
            get;
            set;
        }
        /// <summary>
        /// Kreiskennzeichen/StVa
        /// </summary>
        public String KreisKennz
        {
            get;
            set;
        }
        /// <summary>
        /// Kreisbezeichnung
        /// </summary>
        public String Kreis
        {
            get;
            set;
        }
        /// <summary>
        /// Wunschkennzeichen SQL/SAP
        /// </summary>
        public Boolean WunschKennz
        {
            get;
            set;
        }
        /// <summary>
        /// bereits Reserviert SQL/SAP
        /// </summary>
        public Boolean Reserviert
        {
            get;
            set;
        }
        /// <summary>
        /// Reservierungsnummer SQL/SAP
        /// </summary>
        public String ReserviertKennz
        {
            get;
            set;
        }
        /// <summary>
        /// Feinstaubplakette SQL/SAP
        /// </summary>
        public Boolean Feinstaub
        {
            get;
            set;
        }
        /// <summary>
        /// Zulassungsdatum SQL/SAP
        /// </summary>
        public String ZulDate
        {
            get;
            set;
        }
        /// <summary>
        /// Kennzeichen komplett SQL/SAP
        /// </summary>
        public String Kennzeichen
        {
            get;
            set;
        }
        /// <summary>
        /// Kennzeichentyp(EURO) SAP
        /// </summary>
        public String Kennztyp
        {
            get;
            set;
        }
        /// <summary>
        /// Kennzeichengröße/form
        /// </summary>
        public String KennzForm
        {
            get;
            set;
        }
        /// <summary>
        /// Kennzeichenanzahl
        /// </summary>
        public Int32 KennzAnzahl
        {
            get;
            set;
        }
        /// <summary>
        /// nur ein Kennzeichen
        /// </summary>
        public Boolean EinKennz
        {
            get;
            set;
        }
        /// <summary>
        /// Bemerkung
        /// </summary>
        public String Bemerkung
        {
            get;
            set;
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
        /// EC-Zahlung
        /// </summary>
        public Boolean EC
        {
            get;
            set;
        }
        /// <summary>
        /// Barzahlung
        /// </summary>
        public Boolean Bar
        {
            get;
            set;
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
        /// Vorgang z.B. Vorerfassung, Nacherfassung, Versandzulassung
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
        /// Kundennummer Warenempfängen
        /// </summary>
        public String KundennrWE
        {
            get;
            set;
        }
        /// <summary>
        /// Partnerrolle(z.B. WE = Warenempfänger)
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
        ///  Name des Kunden(abweichende Adressdaten)
        /// </summary>
        public String Name2
        {
            get;
            set; 
        }
        /// <summary>
        ///  Postleitzahl des Kunden(abweichende Adressdaten)
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
        /// Inhaber der Firma
        /// </summary>
        public String Inhaber
        {
            get;
            set;
        }
        /// <summary>
        /// Name des Geldinstitutes
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
        /// Kunde möcht per Rechnung zahlen
        /// </summary>
        public Boolean Rechnung
        {
            get;
            set;
        }
        /// <summary>
        /// Rückgabetabelle(Kunde, Referenzen etc.) der Selektion über Barcode
        /// </summary>
        public DataTable tblBarcodData
        {
            get;
            set;
        }
        /// <summary>
        /// Rückgabetabelle(Dienstleistungen) der Selektion über Barcode
        /// </summary>
        public DataTable tblBarcodMaterial
        {
            get;
            set;
        }
        /// <summary>
        /// Kunde ist Barkunde
        /// </summary>
        public Boolean Barkunde
        {
            get;
            set;
        }
        /// <summary>
        /// Neue Positionen Preisfindung
        /// </summary>
        public DataTable NewPosPreise
        {
            get;
            set;
        }
        /// <summary>
        /// Kunde ist Pauschalkunde
        /// </summary>
        public String PauschalKunde
        {
            get ;
            set ; 

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
        /// CPD-Adresse bestätigt!?
        /// </summary>
        public Boolean ConfirmCPDAdress
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
        /// Wurden Dienstleistungen nachträglich geändert?!
        /// </summary>
        public Boolean ChangeMatnr
        {
            get;
            set;
        }
        /// <summary>
        /// von Welchen benutzer sollen die Daten angezeigt werden
        /// </summary>
        public String SelctedUserID
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

        #endregion

        /// <summary>
        /// Kontruktor der Klasse.
        /// </summary>
        /// <param name="objUser">User-Objekt</param>
        /// <param name="objApp">Anwendungsobjekt</param>
        /// <param name="sVorgang">Vorgang</param>
        public KomplettZLD(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, String sVorgang)
            : base(ref objUser, objApp, "")
        {
            ListePageIndex = 0;
            ListePageSize = 20;
            ListePageSizeIndex = 1;
            Vorgang = sVorgang;
            CreatePosTable();     
        }

        /// <summary>
        /// Bereits vorhanden Daten über DAD-Barcode laden. Bapi: Z_ZLD_GET_DAD_SD_ORDER
        /// </summary>
        /// <param name="strAppId">ApppID</param>
        /// <param name="strSessionId">SessionID</param>
        /// <param name="page">ChangeZLDKomplett.aspx</param>
        public void getDataFromBarcode(String strAppId, String strSessionId, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "KomplettZLD.getDataFromBarcode";
            m_strAppID = strAppId;
            m_strSessionID = strSessionId;
           
            ClearError();

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_GET_DAD_SD_ORDER", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VBELN", Barcode);

                    myProxy.callBapi();

                    tblBarcodData = myProxy.getExportTable("GS_DAD_ORDER");
                    tblBarcodMaterial = myProxy.getExportTable("GT_MAT");

                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out subrc);
                    m_intStatus = subrc;

                    String sapMessage = myProxy.getExportParameter("E_MESSAGE");
                    m_strMessage = sapMessage;
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            RaiseError("-5555","Keine Daten gefunden zum Barcode gefunden.");
                            break;
                        default:
                            RaiseError("-9999","Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Nächste freie Belegnummer aus SAP ziehen. Bapi: Z_ZLD_EXPORT_BELNR
        /// </summary>
        /// <param name="strAppId">ApppID</param>
        /// <param name="strSessionId">SessionID</param>
        /// <param name="page">ChangeZLDKomplett.aspx</param>
        public void GiveSapID(String strAppId, String strSessionId, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "KomplettZLD.GiveSapID";
            m_strAppID = strAppId;
            m_strSessionID = strSessionId;
            
            ClearError();

            SapID = 0;
            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_BELNR", ref m_objApp, ref m_objUser, ref page);

                    myProxy.callBapi();

                    Int32 idSap;
                    Int32.TryParse(myProxy.getExportParameter("E_BELN").ToString(), out idSap);
                    SapID = idSap;

                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    m_intStatus = subrc;
                    
                    String sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    m_strMessage = sapMessage;
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            RaiseError("-5555","Keine Daten gefunden(Kreiskennzeichen).");
                            break;
                        default:
                            RaiseError("-9999","Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Interne Positionstabelle aufbauen.
        /// </summary>
        private void CreatePosTable()
        {
            Positionen = new DataTable();
            Positionen.Columns.Add("id_Kopf", typeof(Int32));
            Positionen.Columns.Add("id_pos", typeof(Int32));
            Positionen.Columns.Add("Menge", typeof(String));
            Positionen.Columns.Add("Matnr", typeof(String));
            Positionen.Columns.Add("Matbez", typeof(String));
            Positionen.Columns.Add("Preis", typeof(String));
            Positionen.Columns.Add("GebPreis", typeof(String));
            Positionen.Columns.Add("Preis_Amt", typeof(String));
            Positionen.Columns.Add("Preis_Amt_Add", typeof(String));
            Positionen.Columns.Add("PosLoesch", typeof(String));
            Positionen.Columns.Add("GebMatnr", typeof(String));
            Positionen.Columns.Add("GebMatbez", typeof(String));
            Positionen.Columns.Add("GebMatnrSt", typeof(String));
            Positionen.Columns.Add("GebMatBezSt", typeof(String));
            Positionen.Columns.Add("PreisKZ", typeof(String));
            Positionen.Columns.Add("KennzMat", typeof(String));
            Positionen.Columns.Add("SDRelevant", typeof(String));
            Positionen.Columns.Add("GebMatPflicht", typeof(String));
            Positionen.Columns.Add("WebMTArt", typeof(String));
            Positionen.Columns.Add("uepos", typeof(String));
            Positionen.Columns.Add("UPreis", typeof(String));
            Positionen.Columns.Add("Differrenz", typeof(String));
            Positionen.Columns.Add("Konditionstab", typeof(String));
            Positionen.Columns.Add("Konditionsart", typeof(String));
            Positionen.Columns.Add("CALCDAT", typeof(String));
            Positionen.Columns.Add("GebPak", typeof(String));
        }

        /// <summary>
        /// Preise der einzelnen Positionen aus SAP ziehen
        /// alle Positionen des Vorgangs(Button:Preis finden). Bapi: Z_ZLD_PREISFINDUNG
        /// </summary>
        /// <param name="strAppId">AppId</param>
        /// <param name="strSessionId">SessionId</param>
        /// <param name="page">ChangeZLDKomplett.aspx</param>
        /// <param name="tblStvaStamm">Stammtabelle StVa</param>
        /// <param name="tblMaterialStamm">Dienstleistungstabelle</param>
        public void GetPreise(String strAppId, String strSessionId, System.Web.UI.Page page, DataTable tblStvaStamm, DataTable tblMaterialStamm)
        {
            m_strClassAndMethod = "NacherfZLD.GetPreise";
            m_strAppID = strAppId;
            m_strSessionID = strSessionId;

            ClearError();
            //var tblData = new DataTable();
            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                var ZLD_DataContext = new ZLDTableClassesDataContext();
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_PREISFINDUNG", ref m_objApp, ref m_objUser, ref page);


                    DataTable importAuftrag = myProxy.getImportTable("GT_IMP_BAK");

                    // Z = Zwischenlösung für Konvertierungsfehler von BCD-Werten im DynProxy

                    // Z - siehe oben ++++++++
                    DataTable importPos = myProxy.getImportTable("GT_IMP_POS_S01");
                    // +++++++++++++++++++++++

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

                    importAuftrRow["RESERVKENN_JN"] = ZLDCommon.BoolToX(Reserviert);
                    importAuftrRow["FEINSTAUBAMT"] = ZLDCommon.BoolToX(Feinstaub);

                    DataRow[] tblPosCount = Positionen.Select("id_Kopf = " + KopfID);
                    Int32 ROWCOUNT = 10;

                    var tblHoldPrices = new DataTable();
                    tblHoldPrices.Columns.Add("ZULPOSNR", typeof(System.String));
                    tblHoldPrices.Columns.Add("Preis", typeof(System.String));
                    // zu jedem Kopf die Positionen aufbauen
                    
                    foreach (DataRow PosRow in tblPosCount)
                    {
                        DataRow importRow = importPos.NewRow();

                        importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                        importRow["ZULPOSNR"] = PosRow["id_pos"].ToString().PadLeft(6, '0');
                        String sUePos = (ROWCOUNT).ToString().PadLeft(6, '0');
                        importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
                        importRow["UEPOS"] = "000000";
                        // Z - siehe oben ++++++++
                        importRow["MENGE_C"] = (PosRow["Menge"].ToString() != "" ? PosRow["Menge"].ToString() : "1");
                        //
                        importRow["WEBMTART"] = "D";
  
                        ROWCOUNT += 10;
                        DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + PosRow["Matnr"].ToString() + "'");
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
                            }
                        }
                        importRow["MATNR"] = PosRow["Matnr"].ToString().PadLeft(18, '0');
                        importRow["MAKTX"] = PosRow["Matbez"].ToString().TrimEnd(' ');
                        importRow["LOEKZ"] = "";
                        importRow["GBPAK"] = "";
                        if (PosRow["PosLoesch"].ToString() == "L"){ importRow["LOEKZ"] = "X"; }
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
                                // Z - siehe oben ++++++++
                                importRow["MENGE_C"] = "1";
                                // +++++++++++++++++++++++
                                importRow["MATNR"] = PosRow["GebMatnr"].ToString();
                                importRow["MAKTX"] = PosRow["GebMatbez"].ToString();
                                importRow["WEBMTART"] = "G";
                                // Z - siehe oben ++++++++
                                importRow["PREIS_C"] = "";
                                if (PosRow["PosLoesch"].ToString() == "L"){ importRow["LOEKZ"] = "X"; }
                                // +++++++++++++++++++++++
                                ROWCOUNT += 10;
                                importPos.Rows.Add(importRow);
                            }
                            else
                            {
                                importRow = importPos.NewRow();
                                importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                                importRow["UEPOS"] = sUePos;
                                importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
                                // Z - siehe oben ++++++++
                                importRow["MENGE_C"] = "1";
                                // +++++++++++++++++++++++
                                importRow["MATNR"] = PosRow["GebMatnrSt"].ToString();
                                importRow["MAKTX"] = PosRow["GebMatBezSt"].ToString();
                                // Z - siehe oben ++++++++
                                importRow["PREIS_C"] = "";
                                // +++++++++++++++++++++++
                                importRow["WEBMTART"] = "G";
                                if (PosRow["PosLoesch"].ToString() == "L"){ importRow["LOEKZ"] = "X"; }
                                ROWCOUNT += 10;
                                importPos.Rows.Add(importRow);
                            }
                        }

                        if (PauschalKunde != "X")
                        {
                            if (PosRow["Kennzmat"].ToString().Trim(' ') != "")
                            {
                                importRow = importPos.NewRow();
                                // Z - siehe oben ++++++++
                                importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                                importRow["UEPOS"] = sUePos;
                                importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
                                // Z - siehe oben ++++++++
                                importRow["MENGE_C"] = "1";
                                // +++++++++++++++++++++++
                                importRow["MATNR"] = PosRow["Kennzmat"].ToString();
                                // Z - siehe oben ++++++++
                                importRow["PREIS_C"] = "";
                                // +++++++++++++++++++++++
                                importRow["MAKTX"] = "";
                                importRow["WEBMTART"] = "K";
                                if (PosRow["PosLoesch"].ToString() == "L")
                                { importRow["LOEKZ"] = "X"; }
                                ROWCOUNT += 10;
                                importPos.Rows.Add(importRow);
                            }
                        }
                        if (sUePos == "000010")// Hauptposition mit Steuermaterieal
                        {
                            importRow = importPos.NewRow();
                            importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                            importRow["UEPOS"] = sUePos;
                            importRow["ZULPOSNR"] = ROWCOUNT.ToString().PadLeft(6, '0');
                            // Z - siehe oben ++++++++
                            importRow["MENGE_C"] = "1";
                            // +++++++++++++++++++++++
                            importRow["MATNR"] = "591".PadLeft(18, '0');
                            importRow["MAKTX"] = "";
                            importRow["WEBMTART"] = "S";
                            if (PosRow["PosLoesch"].ToString() == "L")
                            { importRow["LOEKZ"] = "X"; }
                            ROWCOUNT += 10;
                            importPos.Rows.Add(importRow);
                        }
                    }

                    importAuftrag.Rows.Add(importAuftrRow);

                    myProxy.callBapi();

                    NewPosPreise = myProxy.getExportTable("GT_IMP_POS_S01");
                    Positionen.Rows.Clear();

                    PreisKennz=0.00m;
                    // neugezogene Preise bzw. Daten wieder in die Positionstabelle einfügen
                    // wichtig hier bei das SD_Relevant-Kennzeichen 
                    foreach (DataRow itemRow in NewPosPreise.Rows)
                    {
                        DataRow tblRow = Positionen.NewRow();

                        tblRow["id_Kopf"] = KopfID;
                        tblRow["id_pos"] = itemRow["ZULPOSNR"].ToString();
                        tblRow["uepos"] = itemRow["UEPOS"].ToString(); 
                        tblRow["Matnr"] = itemRow["MATNR"].ToString();
                        tblRow["Matbez"] = itemRow["MAKTX"].ToString();
                        tblRow["SdRelevant"] = itemRow["SD_REL"].ToString();
                        tblRow["WEBMTART"] = itemRow["WEBMTART"].ToString();
                        tblRow["Menge"] = itemRow["Menge_C"].ToString();
                        tblRow["Gebpak"] = itemRow["GBPAK"].ToString();
                        tblRow["Preis_Amt_Add"] = itemRow["GEB_AMT_ADD_C"].ToString();

                        Decimal iMenge = 1;
                        if (ZLDCommon.IsDecimal(itemRow["Menge_C"].ToString().Trim()))
                        {
                            Decimal.TryParse(itemRow["Menge_C"].ToString(), out iMenge);
                        }
                        tblRow["Menge"] = iMenge.ToString("0");
                            
                        tblRow["PosLoesch"] = "";
                        if (itemRow["LOEKZ"].ToString() == "X")
                        { tblRow["PosLoesch"] = "L"; }
                        // Z - siehe oben ++++++++
                        decimal dPreis;
                        if (decimal.TryParse(itemRow["PREIS_C"].ToString(), out dPreis))
                        {
                            tblRow["Preis"] = dPreis;
                        }

                        DataRow[] SelRow = NewPosPreise.Select("ZULBELN = '" + itemRow["ZULBELN"].ToString() +
                                        "' AND UEPOS = '" + itemRow["ZULPOSNR"].ToString() +
                                        "' AND WEBMTART = 'G'");
                        if (SelRow.Length == 1)
                        {  
                            decimal dGebPreis;
                            if (decimal.TryParse(SelRow[0]["PREIS_C"].ToString(), out dGebPreis))
                            {
                                tblRow["GebPreis"] = dGebPreis;
                            }
                            decimal dGebAmt;
                            if (decimal.TryParse(SelRow[0]["GEB_AMT_C"].ToString(), out dGebAmt))
                            {
                                tblRow["Preis_Amt"] = dGebAmt;
                            }
                            tblRow["GebMatnr"] = SelRow[0]["Matnr"].ToString();
                            tblRow["Gebpak"] = SelRow[0]["GBPAK"].ToString();
                            // ++++++++++++++++++++++
                        }
                        else { tblRow["GebPreis"] = 0; tblRow["Preis_Amt"] = 0; }

                        tblRow["ID_POS"] = itemRow["ZULPOSNR"].ToString();

                        SelRow = NewPosPreise.Select("ZULBELN = '" + itemRow["ZULBELN"].ToString() +
                                                                    "' AND UEPOS = '" + itemRow["ZULPOSNR"].ToString() +
                                                                    "' AND WEBMTART = 'K'");
                        if (SelRow.Length == 1)
                        {
                            if (ZLDCommon.IsDecimal(SelRow[0]["PREIS_C"].ToString()))
                            {
                                Decimal Preis;
                                PreisKennz = Decimal.TryParse(SelRow[0]["PREIS_C"].ToString(), out Preis) ? Preis : 0;
                            }
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
        /// Preise der einzelnen Positionen aus SAP ziehen
        /// nur die hinzugefügten Positionen des Vorgangs(Button:Preis ergänzte DL). Bapi: Z_ZLD_PREISFINDUNG
        /// </summary>
        /// <param name="strAppId">AppId</param>
        /// <param name="strSessionId">SessionId</param>
        /// <param name="page">ChangeZLDKomplett.aspx</param>
        /// <param name="tblPositionen">Positionstabelle</param>
        /// <param name="tblStvaStamm">Stammtabelle StVa</param>
        /// <param name="tblMaterialStamm">Dienstleistungstabelle</param>
        public void GetPreiseNewPositionen(String strAppId, String strSessionId, System.Web.UI.Page page, DataTable tblPositionen, 
                                                                    DataTable tblStvaStamm, DataTable tblMaterialStamm)
        {
            m_strClassAndMethod = "NacherfZLD.GetPreiseNewPositionen";
            m_strAppID = strAppId;
            m_strSessionID = strSessionId;
           
            ClearError();

            //DataTable tblData = new DataTable();
            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                var ZLD_DataContext = new ZLDTableClassesDataContext();
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_PREISFINDUNG", ref m_objApp, ref m_objUser, ref page);

                    DataTable importAuftrag = myProxy.getImportTable("GT_IMP_BAK");

                    // Z = Zwischenlösung für Konvertierungsfehler von BCD-Werten im DynProxy

                    // Z - siehe oben ++++++++
                    DataTable importPos = myProxy.getImportTable("GT_IMP_POS_S01");
                    // +++++++++++++++++++++++

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
                    importAuftrRow["ZZZLDAT"] = System.DateTime.Now.ToShortDateString();
                    importAuftrRow["WUNSCHKENN_JN"] = ZLDCommon.BoolToX(WunschKennz);
                    importAuftrRow["ZUSKENNZ"] = ZLDCommon.BoolToX(ZusatzKZ);
                    importAuftrRow["WU_KENNZ2"] = WunschKZ2;
                    importAuftrRow["WU_KENNZ3"] = WunschKZ3;
                    importAuftrRow["O_G_VERSSCHEIN"] = ZLDCommon.BoolToX(OhneGruenenVersSchein);
                    importAuftrRow["RESERVKENN_JN"] = ZLDCommon.BoolToX(Reserviert);
                    importAuftrRow["FEINSTAUBAMT"] = ZLDCommon.BoolToX(Feinstaub);

                    var tblHoldPrices= new DataTable();
                    tblHoldPrices.Columns.Add("ZULPOSNR", typeof(System.String));
                    tblHoldPrices.Columns.Add("Preis", typeof(System.String));

                    DataRow[] tblPosCount = tblPositionen.Select("id_Kopf = " + KopfID);
                    Int32 ROWCOUNT = 10;
                    int posCount = 1;
                    foreach (DataRow PosRow in tblPosCount)
                    {
                        DataRow importRow = importPos.NewRow();
                        DataRow PreisRow = tblHoldPrices.NewRow();

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
                        PreisRow["ZULPOSNR"] = importRow["ZULPOSNR"] ;
                        String sUePos = (ROWCOUNT).ToString().PadLeft(6, '0');
                        importRow["UEPOS"] = "000000";
                        // Z - siehe oben ++++++++
                        importRow["MENGE_C"] = PosRow["Menge"].ToString() != "" ? PosRow["Menge"].ToString() : "1";
                        //
                        importRow["WEBMTART"] = "D";

                        ROWCOUNT += 10;
                        DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + PosRow["Matnr"].ToString() + "'");
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
                            }
                        }
                        importRow["MATNR"] = PosRow["Matnr"].ToString().PadLeft(18, '0');
                        importRow["MAKTX"] = PosRow["Matbez"].ToString().TrimEnd(' ');
                        importRow["LOEKZ"] = "";
                        importRow["GBPAK"] = "";
                        if (PosRow["PosLoesch"].ToString() == "L"){ importRow["LOEKZ"] = "X"; }

                        PreisRow["Preis"] = PosRow["Preis"];
                        tblHoldPrices.Rows.Add(PreisRow);

                        importRow["PREIS_C"] = "";
                        // +++++++++++++++++++++++
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
                                // Z - siehe oben ++++++++
                                importRow["MENGE_C"] = "1";
                                // +++++++++++++++++++++++
                                importRow["MATNR"] = PosRow["GebMatnr"].ToString();
                                importRow["MAKTX"] = PosRow["GebMatbez"].ToString();
                                importRow["WEBMTART"] = "G";
                                // Z - siehe oben ++++++++
                                importRow["PREIS_C"] = "";
                                if (PosRow["PosLoesch"].ToString() == "L"){ importRow["LOEKZ"] = "X"; }
                                // +++++++++++++++++++++++
                                ROWCOUNT += 10;
                                importPos.Rows.Add(importRow);
                            }
                            else
                            {
                                importRow = importPos.NewRow();
                                importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                                importRow["UEPOS"] = sUePos;
                                importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
                                // Z - siehe oben ++++++++
                                importRow["MENGE_C"] = "1";
                                // +++++++++++++++++++++++
                                importRow["MATNR"] = PosRow["GebMatnrSt"].ToString();
                                importRow["MAKTX"] = PosRow["GebMatBezSt"].ToString();
                                // Z - siehe oben ++++++++
                                importRow["PREIS_C"] = "";
                                // +++++++++++++++++++++++
                                importRow["WEBMTART"] = "G";
                                if (PosRow["PosLoesch"].ToString() == "L"){ importRow["LOEKZ"] = "X"; }
                                ROWCOUNT += 10;
                                importPos.Rows.Add(importRow);
                            }
                        }

                        if (PauschalKunde != "X")
                        {
                            if (PosRow["Kennzmat"].ToString().Trim(' ') != "")
                            {
                                importRow = importPos.NewRow();
                                // Z - siehe oben ++++++++
                                importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                                importRow["UEPOS"] = sUePos;
                                importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
                                // Z - siehe oben ++++++++
                                importRow["MENGE_C"] = "1";
                                // +++++++++++++++++++++++
                                importRow["MATNR"] = PosRow["Kennzmat"].ToString();
                                // Z - siehe oben ++++++++
                                importRow["PREIS_C"] = "";
                                // +++++++++++++++++++++++
                                importRow["MAKTX"] = "";
                                importRow["WEBMTART"] = "K";if (PosRow["PosLoesch"].ToString() == "L")
                                { importRow["LOEKZ"] = "X"; }
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
                            // Z - siehe oben ++++++++
                            importRow["MENGE_C"] = "1";
                            // +++++++++++++++++++++++
                            importRow["MATNR"] = "591".PadLeft(18, '0');
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
                        tblRow["Matnr"] = itemRow["MATNR"].ToString();
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

                        tblRow["PosLoesch"] = "";
                        if (itemRow["LOEKZ"].ToString() == "X"){ tblRow["PosLoesch"] = "L"; }
                        // Z - siehe oben ++++++++

                        decimal dPreis;
                        if (decimal.TryParse(itemRow["PREIS_C"].ToString(), out dPreis))
                        {
                            tblRow["Preis"] = dPreis;
                        }

                        DataRow[] HoldPreis = tblHoldPrices.Select("ZULPOSNR='" + itemRow["ZULPOSNR"].ToString() + "'");

                        if (HoldPreis.Length == 1) 
                        {
                            decimal InputPreis = 0;
                            if (ZLDCommon.IsDecimal(HoldPreis[0]["Preis"].ToString()))
                            {
                                decimal.TryParse(HoldPreis[0]["Preis"].ToString(), out InputPreis);
                            }
                            if (InputPreis != 0 && itemRow["WEBMTART"].ToString() == "D")  //falls schon ein Preis eingeben wurde diesen übernehmen
                            {
                                tblRow["Preis"] = InputPreis;
                            }
                        }

                        // +++++++++++++++++++++++
                        DataRow[] SelRow = NewPosPreise.Select("ZULBELN = '" + itemRow["ZULBELN"].ToString() +
                                     "' AND UEPOS = '" + itemRow["ZULPOSNR"].ToString() +
                                     "' AND WEBMTART = 'G'");
                        if (SelRow.Length == 1)
                        {
                            decimal dGebPreis;
                            if (decimal.TryParse(SelRow[0]["PREIS_C"].ToString(), out dGebPreis))
                            {
                                tblRow["GebPreis"] = dGebPreis;
                                tblRow["Gebpak"] = SelRow[0]["GBPAK"].ToString();
                            }
                            decimal dGebAmt;
                            if (decimal.TryParse(SelRow[0]["GEB_AMT_C"].ToString(), out dGebAmt))
                            {
                                tblRow["Preis_Amt"] = dGebAmt;
                            }
                            // ++++++++++++++++++++++
                        }
                        else { tblRow["GebPreis"] = 0; tblRow["Preis_Amt"] = 0; }
                        tblRow["ID_POS"] = itemRow["ZULPOSNR"].ToString();

                        SelRow = NewPosPreise.Select("ZULBELN = '" + itemRow["ZULBELN"].ToString() +
                                                                 "' AND UEPOS = '" + itemRow["ZULPOSNR"].ToString() +
                                                                 "' AND WEBMTART = 'K'");
                        if (SelRow.Length == 1)
                        {
                            if (ZLDCommon.IsDecimal(SelRow[0]["PREIS_C"].ToString()))
                            {
                                Decimal Preis;
                                PreisKennz = Decimal.TryParse(SelRow[0]["PREIS_C"].ToString(), out Preis) ? Preis : 0;
                            }
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
        /// Daten in die Sql-Tabellen speichern(ZLDKopfTabelle,ZLDPositionsTabelle, ZLDBankverbindung, ZLDKundenadresse ).
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ChangeZLDKomplett.aspx</param>
        /// <param name="tblKundenStamm">Kundenstammtabelle</param>
        /// <param name="tblMaterialStamm">Dienstleistungstabelle</param>
        public void InsertDB_ZLD(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblKundenStamm, DataTable tblMaterialStamm)
        {
            ClearError();

            var ZLD_Data = new ZLDTableClassesDataContext();// Linq initiieren
            
            try
            {
                GiveSapID(strAppID, strSessionID, page);//SAP-Nummer ziehen
                if (SapID != 0)
                {
                    ZLD_Data.Connection.Open();
                    // Kopfdaten füllen
                    {
                        var tblKopf = new ZLDKopfTabelle
                            {
                                id_sap = SapID,
                                id_user = m_objUser.UserID,
                                id_session = strSessionID,
                                abgerechnet = false,
                                username = m_objUser.UserName,
                                kundenname = Kundenname,
                                kundennr = Kunnr
                            };
                        // Daten des Kunden aus der Stammdatentab. ziehen
                        DataRow[] KundeRow = tblKundenStamm.Select("KUNNR='" + Kunnr + "'");

                        if (KundeRow.Length == 1)
                        {
                            tblKopf.OhneSteuer = KundeRow[0]["OHNEUST"].ToString();
                            tblKopf.PauschalKunde = KundeRow[0]["ZZPAUSCHAL"].ToString();
                            tblKopf.KunnrLF = KundeRow[0]["KUNNR_LF"].ToString();
                            tblKopf.KreisKZ_Direkt = KundeRow[0]["KREISKZ_DIREKT"].ToString();

                            if (KundeRow[0]["EXTENSION1"].ToString().Length > 0)
                            { 
                                tblKopf.kundenname += " / " + KundeRow[0]["EXTENSION1"].ToString(); 
                            }
                        }

                        tblKopf.referenz1 = Ref1;
                        tblKopf.referenz2 = Ref2;
                        tblKopf.KreisKZ = KreisKennz;
                        tblKopf.KreisBez = Kreis;
                        tblKopf.WunschKenn = WunschKennz;
                        tblKopf.ZusatzKZ = ZLDCommon.BoolToX(ZusatzKZ);
                        tblKopf.WunschKZ2 = WunschKZ2;
                        tblKopf.WunschKZ3 = WunschKZ3;
                        tblKopf.OhneGruenenVersSchein = ZLDCommon.BoolToX(OhneGruenenVersSchein);
                        tblKopf.Reserviert = Reserviert;
                        tblKopf.ReserviertKennz = ReserviertKennz;
                        tblKopf.Feinstaub = Feinstaub;
                        Int32 iMenge = 1;
                        DateTime tmpDate;
                        DateTime.TryParse(ZulDate, out tmpDate);
                        tblKopf.Zulassungsdatum = tmpDate;
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

                        tblKopf.KennzAnz = EinKennz ? 1 : 2;
                        tblKopf.EinKennz = EinKennz;
                        
                        tblKopf.Bemerkung = Bemerkung;
                        tblKopf.EC = EC;
                        tblKopf.Bar = Bar;
                        tblKopf.RE = RE;
                        tblKopf.saved = saved;
                        tblKopf.toDelete = "";
                        tblKopf.bearbeitet = false;
                        tblKopf.Vorgang = "K";
                        tblKopf.Barcode = Barcode;
                        tblKopf.Barkunde = Barkunde;
                        tblKopf.Steuer = Steuer;
                        tblKopf.testuser = m_objUser.IsTestUser;
                        tblKopf.Filiale = m_objUser.Reference;

                        tblKopf.Vorerfasser = m_objUser.UserName;
                        tblKopf.VorerfDatum = DateTime.Now;
                        tblKopf.VorhKennzReserv = false;
                        tblKopf.ZBII_ALT_NEU = false;
                        tblKopf.KennzVH = false;
                        tblKopf.VKKurz = "";
                        tblKopf.interneRef = "";
                        tblKopf.KundenNotiz = "";
                        tblKopf.KennzAlt = "";


                        ZLD_Data.ZLDKopfTabelle.InsertOnSubmit(tblKopf);
                        ZLD_Data.SubmitChanges();

                        KopfID = tblKopf.id;
                        bool createKopf = true;
                        //wenn kein Fehler beim Einfügen des Kopfes dann true
                        //bei Fehler Positionsdaten Kopf dann löschen

                        try
                        {   // Positionsdaten füllen
                            if (Positionen.Rows.Count > 0)
                            {
                                foreach (DataRow drow in Positionen.Rows)
                                {
                                    var tblPos = new ZLDPositionsTabelle
                                        {
                                            id_Kopf = KopfID,
                                            id_pos = (Int32) drow["id_pos"]
                                        };

                                    String sUEPOS = drow["uepos"].ToString().TrimStart('0');
                                    if (sUEPOS.Length == 0)
                                    { 
                                        tblPos.UEPOS = 0; 
                                    }
                                    else
                                    {
                                        int uepos;
                                        tblPos.UEPOS = Int32.TryParse(sUEPOS, out uepos) ? uepos : 0;
                                    }
                                    
                                    tblPos.Matnr = drow["Matnr"].ToString();

                                    tblPos.Menge = drow["Menge"].ToString();
                                    if (ZLDCommon.IsNumeric(drow["Menge"].ToString()))
                                    {
                                        Int32.TryParse(drow["Menge"].ToString(), out iMenge);
                                    }
                                    
                                    String strMatbz = drow["Matbez"].ToString();
                                    if (drow["Matbez"].ToString().Length > 0 && drow["WebMTArt"].ToString() == "D")
                                    {
                                        strMatbz = CombineBezeichnungMenge(drow["Matbez"].ToString(), iMenge);
                                    }
                                    tblPos.Matbez = strMatbz;

                                    tblPos.GebMatbez = drow["GebMatbez"].ToString();
                                    tblPos.GebMatnr = drow["GebMatnr"].ToString();
                                    tblPos.GebMatnrSt = drow["GebMatnrSt"].ToString();
                                    tblPos.GebMatBezSt = drow["GebMatBezSt"].ToString();
                                    tblPos.Kennzmat = drow["KennzMat"].ToString();
                                    tblPos.SDRelevant = drow["SDRelevant"].ToString();
                                    tblPos.PreisKZ = PreisKennz;
                                    tblPos.PosLoesch = "";
                                    tblPos.GebMatPflicht = drow["GebMatPflicht"].ToString();
                                    tblPos.WebMTArt = drow["WebMTArt"].ToString();
                                    DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + tblPos.Matnr.TrimStart('0') + "'");

                                    if (MatRow.Length == 1)
                                    {
                                        if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                                        {
                                            tblPos.GebMatPflicht = "X";
                                        }
                                    }

                                    Decimal Preis;
                                    if (ZLDCommon.IsDecimal(drow["Preis"].ToString()))
                                    {
                                        tblPos.Preis = Decimal.TryParse(drow["Preis"].ToString(), out Preis) ? Preis : 0;
                                    }
                                    else
                                    {
                                        tblPos.Preis = 0;
                                    }

                                    if (ZLDCommon.IsDecimal(drow["GebPreis"].ToString()))
                                    {
                                        tblPos.GebPreis = Decimal.TryParse(drow["GebPreis"].ToString(), out Preis) ? Preis : 0;
                                    }
                                    else
                                    {
                                        tblPos.GebPreis = 0;
                                    }

                                    if (ZLDCommon.IsDecimal(drow["Preis_Amt"].ToString()))
                                    {
                                        tblPos.Preis_Amt = Decimal.TryParse(drow["Preis_Amt"].ToString(), out Preis) ? Preis : 0;
                                    }
                                    else
                                    {
                                        tblPos.Preis_Amt = 0;
                                    }

                                    if (ZLDCommon.IsDecimal(drow["Preis_Amt_Add"].ToString()))
                                    {
                                        tblPos.Preis_Amt_Add = Decimal.TryParse(drow["Preis_Amt_Add"].ToString(), out Preis) ? Preis : 0;
                                    }
                                    else
                                    {
                                        tblPos.Preis_Amt_Add = 0;
                                    }

                                    if (ZLDCommon.IsDecimal(drow["UPREIS"].ToString()))
                                    {
                                        tblPos.UPreis = drow["UPREIS"].ToString();
                                    }
                                    else { tblPos.UPreis = "0"; }

                                    if (ZLDCommon.IsDecimal(drow["Differrenz"].ToString()))
                                    {
                                        tblPos.Differrenz = drow["Differrenz"].ToString();
                                    }
                                    else { tblPos.Differrenz = "0"; }

                                    tblPos.Konditionstab = drow["Konditionstab"].ToString();
                                    tblPos.Konditionsart = drow["Konditionsart"].ToString();
                                    if (ZLDCommon.IsDate(drow["CALCDAT"].ToString()))
                                    {
                                        DateTime.TryParse(drow["CALCDAT"].ToString(), out tmpDate);
                                        tblPos.CalcDat = tmpDate;
                                    }
                                    tblPos.GebPak = drow["GebPak"].ToString();
                                    ZLD_Data.ZLDPositionsTabelle.InsertOnSubmit(tblPos);
                                }

                            }
                            ZLD_Data.SubmitChanges();

                            // Bankdaten füllen
                            var tblBank = new ZLDBankverbindung
                                {
                                    id_Kopf = KopfID,
                                    Inhaber = Inhaber,
                                    IBAN = IBAN,
                                    Geldinstitut = Geldinstitut.Length > 40 ? Geldinstitut.Substring(0, 40) : Geldinstitut,
                                    SWIFT = SWIFT,
                                    BankKey = BankKey,
                                    Kontonr = Kontonr,
                                    EinzugErm = EinzugErm,
                                    Rechnung = Rechnung
                                };

                            ZLD_Data.ZLDBankverbindung.InsertOnSubmit(tblBank);

                            // Bankdaten füllen
                            var tblKunnadresse = new ZLDKundenadresse
                                {
                                    id_Kopf = KopfID,
                                    Partnerrolle = Partnerrolle,
                                    Name1 = Name1,
                                    Name2 = Name2,
                                    Strasse = Strasse,
                                    Ort = Ort,
                                    PLZ = PLZ
                                };
                            ZLD_Data.ZLDKundenadresse.InsertOnSubmit(tblKunnadresse);
                            ZLD_Data.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            RaiseError("9999",ex.Message + ":  Pos,-Adr,Banktabelle");
                            
                            if (createKopf) // Kopf löschen bei Fehler
                            {
                                var ZLD_DataKopf = new ZLDTableClassesDataContext();
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
                   RaiseError("9999","Fehler beim exportieren der Belegnummer!");
                }
            }
            catch (Exception ex)
            {
                RaiseError("9999",ex.Message + ":  Kopftabelle");
            }
            finally
            {
                if (ZLD_Data.Connection.State == ConnectionState.Open) { ZLD_Data.Connection.Close(); }
            }
        }

        /// <summary>
        /// Kombiniert die Materialbezeichnung mit einem Mengenwert Gesamtlänge 40 Zeichen
        /// </summary>
        /// <param name="bezeichnung">Materialbezeichnung</param>
        /// <param name="menge">Menge</param>
        /// <returns>Kombiniertet String</returns>
        private string CombineBezeichnungMenge(string bezeichnung, int menge)
        {
            return CombineBezeichnungMenge(bezeichnung, menge, 40);
        }

        /// <summary>
        /// Kombiniert die Materialbezeichnung mit einem Mengenwert
        /// </summary>
        /// <param name="bezeichnung">Materialbezeichnung</param>
        /// <param name="menge">Menge</param>
        /// <param name="max">Maximale Länge des Strings</param>
        /// <returns>Kombiniertet String</returns>
        private string CombineBezeichnungMenge(string bezeichnung, int menge,int max)
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
        /// die im Gridview editierbaren Daten abspeichern
        /// </summary>
        /// <param name="IDRecordset">ID SQL</param>
        /// <param name="IDPos">ID der Position</param>
        /// <param name="Preis">Preis Dienstleistung</param>
        /// <param name="Gebuehr">Gebühr für die Dienstleistung</param>
        /// <param name="Steuern">Steuern</param>
        /// <param name="PreisKZ">Preis für das Kennzeichen</param>
        /// <param name="KennzAbc">Kennzeichen Teil 2</param>
        /// <param name="Bar">Barzahlung</param>
        /// <param name="EC">EC-Zahlung</param>
        /// <param name="GebAmt">Gebühr Amt</param>
        public void UpdateDB_GridData(Int32 IDRecordset, Int32 IDPos, Decimal Preis,
                            Decimal Gebuehr, Decimal Steuern, Decimal PreisKZ, String KennzAbc, Boolean Bar, Boolean EC, Decimal GebAmt)
        {
            var ZLD_DataContext = new ZLDTableClassesDataContext();
            
            ClearError();

            try
            {
                var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                               where k.id == IDRecordset
                               select k).Single();

                if (SelctedUserID != null)
                {  
                    tblKopf.id_user = Convert.ToInt32(SelctedUserID); 
                } 
                else 
                {
                    tblKopf.id_user = m_objUser.UserID;
                    tblKopf.username = m_objUser.UserName; 
                }
               
                if (IDPos == 10)
                {
                    tblKopf.Steuer = Steuern;
                    if (Bar)
                    {
                        tblKopf.Bar = true;
                        tblKopf.EC = false;
                        tblKopf.RE = false;
                    }
                    else if (EC)
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
                    tblKopf.Kennzeichen = tblKopf.KennKZ + "-" + KennzAbc;
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
                        PosRow.Preis = Preis;
                        PosRow.GebPreis = Gebuehr;
                        PosRow.Preis_Amt = GebAmt;
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
                        PosRow.Preis_Amt = GebAmt;
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
                var command = new SqlCommand();
                String query = "";

                if (PosID == 10 )
                {
                    if (LoeschKZ == "L")
                    { query = ", toDelete='X'";}
                    else
                    { query = ", toDelete=''";}
                }

                String str = "Update ZLDKopfTabelle Set id_user='" + m_objUser.UserID + "', " +
                             " username='" + m_objUser.UserName + "', " +
                             "bearbeitet= 1 " + query +
                             " Where id = " + IDRecordset;

                command = new SqlCommand
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

                command = new SqlCommand {Connection = connection, CommandType = CommandType.Text, CommandText = str};
                command.ExecuteNonQuery();
                
            }
            finally { connection.Close(); }
        }

        /// <summary>
        /// die Vorgänge die in SAP erfolgreich angelegt wurden auf abgerechnet setzen
        /// damit sie später nicht mehr angezeigt werden
        /// </summary>
        /// <param name="IDRecordset">ID SQL</param>
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
        /// Daten aus der Eingabemaske speichern bzw. aktualisieren
        /// </summary>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="tblKundenStamm">Kundenstammtabelle</param>
        /// <param name="tblMaterialStamm">Dienstleistungstabelle</param>
        /// <param name="page">ChangeZLDKomplett.aspx</param>
        public void UpdateDB_ZLD(String strSessionID, DataTable tblKundenStamm, DataTable tblMaterialStamm, System.Web.UI.Page page)
        {
            var ZLD_DataContext = new ZLDTableClassesDataContext();
           
            ClearError();

            try
            {
                var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                               where k.id == KopfID
                               select k).Single();

                if (SelctedUserID != null)
                {
                    tblKopf.id_user = Convert.ToInt32(SelctedUserID);
                }
                else
                {
                    tblKopf.id_user = m_objUser.UserID;
                    tblKopf.username = m_objUser.UserName;
                }
               
                tblKopf.id_session = strSessionID;
                tblKopf.abgerechnet = false;
                
                tblKopf.kundenname = Kundenname;
                tblKopf.kundennr = Kunnr;
                DataRow[] KundeRow = tblKundenStamm.Select("KUNNR='" + Kunnr + "'");

                if (KundeRow.Length == 1)
                {
                    tblKopf.OhneSteuer = KundeRow[0]["OHNEUST"].ToString();
                    tblKopf.PauschalKunde = KundeRow[0]["ZZPAUSCHAL"].ToString();
                    tblKopf.KunnrLF = KundeRow[0]["KUNNR_LF"].ToString();
                    tblKopf.KreisKZ_Direkt = KundeRow[0]["KREISKZ_DIREKT"].ToString();
                    if (KundeRow[0]["EXTENSION1"].ToString().Length > 0)
                    { tblKopf.kundenname += " / " + KundeRow[0]["EXTENSION1"].ToString(); }
                }

                tblKopf.referenz1 = Ref1;
                tblKopf.referenz2 = Ref2;
                tblKopf.KreisKZ = KreisKennz;
                tblKopf.KreisBez = Kreis;
                tblKopf.WunschKenn = WunschKennz;
                tblKopf.ZusatzKZ = ZLDCommon.BoolToX(ZusatzKZ);
                tblKopf.WunschKZ2 = WunschKZ2;
                tblKopf.WunschKZ3 = WunschKZ3;
                tblKopf.OhneGruenenVersSchein = ZLDCommon.BoolToX(OhneGruenenVersSchein);

                tblKopf.Reserviert = Reserviert;
                tblKopf.ReserviertKennz = ReserviertKennz;
                tblKopf.Feinstaub = Feinstaub;
                Int32 iMenge = 1;
                DateTime tmpDate;
                DateTime.TryParse(ZulDate, out tmpDate);
                tblKopf.Zulassungsdatum = tmpDate;
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
                
                tblKopf.KennzAnz = EinKennz ? 1 : 2;
                tblKopf.EinKennz = EinKennz;

                tblKopf.Bemerkung = Bemerkung;
                tblKopf.EC = EC;
                tblKopf.Bar = Bar;
                tblKopf.RE = RE;
                tblKopf.Barkunde = Barkunde;
                tblKopf.saved = saved;
                tblKopf.toDelete = toDelete;
                tblKopf.bearbeitet = bearbeitet;
                tblKopf.Vorgang = "K";
                tblKopf.Barcode = Barcode;
                tblKopf.Steuer = Steuer;
                tblKopf.Filiale = m_objUser.Reference;

                ZLD_DataContext.Connection.Open();
                ZLD_DataContext.SubmitChanges();
                KopfID = tblKopf.id;
                ZLD_DataContext.Connection.Close();

                ZLD_DataContext = new ZLDTableClassesDataContext();
                ZLD_DataContext.Connection.Open();

                if (Positionen.Rows.Count > 0)
                {
                    var tblPosCount = (from p in ZLD_DataContext.ZLDPositionsTabelle
                                       where p.id_Kopf == KopfID
                                       select p);

                    if (tblPosCount.Count() == Positionen.Rows.Count || tblPosCount.Count() < Positionen.Rows.Count)
                    {
                        foreach (DataRow drow in Positionen.Rows)
                        {

                            var tblPos = (from p in ZLD_DataContext.ZLDPositionsTabelle
                                          where p.id_Kopf == KopfID && p.id_pos == (Int32)drow["id_pos"]
                                          select p);
                            if (tblPos.Count() > 0)
                            {
                                foreach (var PosRow in tblPos)
                                {
                                    PosRow.id_Kopf = KopfID;
                                    PosRow.id_pos = (Int32)drow["id_pos"];
                                    PosRow.Matnr = drow["Matnr"].ToString();
                                    PosRow.Matbez = drow["Matbez"].ToString();
                                    PosRow.GebMatbez = drow["GebMatbez"].ToString();
                                    PosRow.GebMatnr = drow["GebMatnr"].ToString();
                                    PosRow.GebMatnrSt = drow["GebMatnrSt"].ToString();
                                    PosRow.GebMatBezSt = drow["GebMatBezSt"].ToString();
                                    PosRow.Kennzmat = drow["KennzMat"].ToString();
                                    PosRow.SDRelevant = drow["SDRelevant"].ToString();
                                    PosRow.PosLoesch = drow["PosLoesch"].ToString();
                                    PosRow.PreisKZ = PreisKennz;
                                    
                                    Decimal Preis;
                                    if (ZLDCommon.IsDecimal(drow["Preis"].ToString()))
                                    {
                                        PosRow.Preis = Decimal.TryParse(drow["Preis"].ToString(), out Preis) ? Preis : 0;
                                    }
                                    else
                                    {
                                        PosRow.Preis = 0;
                                    }

                                    if (ZLDCommon.IsDecimal(drow["GebPreis"].ToString()))
                                    {
                                        PosRow.GebPreis = Decimal.TryParse(drow["GebPreis"].ToString(), out Preis) ? Preis : 0;
                                    }
                                    else
                                    {
                                        PosRow.GebPreis = 0;
                                    }

                                    if (ZLDCommon.IsDecimal(drow["Preis_Amt"].ToString()))
                                    {
                                        PosRow.Preis_Amt = Decimal.TryParse(drow["Preis_Amt"].ToString(), out Preis) ? Preis : 0;
                                    }
                                    else
                                    {
                                        PosRow.Preis_Amt = 0;
                                    }

                                    if (ZLDCommon.IsDecimal(drow["Preis_Amt_Add"].ToString()))
                                    {
                                        PosRow.Preis_Amt_Add = Decimal.TryParse(drow["Preis_Amt_Add"].ToString(), out Preis) ? Preis : 0;
                                    }
                                    else
                                    {
                                        PosRow.Preis_Amt_Add = 0;
                                    }

                                    DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + PosRow.Matnr.TrimStart('0') + "'");
                                    PosRow.GebMatPflicht = "";
                                    if (MatRow.Length == 1)
                                    {
                                        if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                                        {
                                            PosRow.GebMatPflicht = "X";
                                        }
                                    }

                                    PosRow.Menge = drow["Menge"].ToString();
                                    if (ZLDCommon.IsNumeric(drow["Menge"].ToString()))
                                    {
                                        Int32.TryParse(drow["Menge"].ToString(), out iMenge);
                                    }
                                    
                                    String strMatbz = drow["Matbez"].ToString();
                                    if (drow["Matbez"].ToString().Length > 0 && PosRow.id_pos == 10)
                                    {
                                        strMatbz = CombineBezeichnungMenge(drow["Matbez"].ToString(), iMenge);
                                    }
                                    PosRow.Matbez = strMatbz;

                                    PosRow.WebMTArt = drow["WEBMTART"].ToString();

                                    PosRow.Matnr = drow["Matnr"].ToString().PadLeft(18, '0');

                                    MatRow = tblMaterialStamm.Select("MATNR='" + PosRow.Matnr.TrimStart('0') + "'");
                                    PosRow.GebMatPflicht = "";
                                    if (MatRow.Length == 1)
                                    {
                                        if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                                        {
                                            PosRow.GebMatPflicht = "X";
                                        }
                                    }

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

                                }
                                ZLD_DataContext.SubmitChanges();
                            }
                            else if (tblPos.Count() == 0)
                            {
                                var tblPosNew = new ZLDPositionsTabelle
                                    {
                                        id_Kopf = KopfID,
                                        id_pos = (Int32) drow["id_pos"],
                                        UEPOS = (Int32) drow["uepos"],
                                        Matnr = drow["Matnr"].ToString(),
                                        Menge = drow["Menge"].ToString()
                                    };

                                if (ZLDCommon.IsNumeric(drow["Menge"].ToString()))
                                {
                                    Int32.TryParse(drow["Menge"].ToString(), out iMenge);
                                }

                                String strMatbz = drow["Matbez"].ToString().Length > 0 ? 
                                    CombineBezeichnungMenge(drow["Matbez"].ToString(), iMenge) : drow["Matbez"].ToString();
                                tblPosNew.Matbez = strMatbz;

                                tblPosNew.GebMatbez = drow["GebMatbez"].ToString();
                                tblPosNew.GebMatnr = drow["GebMatnr"].ToString();
                                tblPosNew.GebMatnrSt = drow["GebMatnrSt"].ToString();
                                tblPosNew.GebMatBezSt = drow["GebMatBezSt"].ToString();
                                tblPosNew.Kennzmat = drow["KennzMat"].ToString();
                                tblPosNew.SDRelevant = drow["SDRelevant"].ToString();
                                tblPosNew.PreisKZ = PreisKennz;
                                tblPosNew.PosLoesch = drow["PosLoesch"].ToString();
                               
                                tblPosNew.WebMTArt = drow["WebMTArt"].ToString();
                                DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + tblPosNew.Matnr.TrimStart('0') + "'");
                                tblPosNew.GebMatPflicht = "";
                                if (MatRow.Length == 1)
                                {
                                    if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                                    {
                                        tblPosNew.GebMatPflicht = "X";
                                    }

                                }

                                Decimal Preis;
                                if (ZLDCommon.IsDecimal(drow["Preis"].ToString()))
                                {
                                    tblPosNew.Preis = Decimal.TryParse(drow["Preis"].ToString(), out Preis) ? Preis : 0;
                                }
                                else
                                {
                                    tblPosNew.Preis = 0;
                                }

                                if (ZLDCommon.IsDecimal(drow["GebPreis"].ToString()))
                                {
                                    tblPosNew.GebPreis = Decimal.TryParse(drow["GebPreis"].ToString(), out Preis) ? Preis : 0;
                                }
                                else
                                {
                                    tblPosNew.GebPreis = 0;
                                }

                                if (ZLDCommon.IsDecimal(drow["Preis_Amt"].ToString()))
                                {
                                    tblPosNew.Preis_Amt = Decimal.TryParse(drow["Preis_Amt"].ToString(), out Preis) ? Preis : 0;
                                }
                                else
                                {
                                    tblPosNew.Preis_Amt = 0;
                                }

                                if (ZLDCommon.IsDecimal(drow["Preis_Amt_Add"].ToString()))
                                {
                                    tblPosNew.Preis_Amt_Add = Decimal.TryParse(drow["Preis_Amt_Add"].ToString(), out Preis) ? Preis : 0;
                                }
                                else
                                {
                                    tblPosNew.Preis_Amt_Add = 0;
                                }

                                if (ZLDCommon.IsDecimal(drow["UPREIS"].ToString()))
                                {
                                    tblPosNew.UPreis = drow["UPREIS"].ToString();
                                }
                                else { tblPosNew.UPreis = "0"; }

                                if (ZLDCommon.IsDecimal(drow["Differrenz"].ToString()))
                                {
                                    tblPosNew.Differrenz = drow["Differrenz"].ToString();
                                }
                                else { tblPosNew.Differrenz = "0"; }

                                tblPosNew.Konditionstab = drow["Konditionstab"].ToString();
                                tblPosNew.Konditionsart = drow["Konditionsart"].ToString();
                                if (ZLDCommon.IsDate(drow["CALCDAT"].ToString()))
                                {
                                    DateTime.TryParse(drow["CALCDAT"].ToString(), out tmpDate);
                                    tblPosNew.CalcDat = tmpDate;
                                }
                                tblPosNew.GebPak = drow["GebPak"].ToString();
                                ZLD_DataContext.ZLDPositionsTabelle.InsertOnSubmit(tblPosNew);
                                ZLD_DataContext.SubmitChanges();
                            }
                        }
                    }
                    else if (tblPosCount.Count() > Positionen.Rows.Count)
                    {
                        foreach (var PosRow in tblPosCount)
                        {
                            DataRow[] drow = Positionen.Select("id_pos = " + PosRow.id_pos);
                            if (drow.Length == 1)
                            {
                                Boolean diffMatnr = false;
                                PosRow.id_Kopf = KopfID;
                                PosRow.id_pos = (Int32)drow[0]["id_pos"];

                                PosRow.Menge = drow[0]["Menge"].ToString();
                                if (PosRow.Matnr != drow[0]["Matnr"].ToString())
                                {
                                    diffMatnr = true;
                                }
                                PosRow.Matnr = drow[0]["Matnr"].ToString();
                                
                                PosRow.Menge = drow[0]["Menge"].ToString();

                                if (ZLDCommon.IsNumeric(drow[0]["Menge"].ToString()))
                                {
                                    Int32.TryParse(drow[0]["Menge"].ToString(), out iMenge);
                                }

                                String strMatbz = drow[0]["Matbez"].ToString();
                                if (drow[0]["Matbez"].ToString().Length > 0)
                                {
                                    strMatbz = CombineBezeichnungMenge(drow[0]["Matbez"].ToString(), iMenge);
                                }
                                PosRow.Matbez = strMatbz;

                                PosRow.WebMTArt = drow[0]["WebMTArt"].ToString();
                                PosRow.GebMatbez = drow[0]["GebMatbez"].ToString();
                                PosRow.GebMatnr = drow[0]["GebMatnr"].ToString();
                                PosRow.GebMatnrSt = drow[0]["GebMatnrSt"].ToString();
                                PosRow.GebMatBezSt = drow[0]["GebMatBezSt"].ToString();
                                PosRow.Kennzmat = drow[0]["KennzMat"].ToString();
                                PosRow.SDRelevant = drow[0]["SDRelevant"].ToString();
                                PosRow.PosLoesch = drow[0]["PosLoesch"].ToString();
                                
                                Decimal Preis;
                                if (ZLDCommon.IsDecimal(drow[0]["Preis"].ToString()))
                                {
                                    PosRow.Preis = Decimal.TryParse(drow[0]["Preis"].ToString(), out Preis) ? Preis : 0;
                                }
                                else
                                {
                                    PosRow.Preis = 0;
                                }

                                if (ZLDCommon.IsDecimal(drow[0]["GebPreis"].ToString()))
                                {
                                    PosRow.GebPreis = Decimal.TryParse(drow[0]["GebPreis"].ToString(), out Preis) ? Preis : 0;
                                }
                                else
                                {
                                    PosRow.GebPreis = 0;
                                }

                                if (ZLDCommon.IsDecimal(drow[0]["Preis_Amt"].ToString()))
                                {
                                    PosRow.Preis_Amt = Decimal.TryParse(drow[0]["Preis_Amt"].ToString(), out Preis) ? Preis : 0;
                                }
                                else
                                {
                                    PosRow.Preis_Amt = 0;
                                }

                                if (ZLDCommon.IsDecimal(drow[0]["Preis_Amt_Add"].ToString()))
                                {
                                    PosRow.Preis_Amt_Add = Decimal.TryParse(drow[0]["Preis_Amt_Add"].ToString(), out Preis) ? Preis : 0;
                                }
                                else
                                {
                                    PosRow.Preis_Amt_Add = 0;
                                }

                                DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + PosRow.Matnr.TrimStart('0') + "'");
                                PosRow.GebMatPflicht = "";
                                if (MatRow.Length == 1)
                                {
                                    if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                                    {
                                        PosRow.GebMatPflicht = "X";
                                    }
                                }
                                if (diffMatnr)
                                {
                                    if (ZLDCommon.IsDecimal(drow[0]["UPREIS"].ToString()))
                                    {
                                        PosRow.UPreis = drow[0]["UPREIS"].ToString();
                                    }
                                    else { PosRow.UPreis = "0"; }
                                    if (ZLDCommon.IsDecimal(drow[0]["Differrenz"].ToString()))
                                    {
                                        PosRow.Differrenz = drow[0]["Differrenz"].ToString();
                                    }
                                    else { PosRow.Differrenz = "0"; }

                                    PosRow.Konditionstab = drow[0]["Konditionstab"].ToString();
                                    PosRow.Konditionsart = drow[0]["Konditionsart"].ToString();
                                    if (ZLDCommon.IsDate(drow[0]["CALCDAT"].ToString()))
                                    {
                                        DateTime.TryParse(drow[0]["CALCDAT"].ToString(), out tmpDate);
                                        PosRow.CalcDat = tmpDate;
                                    }
                                }
                                PosRow.GebPak = drow[0]["GebPak"].ToString();
                                ZLD_DataContext.SubmitChanges();
                            }
                            else
                            {
                                ZLD_DataContext.ZLDPositionsTabelle.DeleteOnSubmit(PosRow);
                                ZLD_DataContext.SubmitChanges();
                            }
                        }
                        ZLD_DataContext.Connection.Close();
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

                    if (Geldinstitut.Length > 40)
                    {
                        tblBank.Geldinstitut = Geldinstitut.Substring(0, 40);
                    }
                    else 
                    { 
                        tblBank.Geldinstitut = Geldinstitut; 
                    }

                    tblBank.EinzugErm = EinzugErm;
                    tblBank.Rechnung = Rechnung;

                    ZLD_DataContext.Connection.Open();
                    ZLD_DataContext.SubmitChanges();
                    ZLD_DataContext.Connection.Close();

                    ZLD_DataContext = new ZLDTableClassesDataContext();
                    var tblKunnadresse = (from k in ZLD_DataContext.ZLDKundenadresse
                                          where k.id_Kopf == KopfID
                                          select k).Single();

                    tblKunnadresse.Partnerrolle = Partnerrolle;
                    tblKunnadresse.Name1 = Name1;
                    tblKunnadresse.Name2 = Name2;
                    tblKunnadresse.Ort = Ort;
                    tblKunnadresse.PLZ = PLZ;
                    tblKunnadresse.Strasse = Strasse;
                    ZLD_DataContext.SubmitChanges();
                }
                else
                {
                    RaiseError("9999", "Fehler beim exportieren der Belegnummer!");
                }
            }
            catch (Exception ex)
            {
                RaiseError("9999",ex.Message);
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
            ClearError();

            var ds = new DataSet();
            FillDataSet(IDRecordset, ref ds);
            
            try
            {
                DataTable tmpKopf = ds.Tables[0];
                DataTable tmpPos = ds.Tables[1];
                DataTable tmpBank = ds.Tables[2];
                DataTable tmpKunde = ds.Tables[3];

                _kopfTabelle = tmpKopf;

                KopfID = (Int32)_kopfTabelle.Rows[0]["id"];
                SapID = (Int32)_kopfTabelle.Rows[0]["id_sap"];
                Abgerechnet = (Boolean)_kopfTabelle.Rows[0]["abgerechnet"];
                Kundenname = _kopfTabelle.Rows[0]["kundenname"].ToString();
                Kunnr = _kopfTabelle.Rows[0]["kundennr"].ToString();
                Ref1 = _kopfTabelle.Rows[0]["referenz1"].ToString();
                Ref2 = _kopfTabelle.Rows[0]["referenz2"].ToString();
                KreisKennz = _kopfTabelle.Rows[0]["KreisKZ"].ToString();
                Kreis = _kopfTabelle.Rows[0]["KreisBez"].ToString();
                WunschKennz = (Boolean)_kopfTabelle.Rows[0]["WunschKenn"];
                ZusatzKZ = ZLDCommon.XToBool(_kopfTabelle.Rows[0]["ZusatzKZ"].ToString());
                WunschKZ2 = _kopfTabelle.Rows[0]["WunschKZ2"].ToString();
                WunschKZ3 = _kopfTabelle.Rows[0]["WunschKZ3"].ToString();
                OhneGruenenVersSchein = ZLDCommon.XToBool(_kopfTabelle.Rows[0]["OhneGruenenVersSchein"].ToString());
                Reserviert = (Boolean)_kopfTabelle.Rows[0]["Reserviert"];
                ReserviertKennz = _kopfTabelle.Rows[0]["ReserviertKennz"].ToString();
                Feinstaub = (Boolean)_kopfTabelle.Rows[0]["Feinstaub"];
                
                ZulDate = _kopfTabelle.Rows[0]["Zulassungsdatum"].ToString();
                if (ZLDCommon.IsDate(ZulDate)) { ZulDate = ((DateTime)_kopfTabelle.Rows[0]["Zulassungsdatum"]).ToShortDateString(); }
                
                Kennzeichen = _kopfTabelle.Rows[0]["Kennzeichen"].ToString();
                Bemerkung = _kopfTabelle.Rows[0]["Bemerkung"].ToString();
                KennzForm = _kopfTabelle.Rows[0]["KennzForm"].ToString();
                EinKennz = (Boolean)_kopfTabelle.Rows[0]["EinKennz"];
                EC = (Boolean)_kopfTabelle.Rows[0]["EC"];
                Bar = (Boolean)_kopfTabelle.Rows[0]["Bar"];
                RE = (Boolean)_kopfTabelle.Rows[0]["RE"];
                PauschalKunde = _kopfTabelle.Rows[0]["PauschalKunde"].ToString();
                OhneSteuer = _kopfTabelle.Rows[0]["OhneSteuer"].ToString();
                saved = (Boolean)_kopfTabelle.Rows[0]["saved"];
                toDelete = _kopfTabelle.Rows[0]["toDelete"].ToString();

                bearbeitet = (Boolean)_kopfTabelle.Rows[0]["bearbeitet"];
                Vorgang = _kopfTabelle.Rows[0]["Vorgang"].ToString();
                Barcode = _kopfTabelle.Rows[0]["Barcode"].ToString();
                Barkunde = (Boolean)_kopfTabelle.Rows[0]["Barkunde"];
                
                Decimal Preis;
                if (ZLDCommon.IsDecimal(_kopfTabelle.Rows[0]["Steuer"].ToString()))
                {
                    Steuer = Decimal.TryParse(_kopfTabelle.Rows[0]["Steuer"].ToString(), out Preis) ? Preis : 0; 
                }

                //Positionen = new DataTable();
                Positionen = tmpPos;
                foreach (DataRow PosRow in Positionen.Rows)
                {
                    PosRow["Matnr"] = PosRow["Matnr"].ToString().TrimStart('0');
                    if (PosRow["id_pos"].ToString() == "10")
                    {
                        if (ZLDCommon.IsDecimal(PosRow["PreisKZ"].ToString()))
                        {
                            PreisKennz =Decimal.TryParse(PosRow["PreisKZ"].ToString(), out Preis) ? Preis : 0;
                        }
                    }
                }
                //_bankverbindung = new DataTable();
                _bankverbindung = tmpBank;

                SWIFT = _bankverbindung.Rows[0]["SWIFT"].ToString();
                IBAN = _bankverbindung.Rows[0]["IBAN"].ToString();
                BankKey = _bankverbindung.Rows[0]["BankKey"].ToString();
                Kontonr = _bankverbindung.Rows[0]["Kontonr"].ToString();
                Inhaber = _bankverbindung.Rows[0]["Inhaber"].ToString();
                Geldinstitut = _bankverbindung.Rows[0]["Geldinstitut"].ToString();
                EinzugErm = (Boolean)_bankverbindung.Rows[0]["EinzugErm"];
                Rechnung = (Boolean)_bankverbindung.Rows[0]["Rechnung"];

                //_kundenadresse = new DataTable();
                _kundenadresse = tmpKunde;
                KundennrWE = _kundenadresse.Rows[0]["Kundennr"].ToString();
                Name1 = _kundenadresse.Rows[0]["Name1"].ToString();
                Name2 = _kundenadresse.Rows[0]["Name2"].ToString();
                PLZ = _kundenadresse.Rows[0]["PLZ"].ToString();
                Ort = _kundenadresse.Rows[0]["Ort"].ToString();
                Strasse = _kundenadresse.Rows[0]["Strasse"].ToString();
            }
            catch (Exception ex)
            {
                RaiseError("9999",ex.Message);
            }
        }

        /// <summary>
        /// Laden eines Vorgange anhand der ID für die Eingabemasken.
        /// Speicherung der Records in einem Dataset.
        /// Aufgerufen von LoadDB_ZLDRecordset().
        /// </summary>
        /// <param name="RecordID">ID SQL</param>
        /// <param name="ds">Dataset mit den Tabellen ZLDKopfTabelle, ZLDPositionsTabelle, ZLDKundenadresse</param>
        private void FillDataSet(Int32 RecordID, ref DataSet ds)
        {
            var connection = new SqlConnection
                {
                    ConnectionString = ConfigurationManager.AppSettings["Connectionstring"]
                };

            connection.Open();
            String DBUserID = m_objUser.UserID.ToString();

            DBUserID = SelctedUserID ?? m_objUser.UserID.ToString();

            var adapter = new SqlDataAdapter(

                                         "SELECT * FROM ZLDKopfTabelle as KopfTabelle where id=" + RecordID + " AND id_user=" + DBUserID + " AND Vorgang='K';" +

                                        "SELECT * FROM ZLDPositionsTabelle As PositionsTabelle where id_Kopf=" + RecordID + "; " +

                                        "SELECT * FROM ZLDBankverbindung As Bankverbindung where id_Kopf=" + RecordID + ";  " +

                                        "SELECT * FROM ZLDKundenadresse as Kundenadresse where id_Kopf=" + RecordID + ";  ", connection);

            adapter.TableMappings.Add("ZLDKopfTabelle", "KopfTabelle");
            adapter.TableMappings.Add("ZLDPositionsTabelle", "PositionsTabelle");
            adapter.TableMappings.Add("ZLDBankverbindung", "Bankverbindung");
            adapter.TableMappings.Add("ZLDKundenadresse", "Kundenadresse");
            adapter.Fill(ds);
            
            //DataTable tmpKopf, tmpPos, tmpBank, tmpAdresse;
            //tmpKopf = ds.Tables[0];
            //tmpPos = ds.Tables[1];
            //tmpBank = ds.Tables[2];
            //tmpAdresse = ds.Tables[3];

            connection.Close();
        }

        /// <summary>
        /// Laden der in der SQL-Tabelle angelegten Vorgänge für die Übersicht(ChangeZLDKomListe.aspx)
        /// </summary>
        /// <param name="sVorgang">Vorgang</param>
        public void LadeKompletterfDB_ZLD(String sVorgang)
        {
            ClearError();

            var connection = new System.Data.SqlClient.SqlConnection
                {
                    ConnectionString = ConfigurationManager.AppSettings["Connectionstring"]
                };

            try
            {
                tblEingabeListe = new DataTable();

                var command = new System.Data.SqlClient.SqlCommand();
                var adapter = new System.Data.SqlClient.SqlDataAdapter();

                command.CommandText = "SELECT dbo.ZLDKopfTabelle.*, dbo.ZLDPositionsTabelle.Matnr, dbo.ZLDPositionsTabelle.Matbez, dbo.ZLDPositionsTabelle.id_pos," +
                                      " dbo.ZLDPositionsTabelle.Preis, dbo.ZLDPositionsTabelle.GebPreis, dbo.ZLDPositionsTabelle.Preis_Amt, dbo.ZLDPositionsTabelle.Preis_Amt_Add, dbo.ZLDPositionsTabelle.PreisKZ," +
                                      " dbo.ZLDPositionsTabelle.PosLoesch, dbo.ZLDPositionsTabelle.GebMatPflicht, dbo.ZLDPositionsTabelle.GebMatnr," +
                                      " dbo.ZLDPositionsTabelle.GebMatnrSt, dbo.ZLDPositionsTabelle.Menge, dbo.ZLDPositionsTabelle.GebPak " +
                                      " FROM dbo.ZLDKopfTabelle INNER JOIN" +
                                      " dbo.ZLDPositionsTabelle ON dbo.ZLDKopfTabelle.id = dbo.ZLDPositionsTabelle.id_Kopf" +
                                      " WHERE id_User = @id_User AND Vorgang = @Vorgang AND testuser = @testuser AND WebMTArt = 'D'  AND abgerechnet = 0 ";
                                     
                if (SelctedUserID != null)
                { command.Parameters.AddWithValue("@id_User", SelctedUserID); }
                else { command.Parameters.AddWithValue("@id_User", m_objUser.UserID); }
                
                command.Parameters.AddWithValue("@Vorgang", sVorgang);
                command.Parameters.AddWithValue("@testuser", m_objUser.IsTestUser);
                
                command.CommandText += " ORDER BY  kundenname asc,dbo.ZLDKopfTabelle.id_sap, dbo.ZLDPositionsTabelle.id_pos, referenz1 asc, Kennzeichen";
                
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
                    RaiseError("9999","Keine Daten gefunden!");
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
        /// Übergeben der Daten an SAP aus den SQL-Tabellen. Bapi: Z_ZLD_IMP_KOMPER
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ChangeZLDKomListe.aspx</param>
        /// <param name="tblKundenStamm">Kundenstammtabelle</param>
        /// <param name="tblStvaStamm">Stammtabelle StVa</param>
        /// <param name="tblMaterialStamm">Dienstleistungstabelle</param>
        public void SaveZLDKompletterfassung(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblKundenStamm, 
                                                DataTable tblStvaStamm, DataTable tblMaterialStamm)
        {
            m_strClassAndMethod = "KomplettZLD.SaveZLDKompletterfassung";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
           
            ClearError();

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                var ZLD_DataContext = new ZLDTableClassesDataContext();
                
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_IMP_KOMPER", ref m_objApp, ref m_objUser, ref page);
                    

                    DataTable importAuftrag = myProxy.getImportTable("GT_IMP_BAK");
                    
                    // Z = Zwischenlösung für Konvertierungsfehler von BCD-Werten im DynProxy

                    // Z - siehe oben ++++++++
                    DataTable importPos = myProxy.getImportTable("GT_IMP_POS_S01");
                    // +++++++++++++++++++++++

                    DataTable importBank = myProxy.getImportTable("GT_IMP_BANK");
                    DataTable importAdresse = myProxy.getImportTable("GT_IMP_ADRS");

                    var LastID = 0;
                    
                    // nur die Auträge die auch im Grid angezeigt werden
                    foreach (DataRow SaveRow in tblEingabeListe.Select(tblEingabeListe.DefaultView.RowFilter))
                    {
                        var tmpID = (Int32)SaveRow["id"];
                        if (LastID != tmpID)
                        {
                            var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
                                            where k.id == tmpID
                                            select k).Single();
                            //String isCPD = "";
                            DataRow importRowAuftrag = importAuftrag.NewRow();
                            importRowAuftrag["MANDT"] = "010";
                            importRowAuftrag["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                            importRowAuftrag["VBELN"] = "";
                            importRowAuftrag["VKORG"] = VKORG;
                            importRowAuftrag["VKBUR"] = VKBUR;
                            importRowAuftrag["ERNAM"] = tblKopf.username.PadLeft(12);
                            importRowAuftrag["ERDAT"] = DateTime.Now;
                            importRowAuftrag["FLAG"] = "";
                            importRowAuftrag["BARCODE"] = tblKopf.Barcode;
                            importRowAuftrag["KUNNR"] = tblKopf.kundennr.PadLeft(10, '0');
                            importRowAuftrag["ZZREFNR1"] = tblKopf.referenz1;
                            importRowAuftrag["ZZREFNR2"] = tblKopf.referenz2;
                            
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
                            
                            importRowAuftrag["WUNSCHKENN_JN"] = ZLDCommon.BoolToX((Boolean)tblKopf.WunschKenn);
                            importRowAuftrag["ZUSKENNZ"] = tblKopf.ZusatzKZ;
                            importRowAuftrag["WU_KENNZ2"] = tblKopf.WunschKZ2;
                            importRowAuftrag["WU_KENNZ3"] = tblKopf.WunschKZ3;
                            importRowAuftrag["O_G_VERSSCHEIN"] = tblKopf.OhneGruenenVersSchein;

                            importRowAuftrag["RESERVKENN_JN"] = ZLDCommon.BoolToX((Boolean)tblKopf.Reserviert);
                            importRowAuftrag["RESERVKENN"] = tblKopf.ReserviertKennz;
                            importRowAuftrag["FEINSTAUBAMT"] = ZLDCommon.BoolToX((Boolean)tblKopf.Feinstaub);
                            importRowAuftrag["ZZZLDAT"] = tblKopf.Zulassungsdatum;
                            importRowAuftrag["ZZKENN"] = tblKopf.Kennzeichen;

                            importRowAuftrag["KENNZTYP"] = "";
                            importRowAuftrag["KENNZFORM"] = tblKopf.KennzForm;
                            importRowAuftrag["KENNZANZ"] = "0";
                            importRowAuftrag["EINKENN_JN"] = ZLDCommon.BoolToX((Boolean)tblKopf.EinKennz);
                            importRowAuftrag["BEMERKUNG"] = tblKopf.Bemerkung;
                            importRowAuftrag["EC_JN"] = ZLDCommon.BoolToX((Boolean)tblKopf.EC);
                            importRowAuftrag["BAR_JN"] = ZLDCommon.BoolToX((Boolean)tblKopf.Bar);
                            importRowAuftrag["RE_JN"] = ZLDCommon.BoolToX((Boolean)tblKopf.RE);
                            importRowAuftrag["KUNDEBAR_JN"] = ZLDCommon.BoolToX((Boolean)tblKopf.Barkunde);
                            //DataRow[] KundeRow = tblKundenStamm.Select("KUNNR='" + tblKopf.kundennr + "'");

                            //if (KundeRow.Length == 1)
                            //{
                            //    isCPD = KundeRow[0]["XCPDK"].ToString();
                            //}
                            importRowAuftrag["LOEKZ"] = tblKopf.toDelete;

                            importRowAuftrag["VH_KENNZ_RES"] = ZLDCommon.BoolToX((Boolean)tblKopf.VorhKennzReserv);
                            importRowAuftrag["ZBII_ALT_NEU"] = ZLDCommon.BoolToX((Boolean)tblKopf.ZBII_ALT_NEU);
                            importRowAuftrag["KENNZ_VH"] = ZLDCommon.BoolToX((Boolean)tblKopf.KennzVH);
                            importRowAuftrag["VK_KUERZEL"] = tblKopf.VKKurz;
                            importRowAuftrag["KUNDEN_REF"] = tblKopf.interneRef;
                            importRowAuftrag["KUNDEN_NOTIZ"] = tblKopf.KundenNotiz;
                            importRowAuftrag["ALT_KENNZ"] = tblKopf.KennzAlt;
                            importRowAuftrag["VE_ERNAM"] = tblKopf.Vorerfasser;
                            importRowAuftrag["VE_ERDAT"] = DateTime.Now;
                            if (ZLDCommon.IsDate(tblKopf.Still_Datum.ToString()))
                            {
                                importRowAuftrag["STILL_DAT"] = tblKopf.Still_Datum;
                            }

                            DataRow importRow;

                            //----------------

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
                                // Z - siehe oben ++++++++
                                importRow["MENGE_C"] = PosRow.Menge;
                                // 
                                importRow["WEBMTART"] = PosRow.WebMTArt;

                                DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + PosRow.Matnr.TrimStart('0') + "'");
                                if (MatRow.Length == 1)
                                {
                                    if (MatRow[0]["KENNZREL"].ToString() == "X")
                                    {
                                        if (tblKopf.EinKennz == true)
                                        {
                                            importRowAuftrag["KENNZANZ"] = "1"; ;
                                        }
                                        else
                                        {
                                            importRowAuftrag["KENNZANZ"] = "2";
                                        }
                                    }
                                    importRow["NULLPREIS_OK"] = MatRow[0]["NULLPREIS_OK"].ToString();
                                }

                                importRow["MATNR"] = PosRow.Matnr.PadLeft(18, '0');
                                importRow["MAKTX"] = PosRow.Matbez;
                                importRow["LOEKZ"] = "";
                                importRow["GBPAK"] = "";
                                importRow["GEB_AMT_ADD_C"] = PosRow.Preis_Amt_Add.ToString();
                                if (PosRow.PosLoesch == "L")
                                { importRow["LOEKZ"] = "X"; }
                                // Z - siehe oben ++++++++
                                importRow["PREIS_C"] = PosRow.Preis.ToString();

                                importRow["WEBMTART"] = PosRow.WebMTArt;

                                // Wenn Gebühr Amt nicht angezeigt wird(.Authorizationright == 1), 
                                // dann Preis der norm. Gebühr übernehmen
                                if (PosRow.WebMTArt == "G")
                                {
                                    importRow["GEB_AMT_C"] = PosRow.Preis_Amt.ToString();
                                    importRow["GBPAK"] = PosRow.GebPak;
                                    if (m_objUser.Groups[0].Authorizationright == 1) { importRow["GEB_AMT_C"] = PosRow.Preis.ToString(); }
                                }
                                if (PosRow.UEPOS != 10)
                                {
                                    if (PosRow.WebMTArt == "S")
                                    {
                                        importRow["LOEKZ"] = "X";
                                    }
                                }
                                // bei Steuer wo kein Preis hinterlegt Löschkz. setzen
                                else if (PosRow.WebMTArt == "S" && PosRow.Preis == 0)
                                {
                                    importRow["LOEKZ"] = "X";
                                }
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
                                importPos.Rows.Add(importRow);

                            }

                            importAuftrag.Rows.Add(importRowAuftrag);

                            var tblBank = (from b in ZLD_DataContext.ZLDBankverbindung
                                            where b.id_Kopf == tmpID
                                            select b).Single();

                            // MJE, 2014-01-21:
                            // changed from "Length > 0" to "string.IsNullOrEmpty()"
                            if (!string.IsNullOrEmpty(tblBank.IBAN))
                            {
                                importRow = importBank.NewRow();
                                importRow["MANDT"] = "010";
                                importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
                                importRow["SWIFT"] = tblBank.SWIFT;
                                importRow["IBAN"] = tblBank.IBAN;
                                importRow["BANKL"] = tblBank.BankKey;
                                importRow["BANKN"] = tblBank.Kontonr;
                                importRow["EBPP_ACCNAME"] = tblBank.Geldinstitut;
                                importRow["KOINH"] = tblBank.Inhaber;
                                importRow["EINZ_JN"] = ZLDCommon.BoolToX((Boolean)tblBank.EinzugErm);
                                importRow["RECH_JN"] = ZLDCommon.BoolToX((Boolean)tblBank.Rechnung);
                                importBank.Rows.Add(importRow);
                            }
                                    
                            var tblKunnadresse = (from k in ZLD_DataContext.ZLDKundenadresse
                                                    where k.id_Kopf == tmpID
                                                    select k).Single();

                            // MJE, 2014-01-21:
                            // changed from "Length > 0" to "string.IsNullOrEmpty()"
                            if (!string.IsNullOrEmpty(tblKunnadresse.Name1))
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
                                importAdresse.Rows.Add(importRow);
                            }
                                    
                            LastID = tmpID;
                        }
                    }
                    if (importAuftrag.Rows.Count > 0) 
                    { 
                        myProxy.callBapi();

                        tblFehler = myProxy.getExportTable("GT_EX_ERRORS");

                        // sind in den Auträgen Barkunden dabei kommen aus SAP Pfade 
                        // zu den Barquittungen in diese Tabelle
                        tblBarquittungen = myProxy.getExportTable("GT_BARQ");

                        // Fehler 
                        if (tblFehler.Rows.Count > 0)
                        {
                            RaiseError("-9999","");
                            foreach (DataRow rowError in tblFehler.Rows)
                            {
                                Int32 id_sap;
                                Int32.TryParse(rowError["ZULBELN"].ToString(), out id_sap);
                                DataRow[] rowListe = tblEingabeListe.Select("id_sap=" + id_sap);
                                if (rowListe.Length > 0)
                                {
                                    rowListe[0]["Status"] = rowError["ERROR_TEXT"];
                                }
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
        /// Läd das SDRelevant-Flag eines Gebührenmat. aus der Datenbank
        /// ist der Kunde ein Pauschalkunde,  Gebühr und Gebühr Amt unterschiedlich und 
        /// das Gebührenmaterial nicht SD relevant darf der Vorgang nicht abgesendet werden
        /// Funktion Checkgrid (ChangeZLDKomListe.aspx)
        /// </summary>
        /// <param name="IDRecordset">ID SQL</param>
        /// <param name="idPos">ID der Position</param>
        /// <param name="sMatnr">Materialnummer</param>
        /// <returns>SDRelevant</returns>
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
        /// Benutzer der gleichen Filiale laden, die nicht angemeldet sind
        /// </summary>
        public void LadeBenutzer()
        {
            ClearError();

            var connection = new System.Data.SqlClient.SqlConnection
                {
                    ConnectionString = ConfigurationManager.AppSettings["Connectionstring"]
                };
            
            try
            {
                tblUser = new DataTable();

                var command = new System.Data.SqlClient.SqlCommand();
                var adapter = new System.Data.SqlClient.SqlDataAdapter();

                command.CommandText = "SELECT UserID, Username FROM dbo.WebUser  " +
                                      "WHERE Reference = @Reference AND LoggedOn = 0 AND NOT Username = @Username " +
                                      " ORDER BY dbo.WebUser.Username";

                command.Parameters.AddWithValue("@Reference", m_objUser.Reference);
                command.Parameters.AddWithValue("@Username", m_objUser.UserName);

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                adapter.SelectCommand = command; 
                adapter.Fill(tblUser);
            }
            catch (Exception ex)
            {
                RaiseError("9999", "Fehler beim Laden der Benutzerliste: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Vor dem ziehen der Daten von anderen Benutzern , prüfen 
        /// ob diese angemeldet sind
        /// </summary>
        public string CheckBenutzerOnline()
        {
            ClearError();
            
            var connection = new System.Data.SqlClient.SqlConnection
                {
                    ConnectionString = ConfigurationManager.AppSettings["Connectionstring"]
                };
            var abgemeldet = "";
            
            try
            {
                tblUser = new DataTable();

                var command = new System.Data.SqlClient.SqlCommand
                    {
                        CommandText = "SELECT LoggedOn FROM dbo.WebUser  " +
                                      "WHERE UserID = @UserID"
                    };

                command.Parameters.AddWithValue("@UserID", SelctedUserID);

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                abgemeldet= command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                RaiseError("9999","Fehler beim überprüfen des Benutzers: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return abgemeldet;
        }

        /// <summary>
        /// Löscht den Fehlerstatus der Klasse inklusive Fehlertabelle
        /// </summary>
        protected override void ClearError()
        {
            base.ClearError();
            tblFehler = null;
        }
    }
}
  