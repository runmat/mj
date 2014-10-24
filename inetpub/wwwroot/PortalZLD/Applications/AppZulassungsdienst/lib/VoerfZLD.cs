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
    /// Klasse für die Vorerfassung und Preisanlage.
    /// </summary>
	public class VoerfZLD : DatenimportBase
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
		Boolean abgerechnet;
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
		String mBarcode;
		Boolean mEC;
		Boolean mBar;
		Boolean msaved;
		Int16 mtoSave;
		String mtoDelete;
		Boolean mbearbeitet;

		// Positionstabelle

		DataTable tblPositionen;

		// Adresstabelle
		String mKundennrWE;
        String mPartnerrolle;
		String mName1;
		String mName2;
		String mPLZ;
		String mOrt;
		String mStrasse;

		// Bankdatentabelle
		String mSWIFT;
		String mIBAN;
		String mGeldinstitut;
		String mInhaber;
		Boolean mEinzugErm;
		Boolean mRechnung;
        String mBankKey;
        String mKontonr;

		#endregion

		#region "Properties"

		public DataTable tblEingabeListe
		{
			get { return tblListe; }
			set { tblListe = value; }
		}

		public DataTable tblFehler
		{
			get { return tblErrors; }
			set { tblErrors = value; }
		}

		public String VKORG
		{
			get { return strVKORG; }
			set { strVKORG = value; }
		}
		public String VKBUR
		{
			get { return strVKBUR; }
			set { strVKBUR = value; }
		}

		public DataTable Positionen
		{
			get { return tblPositionen; }
			set { tblPositionen = value; }
		}
		public Int32 SapID
		{
			get { return id_sap; }
			set { id_sap = value; }
		}
		public Int32 KopfID
		{
			get { return id_Kopf; }
			set { id_Kopf = value; }
		}

		public Boolean Abgerechnet
		{
			get { return abgerechnet; }
			set { abgerechnet = value; }
		}
		public String Kundenname
		{
			get { return kundenname; }
			set { kundenname = value; }
		}
		public String Kunnr
		{
			get { return kundennr; }
			set { kundennr = value; }
		}
		public String Ref1
		{
			get { return Referenz1; }
			set { Referenz1 = value; }
		}

		public String Ref2
		{
			get { return Referenz2; }
			set { Referenz2 = value; }
		}
		public String KreisKennz
		{
			get { return KreisKZ; }
			set { KreisKZ = value; }
		}
		public String Kreis
		{
			get { return KreisBez; }
			set { KreisBez = value; }
		}
		public Boolean WunschKennz
		{
			get { return WunschKenn; }
			set { WunschKenn = value; }
		}
		public Boolean Reserviert
		{
			get { return mReserviert; }
			set { mReserviert = value; }
		}
		public String ReserviertKennz
		{
			get { return mReserviertKennz; }
			set { mReserviertKennz = value; }
		}

		public Boolean Feinstaub
		{
			get { return mFeinstaub; }
			set { mFeinstaub = value; }
		}
		public String ZulDate
		{
			get { return mZulDate; }
			set { mZulDate = value; }
		}
		public String Kennzeichen
		{
			get { return mKennzeichen; }
			set { mKennzeichen = value; }
		}
		public String Kennztyp
		{
			get { return mKennztyp; }
			set { mKennztyp = value; }
		}
		public String KennzForm
		{
			get { return mKennzForm; }
			set { mKennzForm = value; }
		}
		public Int32 KennzAnzahl
		{
			get { return mKennzAnz; }
			set { mKennzAnz = value; }
		}
		public Boolean EinKennz
		{
			get { return mEinKennz; }
			set { mEinKennz = value; }
		}
		public String Bemerkung
		{
			get { return mBemerkung; }
			set { mBemerkung = value; }
		}
		public String Barcode
		{
			get { return mBarcode; }
			set { mBarcode = value; }
		}
		public Boolean EC
		{
			get { return mEC; }
			set { mEC = value; }
		}
		public Boolean Bar
		{
			get { return mBar; }
			set { mBar = value; }
		}
		public Boolean saved
		{
			get { return msaved; }
			set { msaved = value; }
		}

		public Boolean bearbeitet
		{
			get { return mbearbeitet; }
			set { mbearbeitet = value; }
		}
		public String Vorgang
		{
			get;
			set;
		}
		public Int16 toSave
		{
			get { return mtoSave; }
			set { mtoSave = value; }
		}
		public String toDelete
		{
			get { return mtoDelete; }
			set { mtoDelete = value; }
		}

		public String KundennrWE
		{
			get { return mKundennrWE; }
			set { mKundennrWE = value; }
		}
        public String Partnerrolle
        {
            get { return mPartnerrolle; }
            set { mPartnerrolle = value; }
        }
		public String Name1
		{
			get { return mName1; }
			set { mName1 = value; }
		}
		public String Name2
		{
			get { return mName2; }
			set { mName2 = value; }
		}
		public String PLZ
		{
			get { return mPLZ; }
			set { mPLZ = value; }
		}
		public String Ort
		{
			get { return mOrt; }
			set { mOrt = value; }
		}

		public String Strasse
		{
			get { return mStrasse; }
			set { mStrasse = value; }
		}
		public String SWIFT
		{
			get { return mSWIFT; }
			set { mSWIFT = value; }
		}
		public String IBAN
		{
			get { return mIBAN; }
			set { mIBAN = value; }
		}
        public String BankKey
        {
            get { return mBankKey; }
            set { mBankKey = value; }
        }
        public String Kontonr
        {
            get { return mKontonr; }
            set { mKontonr = value; }
        }
		public String Inhaber
		{
			get { return mInhaber; }
			set { mInhaber = value; }
		}
		public String Geldinstitut
		{
			get { return mGeldinstitut; }
			set { mGeldinstitut = value; }
		}
		public Boolean EinzugErm
		{
			get { return mEinzugErm; }
			set { mEinzugErm = value; }
		}
		public Boolean Rechnung
		{
			get { return mRechnung; }
			set { mRechnung = value; }
		}

		public DataTable BestLieferanten
		{
			get;
			set;
		}

		public String Lieferant_ZLD
		{
			get;
			set;
		}

		public String FrachtNrHin
		{
			get;
			set;
		}
		public String FrachtNrBack
		{
			get;
			set;
		}

        public DataTable tblBarcodData
		{
			get;
			set;
		}

        public DataTable tblBarcodMaterial
        {
            get;
            set;
        }

		public Boolean Barkunde
		{
			get;
			set;
		}

		public String IsZLD
		{
			get;
			set;
		}
		public DataTable tblNeueKunden
		{
			get;
			set;
		}
		public String NeueKundenNr
		{
			get;
			set;
		}
		public String NeueKundenName
		{
			get;
			set;
		}
        public String KennzTeil1
        {
            get;
            set;
        }
        public String IDCount
        {
            get;
            set;
        }
        public String Name1Hin
        {
            get;
            set;
        }
        public String Name2Hin
        {
            get;
            set;
        }
        public String StrasseHin
        {
            get;
            set;
        }
        public String PLZHin
        {
            get;
            set;
        }
        public String OrtHin
        {
            get;
            set;
        }
        public String DocRueck1
        {
            get;
            set;
        }
        public String NameRueck1
        {
            get;
            set;
        }
        public String NameRueck2
        {
            get;
            set;
        }
        public String StrasseRueck
        {
            get;
            set;
        }
        public String PLZRueck
        {
            get;
            set;
        }
        public String OrtRueck
        {
            get;
            set;
        }
        public String Doc2Rueck
        {
            get;
            set;
        }
        public String Name1Rueck2
        {
            get;
            set;
        }
        public String Name2Rueck2
        {
            get;
            set;
        }
        public String Strasse2Rueck
        {
            get;
            set;
        }
        public String PLZ2Rueck
        {
            get;
            set;
        }
        public String Ort2Rueck
        {
            get;
            set;
        }
        public Boolean ConfirmCPDAdress 
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
		#endregion

		#region "Methods"

        /// <summary>
        /// Kontruktor
        /// </summary>
        /// <param name="objUser">User-Objekt</param>
        /// <param name="objApp">Anwendungsobjekt</param>
        /// <param name="sVorgang">Vorgang</param>
		public VoerfZLD(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, String sVorgang)
			: base(ref objUser, objApp, "")
		{
			Vorgang = sVorgang;
			CreatePosTable(); 
		}

        /// <summary>
        /// Bereits vorhanden Daten über DAD-Barcode laden. Bapi: Z_ZLD_GET_DAD_SD_ORDER
        /// </summary>
        /// <param name="strAppId">ApppID</param>
        /// <param name="strSessionId">SessionID</param>
        /// <param name="page">Change01ZLD.aspx</param>
        public void getDataFromBarcode(String strAppId, String strSessionId, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "VoerfZLD.getDataFromBarcode";
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
                            RaiseError("-9999","Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + 
                                HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Nächste freie Belegnummer aus SAP ziehen. Bapi: Z_ZLD_EXPORT_BELNR
        /// </summary>
        /// <param name="strAppID">ApppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Change01ZLD.aspx</param>
        public void GiveSapID(String strAppID, String strSessionID, System.Web.UI.Page page)
		{
			m_strClassAndMethod = "VoerfZLD.GiveSapID";
			m_strAppID = strAppID;
			m_strSessionID = strSessionID;
			
            ClearError();

			id_sap = 0;
			if (m_blnGestartet == false)
			{
				m_blnGestartet = true;
				try
				{
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_BELNR", ref m_objApp, ref m_objUser, ref page);

					myProxy.callBapi();

					Int32.TryParse(myProxy.getExportParameter("E_BELN").ToString(), out id_sap);
					
                    Int32 subrc;
					Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
					m_intStatus = subrc;
					
                    String sapMessage= myProxy.getExportParameter("E_MESSAGE").ToString();
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
                            RaiseError("-9999","Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + 
                                HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
							break;
					}
				}
				finally { m_blnGestartet = false; }
			}
		}

        /// <summary>
        /// Prüft ob es sich bei dem ausgewählten Lieferant um ein
        /// Kroschke Zulassungsdiensthandelt
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ChangeZLDVorVersand_2.aspx</param>
		public void CheckLieferant(String strAppID, String strSessionID, System.Web.UI.Page page)
		{
			m_strClassAndMethod = "VoerfZLD.CheckLieferant";
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

					IsZLD = myProxy.getExportParameter("E_ZLD").ToString();
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
        /// Liefert an Hand des Kreiskennzeichen die passenden Zulassungdienste bzw. externen Dienstleister.
        /// Bapi: Z_ZLD_EXPORT_INFOPOOL
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ChangeZLDVorVersand_2.aspx, AHVersandChange_2.aspx</param>
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

					myProxy.setImportParameter("I_KREISKZ", KreisKennz);

					myProxy.callBapi();

					BestLieferanten = myProxy.getExportTable("GT_EX_ZUSTLIEF");
					
				}
				catch (Exception ex)
				{
					switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
					{
                        case "NO_LIEF":
                            RaiseError("103","Kein Lieferanten/Zulassungsdienst gepflegt! Keine Versandzulassung möglich! ");
                            break;
						default:
                            RaiseError("999","Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + 
                                HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")");
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
			tblPositionen = new DataTable();
			tblPositionen.Columns.Add("id_Kopf", typeof(Int32));
			tblPositionen.Columns.Add("id_pos", typeof(Int32));
			tblPositionen.Columns.Add("Menge", typeof(String));
			tblPositionen.Columns.Add("Matnr", typeof(String));
			tblPositionen.Columns.Add("Matbez", typeof(String));
			tblPositionen.Columns.Add("Preis", typeof(String));
			tblPositionen.Columns.Add("PosLoesch", typeof(String));
			tblPositionen.Columns.Add("GebMatnr", typeof(String));
			tblPositionen.Columns.Add("GebMatbez", typeof(String));
			tblPositionen.Columns.Add("GebMatnrSt", typeof(String));
			tblPositionen.Columns.Add("GebMatBezSt", typeof(String));
			tblPositionen.Columns.Add("KennzMat", typeof(String));
		}

        /// <summary>
        /// Daten in die Sql-Tabellen speichern(ZLDKopfTabelle,ZLDPositionsTabelle, ZLDBankverbindung, ZLDKundenadresse ).
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Change01ZLD.aspx</param>
        /// <param name="tblKunde">Kundenstammtabelle</param>
		public void InsertDB_ZLD(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblKunde) 
		{
			ClearError();

			var zldDataContext = new ZLDTableClassesDataContext();
			
            try
			{
				m_intStatus = 0;
				m_strMessage = "";
				GiveSapID(strAppID, strSessionID, page);

				if (id_sap != 0)
				{
				    var tblKopf = new ZLDKopfTabelle
				    {
				        id_sap = id_sap,
				        id_user = m_objUser.UserID,
				        id_session = strSessionID,
				        abgerechnet = false,
				        username = m_objUser.UserName,
				        Vorerfasser = m_objUser.UserName,
				        VorerfDatum = DateTime.Now,
				        VorhKennzReserv = false,
				        ZBII_ALT_NEU = false,
				        KennzVH = false,
				        VKKurz = "",
				        interneRef = "",
				        KundenNotiz = "",
				        KennzAlt = "",
				        kundenname = kundenname,
				        kundennr = Kunnr

				    };

				    DataRow[] KundeRow = tblKunde.Select("KUNNR='" + Kunnr + "'");

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
                    tblKopf.KreisKZ = KreisKZ;
                    tblKopf.KreisBez = KreisBez;
                    tblKopf.WunschKenn = WunschKenn;
                    tblKopf.ZusatzKZ = ZLDCommon.BoolToX(ZusatzKZ);
                    tblKopf.WunschKZ2 = WunschKZ2;
                    tblKopf.WunschKZ3 = WunschKZ3;
                    tblKopf.OhneGruenenVersSchein = ZLDCommon.BoolToX(OhneGruenenVersSchein);
                    tblKopf.SofortabrechnungErledigt = SofortabrechnungErledigt;
				    tblKopf.SofortabrechnungPfad = SofortabrechnungPfad;
                    tblKopf.Reserviert = mReserviert;
                    tblKopf.ReserviertKennz = mReserviertKennz;
                    tblKopf.Feinstaub = mFeinstaub;
                    DateTime tmpDate;
                    DateTime.TryParse(mZulDate, out tmpDate);
                    tblKopf.Zulassungsdatum = tmpDate;
                    tblKopf.Kennzeichen = Kennzeichen;
                    tblKopf.KennzForm = KennzForm;

				    tblKopf.KennzAnz = EinKennz ? 1 : 2;
                    tblKopf.EinKennz = EinKennz;

                    tblKopf.Bemerkung = Bemerkung;
                    tblKopf.EC = false;
                    tblKopf.Bar = false;
                    tblKopf.RE = false;
                    tblKopf.saved = saved;
                    tblKopf.toDelete = toDelete;
                    tblKopf.bearbeitet = false;
                    tblKopf.Vorgang = "V";
                    tblKopf.Barcode = Barcode;
                    tblKopf.testuser = m_objUser.IsTestUser;
                    tblKopf.Barkunde = false;
                    tblKopf.Zahlungsart = "";

					zldDataContext.Connection.Open();
					zldDataContext.ZLDKopfTabelle.InsertOnSubmit(tblKopf);
					zldDataContext.SubmitChanges();
					id_Kopf = tblKopf.id;
					zldDataContext.Connection.Close();

					zldDataContext = new ZLDTableClassesDataContext();
					if (tblPositionen.Rows.Count > 0)
					{
						foreach (DataRow drow in tblPositionen.Rows)
						{       
                            Int32 iMenge=1;
                            if (ZLDCommon.IsNumeric(drow["Menge"].ToString()))
                            {
                                Int32.TryParse(drow["Menge"].ToString(),out iMenge);
                            }

                            String strMatbz = drow["Matbez"].ToString();
                            if (iMenge > 0)
                            {
                                strMatbz = CombineBezeichnungMenge(drow["Matbez"].ToString(), iMenge);
                            }

						    var tblPos = new ZLDPositionsTabelle
						    {
						        id_Kopf = id_Kopf,
						        id_pos = (Int32) drow["id_pos"],
						        Menge = drow["Menge"].ToString(),
  
						        Matnr =  drow["Matnr"].ToString(),
						        Matbez = strMatbz,
						        GebMatbez = drow["GebMatbez"].ToString(),
						        GebMatnr = drow["GebMatnr"].ToString(),
						        GebMatnrSt = drow["GebMatnrSt"].ToString(),
						        GebMatBezSt = drow["GebMatBezSt"].ToString(),
						        Kennzmat = drow["KennzMat"].ToString(),
						        PosLoesch = ""
						    };
						    zldDataContext.Connection.Open();
							zldDataContext.ZLDPositionsTabelle.InsertOnSubmit(tblPos);
							zldDataContext.SubmitChanges();
							zldDataContext.Connection.Close();

						}
					}

				    var tblBank = new ZLDBankverbindung
				        {
				            id_Kopf = id_Kopf,
				            Inhaber = mInhaber,
				            IBAN = mIBAN,
				            Geldinstitut = Geldinstitut.Length > 40 ? mGeldinstitut.Substring(0, 40) : mGeldinstitut,
				            SWIFT = mSWIFT,
                            BankKey = BankKey,
                            Kontonr = Kontonr,
				            EinzugErm = mEinzugErm,
				            Rechnung = mRechnung
				        };

				    zldDataContext.Connection.Open();
					zldDataContext.ZLDBankverbindung.InsertOnSubmit(tblBank);
					zldDataContext.SubmitChanges();
					zldDataContext.Connection.Close();

				    var tblKunnadresse = new ZLDKundenadresse
				    {
                        id_Kopf = id_Kopf,
                        Partnerrolle = mPartnerrolle,
                        Name1 = mName1,
				        Name2 = mName2,
				        Strasse = mStrasse,
				        Ort = mOrt,
				        PLZ = mPLZ
				    };

				    zldDataContext.Connection.Open();
					zldDataContext.ZLDKundenadresse.InsertOnSubmit(tblKunnadresse);
					zldDataContext.SubmitChanges();
					zldDataContext.Connection.Close();
				}
				else
				{
                    RaiseError("9999","Fehler beim exportieren der Belegnummer!");
				}

			}
			catch (Exception ex)
			{
                RaiseError("9999", ex.Message);
			}
			finally 		
			{ 
				if (zldDataContext.Connection.State == ConnectionState.Open) 
				{
					zldDataContext.Connection.Close();
				}            
			
			}
		}

        /// <summary>
        /// aus Gridview Positionen löschen,  in der Datenbank Loeschkennzeichen setzen
        /// wenn Pos_id == 10(Hauptposition) dann Loeschkennzeichen auch im Kopf setzen(ChangeZLDListe.aspx).
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
                else
                {
                    str += " Where id_Kopf = " + IDRecordset;
                }

                command = new SqlCommand {Connection = connection, CommandType = CommandType.Text, CommandText = str};
			    command.ExecuteNonQuery();					  
			}
			catch (Exception ex)
			{
				RaiseError("-9999",ex.Message);
			}
			finally
			{
                connection.Close();
			}
		}

        /// <summary>
        /// Löschen der der Vorgänge nach dem absenden
        /// Versandszulassungen (ChangeZLDListe.aspx)
        /// </summary>
        /// <param name="IDRecordset">ID SQL</param>
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
        /// Daten aus der Eingabemaske(Change01ZLD.aspx) speichern bzw. aktualisieren.
        /// </summary>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="tblKunde">Kundenstammtabelle</param>
		public void UpdateDB_ZLD(String strSessionID, DataTable tblKunde)
		{
			var ZLD_DataContext = new ZLDTableClassesDataContext();
			try
			{
				ClearError();

                Int32 iMenge = 1;
				var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
							where k.id == id_Kopf
							select k).Single();

				tblKopf.id_user = m_objUser.UserID;
				tblKopf.id_session = strSessionID;
				tblKopf.abgerechnet = false;
				tblKopf.username = m_objUser.UserName;
				tblKopf.kundenname = kundenname;
				tblKopf.kundennr = Kunnr;
				DataRow[] KundeRow = tblKunde.Select("KUNNR='" + Kunnr + "'");

				if (KundeRow.Length == 1)
				{
					tblKopf.OhneSteuer = KundeRow[0]["OHNEUST"].ToString();
					tblKopf.PauschalKunde = KundeRow[0]["ZZPAUSCHAL"].ToString();
					tblKopf.KunnrLF = KundeRow[0]["KUNNR_LF"].ToString();
					tblKopf.KreisKZ_Direkt = KundeRow[0]["KREISKZ_DIREKT"].ToString();
                    if (KundeRow[0]["EXTENSION1"].ToString().Length > 0){ tblKopf.kundenname += " / " + KundeRow[0]["EXTENSION1"].ToString(); }
				}

				tblKopf.referenz1 = Ref1;
				tblKopf.referenz2 = Ref2;
				tblKopf.KreisKZ = KreisKZ;
				tblKopf.KreisBez = KreisBez;
				tblKopf.WunschKenn = WunschKenn;
                tblKopf.ZusatzKZ = ZLDCommon.BoolToX(ZusatzKZ);
                tblKopf.WunschKZ2 = WunschKZ2;
                tblKopf.WunschKZ3 = WunschKZ3;
                tblKopf.OhneGruenenVersSchein = ZLDCommon.BoolToX(OhneGruenenVersSchein);
                tblKopf.SofortabrechnungErledigt = SofortabrechnungErledigt;
			    tblKopf.SofortabrechnungPfad = SofortabrechnungPfad;
				tblKopf.Reserviert = mReserviert;
				tblKopf.ReserviertKennz = mReserviertKennz;
				tblKopf.Feinstaub = mFeinstaub;
				DateTime tmpDate;
				DateTime.TryParse(mZulDate, out tmpDate);
				tblKopf.Zulassungsdatum = tmpDate;
				tblKopf.Kennzeichen = Kennzeichen;
				tblKopf.KennzForm = KennzForm;

			    tblKopf.KennzAnz = EinKennz ? 1 : 2;
				tblKopf.EinKennz = EinKennz;
			
                tblKopf.Bemerkung = Bemerkung;
				tblKopf.EC = EC;
				tblKopf.Bar = Bar;
				tblKopf.saved = saved;
				tblKopf.toDelete = toDelete;
				tblKopf.bearbeitet = bearbeitet;
				tblKopf.Vorgang = "V";
				tblKopf.Barcode = Barcode;
				tblKopf.Barkunde = Barkunde;

				ZLD_DataContext.Connection.Open();
				ZLD_DataContext.SubmitChanges();
				id_Kopf = tblKopf.id;
				ZLD_DataContext.Connection.Close();

				ZLD_DataContext = new ZLDTableClassesDataContext();
				ZLD_DataContext.Connection.Open();

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

										PosRow.id_Kopf = id_Kopf;
                                        PosRow.id_pos = idpos;
                                        PosRow.Menge = drow["Menge"].ToString();
										PosRow.Matnr = drow["Matnr"].ToString();
											
										PosRow.GebMatbez = drow["GebMatbez"].ToString();
										PosRow.GebMatnr = drow["GebMatnr"].ToString();
										PosRow.GebMatnrSt = drow["GebMatnrSt"].ToString();
										PosRow.GebMatBezSt = drow["GebMatBezSt"].ToString();
										PosRow.Kennzmat = drow["KennzMat"].ToString();
                                        
                                        if (ZLDCommon.IsNumeric(drow["Menge"].ToString()))
                                        {
                                            Int32.TryParse(drow["Menge"].ToString(), out iMenge);
                                        }

                                        String strMatbz = drow["Matbez"].ToString();
                                        if (iMenge > 1)
                                        {
                                            strMatbz = CombineBezeichnungMenge(drow["Matbez"].ToString(), iMenge);
                                        }
                                        PosRow.Matbez = strMatbz;
								}
								ZLD_DataContext.SubmitChanges();
							}
							else
							{
								var tblPosNew = new ZLDPositionsTabelle
								    {
								        id_Kopf = id_Kopf,
                                        id_pos = idpos,
								        Menge = drow["Menge"].ToString(),
								        Matnr = drow["Matnr"].ToString()
								    };
							   
                                if (ZLDCommon.IsNumeric(drow["Menge"].ToString()))
                                {
                                    Int32.TryParse(drow["Menge"].ToString(), out iMenge);
                                }

                                String strMatbz = drow["Matbez"].ToString();
                                if (iMenge > 1)
                                {
                                    strMatbz = CombineBezeichnungMenge(drow["Matbez"].ToString(), iMenge);
                                }
                                tblPosNew.Matbez = strMatbz;

								tblPosNew.GebMatbez = drow["GebMatbez"].ToString();
								tblPosNew.GebMatnr = drow["GebMatnr"].ToString();
								tblPosNew.GebMatnrSt = drow["GebMatnrSt"].ToString();
								tblPosNew.GebMatBezSt = drow["GebMatBezSt"].ToString();
								tblPosNew.Kennzmat = drow["KennzMat"].ToString();
								ZLD_DataContext.ZLDPositionsTabelle.InsertOnSubmit(tblPosNew);
								ZLD_DataContext.SubmitChanges();
							}
						}
					}
					else if (tblPosCount.Count() > tblPositionen.Rows.Count)
					{
						foreach (var PosRow in tblPosCount)
						{ 
							DataRow [] drow  = tblPositionen.Select("id_pos = " + PosRow.id_pos );
							if (drow.Length == 1)
							{
								PosRow.id_Kopf = id_Kopf;
								PosRow.id_pos = (Int32)drow[0]["id_pos"];
								PosRow.Menge = drow[0]["Menge"].ToString();
								PosRow.Matnr = drow[0] ["Matnr"].ToString();
                                
                                if (ZLDCommon.IsNumeric(drow[0]["Menge"].ToString()))
                                {
                                    Int32.TryParse(drow[0]["Menge"].ToString(), out iMenge);
                                }

                                String strMatbz = drow[0]["Matbez"].ToString();
                                if (iMenge > 1)
                                {
                                    strMatbz = CombineBezeichnungMenge(drow[0]["Matbez"].ToString(), iMenge);
                                }
                                PosRow.Matbez = strMatbz;

								PosRow.GebMatbez = drow[0]["GebMatbez"].ToString();
								PosRow.GebMatnr = drow[0]["GebMatnr"].ToString();
								PosRow.GebMatnrSt = drow[0]["GebMatnrSt"].ToString();
								PosRow.GebMatBezSt = drow[0]["GebMatBezSt"].ToString();
								PosRow.Kennzmat = drow[0]["KennzMat"].ToString();
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
										PosRow.Menge = drow["Menge"].ToString();
										PosRow.Matnr = drow["Matnr"].ToString();
                                       
                                        if (ZLDCommon.IsNumeric(drow["Menge"].ToString()))
                                        {
                                            Int32.TryParse(drow["Menge"].ToString(), out iMenge);
                                        }

                                        String strMatbz = drow["Matbez"].ToString();
                                        if (iMenge > 1)
                                        {
                                            strMatbz = CombineBezeichnungMenge(drow["Matbez"].ToString(), iMenge);
                                        }
                                        PosRow.Matbez = strMatbz;

										PosRow.GebMatbez = drow["GebMatbez"].ToString();
										PosRow.GebMatnr = drow["GebMatnr"].ToString();
										PosRow.GebMatnrSt = drow["GebMatnrSt"].ToString();
										PosRow.GebMatBezSt = drow["GebMatBezSt"].ToString();
										PosRow.Kennzmat = drow["KennzMat"].ToString();
										PosRow.Kennzmat = "";
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
								        Menge = drow["Menge"].ToString(),
								        Matnr = drow["Matnr"].ToString()
								    };
							    
                                if (ZLDCommon.IsNumeric(drow["Menge"].ToString()))
                                {
                                    Int32.TryParse(drow["Menge"].ToString(), out iMenge);
                                }

                                String strMatbz = drow["Matbez"].ToString();
                                if (iMenge > 1)
                                {
                                    strMatbz = CombineBezeichnungMenge(drow["Matbez"].ToString(), iMenge);
                                }
                                tblPosNew.Matbez = strMatbz;

								tblPosNew.GebMatbez = drow["GebMatbez"].ToString();
								tblPosNew.GebMatnr = drow["GebMatnr"].ToString();
								tblPosNew.GebMatnrSt = drow["GebMatnrSt"].ToString();
								tblPosNew.GebMatBezSt = drow["GebMatBezSt"].ToString();
								tblPosNew.Kennzmat = drow["KennzMat"].ToString();
								tblPosNew.Kennzmat = "";
							}
						}
					}

					ZLD_DataContext = new ZLDTableClassesDataContext();

					var tblBank = (from b in ZLD_DataContext.ZLDBankverbindung
								  where b.id_Kopf == id_Kopf
								  select b).Single();

					tblBank.id_Kopf = id_Kopf;
					tblBank.IBAN = IBAN;
					tblBank.SWIFT = SWIFT;
				    tblBank.BankKey = BankKey;
				    tblBank.Kontonr = Kontonr;
					tblBank.EinzugErm = EinzugErm;
                    tblBank.Rechnung = Rechnung;
                    tblBank.Geldinstitut = Geldinstitut.Length > 40 ? Geldinstitut.Substring(0, 40) : Geldinstitut;
                   
					ZLD_DataContext.Connection.Open();
					ZLD_DataContext.SubmitChanges();
					ZLD_DataContext.Connection.Close();

					ZLD_DataContext = new ZLDTableClassesDataContext();
					var tblKunnadresse = (from k in ZLD_DataContext.ZLDKundenadresse
								   where k.id_Kopf == id_Kopf
								   select k).Single();

                    tblKunnadresse.Partnerrolle = mPartnerrolle;
					tblKunnadresse.Name1 = Name1;
					tblKunnadresse.Name2 = Name2;
					tblKunnadresse.Ort = Ort;
					tblKunnadresse.PLZ = PLZ;
					tblKunnadresse.Strasse = Strasse;
					ZLD_DataContext.SubmitChanges();
				}
				else
				{
                    RaiseError("9999","Fehler beim exportieren der Belegnummer!");
				}
			}
			catch (Exception ex)
			{
                RaiseError("9999", ex.Message);
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
        /// Laden eines Vorgange anhand der ID für die Eingabemaske(Change01ZLD.aspx).
        /// Laden der Daten aus den Datasets in die Klasseneigenschaften.
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
				
				KopfTabelle = new DataTable();
				KopfTabelle = tmpKopf;

				id_Kopf =(Int32)KopfTabelle.Rows[0]["id"];
				id_sap = (Int32)KopfTabelle.Rows[0]["id_sap"];
				abgerechnet = (Boolean)KopfTabelle.Rows[0]["abgerechnet"];
				kundenname = KopfTabelle.Rows[0]["kundenname"].ToString();
				Kunnr = KopfTabelle.Rows[0]["kundennr"].ToString();
				Ref1 = KopfTabelle.Rows[0]["referenz1"].ToString();
				Ref2 = KopfTabelle.Rows[0]["referenz2"].ToString();
				KreisKZ = KopfTabelle.Rows[0]["KreisKZ"].ToString();
				KreisBez = KopfTabelle.Rows[0]["KreisBez"].ToString();
				WunschKenn = (Boolean)KopfTabelle.Rows[0]["WunschKenn"];
                ZusatzKZ = ZLDCommon.XToBool(KopfTabelle.Rows[0]["ZusatzKZ"].ToString());
                WunschKZ2 = KopfTabelle.Rows[0]["WunschKZ2"].ToString();
                WunschKZ3 = KopfTabelle.Rows[0]["WunschKZ3"].ToString();
                OhneGruenenVersSchein = ZLDCommon.XToBool(KopfTabelle.Rows[0]["OhneGruenenVersSchein"].ToString());
                SofortabrechnungErledigt = (Boolean)KopfTabelle.Rows[0]["SofortabrechnungErledigt"];
			    SofortabrechnungPfad = KopfTabelle.Rows[0]["SofortabrechnungPfad"].ToString();
				mReserviert = (Boolean)KopfTabelle.Rows[0]["Reserviert"];
				mReserviertKennz = KopfTabelle.Rows[0]["ReserviertKennz"].ToString();
				mFeinstaub = (Boolean)KopfTabelle.Rows[0]["Feinstaub"];

				mZulDate = KopfTabelle.Rows[0]["Zulassungsdatum"].ToString();
                if (ZLDCommon.IsDate(mZulDate)) { mZulDate = ((DateTime)KopfTabelle.Rows[0]["Zulassungsdatum"]).ToShortDateString(); }

				mKennzeichen = KopfTabelle.Rows[0]["Kennzeichen"].ToString();
				Bemerkung = KopfTabelle.Rows[0]["Bemerkung"].ToString();
				KennzForm = KopfTabelle.Rows[0]["KennzForm"].ToString();
				EinKennz = (Boolean)KopfTabelle.Rows[0]["EinKennz"];
				mEC = (Boolean)KopfTabelle.Rows[0]["EC"];
				mBar = (Boolean)KopfTabelle.Rows[0]["Bar"];

				saved = (Boolean)KopfTabelle.Rows[0]["saved"];
				toDelete = KopfTabelle.Rows[0]["toDelete"].ToString();

				bearbeitet = (Boolean)KopfTabelle.Rows[0]["bearbeitet"];
				Vorgang = KopfTabelle.Rows[0]["Vorgang"].ToString();
				Barcode = KopfTabelle.Rows[0]["Barcode"].ToString();

				tblPositionen = tmpPos;

				Bankverbindung = tmpBank;

				mSWIFT = Bankverbindung.Rows[0]["SWIFT"].ToString();
				mIBAN = Bankverbindung.Rows[0]["IBAN"].ToString();
                mBankKey = Bankverbindung.Rows[0]["BankKey"].ToString();
                mKontonr = Bankverbindung.Rows[0]["Kontonr"].ToString();
				mInhaber = Bankverbindung.Rows[0]["Inhaber"].ToString();
				mGeldinstitut = Bankverbindung.Rows[0]["Geldinstitut"].ToString();
				mEinzugErm = (Boolean)Bankverbindung.Rows[0]["EinzugErm"];
				mRechnung = (Boolean)Bankverbindung.Rows[0]["Rechnung"];

				Kundenadresse = tmpKunde;
				mKundennrWE = Kundenadresse.Rows[0]["Kundennr"].ToString();
				mName1 = Kundenadresse.Rows[0]["Name1"].ToString();
				mName2 = Kundenadresse.Rows[0]["Name2"].ToString();
				mPLZ = Kundenadresse.Rows[0]["PLZ"].ToString();
				mOrt = Kundenadresse.Rows[0]["Ort"].ToString();
				mStrasse = Kundenadresse.Rows[0]["Strasse"].ToString();
			}
			catch (Exception ex)
			{
				m_strMessage = ex.Message;
			}
		}

        /// <summary>
        /// Laden eines Vorgange anhand der ID für die Eingabemasken.
        /// Speicherung der Records in einem Dataset.
        /// Aufgerufen von LoadDB_ZLDRecordset().
        /// </summary>
        /// <param name="RecordID">ID SQL</param>
        /// <param name="ds">Dataset</param>
		private void FillDataSet(Int32 RecordID, ref DataSet ds) 
		{
			var connection = new SqlConnection {ConnectionString = ConfigurationManager.AppSettings["Connectionstring"]};
            connection.Open();

			var adapter = new SqlDataAdapter(
										"SELECT * FROM ZLDKopfTabelle as KopfTabelle where id=" + RecordID + " AND id_user=" + m_objUser.UserID + " AND Vorgang='V';" +

										"SELECT * FROM ZLDPositionsTabelle As PositionsTabelle where id_Kopf=" + RecordID + "; " +

										"SELECT * FROM ZLDBankverbindung As Bankverbindung where id_Kopf=" + RecordID + ";  " +

										"SELECT * FROM ZLDKundenadresse as Kundenadresse where id_Kopf=" + RecordID + ";  ", connection);

			adapter.TableMappings.Add("ZLDKopfTabelle",  "KopfTabelle");
			adapter.TableMappings.Add("ZLDPositionsTabelle", "PositionsTabelle");
			adapter.TableMappings.Add("ZLDBankverbindung", "Bankverbindung");
			adapter.TableMappings.Add("ZLDKundenadresse", "Kundenadresse");
			adapter.Fill(ds);
			
			connection.Close();	
		}

        /// <summary>
        /// Laden der in der SQL-Tabelle angelegten Vorgänge für die Übersicht(ChangeZLDListe.aspx)
        /// </summary>
        /// <param name="sVorgang">Vorgang</param>
		public void LadeVorerfassungDB_ZLD(String sVorgang) 
		{
			ClearError();

			var connection = new SqlConnection{ConnectionString = ConfigurationManager.AppSettings["Connectionstring"]};
            
            try
			{
				tblListe = new DataTable();

				var command = new SqlCommand();
				var adapter = new SqlDataAdapter();

				command.CommandText = "SELECT dbo.ZLDKopfTabelle.*, dbo.ZLDPositionsTabelle.Matnr, dbo.ZLDPositionsTabelle.Matbez, dbo.ZLDPositionsTabelle.id_pos" +
                                      ", dbo.ZLDPositionsTabelle.PosLoesch" +
                                      " FROM dbo.ZLDKopfTabelle INNER JOIN" +
									  " dbo.ZLDPositionsTabelle ON dbo.ZLDKopfTabelle.id = dbo.ZLDPositionsTabelle.id_Kopf" +
									  " WHERE id_User = @id_User AND Vorgang = @Vorgang AND testuser = @testuser";

				command.Parameters.AddWithValue("@id_User", m_objUser.UserID);
				command.Parameters.AddWithValue("@Vorgang", sVorgang);
				command.Parameters.AddWithValue("@testuser", m_objUser.IsTestUser);
                command.CommandText += " ORDER BY  kundenname asc,dbo.ZLDKopfTabelle.id_sap, dbo.ZLDPositionsTabelle.id_pos, referenz1 asc, Kennzeichen";
				
                connection.Open();

				command.Connection = connection;
				command.CommandType = CommandType.Text;
				adapter.SelectCommand = command;

				adapter.Fill(tblListe);

				tblListe.Columns.Add("Status", typeof(String));
				foreach (DataRow rowListe in tblListe.Rows)
				{
					rowListe["Status"] = "";
				}
                if (tblListe.Rows.Count == 0)
                {
                    RaiseError("9999","Keine Daten gefunden!");
                }
                
                else 
                {
                    command.CommandText = "SELECT Count (ID) FROM dbo.ZLDKopfTabelle" +
                      " WHERE id_User = @id_User AND Vorgang = @Vorgang AND testuser = @testuser";

                    command.CommandType = CommandType.Text;
                    IDCount = command.ExecuteScalar().ToString();
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
        /// Übergeben der Daten an SAP aus den SQL-Tabellen(Vorerfassung). Bapi: Z_ZLD_IMPORT_ERFASSUNG1
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ChangeZLDListe.aspx</param>
        /// <param name="tblKunde">Kundenstammtabelle</param>
        /// <param name="tblMaterialStamm">Dienstleistungstabelle</param>
        /// <param name="tblStvaStamm">Stammtabelle StVa</param>
		public void SaveZLDVorerfassung(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblKunde, 
                                        DataTable tblMaterialStamm, DataTable tblStvaStamm) 
		{
			m_strClassAndMethod = "VoerfZLD.SaveZLDVorerfassung";
			m_strAppID = strAppID;
			m_strSessionID = strSessionID;
			
            ClearError();

			if (m_blnGestartet == false)
			{
				m_blnGestartet = true;
				var ZLD_DataContext = new ZLDTableClassesDataContext();

				try
				{
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_IMPORT_ERFASSUNG1", ref m_objApp, ref m_objUser, ref page);
			
					DataTable importAuftrag = myProxy.getImportTable("GT_IMP_BAK");

					// Z = Zwischenlösung für Konvertierungsfehler von BCD-Werten im DynProxy

					// Z - siehe oben ++++++++
					DataTable importPos = myProxy.getImportTable("GT_IMP_POS_S01");
					// +++++++++++++++++++++++
					DataTable importBank = myProxy.getImportTable("GT_IMP_BANK");
					DataTable importAdresse = myProxy.getImportTable("GT_IMP_ADRS");

					Int32 LastID = 0;
                    Int32 OKLoeschCount = 0;

					foreach (DataRow SaveRow in tblListe.Rows)
					{
						var tmpID = (Int32)SaveRow["id"];
						if (LastID != tmpID)
						{
							var tblKopf = (from k in ZLD_DataContext.ZLDKopfTabelle
											where k.id == tmpID
											select k).Single();

                            OKLoeschCount++;

							DataRow importRowAuftrag = importAuftrag.NewRow();
							importRowAuftrag["MANDT"] = "010";
							importRowAuftrag["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
							importRowAuftrag["VBELN"] = "";
							importRowAuftrag["VKORG"] = strVKORG;
							importRowAuftrag["VKBUR"] = strVKBUR;
							importRowAuftrag["ERNAM"] = tblKopf.username;
							importRowAuftrag["ERDAT"] = DateTime.Now;
							importRowAuftrag["FLAG"] = "";
							importRowAuftrag["BARCODE"] = tblKopf.Barcode;
							importRowAuftrag["KUNNR"] = tblKopf.kundennr.PadLeft(10, '0');
							importRowAuftrag["ZZREFNR1"] = tblKopf.referenz1;
							importRowAuftrag["ZZREFNR2"] = tblKopf.referenz2;
							importRowAuftrag["KREISKZ"] = tblKopf.KreisKZ;
							importRowAuftrag["PRALI_PRINT"] = "";
							importRowAuftrag["ERROR_TEXT"] = "";

							DataRow[] RowStva = tblStvaStamm.Select("KREISKZ='" + tblKopf.KreisKZ + "'");
							importRowAuftrag["KREISBEZ"] = RowStva.Length == 1 ? RowStva[0]["KREISBEZ"]:tblKopf.KreisBez;
                           
							importRowAuftrag["WUNSCHKENN_JN"] = ZLDCommon.BoolToX(tblKopf.WunschKenn);
							importRowAuftrag["ZUSKENNZ"] = tblKopf.ZusatzKZ;
							importRowAuftrag["WU_KENNZ2"] = tblKopf.WunschKZ2;
							importRowAuftrag["WU_KENNZ3"] = tblKopf.WunschKZ3;
							importRowAuftrag["O_G_VERSSCHEIN"] = tblKopf.OhneGruenenVersSchein;
                            importRowAuftrag["SOFORT_ABR_ERL"] = ZLDCommon.BoolToX(tblKopf.SofortabrechnungErledigt);
                            importRowAuftrag["SA_PFAD"] = tblKopf.SofortabrechnungPfad;
							importRowAuftrag["RESERVKENN_JN"] = ZLDCommon.BoolToX(tblKopf.Reserviert);
							importRowAuftrag["RESERVKENN"] = tblKopf.ReserviertKennz;
							importRowAuftrag["FEINSTAUBAMT"] = ZLDCommon.BoolToX(tblKopf.Feinstaub);
							importRowAuftrag["ZZZLDAT"] = tblKopf.Zulassungsdatum;
							importRowAuftrag["ZZKENN"] = tblKopf.Kennzeichen;
                            
                            importRowAuftrag["LOEKZ"] = tblKopf.toDelete;
                            if (tblKopf.toDelete == "X")
                            {
                                importRowAuftrag["KSTATUS"] = "L";
                                importRowAuftrag["BEB_STATUS"] = "L";
                            }

							importRowAuftrag["KENNZTYP"] = "";
							importRowAuftrag["KENNZFORM"] = tblKopf.KennzForm;
							importRowAuftrag["KENNZANZ"] = "0";
							importRowAuftrag["EINKENN_JN"] = ZLDCommon.BoolToX(tblKopf.EinKennz);
							importRowAuftrag["BEMERKUNG"] = tblKopf.Bemerkung;
							importRowAuftrag["EC_JN"] = ZLDCommon.BoolToX(tblKopf.EC);
							importRowAuftrag["BAR_JN"] = ZLDCommon.BoolToX(tblKopf.Bar);
							importRowAuftrag["RE_JN"] = ZLDCommon.BoolToX(tblKopf.RE);

							DataRow[] KundeRow = tblKunde.Select("KUNNR='" + tblKopf.kundennr + "'");

							if (KundeRow.Length == 1)
							{
							    importRowAuftrag["KUNDEBAR_JN"] = KundeRow[0]["BARKUNDE"].ToString();
							}

							importRowAuftrag["VH_KENNZ_RES"] = ZLDCommon.BoolToX(tblKopf.VorhKennzReserv);
							importRowAuftrag["ZBII_ALT_NEU"] = ZLDCommon.BoolToX(tblKopf.ZBII_ALT_NEU);
							importRowAuftrag["KENNZ_VH"] = ZLDCommon.BoolToX(tblKopf.KennzVH);
							importRowAuftrag["VK_KUERZEL"] = tblKopf.VKKurz;
							importRowAuftrag["KUNDEN_REF"] = tblKopf.interneRef;
							importRowAuftrag["KUNDEN_NOTIZ"] = tblKopf.KundenNotiz;
							importRowAuftrag["ALT_KENNZ"] = tblKopf.KennzAlt;
							importRowAuftrag["VE_ERNAM"] = tblKopf.Vorerfasser;
							importRowAuftrag["VE_ERDAT"] = tblKopf.VorerfDatum;
							if (!String.IsNullOrEmpty(tblKopf.VorerfUhrzeit))
							{
							    importRowAuftrag["VE_ERZEIT"] = tblKopf.VorerfUhrzeit;
							}

							if (ZLDCommon.IsDate(tblKopf.Still_Datum.ToString()))
							{
							    importRowAuftrag["STILL_DAT"] = tblKopf.Still_Datum;
							}

							DataRow importRow;

							var tblPosCount = (from p in ZLD_DataContext.ZLDPositionsTabelle
							                    where p.id_Kopf == tmpID
							                    select p);

							Int32 ROWCOUNT = 10;
							foreach (var PosRow in tblPosCount)
							{
							    importRow = importPos.NewRow();
							    // Z - siehe oben ++++++++
							    // +++++++++++++++++++++++
							    importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
							    String sUePos = (ROWCOUNT).ToString().PadLeft(6, '0');
							    importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
							    importRow["UEPOS"] = "000000";
							    importRow["SD_REL"] = PosRow.SDRelevant;

							    // Z - siehe oben ++++++++

							    importRow["MENGE_C"] = PosRow.Menge != ""
							                                ? importRow["MENGE_C"] = PosRow.Menge
							                                : importRow["MENGE_C"] = "1";
							    // +++++++++++++++++++++++
							    importRow["WEBMTART"] = "D";
							    ROWCOUNT += 10;
							    DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + PosRow.Matnr + "'");
							    if (MatRow.Length == 1)
							    {
							        if (MatRow[0]["KENNZREL"].ToString() == "X")
							        {
							            importRowAuftrag["KENNZANZ"] = tblKopf.EinKennz == true ? "1" : "2";
							        }
							    }

							    importRow["MATNR"] = PosRow.Matnr.PadLeft(18, '0');
							    importRow["MAKTX"] = PosRow.Matbez;
							    
                                importRow["LOEKZ"] = PosRow.PosLoesch == "L" ? "X" : "";
							  
							    importPos.Rows.Add(importRow);
							    if (MatRow[0]["GEBMAT"].ToString().Length > 0)
							    {
							        //--- dazu gehörige Gebührenmaterial
							        if (tblKopf.OhneSteuer == "X")
							        {
							            importRow = importPos.NewRow();
							            // Z - siehe oben ++++++++
							            // +++++++++++++++++++++++ 
							            importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
							            importRow["UEPOS"] = sUePos;
							            importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
							            // Z - siehe oben ++++++++
							            importRow["MENGE_C"] = "1";
							            //
							            importRow["MATNR"] = PosRow.GebMatnr;
							            importRow["MAKTX"] = PosRow.GebMatbez;
							            importRow["WEBMTART"] = "G";
							            importRow["SD_REL"] = PosRow.SDRelevant;

                                        importRow["LOEKZ"] = PosRow.PosLoesch == "L" ? "X" : "";
							  
							            ROWCOUNT += 10;
							            importPos.Rows.Add(importRow);
							        }
							        else
							        {
							            importRow = importPos.NewRow();
							            // Z - siehe oben ++++++++
							            // +++++++++++++++++++++++
							            importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
							            importRow["UEPOS"] = sUePos;
							            importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
							            // Z - siehe oben ++++++++
							            importRow["MENGE_C"] = "1";
							            // +++++++++++++++++++++++
							            importRow["MATNR"] = PosRow.GebMatnrSt;
							            importRow["MAKTX"] = PosRow.GebMatBezSt;
							            importRow["WEBMTART"] = "G";
							            importRow["SD_REL"] = PosRow.SDRelevant;
                              
                                        importRow["LOEKZ"] = PosRow.PosLoesch == "L" ? "X" : "";
							  
							            ROWCOUNT += 10;
							            importPos.Rows.Add(importRow);
							        }
							    }
							    if (tblKopf.PauschalKunde != "X")
							    {
							        if (PosRow.Kennzmat.Trim(' ') != "")
							        {
							            importRow = importPos.NewRow();
							            importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
							            importRow["UEPOS"] = sUePos;
							            importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
							            // Z - siehe oben ++++++++
							            importRow["MENGE_C"] = "1";
							            // +++++++++++++++++++++++
							            importRow["MATNR"] = PosRow.Kennzmat;
							            importRow["MAKTX"] = "";
							            importRow["WEBMTART"] = "K";
							            importRow["SD_REL"] = PosRow.SDRelevant;

                                        importRow["LOEKZ"] = PosRow.PosLoesch == "L" ? "X" : "";
							  
							            ROWCOUNT += 10;
							            importPos.Rows.Add(importRow);
							        }
							    }

							    if (sUePos == "000010") // Hauptposition mit Steuermaterieal
							    {
							        importRow = importPos.NewRow();
							        importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
							        importRow["UEPOS"] = sUePos;
							        importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
							        // Z - siehe oben ++++++++
							        importRow["MENGE_C"] = "1";
							        // +++++++++++++++++++++++
							        importRow["MATNR"] = "591".PadLeft(1, '0');
							        importRow["MAKTX"] = "";
							        importRow["WEBMTART"] = "S";
							        importRow["SD_REL"] = PosRow.SDRelevant;

                                    importRow["LOEKZ"] = PosRow.PosLoesch == "L" ? "X" : "";
							  
							        ROWCOUNT += 10;
							        importPos.Rows.Add(importRow);
							    }
							}

							importAuftrag.Rows.Add(importRowAuftrag);

                            // MJE, 2014-01-21:
                            // changed from Single() to SingleOrDefault()
							var tblBank = (from b in ZLD_DataContext.ZLDBankverbindung
							                where b.id_Kopf == tmpID
							                select b).SingleOrDefault();

							if (tblBank != null && tblBank.Inhaber != null)
							{
							    if (tblBank.Inhaber.Length > 0)
							    {
							        importRow = importBank.NewRow();

							        importRow["MANDT"] = "010";
							        importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
							        if (tblBank.SWIFT != null)
							        {
							            importRow["SWIFT"] = tblBank.SWIFT;
							        }
							        if (tblBank.IBAN != null)
							        {
							            importRow["IBAN"] = tblBank.IBAN;
							        }
                                    if (tblBank.BankKey != null)
                                    {
                                        importRow["BANKL"] = tblBank.BankKey;
                                    }
                                    if (tblBank.Kontonr != null)
                                    {
                                        importRow["BANKN"] = tblBank.Kontonr;
                                    }
							        if (tblBank.Geldinstitut != null)
							        {
							            importRow["EBPP_ACCNAME"] = tblBank.Geldinstitut;
							        }
							        if (tblBank.Inhaber != null)
							        {
							            importRow["KOINH"] = tblBank.Inhaber;
							        }
							        if (tblBank.EinzugErm != null)
							        {
							            importRow["EINZ_JN"] = ZLDCommon.BoolToX(tblBank.EinzugErm);
							        }
							        if (tblBank.Rechnung != null)
							        {
							            importRow["RECH_JN"] = ZLDCommon.BoolToX(tblBank.Rechnung);
							        }

							        importBank.Rows.Add(importRow);
							    }
							}
                            
                            // MJE, 2014-01-21:
                            // changed from Single() to SingleOrDefault()
                            var tblKunnadresse = (from k in ZLD_DataContext.ZLDKundenadresse
							                        where k.id_Kopf == tmpID
							                        select k).SingleOrDefault();

                            if (tblKunnadresse != null && tblKunnadresse.Name1 != null)
							{
							    if (tblKunnadresse.Name1.Length > 0)
							    {
							        importRow = importAdresse.NewRow();

							        importRow["MANDT"] = "010";
							        importRow["ZULBELN"] = tblKopf.id_sap.ToString().PadLeft(10, '0');
							        importRow["KUNNR"] = tblKopf.kundennr.PadLeft(10, '0');
							        if (tblKunnadresse.Partnerrolle != null)
							        {
							            importRow["PARVW"] = tblKunnadresse.Partnerrolle;
							        }
							        if (tblKunnadresse.Name1 != null)
							        {
							            importRow["LI_NAME1"] = tblKunnadresse.Name1;
							        }
							        if (tblKunnadresse.Name2 != null)
							        {
							            importRow["LI_NAME2"] = tblKunnadresse.Name2;
							        }
							        if (tblKunnadresse.PLZ != null)
							        {
							            importRow["LI_PLZ"] = tblKunnadresse.PLZ;
							        }
							        if (tblKunnadresse.Ort != null)
							        {
							            importRow["LI_CITY1"] = tblKunnadresse.Ort;
							        }
							        if (tblKunnadresse.Strasse != null)
							        {
							            importRow["LI_STREET"] = tblKunnadresse.Strasse;
							        }

							        importAdresse.Rows.Add(importRow);
							    }
							}
							LastID = tmpID;
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
                                Int32 id_Pos;
                                Int32.TryParse(rowError["ZULPOSNR"].ToString(), out id_Pos);
                                DataRow[] rowListe = tblListe.Select("id_sap=" + idsap + " AND id_pos =" + id_Pos);
                                if (rowListe.Length == 1)
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
				finally { 
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
        /// Übergeben der Daten an SAP aus den SQL-Tabellen(Versandzulassung). Bapi: Z_ZLD_IMPORT_ERFASSUNG1
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ChangeZLDVorVersand_2.aspx</param>
        /// <param name="tblKunde">Kundenstammtabelle</param>
        /// <param name="tblMaterialStamm">Dienstleistungstabelle</param>
        /// <param name="tblStvaStamm">Stammtabelle StVa</param>
		public void SaveZLDVorerfVersand(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblKunde, DataTable tblMaterialStamm, DataTable tblStvaStamm)
		{
			m_strClassAndMethod = "VoerfZLD.SaveZLDVorerfVersand";
			m_strAppID = strAppID;
			m_strSessionID = strSessionID;
			
            ClearError();

			GiveSapID(strAppID, strSessionID, page);

			if (m_blnGestartet == false)
			{
				m_blnGestartet = true;
				try
				{
					if (id_sap !=0)
					{
						DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_IMPORT_ERFASSUNG1", ref m_objApp, ref m_objUser, ref page);

						DataTable importAuftrag = myProxy.getImportTable("GT_IMP_BAK");
						
						// Z = Zwischenlösung für Konvertierungsfehler von BCD-Werten im DynProxy

						// Z - siehe oben ++++++++
						DataTable importPos = myProxy.getImportTable("GT_IMP_POS_S01");
						// +++++++++++++++++++++++
						DataTable importBank = myProxy.getImportTable("GT_IMP_BANK");
						DataTable importAdresse = myProxy.getImportTable("GT_IMP_ADRS");
						
						DataRow importRowAuftrag = importAuftrag.NewRow();
						importRowAuftrag["MANDT"] = "010";
						importRowAuftrag["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
						importRowAuftrag["VBELN"] = "";
						importRowAuftrag["VKORG"] = strVKORG;
						importRowAuftrag["VKBUR"] = strVKBUR;
						importRowAuftrag["ERNAM"] = m_objUser.UserName;
						importRowAuftrag["ERDAT"] = DateTime.Now;
						importRowAuftrag["FLAG"] = "";
						importRowAuftrag["STATUS"] = "N";
						importRowAuftrag["BARCODE"] = Barcode;
						importRowAuftrag["KUNNR"] = kundennr.PadLeft(10, '0');
						importRowAuftrag["ZZREFNR1"] = Referenz1;
						importRowAuftrag["ZZREFNR2"] = Referenz2;
						importRowAuftrag["KREISKZ"] = KreisKZ;

						DataRow[] tblKundeRow = tblKunde.Select("KUNNR='" + Kunnr + "'");

						if (tblKundeRow.Length == 1)
						{
							importRowAuftrag["KUNDEBAR_JN"] = tblKundeRow[0]["BARKUNDE"].ToString();
						}

						DataRow[] RowStva = tblStvaStamm.Select("KREISKZ='" + KreisKZ + "'");
						if (RowStva.Length == 1)
						{
							importRowAuftrag["KREISBEZ"] = RowStva[0]["KREISBEZ"];
						}
						else
						{
							importRowAuftrag["KREISBEZ"] = KreisBez;
						}
                        importRowAuftrag["WUNSCHKENN_JN"] = ZLDCommon.BoolToX(WunschKenn);

                        importRowAuftrag["RESERVKENN_JN"] = ZLDCommon.BoolToX(Reserviert);
                        importRowAuftrag["ZUSKENNZ"] = ZLDCommon.BoolToX(ZusatzKZ);
                        importRowAuftrag["WU_KENNZ2"] = WunschKZ2;
                        importRowAuftrag["WU_KENNZ3"] = WunschKZ3;
                        importRowAuftrag["O_G_VERSSCHEIN"] = ZLDCommon.BoolToX(OhneGruenenVersSchein);
                        importRowAuftrag["SOFORT_ABR_ERL"] = ZLDCommon.BoolToX(SofortabrechnungErledigt);
                        importRowAuftrag["SA_PFAD"] = SofortabrechnungPfad;
						importRowAuftrag["RESERVKENN"] = ReserviertKennz;
                        importRowAuftrag["FEINSTAUBAMT"] = ZLDCommon.BoolToX(Feinstaub);
						if (ZulDate.Length > 0) { importRowAuftrag["ZZZLDAT"] = ZulDate; }

						importRowAuftrag["ZZKENN"] = Kennzeichen;

						importRowAuftrag["KENNZTYP"] = "";
						importRowAuftrag["KENNZFORM"] = KennzForm;
						importRowAuftrag["KENNZANZ"] = "2";
                        importRowAuftrag["EINKENN_JN"] = ZLDCommon.BoolToX(EinKennz);
						importRowAuftrag["BEMERKUNG"] = Bemerkung;
						importRowAuftrag["EC_JN"] = "X";
						importRowAuftrag["BAR_JN"] = "";
                        importRowAuftrag["RE_JN"] = "";
						importRowAuftrag["ZL_RL_FRBNR_ZUR"] = FrachtNrBack;
						importRowAuftrag["ZL_RL_FRBNR_HIN"] = FrachtNrHin;

						importRowAuftrag["ZL_LIFNR"] = Lieferant_ZLD;

						if (Lieferant_ZLD.TrimStart('0').Substring(0, 2) == "56" && IsZLD == "X")
                        { 
                            importRowAuftrag["VZD_VKBUR"] = Lieferant_ZLD.TrimStart('0').Substring(2,4); 
                        }

                        importRowAuftrag["VH_KENNZ_RES"] = "";
                        importRowAuftrag["ZBII_ALT_NEU"] = "";
                        importRowAuftrag["KENNZ_VH"] = "";
                        importRowAuftrag["VK_KUERZEL"] = "";
                        importRowAuftrag["KUNDEN_REF"] = "";
                        importRowAuftrag["KUNDEN_NOTIZ"] = "";
                        importRowAuftrag["ALT_KENNZ"] = "";

                        importRowAuftrag["VE_ERNAM"] = m_objUser.UserName;
                        importRowAuftrag["VE_ERDAT"] = DateTime.Now;
  
						DataRow importRow;

						//----------------

						Int32 ROWCOUNT = 10;
						foreach (DataRow PosRow in Positionen.Rows)
						{
							importRow = importPos.NewRow();
							importRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
							String sUePos = (ROWCOUNT).ToString().PadLeft(6, '0');
							importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
							importRow["UEPOS"] = "000000";
							// Z - siehe oben ++++++++
                            importRow["MENGE_C"] = PosRow["Menge"].ToString() != "" ? importRow["MENGE_C"] = PosRow["Menge"].ToString() : importRow["MENGE_C"] = "1";
							//
							importRow["WEBMTART"] = "D";

							ROWCOUNT += 10;

							DataRow[] MatRow = tblMaterialStamm.Select("MATNR='" + PosRow["Matnr"].ToString() + "'");
							if (MatRow.Length == 1)
							{
								if (MatRow[0]["KENNZREL"].ToString() == "X")
								{
								    importRowAuftrag["KENNZANZ"] = EinKennz ? "1" : "2";
								}
							}

							importRow["MATNR"] = PosRow["Matnr"].ToString().PadLeft(18, '0');
							importRow["MAKTX"] = PosRow["Matbez"].ToString();
							importPos.Rows.Add(importRow);

							//--- dazu gehörige Gebührenmaterial GebMatnr
							DataRow[] KundeRow = tblKunde.Select("KUNNR='" + Kunnr + "'");
							String tmpSteuer = "";
							String tmpPauschalKunde = "";

							if (KundeRow.Length == 1)
							{
								tmpSteuer = KundeRow[0]["OHNEUST"].ToString();
								tmpPauschalKunde = KundeRow[0]["ZZPAUSCHAL"].ToString();
							}

                            if (MatRow[0]["GEBMAT"].ToString().Length > 0)
                            {
                                if (tmpSteuer == "X")
                                {
                                    importRow = importPos.NewRow();
                                    importRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
                                    importRow["UEPOS"] = sUePos;
                                    importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
                                    // Z - siehe oben ++++++++
                                    importRow["MENGE_C"] = "1";
                                    // +++++++++++++++++++++++
                                    importRow["MATNR"] = PosRow["GebMatnr"].ToString().PadLeft(18, '0');
                                    importRow["MAKTX"] = PosRow["GebMatbez"].ToString();
                                    importRow["WEBMTART"] = "G";
                                    ROWCOUNT += 10;
                                    importPos.Rows.Add(importRow);
                                }
                                else
                                {
                                    importRow = importPos.NewRow();
                                    importRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
                                    importRow["UEPOS"] = sUePos;
                                    importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
                                    // Z - siehe oben ++++++++
                                    importRow["MENGE_C"] = "1";
                                    // +++++++++++++++++++++++
                                    importRow["MATNR"] = PosRow["GebMatnrSt"].ToString();
                                    importRow["MAKTX"] = PosRow["GebMatBezSt"].ToString();
                                    importRow["WEBMTART"] = "G";

                                    ROWCOUNT += 10;
                                    importPos.Rows.Add(importRow);
                                }
                            }

							if (tmpPauschalKunde != "X")
							{
								if (PosRow["Kennzmat"].ToString().Trim(' ') != "")
								{
									importRow = importPos.NewRow();
									importRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
									importRow["UEPOS"] = sUePos;
									importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
									// Z - siehe oben ++++++++
									importRow["MENGE_C"] = "1";
									//
									importRow["MATNR"] = PosRow["Kennzmat"].ToString();
									importRow["MAKTX"] = "";
									importRow["WEBMTART"] = "K";

									ROWCOUNT += 10;
									importPos.Rows.Add(importRow);
								}
                            }

							if (sUePos == "000010")// Hauptposition mit Steuermaterieal
							{
								importRow = importPos.NewRow();
								importRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
								importRow["UEPOS"] = sUePos;
								importRow["ZULPOSNR"] = (ROWCOUNT).ToString().PadLeft(6, '0');
								// Z - siehe oben ++++++++
								importRow["MENGE_C"] = "1";
								//
								importRow["MATNR"] = "591".PadLeft(18, '0');
								importRow["MAKTX"] = "";
								importRow["WEBMTART"] = "S";
								ROWCOUNT += 10;
								importPos.Rows.Add(importRow);
							}
						}

						importAuftrag.Rows.Add(importRowAuftrag);

                        importRow = importBank.NewRow();

                        importRow["MANDT"] = "010";
                        importRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
                        importRow["SWIFT"] = SWIFT;
                        importRow["IBAN"] = IBAN;
                        importRow["BANKL"] = BankKey;
                        importRow["BANKN"] = Kontonr;
                        importRow["EBPP_ACCNAME"] = Geldinstitut;
                        importRow["KOINH"] = Inhaber;
                        importRow["EINZ_JN"] = ZLDCommon.BoolToX(EinzugErm);
                        importRow["RECH_JN"] = ZLDCommon.BoolToX(Rechnung);
                        importBank.Rows.Add(importRow);

						importRow = importAdresse.NewRow();

						importRow["MANDT"] = "010";
						importRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
						importRow["KUNNR"] = kundennr.PadLeft(10, '0');
                        importRow["PARVW"] = "AG";
						importRow["LI_NAME1"] = Name1;
						importRow["LI_NAME2"] = Name2;
						importRow["LI_PLZ"] = PLZ;
						importRow["LI_CITY1"] = Ort;
						importRow["LI_STREET"] = Strasse;
						importAdresse.Rows.Add(importRow);

                        if (DocRueck1.Length > 0) 
                        {
                            importRow = importAdresse.NewRow();

                            importRow["MANDT"] = "010";
                            importRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
                            importRow["KUNNR"] = kundennr.PadLeft(10, '0');
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
                            importRow["ZULBELN"] = id_sap.ToString().PadLeft(10, '0');
                            importRow["KUNNR"] = kundennr.PadLeft(10, '0');
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
								Int32 tmpID_sap;
								Int32.TryParse(rowError["ZULBELN"].ToString(), out tmpID_sap);
								Int32 id_Pos;
								Int32.TryParse(rowError["ZULPOSNR"].ToString(), out id_Pos);
								DataRow[] rowListe = tblListe.Select("id_sap=" + id_sap + " AND id_pos =" + id_Pos);
								if (rowListe.Length == 1)
								{
									rowListe[0]["Status"] = rowError["ERROR_TEXT"];
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
				}
			}
		}

        /// <summary>
        /// Selektion neuer Kunden für die Preisanlage. Bapi: Z_ZLD_EXPORT_NEW_DEBI
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Preisanlage.aspx</param>
		public void getNeueKunden(String strAppID, String strSessionID, System.Web.UI.Page page)
		{
			m_strClassAndMethod = "VoerfZLD.getNeueKunden";
			m_strAppID = strAppID;
			m_strSessionID = strSessionID;
			
            ClearError();

			if (m_blnGestartet == false)
			{
				m_blnGestartet = true;
				try
				{
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_NEW_DEBI", ref m_objApp, ref m_objUser, ref page);

					myProxy.setImportParameter("I_VKORG", VKORG);
					myProxy.setImportParameter("I_VKBUR", VKBUR);
					
					myProxy.callBapi();

					tblNeueKunden = myProxy.getExportTable("GT_KUNDEN");

					foreach (DataRow item in tblNeueKunden.Rows)
					{
						item["KUNNR"] = item["KUNNR"].ToString().TrimStart('0');
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
        /// Setzen des Preispflege Status. Bapi: Z_ZLD_SETNEW_DEBI_ERL
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Preisanlage.aspx</param>
        /// <param name="NewRow">TableRow</param>
		public void SaveNeueKunden(String strAppID, String strSessionID, System.Web.UI.Page page, DataRow NewRow)
		{
			m_strClassAndMethod = "VoerfZLD.SaveNeueKunden";
			m_strAppID = strAppID;
			m_strSessionID = strSessionID;
			
            ClearError();

			if (m_blnGestartet == false)
			{
				m_blnGestartet = true;
				try
				{
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_SETNEW_DEBI_ERL", ref m_objApp, ref m_objUser, ref page);

					DataTable SapTable = myProxy.getImportTable("GT_KUNDEN");

					DataRow SapNewRow = SapTable.NewRow();

					SapNewRow["KUNNR"] = NewRow["KUNNR"].ToString();
					SapNewRow["VKUNNR"] = NewRow["VKUNNR"].ToString();
					SapNewRow["KONDA"] = NewRow["KONDA"].ToString();
					SapNewRow["NAME1"] = NewRow["NAME1"].ToString();
					SapNewRow["PREIS_ZLD"] = "X";

					SapTable.Rows.Add(SapNewRow);

					myProxy.callBapi();
                    
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out m_intStatus);
				   
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
        /// <param name="max">Maximale Länge des Strings, default: 40</param>
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
                    var count = bezeichnung.Length - idxRemove;
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
