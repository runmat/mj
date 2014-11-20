using System;
using CKG.Base.Common;
using System.Data;
using CKG.Base.Business;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Klasse für verschiedene Reports (Tagesliste, Prägeliste, Lieferscheine, Auswertung)
    /// </summary>
    public class Listen: BankBase
    {
        #region "Declarations"
        String strKennzeichenVon;
        String strKennzeichenBis;
        String strKundeVon;
        String strKundeBis;
        String strZuldat;
        String strDelta;
        String strGesamt;
        DataTable tblListe;
        DataTable tblKopfListe;
        DataTable tblBemListe;
        #endregion

        #region "Properties"

        /// <summary>
        /// Selektionsparameter Kennzeichen von
        /// </summary>
        public String KennzeichenVon
        {
            get { return strKennzeichenVon; }
            set { strKennzeichenVon = value; }
        }
        /// <summary>
        /// Selektionsparameter Kennzeichen bis
        /// </summary>
        public String KennzeichenBis
        {
            get { return strKennzeichenBis; }
            set { strKennzeichenBis = value; }
        }

        /// <summary>
        /// Selektionsparameter Kundennummer von
        /// </summary>
        public String KundeVon
        {
            get { return strKundeVon; }
            set { strKundeVon = value; }
        }
        /// <summary>
        /// Selektionsparameter Kundennummer bis
        /// </summary>
        public String KundeBis
        {
            get { return strKundeBis; }
            set { strKundeBis = value; }
        }
        /// <summary>
        /// Selektionsparameter Zulassungsdatum
        /// </summary>
        public String Zuldat
        {
            get { return strZuldat; }
            set { strZuldat = value; }
        }
        /// <summary>
        /// Selektionsparameter Deltaliste
        /// </summary>
        public String Delta
        {
            get { return strDelta; }
            set { strDelta = value; }
        }
        /// <summary>
        /// Selektionsparameter Gesamtliste
        /// </summary>
        public String Gesamt
        {
            get { return strGesamt; }
            set { strGesamt = value; }
        }
        /// <summary>
        /// Rückgabetabelle Tagesliste
        /// </summary>
        public DataTable TagesListe
        {
            get { return tblListe; }
            set { tblListe = value; }
        }
        /// <summary>
        /// Rückgabetablle Tagesliste Kopf
        /// </summary>
        public DataTable KopfListe
        {
            get { return tblKopfListe; }
            set { tblKopfListe = value; }
        }
        /// <summary>
        /// Rückgabetabelle Bemerkungen
        /// </summary>
        public DataTable BemListe
        {
            get { return tblBemListe; }
            set { tblBemListe = value; }
        }
        /// <summary>
        /// Rückgabetabelle Prägeliste Kopftabelle
        /// </summary>
        public DataTable PraegListeKopf 
        { 
            get; 
            set; 
        }
        /// <summary>
        /// Rückgabetabelle Prägeliste Positionen
        /// </summary>
        public DataTable PraegListePos
        {
            get;
            set;
        }
        /// <summary>
        /// Rückgabetabelle Gesamtliste
        /// </summary>
        public DataTable GesamtListe
        {
            get;
            set;
        }
        /// <summary>
        /// Rückgabetabelle Lieferscheine
        /// </summary>
        public DataTable Lieferscheine
        {
            get;
            set;
        }
        public byte[] pdfTagesliste 
        { 
            get; 
            set; 
        }
        public byte[] pdfPraegeliste
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
        /// Selektionparameter Materialnummer für die Auswertung
        /// </summary>
        public String SelMatnr
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionparameter Zulassungsdatum von für die Auswertung
        /// </summary>
        public String SelDatum
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionparameter Zulassungsdatum bis für die Auswertung
        /// </summary>
        public String SelDatumBis
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionparameter SAPID für die Auswertung
        /// </summary>
        public String SelID
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionparameter Kundenummer für die Auswertung
        /// </summary>
        public String SelKunde
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionparameter Kennzeichen für die Auswertung
        /// </summary>
        public String SelKennz
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionparameter Referenz1 für die Auswertung
        /// </summary>
        public String SelRef1
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionparameter Kreiskennzeichen für die Auswertung
        /// </summary>
        public String SelStvA
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionparameter Zahlungsart für die Auswertung
        /// </summary>
        public String SelZahlart
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionparameter GroupTourID für die Auswertung
        /// </summary>
        public String SelGroupTourID
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionparameter "Alle Datensätze" für die Auswertung
        /// </summary>
        public String alleDaten
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionparameter "Abgerechnete Zulassungen" für die Auswertung
        /// </summary>
        public String Abgerechnet
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionparameter "Nicht durchgeführte Zulassungen" für die Auswertung
        /// </summary>
        public String NochNichtDurchgefuehrt
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionparameter "Noch nicht abgerechnete Zulassungen " für die Auswertung
        /// </summary>
        public String nichtAbgerechnet
        {
            get;
            set;
        }
        /// <summary>
        /// Rückgabewer Filialname Tagesliste
        /// </summary>
        public String Filialname
        {
            get;
            set;
        }
        /// <summary>
        /// Rückgabetabelle für die Auswertung
        /// </summary>
        public DataTable Auswertung
        {
            get;
            set;
        }
        /// <summary>
        /// Adressdaten der Filiale Tagesliste
        /// </summary>
        public DataTable ZLDAdresseTagli
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionsparameter Zulassungsdatum bis für Lieferscheine
        /// </summary>
        public String ZuldatBis
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionsparameter Sortierung nach für die Prägeliste
        /// </summary>
        public String Sortierung
        {
            get;
            set;
        }
        /// <summary>
        /// Filename aus SAP für die Tagesliste
        /// </summary>
        public String Filename
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="objUser">User-objekt</param>
        /// <param name="objApp">Applikationsobjekt</param>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="strFilename"></param>
        public Listen(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, String strAppID, String strSessionID, string strFilename)
            : base(ref objUser,ref objApp, strAppID,  strSessionID, strFilename )
        {}

        /// <summary>
        /// Overrides Change
        /// </summary>
        public override void Change()
        {
        }

        /// <summary>
        /// Overrides Show
        /// </summary>
        public override void Show()
        {
        }

        /// <summary>
        /// Selektion und Druck von Tageslisten. Tageslisten werden von SAP in einem Verzeichnis abgelegt.
        /// Bapi: Z_ZLD_EXPORT_TAGLI
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ReportTagList.aspx</param>
        public void Fill(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "Listen.Fill";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            DataTable tblKopf = new DataTable();

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_TAGLI", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKBUR", VKBUR);

                    if (strKundeVon.Length > 0) { myProxy.setImportParameter("I_KUNNR_VON", strKundeVon.PadLeft(10, '0')); }
                   
                    if (strKundeBis.Length > 0) { myProxy.setImportParameter("I_KUNNR_BIS", strKundeBis.PadLeft(10, '0')); }

                    myProxy.setImportParameter("I_KREISKZ_VON", strKennzeichenVon);

                    if (strKennzeichenBis.Length > 0)
                    { myProxy.setImportParameter("I_KREISKZ_BIS", strKennzeichenBis); }
                    else { myProxy.setImportParameter("I_KREISKZ_BIS", strKennzeichenVon); }

                    myProxy.setImportParameter("I_ZZZLDAT", strZuldat);

                    if (strDelta.Length > 0) { myProxy.setImportParameter("I_ZDELTA", strDelta); }
                    else { myProxy.setImportParameter("I_ZDELTA", null); }

                    if (strGesamt.Length > 0) { myProxy.setImportParameter("I_ZGESAMT", strGesamt); }
                    else { myProxy.setImportParameter("I_ZGESAMT", null); }

                    myProxy.setImportParameter("I_AUSGABE", "S"); //XSTRING-PDF-Ausgabe

                    myProxy.callBapi();

                    Filename = myProxy.getExportParameter("E_FILENAME");

                    Filialname = myProxy.getExportParameter("E_KTEXT");
                    tblKopfListe = myProxy.getExportTable("GT_TAGLI_K");
                    tblListe = myProxy.getExportTable("GT_TAGLI_P");
                    tblBemListe = myProxy.getExportTable("GT_TAGLI_BEM");
                    ZLDAdresseTagli = myProxy.getExportTable("ES_FIL_ADRS");
                    pdfTagesliste = myProxy.getExportParameterByte("E_PDF");

                        WriteLogEntry(true, "Kunde von=" + strKundeVon + ", Kunde bis=" + strKundeVon +
                         ", Kennzeichen von=" + strKennzeichenVon + ", Kennzeichen bis=" + strKennzeichenBis +
                         ", Zul.Datum=" + strZuldat + ", Deltaliste=" + strDelta + ", Gesamtliste=" + strGesamt, ref tblKopf);
                    
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            m_intStatus = -5555;
                            m_strMessage = "Keine Daten gefunden.";
                            WriteLogEntry(false, "Kunde von=" + strKundeVon + ", Kunde bis=" + strKundeVon +
                            ", Kennzeichen von=" + strKennzeichenVon + ", Kennzeichen bis=" + strKennzeichenBis +
                            ", Zul.Datum=" + strZuldat + ", Deltaliste=" + strDelta + ", Gesamtliste=" + strGesamt + ", " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message), ref tblKopf);
                            break;
                        default:
                            m_intStatus = -9999;
                            m_strMessage = m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            WriteLogEntry(false, "Kunde von=" + strKundeVon + ", Kunde bis=" + strKundeVon +
                            ", Kennzeichen von=" + strKennzeichenVon + ", Kennzeichen bis=" + strKennzeichenBis +
                            ", Zul.Datum=" + strZuldat + ", Deltaliste=" + strDelta + ", Gesamtliste=" + strGesamt + ", " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message), ref tblKopf);
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Selektion und Druck von Praegelisten. Praegelisten werden von SAP in einem Verzeichnis abgelegt.
        /// Bapi: Z_ZLD_EXPORT_PRALI
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ReportPraegeliste.aspx</param>
        public void FillPraegeliste(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "Listen.FillPraegeliste";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            DataTable tblPraeg = new DataTable();


            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_PRALI", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKBUR", VKBUR);
                    myProxy.setImportParameter("I_KREISKZ_VON", strKennzeichenVon);

                    if (strKennzeichenBis.Length > 0) 
                    { myProxy.setImportParameter("I_KREISKZ_BIS", strKennzeichenBis); }
                    else { myProxy.setImportParameter("I_KREISKZ_BIS", strKennzeichenVon); }

                    myProxy.setImportParameter("I_ZZZLDAT", strZuldat);

                    if (strDelta.Length > 0) { myProxy.setImportParameter("I_ZDELTA", strDelta); }
                    else { myProxy.setImportParameter("I_ZDELTA", null); }

                    if (strGesamt.Length > 0) { myProxy.setImportParameter("I_ZGESAMT", strGesamt); }
                    else { myProxy.setImportParameter("I_ZGESAMT", null); }

                    myProxy.setImportParameter("I_SORT", Sortierung);
                    myProxy.setImportParameter("I_AUSGABE", "S"); //XSTRING-PDF-Ausgabe

                    myProxy.callBapi();

                    Filename = myProxy.getExportParameter("E_FILENAME");
                    PraegListeKopf = myProxy.getExportTable("GT_BELEG");
                    pdfPraegeliste = myProxy.getExportParameterByte("E_PDF");

                    WriteLogEntry(true, " Kennzeichen von=" + strKennzeichenVon + ", Kennzeichen bis=" + strKennzeichenBis +
                                        ", Zul.Datum=" + strZuldat , ref tblPraeg);
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            m_intStatus = -5555;
                            m_strMessage = "Keine Daten gefunden.";
                            WriteLogEntry(false, " Kennzeichen von=" + strKennzeichenVon + ", Kennzeichen bis=" + strKennzeichenBis +
                            ", Zul.Datum=" + strZuldat + ", " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message), ref tblPraeg);
                            break;
                        default:
                            m_intStatus = -9999;
                            m_strMessage = m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            WriteLogEntry(false, " Kennzeichen von=" + strKennzeichenVon + ", Kennzeichen bis=" + strKennzeichenBis +
                            ", Zul.Datum=" + strZuldat + ", " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message), ref tblPraeg);
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// In der SQL-Tabelle (ZLDKopfTabelle) Flag "Prägeliste gedruckt" setzen.
        /// </summary>
        public void setPraliPrint()
        {
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection();
            try
            {

                connection.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["Connectionstring"];
                connection.Open();
                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "alter table dbo.ZLDKopfTabelle " +
                                      "disable trigger WriteTS";
                command.ExecuteNonQuery();


                DateTime tmpDate;

                DateTime.TryParse(strZuldat, out tmpDate);

                foreach (DataRow drow in PraegListeKopf.Rows) 
                {

                    String query = "id_sap = " + drow["ZULBELN"].ToString().TrimStart('0');

                    String str = "Update ZLDKopfTabelle " +
                     "Set Prali_Print= 1 " +
                     " Where " + query;

                    command = new System.Data.SqlClient.SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = str;
                    command.ExecuteNonQuery();
                
                }

                command = new System.Data.SqlClient.SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "alter table dbo.ZLDKopfTabelle " +
                                      "enable trigger WriteTS";
                command.ExecuteNonQuery();
            }
            finally { connection.Close(); }

        }

        /// <summary>
        /// Selektion und Druck von Lieferscheine. Lieferscheine werden von SAP in einem Verzeichnis abgelegt.
        /// Bapi: Z_ZLD_EXPORT_LS
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ReportLieferschein.aspx</param>
        /// <param name="Liefart">Formatierung PDF</param>
        public void FillLieferschein(String strAppID, String strSessionID, System.Web.UI.Page page, String Liefart)
        {

            m_strClassAndMethod = "Listen.FillLieferschein";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_LS", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKBUR", VKBUR);

                    myProxy.setImportParameter("I_KUNNR_VON", strKundeVon.PadLeft(10, '0'));
                    if (strKundeBis.Length > 0) { myProxy.setImportParameter("I_KUNNR_BIS", strKundeBis.PadLeft(10, '0')); }
                    if (strKennzeichenVon.Length > 0)
                    { myProxy.setImportParameter("I_KREISKZ_VON", strKennzeichenVon); }

                    if (strKennzeichenBis.Length > 0)
                    { myProxy.setImportParameter("I_KREISKZ_BIS", strKennzeichenBis); }


                    myProxy.setImportParameter("I_ZZZLDAT_VON", strZuldat);
                    if (ZuldatBis.Length > 0)
                    {
                        myProxy.setImportParameter("I_ZZZLDAT_BIS", ZuldatBis);
                    }
                    if (SelGroupTourID.Length > 0) 
                    {
                        myProxy.setImportParameter("I_GRUPPE", SelGroupTourID.PadLeft(10, '0'));    
                    }
                    myProxy.setImportParameter("I_LS", Liefart);

                    myProxy.callBapi();
                    Lieferscheine = new DataTable();
                    Lieferscheine = myProxy.getExportTable("GT_FILENAME");

                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            m_intStatus = -5555;
                            m_strMessage = "Keine Daten gefunden.";
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
        /// Selektion und Anzeige von erfassten Dienstleistungen. Bapi: Z_ZLD_EXPORT_AUSWERTUNG
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ReportAuswertung.aspx</param>
        /// <param name="tblKunde">Kundenstammtabelle</param>
        public void FillAuswertungNeu(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblKunde)
        {

            m_strClassAndMethod = "Listen.FillAuswertungNeu";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
                m_blnGestartet = true;
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_AUSWERTUNG_1", ref m_objApp, ref m_objUser, ref page);
                if (SelKunde.Length > 0) { SelKunde = SelKunde.PadLeft(10, '0'); } //SelMatnr
                myProxy.setImportParameter("I_KUNNR", SelKunde);
                myProxy.setImportParameter("I_VKORG", VKORG);
                myProxy.setImportParameter("I_VKBUR", VKBUR);
                myProxy.setImportParameter("I_ZZZLDAT_VON", SelDatum);
                myProxy.setImportParameter("I_ZZZLDAT_BIS", SelDatumBis);
                myProxy.setImportParameter("I_KREISKZ", SelStvA);
                if (SelID.Length > 0) { SelID = SelID.PadLeft(10, '0'); }
                myProxy.setImportParameter("I_ZULBELN", SelID);
                myProxy.setImportParameter("I_ZZREFNR1", SelRef1);
                myProxy.setImportParameter("I_ZZKENN", SelKennz);
                if (SelMatnr.Length > 0) { SelMatnr = SelMatnr.PadLeft(18, '0'); }
                myProxy.setImportParameter("I_MATNR", SelMatnr);
                myProxy.setImportParameter("I_ABRKZ", Abgerechnet);
                myProxy.setImportParameter("I_NNDGF", NochNichtDurchgefuehrt);
                myProxy.setImportParameter("I_LOEKZ", "");
                myProxy.setImportParameter("I_ZAHLART", SelZahlart);
                if (!String.IsNullOrEmpty(SelGroupTourID))
                {
                    myProxy.setImportParameter("I_GRUPPE", SelGroupTourID.PadLeft(10, '0'));
                }
                
                myProxy.callBapi();

                Auswertung = myProxy.getExportTable("GT_LISTE1");

                foreach (DataRow dRow in Auswertung.Rows)
                {
                    // Führende Nullen für die Anzeige entfernen
                    if (dRow["ZULBELN"] != null)
                    {
                        dRow["ZULBELN"] = dRow["ZULBELN"].ToString().TrimStart('0');
                    }
                    if (dRow["KUNNR"] != null)
                    {
                        dRow["KUNNR"] = dRow["KUNNR"].ToString().TrimStart('0');
                    }
                }
            }

            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_intStatus = -5555;
                        m_strMessage = "Keine Daten gefunden.";
                        break;
                    default:
                        m_intStatus = -9999;
                        m_strMessage = m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }
            }
            finally { m_blnGestartet = false; }
        }

        /// <summary>
        /// Nachdruck der Aufträge. Aufträge werden von SAP in einem Verzeichnis abgelegt. 
        /// Barcode und EasyID kommen aus den Auswertungstabellen. Bapi: Z_ZLD_GET_BARQ_FROM_EASY
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page"></param>
        /// <param name="BarqNr">Barcode</param>
        /// <param name="EasyID">EasyID</param>
        public void GetBarqFromEasy(String strAppID, String strSessionID, System.Web.UI.Page page, String BarqNr, String EasyID)
        {
            m_strClassAndMethod = "Listen.GetBarqFromEasy";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_GET_BARQ_FROM_EASY", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_BARQ_NR", BarqNr);

                    myProxy.setImportParameter("I_OBJECT_ID", EasyID);

                    myProxy.callBapi();

                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC"), out subrc);
                    m_intStatus = subrc;
                    String sapMessage = myProxy.getExportParameter("E_MESSAGE");
                    m_strMessage = sapMessage;
                    Filename = myProxy.getExportParameter("E_FILENAME");

                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            m_intStatus = -5555;
                            m_strMessage = "Keine Daten gefunden.";
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
        /// Gesamtliste aufbauen.
        /// </summary>
        /// <returns>Gesamtliste</returns>
        public DataTable CreateGesamtListe()
        {
            DataTable tblTemp = new DataTable();
            tblTemp.Columns.Add("KennzAnz", typeof(String));
            tblTemp.Columns.Add("Geb", typeof(String));

            return tblTemp;
        }
    }
}
