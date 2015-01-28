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

		private DataTable KopfTabelle;
		private DataTable Bankverbindung;
		private DataTable Kundenadresse;

		#endregion

		#region "Properties"

        public String VKORG { get; set; }
        public String VKBUR { get; set; }

        public DataTable tblEingabeListe { get; set; }
        public DataTable tblFehler { get; set; }
        public DataTable Positionen { get; set; }
        public DataTable BestLieferanten { get; set; }
        public DataTable tblNeueKunden { get; set; }
        public DataTable tblBarcodData { get; set; }
        public DataTable tblBarcodMaterial { get; set; }

        // Kopfdaten
        public Int32 KopfID { get; set; }
        public Int32 SapID { get; set; }
        public Boolean Abgerechnet { get; set; }
        public String Kundenname { get; set; }
        public String Kunnr { get; set; }
        public String Ref1 { get; set; }
        public String Ref2 { get; set; }
        public String KreisKennz { get; set; }
        public String Kreis { get; set; }
        public Boolean WunschKennz { get; set; }
        public Boolean Reserviert { get; set; }
        public String ReserviertKennz { get; set; }
        public Boolean Feinstaub { get; set; }
        public String ZulDate { get; set; }
        public String Kennzeichen { get; set; }
        public String Kennztyp { get; set; }
        public String KennzForm { get; set; }
        public Int32 KennzAnzahl { get; set; }
        public Boolean EinKennz { get; set; }
        public String Bemerkung { get; set; }
        public Boolean EC { get; set; }
        public Boolean Bar { get; set; }
        public Boolean saved { get; set; }
        public Int16 toSave { get; set; }
        public String toDelete { get; set; }
        public Boolean bearbeitet { get; set; }
        public String Barcode { get; set; }
        public String Vorgang { get; set; }
        public String FrachtNrHin { get; set; }
        public String FrachtNrBack { get; set; }
        public Boolean Barkunde { get; set; }
        public Boolean ZusatzKZ { get; set; }
        public string WunschKZ2 { get; set; }
        public string WunschKZ3 { get; set; }
        public bool OhneGruenenVersSchein { get; set; }
        public bool SofortabrechnungErledigt { get; set; }
        public string SofortabrechnungPfad { get; set; }
        public string Briefnr { get; set; }
        public string Orderid { get; set; }
        public string Hppos { get; set; }

        // Adressdaten
        public String Partnerrolle { get; set; }
        public String KundennrWE { get; set; }
        public String Name1 { get; set; }
        public String Name2 { get; set; }
        public String PLZ { get; set; }
        public String Ort { get; set; }
        public String Strasse { get; set; }

        // Bankdaten
        public String BankKey { get; set; }
        public String Kontonr { get; set; }
        public Boolean EinzugErm { get; set; }
        public Boolean Rechnung { get; set; }
        public String Geldinstitut { get; set; }
        public String Inhaber { get; set; }
        public String IBAN { get; set; }
        public String SWIFT { get; set; }

		public String Lieferant_ZLD { get; set; }
        public String IsZLD { get; set; }
        public String NeueKundenNr { get; set; }
        public String NeueKundenName { get; set; }
        public String KennzTeil1 { get; set; }
        public String IDCount { get; set; }
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
        public String Ort2Rueck { get; set; }
        public Boolean ConfirmCPDAdress { get; set; }

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

			SapID = 0;
			if (m_blnGestartet == false)
			{
				m_blnGestartet = true;
				try
				{
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_BELNR", ref m_objApp, ref m_objUser, ref page);

					myProxy.callBapi();

				    Int32 tmpID;
                    if (Int32.TryParse(myProxy.getExportParameter("E_BELN").ToString(), out tmpID))
                    {
                        SapID = tmpID;
                    }
					
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

            SapID = 0;
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
            Positionen = new DataTable();
            Positionen.Columns.Add("id_Kopf", typeof(Int32));
            Positionen.Columns.Add("id_pos", typeof(Int32));
            Positionen.Columns.Add("Menge", typeof(String));
            Positionen.Columns.Add("Matnr", typeof(String));
            Positionen.Columns.Add("Matbez", typeof(String));
            Positionen.Columns.Add("Preis", typeof(String));
            Positionen.Columns.Add("PosLoesch", typeof(String));
            Positionen.Columns.Add("GebMatnr", typeof(String));
            Positionen.Columns.Add("GebMatbez", typeof(String));
            Positionen.Columns.Add("GebMatnrSt", typeof(String));
            Positionen.Columns.Add("GebMatBezSt", typeof(String));
            Positionen.Columns.Add("KennzMat", typeof(String));
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

                if (SapID != 0)
				{
				    var tblKopf = new ZLDKopfTabelle
				    {
                        id_sap = SapID,
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
				        kundenname = Kundenname,
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
                    tblKopf.KreisKZ = KreisKennz;
                    tblKopf.KreisBez = Kreis;
                    tblKopf.WunschKenn = WunschKennz;
                    tblKopf.ZusatzKZ = ZLDCommon.BoolToX(ZusatzKZ);
                    tblKopf.WunschKZ2 = WunschKZ2;
                    tblKopf.WunschKZ3 = WunschKZ3;
                    tblKopf.OhneGruenenVersSchein = ZLDCommon.BoolToX(OhneGruenenVersSchein);
                    tblKopf.SofortabrechnungErledigt = SofortabrechnungErledigt;
				    tblKopf.SofortabrechnungPfad = SofortabrechnungPfad;
                    tblKopf.Briefnr = Briefnr;
                    tblKopf.Orderid = Orderid;
                    tblKopf.Hppos = Hppos;
                    tblKopf.Reserviert = Reserviert;
                    tblKopf.ReserviertKennz = ReserviertKennz;
                    tblKopf.Feinstaub = Feinstaub;
                    DateTime tmpDate;
                    DateTime.TryParse(ZulDate, out tmpDate);
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
					KopfID = tblKopf.id;
					zldDataContext.Connection.Close();

					zldDataContext = new ZLDTableClassesDataContext();
                    if (Positionen.Rows.Count > 0)
					{
                        foreach (DataRow drow in Positionen.Rows)
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
						        id_Kopf = KopfID,
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

				    zldDataContext.Connection.Open();
					zldDataContext.ZLDBankverbindung.InsertOnSubmit(tblBank);
					zldDataContext.SubmitChanges();
					zldDataContext.Connection.Close();

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
                               where k.id == KopfID
							select k).Single();

				tblKopf.id_user = m_objUser.UserID;
				tblKopf.id_session = strSessionID;
				tblKopf.abgerechnet = false;
				tblKopf.username = m_objUser.UserName;
				tblKopf.kundenname = Kundenname;
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
                tblKopf.KreisKZ = KreisKennz;
				tblKopf.KreisBez = Kreis;
				tblKopf.WunschKenn = WunschKennz;
                tblKopf.ZusatzKZ = ZLDCommon.BoolToX(ZusatzKZ);
                tblKopf.WunschKZ2 = WunschKZ2;
                tblKopf.WunschKZ3 = WunschKZ3;
                tblKopf.OhneGruenenVersSchein = ZLDCommon.BoolToX(OhneGruenenVersSchein);
                tblKopf.SofortabrechnungErledigt = SofortabrechnungErledigt;
			    tblKopf.SofortabrechnungPfad = SofortabrechnungPfad;
                tblKopf.Briefnr = Briefnr;
                tblKopf.Orderid = Orderid;
                tblKopf.Hppos = Hppos;
				tblKopf.Reserviert = Reserviert;
				tblKopf.ReserviertKennz = ReserviertKennz;
				tblKopf.Feinstaub = Feinstaub;
				DateTime tmpDate;
				DateTime.TryParse(ZulDate, out tmpDate);
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
						    var idpos = (Int32) drow["id_pos"];

							var tblPos = (from p in ZLD_DataContext.ZLDPositionsTabelle
                                          where p.id_Kopf == KopfID && p.id_pos == idpos
											select p);
							if (tblPos.Any())
							{
								foreach (var PosRow in tblPos)
								{

                                    PosRow.id_Kopf = KopfID;
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
                                        id_Kopf = KopfID,
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
                    else if (tblPosCount.Count() > Positionen.Rows.Count)
					{
						foreach (var PosRow in tblPosCount)
						{
                            DataRow[] drow = Positionen.Select("id_pos = " + PosRow.id_pos);
							if (drow.Length == 1)
							{
                                PosRow.id_Kopf = KopfID;
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

                        foreach (DataRow drow in Positionen.Rows)
						{
						    var idpos = (Int32) drow["id_pos"];

							var tblPos = (from p in ZLD_DataContext.ZLDPositionsTabelle
                                          where p.id_Kopf == KopfID && p.id_pos == idpos
											select p);
							if (tblPos.Any())
							{
								foreach (var PosRow in tblPos)
								{
                                    if (PosRow.id_Kopf == KopfID)
									{
                                        PosRow.id_Kopf = KopfID;
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
                                        id_Kopf = KopfID,
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
                                   where b.id_Kopf == KopfID
								  select b).Single();

                    tblBank.id_Kopf = KopfID;
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

                KopfID = (Int32)KopfTabelle.Rows[0]["id"];
                SapID = (Int32)KopfTabelle.Rows[0]["id_sap"];
				Abgerechnet = (Boolean)KopfTabelle.Rows[0]["abgerechnet"];
				Kundenname = KopfTabelle.Rows[0]["kundenname"].ToString();
				Kunnr = KopfTabelle.Rows[0]["kundennr"].ToString();
				Ref1 = KopfTabelle.Rows[0]["referenz1"].ToString();
				Ref2 = KopfTabelle.Rows[0]["referenz2"].ToString();
                KreisKennz = KopfTabelle.Rows[0]["KreisKZ"].ToString();
				Kreis = KopfTabelle.Rows[0]["KreisBez"].ToString();
				WunschKennz = (Boolean)KopfTabelle.Rows[0]["WunschKenn"];
                ZusatzKZ = ZLDCommon.XToBool(KopfTabelle.Rows[0]["ZusatzKZ"].ToString());
                WunschKZ2 = KopfTabelle.Rows[0]["WunschKZ2"].ToString();
                WunschKZ3 = KopfTabelle.Rows[0]["WunschKZ3"].ToString();
                OhneGruenenVersSchein = ZLDCommon.XToBool(KopfTabelle.Rows[0]["OhneGruenenVersSchein"].ToString());
                SofortabrechnungErledigt = (Boolean)KopfTabelle.Rows[0]["SofortabrechnungErledigt"];
			    SofortabrechnungPfad = KopfTabelle.Rows[0]["SofortabrechnungPfad"].ToString();
                Briefnr = KopfTabelle.Rows[0]["Briefnr"].ToString();
                Orderid = KopfTabelle.Rows[0]["Orderid"].ToString();
                Hppos = KopfTabelle.Rows[0]["Hppos"].ToString();
				Reserviert = (Boolean)KopfTabelle.Rows[0]["Reserviert"];
				ReserviertKennz = KopfTabelle.Rows[0]["ReserviertKennz"].ToString();
				Feinstaub = (Boolean)KopfTabelle.Rows[0]["Feinstaub"];

				ZulDate = KopfTabelle.Rows[0]["Zulassungsdatum"].ToString();
                if (ZLDCommon.IsDate(ZulDate)) { ZulDate = ((DateTime)KopfTabelle.Rows[0]["Zulassungsdatum"]).ToShortDateString(); }

				Kennzeichen = KopfTabelle.Rows[0]["Kennzeichen"].ToString();
				Bemerkung = KopfTabelle.Rows[0]["Bemerkung"].ToString();
				KennzForm = KopfTabelle.Rows[0]["KennzForm"].ToString();
				EinKennz = (Boolean)KopfTabelle.Rows[0]["EinKennz"];
				EC = (Boolean)KopfTabelle.Rows[0]["EC"];
				Bar = (Boolean)KopfTabelle.Rows[0]["Bar"];

				saved = (Boolean)KopfTabelle.Rows[0]["saved"];
				toDelete = KopfTabelle.Rows[0]["toDelete"].ToString();

				bearbeitet = (Boolean)KopfTabelle.Rows[0]["bearbeitet"];
				Vorgang = KopfTabelle.Rows[0]["Vorgang"].ToString();
				Barcode = KopfTabelle.Rows[0]["Barcode"].ToString();

                Positionen = tmpPos;

				Bankverbindung = tmpBank;

				SWIFT = Bankverbindung.Rows[0]["SWIFT"].ToString();
				IBAN = Bankverbindung.Rows[0]["IBAN"].ToString();
                BankKey = Bankverbindung.Rows[0]["BankKey"].ToString();
                Kontonr = Bankverbindung.Rows[0]["Kontonr"].ToString();
				Inhaber = Bankverbindung.Rows[0]["Inhaber"].ToString();
				Geldinstitut = Bankverbindung.Rows[0]["Geldinstitut"].ToString();
				EinzugErm = (Boolean)Bankverbindung.Rows[0]["EinzugErm"];
				Rechnung = (Boolean)Bankverbindung.Rows[0]["Rechnung"];

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
                tblEingabeListe = new DataTable();

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

                    foreach (DataRow SaveRow in tblEingabeListe.Rows)
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
							importRowAuftrag["VKORG"] = VKORG;
							importRowAuftrag["VKBUR"] = VKBUR;
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
                            importRowAuftrag["BRIEFNR"] = tblKopf.Briefnr;
                            importRowAuftrag["ORDERID"] = tblKopf.Orderid;
                            importRowAuftrag["HPPOS"] = tblKopf.Hppos;
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

                        tblFehler = myProxy.getExportTable("GT_EX_ERRORS");

                        if (tblFehler.Rows.Count > 0)
                        {
                            RaiseError("-9999","Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden");

                            foreach (DataRow rowError in tblFehler.Rows)
                            {
                                Int32 idsap;
                                Int32.TryParse(rowError["ZULBELN"].ToString(), out idsap);
                                Int32 id_Pos;
                                Int32.TryParse(rowError["ZULPOSNR"].ToString(), out id_Pos);
                                DataRow[] rowListe = tblEingabeListe.Select("id_sap=" + idsap + " AND id_pos =" + id_Pos);
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
                    if (SapID != 0)
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
                        importRowAuftrag["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
						importRowAuftrag["VBELN"] = "";
						importRowAuftrag["VKORG"] = VKORG;
						importRowAuftrag["VKBUR"] = VKBUR;
						importRowAuftrag["ERNAM"] = m_objUser.UserName;
						importRowAuftrag["ERDAT"] = DateTime.Now;
						importRowAuftrag["FLAG"] = "";
						importRowAuftrag["STATUS"] = "N";
						importRowAuftrag["BARCODE"] = Barcode;
						importRowAuftrag["KUNNR"] = Kunnr.PadLeft(10, '0');
						importRowAuftrag["ZZREFNR1"] = Ref1;
						importRowAuftrag["ZZREFNR2"] = Ref2;
                        importRowAuftrag["KREISKZ"] = KreisKennz;

						DataRow[] tblKundeRow = tblKunde.Select("KUNNR='" + Kunnr + "'");

						if (tblKundeRow.Length == 1)
						{
							importRowAuftrag["KUNDEBAR_JN"] = tblKundeRow[0]["BARKUNDE"].ToString();
						}

                        DataRow[] RowStva = tblStvaStamm.Select("KREISKZ='" + KreisKennz + "'");
						if (RowStva.Length == 1)
						{
							importRowAuftrag["KREISBEZ"] = RowStva[0]["KREISBEZ"];
						}
						else
						{
							importRowAuftrag["KREISBEZ"] = Kreis;
						}
                        importRowAuftrag["WUNSCHKENN_JN"] = ZLDCommon.BoolToX(WunschKennz);

                        importRowAuftrag["RESERVKENN_JN"] = ZLDCommon.BoolToX(Reserviert);
                        importRowAuftrag["ZUSKENNZ"] = ZLDCommon.BoolToX(ZusatzKZ);
                        importRowAuftrag["WU_KENNZ2"] = WunschKZ2;
                        importRowAuftrag["WU_KENNZ3"] = WunschKZ3;
                        importRowAuftrag["O_G_VERSSCHEIN"] = ZLDCommon.BoolToX(OhneGruenenVersSchein);
                        importRowAuftrag["SOFORT_ABR_ERL"] = ZLDCommon.BoolToX(SofortabrechnungErledigt);
                        importRowAuftrag["SA_PFAD"] = SofortabrechnungPfad;
                        importRowAuftrag["BRIEFNR"] = Briefnr;
                        importRowAuftrag["ORDERID"] = Orderid;
                        importRowAuftrag["HPPOS"] = Hppos;
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
                            importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
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
                                    importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
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
                                    importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
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
                                    importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
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
                                importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
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
                        importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
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
                        importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                        importRow["KUNNR"] = Kunnr.PadLeft(10, '0');
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
                            importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                            importRow["KUNNR"] = Kunnr.PadLeft(10, '0');
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
                            importRow["ZULBELN"] = SapID.ToString().PadLeft(10, '0');
                            importRow["KUNNR"] = Kunnr.PadLeft(10, '0');
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

                        tblFehler = myProxy.getExportTable("GT_EX_ERRORS");

                        if (tblFehler.Rows.Count > 0)
						{
                            RaiseError("-9999","Es konnten ein oder mehrere Aufträge nicht in SAP gespeichert werden");

                            foreach (DataRow rowError in tblFehler.Rows)
							{
								Int32 tmpID_sap;
								Int32.TryParse(rowError["ZULBELN"].ToString(), out tmpID_sap);
								Int32 id_Pos;
								Int32.TryParse(rowError["ZULPOSNR"].ToString(), out id_Pos);
                                DataRow[] rowListe = tblEingabeListe.Select("id_sap=" + SapID + " AND id_pos =" + id_Pos);
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
            tblFehler = null;
        }

		#endregion
    }
}
