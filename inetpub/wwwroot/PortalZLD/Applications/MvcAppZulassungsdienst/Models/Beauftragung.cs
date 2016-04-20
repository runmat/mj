using System;
using System.Configuration;
using System.Text;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Security.Cryptography;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using ERPConnect.BW;

namespace MvcAppZulassungsdienst.ViewModels
{
    public class Beauftragung : DatenimportBase
    {

#region Declarations
		private SqlConnection _connection;
		private string _verkaufsorganisation;
		private string _verkaufsbuero;
		private string _gruppe;
		private DataTable _kunden;
		private DataTable _dienstleistungen;
		private DataTable _kreise;
		private string _typdatenMessage;
		private string _kunnr;
		private string _kundenname;
		private string _halterAnrede;
		private string _haltername1;
		private string _haltername2;
		private string _name;
		private string _geburtsname;
		private string _geburtstag;
		private string _geburtsort;
		private string _halterStrasse;
		private string _halterHausnr;
		private string _halterHausnrZusatz;
		private string _halterPLZ;
		private string _halterOrt;
		private string _grosskundennr;
		private string _stva;
		private string _stvaNr;
		private string _referenz;
		private string _verkKuerz;
		private string _kiReferenz;
		private string _notiz;
		private string _hersteller;
		private string _typ;
		private string _varianteVersion;
		private string _typPruef;
		private string _fahrgestellnummer;
		private string _fahrgestellnummerPruef;
		private string _dienstleistungsnr;
		private string _dienstleistung;
		private string _evb;
		private string _zulassungsdatum;
		private string _kennzeichen;
		private string _bemerkung;
		private bool _einzelkennzeichen;
		private bool _krad;
		private string _kennzeichenTyp;
		private bool _feinstaubplakette;
		private bool _wunschkennzeichen;
		private bool _reserviert;
		private string _reservierungsnr;
		private string _statustext;
		private string _materialnummer;
		private string _halterReferenz;
		private string _briefnummer;
		private string _zuldatVon;
		private string _zuldatBis;
		private string _loeschkennzeichen;
		private string _bLz;
		private string _kontonr;
		private string _einzug;
		private string _he;
		private string _fr;
		private string _fi;
		private bool _npaUsed;
		private string _bestellnummer;
		private string _grosskunde;
		private string _errText;
		
		private char _bankdatenNeeded = 'N';
		private char _evBNeeded = 'N';
		private char _gutachtenNeeded;
		private char _naechsteHuNeeded;
		private char _halterNeeded;
		private char _typDatenNeeded;
		private char _altKennzeichenNeeded;
		
		private string _naechsteHu;
		private string _artGenehmigung;
		private string _prueforganisation;
		private string _gutachtenNummer;
		
		private DataTable _genehmigungArten;
		private DataTable _prueforganisationen;
		
		private string _verkaeufer;
		private string _kundenreferenz;
		private string _kundennotiz;
		private string _altKennzeichen;
		private bool _ausdruckNeeded = true;
		
#endregion
		
#region  Properties
		
		public string Verkaeufer
		{
			get
			{
				return _verkaeufer;
			}
			set
			{
				_verkaeufer = value;
			}
		}
		
		private string Kundenreferenz
		{
			get
			{
				return _kundenreferenz;
			}
			set
			{
				_kundenreferenz = value;
			}
		}
		
		private string Kundennotiz
		{
			get
			{
				return _kundennotiz;
			}
			set
			{
				_kundennotiz = value;
			}
		}
		
		public DataTable GenehmigungsArten
		{
			get
			{
				return _genehmigungArten;
			}
		}
		
		public DataTable Prueforganisationen
		{
			get
			{
				return _prueforganisationen;
			}
		}
		
		public char EvBNeeded
		{
			get
			{
				return _evBNeeded;
			}
			set
			{
				_evBNeeded = value;
			}
		}
		public char GutachtenNeeded
		{
			get
			{
				return _gutachtenNeeded;
			}
			set
			{
				_gutachtenNeeded = value;
			}
		}
		public char NaechsteHUNeeded
		{
			get
			{
				return _naechsteHuNeeded;
			}
			set
			{
				_naechsteHuNeeded = value;
			}
		}
		public char BankdatenNeeded
		{
			get
			{
				return _bankdatenNeeded;
			}
			set
			{
				_bankdatenNeeded = value;
			}
		}
		
		public string NaechsteHU
		{
			get
			{
				return _naechsteHu;
			}
			set
			{
				_naechsteHu = value;
			}
		}
		public string ArtGenehmigung
		{
			get
			{
				return _artGenehmigung;
			}
			set
			{
				_artGenehmigung = value;
			}
		}
		public string Prueforganisation
		{
			get
			{
				return _prueforganisation;
			}
			set
			{
				_prueforganisation = value;
			}
		}
		public string GutachtenNummer
		{
			get
			{
				return _gutachtenNummer;
			}
			set
			{
				_gutachtenNummer = value;
			}
		}
		
		public string Verkaufsorganisation
		{
			get
			{
				return _verkaufsorganisation;
			}
			set
			{
				_verkaufsorganisation = value;
			}
		}
		public string Verkaufsbuero
		{
			get
			{
				return _verkaufsbuero;
			}
			set
			{
				_verkaufsbuero = value;
			}
		}
		public string Gruppe
		{
			get
			{
				return _gruppe;
			}
			set
			{
				_gruppe = value;
			}
		}
		
		public string TypdatenMessage
		{
			get
			{
				return _typdatenMessage;
			}
			set
			{
				_typdatenMessage = value;
			}
		}
		
		public DataTable Kunden
		{
			get
			{
				return _kunden;
			}
			set
			{
				_kunden = value;
			}
		}
		
		public DataTable Dienstleistungen
		{
			get
			{
				return _dienstleistungen;
			}
			set
			{
				_dienstleistungen = value;
			}
		}
		
		public DataTable Kreise
		{
			get
			{
				return _kreise;
			}
			set
			{
				_kreise = value;
			}
		}
		
		public string Kundennr
		{
			get
			{
				return _kunnr;
			}
			set
			{
				_kunnr = value;
			}
		}
		
		public string Kundenname
		{
			get
			{
				return _kundenname;
			}
			set
			{
				_kundenname = value;
			}
		}
		
		public string Grosskundennr
		{
			get
			{
				return _grosskundennr;
			}
			set
			{
				_grosskundennr = value;
			}
		}
		
		public string HalterAnrede
		{
			get
			{
				return _halterAnrede;
			}
			set
			{
				_halterAnrede = value;
			}
		}
		
		public string Haltername1
		{
			get
			{
				return _haltername1;
			}
			set
			{
				_haltername1 = value;
			}
		}
		public string Haltername2
		{
			get
			{
				return _haltername2;
			}
			set
			{
				_haltername2 = value;
			}
		}
		
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
		
		public string Geburtsname
		{
			get
			{
				return _geburtsname;
			}
			set
			{
				_geburtsname = value;
			}
		}
		
		public string Geburtstag
		{
			get
			{
				return _geburtstag;
			}
			set
			{
				_geburtstag = value;
			}
		}
		
		public string Geburtsort
		{
			get
			{
				return _geburtsort;
			}
			set
			{
				_geburtsort = value;
			}
		}
		
		public string HalterStrasse
		{
			get
			{
				return _halterStrasse;
			}
			set
			{
				_halterStrasse = value;
			}
		}
		
		public string HalterHausnr
		{
			get
			{
				return _halterHausnr;
			}
			set
			{
				_halterHausnr = value;
			}
		}
		
		public string HalterHausnrZusatz
		{
			get
			{
				return _halterHausnrZusatz;
			}
			set
			{
				_halterHausnrZusatz = value;
			}
		}
		
		public string HalterPLZ
		{
			get
			{
				return _halterPLZ;
			}
			set
			{
				_halterPLZ = value;
			}
		}
		
		public string HalterOrt
		{
			get
			{
				return _halterOrt;
			}
			set
			{
				_halterOrt = value;
			}
		}
		
		public string Referenz
		{
			get
			{
				return _referenz;
			}
			set
			{
				_referenz = value;
			}
		}
		
		public string VerkKuerz
		{
			get
			{
				return _verkKuerz;
			}
			set
			{
				_verkKuerz = value;
			}
		}
		
		public string KiReferenz
		{
			get
			{
				return _kiReferenz;
			}
			set
			{
				_kiReferenz = value;
			}
		}
		
		public string Notiz
		{
			get
			{
				return _notiz;
			}
			set
			{
				_notiz = value;
			}
		}
		
		public string Hersteller
		{
			get
			{
				return _hersteller;
			}
			set
			{
				_hersteller = value;
			}
		}
		
		public string Typ
		{
			get
			{
				return _typ;
			}
			set
			{
				_typ = value;
			}
		}
		
		public string VarianteVersion
		{
			get
			{
				return _varianteVersion;
			}
			set
			{
				_varianteVersion = value;
			}
		}
		
		public string TypPruef
		{
			get
			{
				return _typPruef;
			}
			set
			{
				_typPruef = value;
			}
		}
		
		public string Fahrgestellnummer
		{
			get
			{
				return _fahrgestellnummer;
			}
			set
			{
				_fahrgestellnummer = value;
			}
		}
		
		public string FahrgestellnummerPruef
		{
			get
			{
				return _fahrgestellnummerPruef;
			}
			set
			{
				_fahrgestellnummerPruef = value;
			}
		}
		
		public string StVA
		{
			get
			{
				return _stva;
			}
			set
			{
				_stva = value;
			}
		}
		
		public string StVANr
		{
			get
			{
				return _stvaNr;
			}
			set
			{
				_stvaNr = value;
			}
		}
		
		public string Dienstleistungsnr
		{
			get
			{
				return _dienstleistungsnr;
			}
			set
			{
				_dienstleistungsnr = value;
			}
		}
		
		public string Dienstleistung
		{
			get
			{
				return _dienstleistung;
			}
			set
			{
				_dienstleistung = value;
			}
		}
		
		public string EVB
		{
			get
			{
				return _evb;
			}
			set
			{
				_evb = value;
			}
		}
		
		public string Zulassungsdatum
		{
			get
			{
				return _zulassungsdatum;
			}
			set
			{
				_zulassungsdatum = value;
			}
		}
		
		public string Kennzeichen
		{
			get
			{
				return _kennzeichen;
			}
			set
			{
				_kennzeichen = value;
			}
		}
		
		public string Bemerkung
		{
			get
			{
				return _bemerkung;
			}
			set
			{
				_bemerkung = value;
			}
		}
		
		public bool Einzelkennzeichen
		{
			get
			{
				return _einzelkennzeichen;
			}
			set
			{
				_einzelkennzeichen = value;
			}
		}
		
		public bool Krad
		{
			get
			{
				return _krad;
			}
			set
			{
				_krad = value;
			}
		}
		
		public string KennzeichenTyp
		{
			get
			{
				return _kennzeichenTyp;
			}
			set
			{
				_kennzeichenTyp = value;
			}
		}
		
		public bool Feinstaubplakette
		{
			get
			{
				return _feinstaubplakette;
			}
			set
			{
				_feinstaubplakette = value;
			}
		}
		
		public bool Wunschkennzeichen
		{
			get
			{
				return _wunschkennzeichen;
			}
			set
			{
				_wunschkennzeichen = value;
			}
		}
		
		public bool Reserviert
		{
			get
			{
				return _reserviert;
			}
			set
			{
				_reserviert = value;
			}
		}
		
		public string Reservierungsnr
		{
			get
			{
				return _reservierungsnr;
			}
			set
			{
				_reservierungsnr = value;
			}
		}
		
		public string Statustext
		{
			get
			{
				return _statustext;
			}
			set
			{
				_statustext = value;
			}
		}
		
		public string Materialnummer
		{
			get
			{
				return _materialnummer;
			}
			set
			{
				_materialnummer = value;
			}
		}
		
		public string HalterReferenz
		{
			get
			{
				return _halterReferenz;
			}
			set
			{
				_halterReferenz = value;
			}
		}
		
		public string Bestellnummer
		{
			get
			{
				return _bestellnummer;
			}
			set
			{
				_bestellnummer = value;
			}
		}
		
		public string Briefnummer
		{
			get
			{
				return _briefnummer;
			}
			set
			{
				_briefnummer = value;
			}
		}
		
		public string ZuldatVon
		{
			get
			{
				return _zuldatVon;
			}
			set
			{
				_zuldatVon = value;
			}
		}
		
		public string ZuldatBis
		{
			get
			{
				return _zuldatBis;
			}
			set
			{
				_zuldatBis = value;
			}
		}
		
		public string Loeschkennzeichen
		{
			get
			{
				return _loeschkennzeichen;
			}
			set
			{
				_loeschkennzeichen = value;
			}
		}
		
		public string BLZ
		{
			get
			{
				return _bLz;
			}
			set
			{
				_bLz = value;
			}
		}
		
		public string Kontonummer
		{
			get
			{
				return _kontonr;
			}
			set
			{
				_kontonr = value;
			}
		}
		
		public string Einzug
		{
			get
			{
				return _einzug;
			}
			set
			{
				_einzug = value;
			}
		}
		
		public string He
		{
			get
			{
				return _he;
			}
			set
			{
				_he = value;
			}
		}
		
		public string Fr
		{
			get
			{
				return _fr;
			}
			set
			{
				_fr = value;
			}
		}
		
		public string Fi
		{
			get
			{
				return _fi;
			}
			set
			{
				_fi = value;
			}
		}
		
		public bool NpaUsed
		{
			get
			{
				return _npaUsed;
			}
			set
			{
				_npaUsed = value;
			}
		}
		
		public string Grosskunde
		{
			get
			{
				return _grosskunde;
			}
			set
			{
				_grosskunde = value;
			}
		}
		
		public string ErrorText
		{
			get
			{
				return _errText;
			}
			set
			{
				_errText = value;
			}
		}
		
		public char HalterNeeded
		{
			get
			{
				return _halterNeeded;
			}
			set
			{
				_halterNeeded = value;
			}
		}
		
		public char TypDatenNeeded
		{
			get
			{
				return _typDatenNeeded;
			}
			set
			{
				_typDatenNeeded = value;
			}
		}
		
		public char AltKennzeichenNeeded
		{
			get
			{
				return _altKennzeichenNeeded;
			}
			set
			{
				_altKennzeichenNeeded = value;
			}
		}
		
		public string AltKennzeichen
		{
			get
			{
				return _altKennzeichen;
			}
			set
			{
				_altKennzeichen = value;
			}
		}
		
		public bool AusdruckNeeded
		{
			get
			{
				return _ausdruckNeeded;
			}
			set
			{
				_ausdruckNeeded = value;
			}
		}

#endregion


#region "Methods"
    
        public Beauftragung(CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string appId, string sessionId, string fileName) : base(ref objUser, objApp, fileName)
		{
			m_strAppID = appId;
			m_strSessionID = sessionId;
		}

        public void Fill(string appId, string sessionId, System.Web.UI.Page page)
		{
			m_strClassAndMethod = "Beauftragung.FILL";
			m_strAppID = appId;
			m_strSessionID = sessionId;
			
            if (!m_blnGestartet)
			{
				m_blnGestartet = true;
				
				try
				{
					
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_F_CK_GRUPPENDATEN", ref m_objApp,ref m_objUser,ref page);
					
					
					myProxy.setImportParameter("I_VKORG", _verkaufsorganisation);
					myProxy.setImportParameter("I_VKBUR", _verkaufsbuero);
					myProxy.setImportParameter("I_GRUPPE", _gruppe);
					
					
					myProxy.callBapi();
					
					
					_kunden = myProxy.getExportTable("GT_KUNNR");
					_kreise = myProxy.getExportTable("GT_KREISKZ");
					
					
					foreach (DataRow row in _kunden.Rows)
					{
						row["NAME1"] = row["NAME1"].ToString() + " ~ " + row["KUNNR"].ToString().PadLeft(8,'0');

						if (row["DATLT"].ToString() != string.Empty)
						{
							row["NAME1"] = row["NAME1"].ToString() + " / " + row["DATLT"].ToString();
						}
					}
					
					_kunden.AcceptChanges();
					
					//StVA füllen
					foreach (DataRow row in _kreise.Rows)
					{
						row["KREISBEZ"] = row["KREISKZ"].ToString().PadRight(4, '.') + row["KREISBEZ"].ToString();
					}
					_kreise.AcceptChanges();
					
				}
				catch (Exception ex)
				{
					m_intStatus = -9999;
					//ToDo ErrMessage
					switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
					{
						default: m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
                            break;
					}
				}
				finally
				{
					m_blnGestartet = false;
				}
			}
		}
        
        public void FillUebersicht2(string appId, string sessionId, System.Web.UI.Page page)
		{
			
			m_strClassAndMethod = "Beauftragung.FillUebersicht";
			m_strAppID = appId;
			m_strSessionID = sessionId;

			if (!m_blnGestartet)
			{
				m_blnGestartet = true;
				
				try
				{
					
					
					var myProxy = DynSapProxy.getProxy("Z_ZLD_ZULASSUNGSDATEN_ONL",ref m_objApp,ref m_objUser,ref page);
					
					
					myProxy.setImportParameter("KUNNR", Kundennr.PadLeft(10, '0'));
					myProxy.setImportParameter("ZZREFNR", Referenz);
					myProxy.setImportParameter("ZZKENN", Kennzeichen);
					myProxy.setImportParameter("ZZZLDAT_VON", ZuldatVon);
					myProxy.setImportParameter("ZZZLDAT_BIS", ZuldatBis);
					myProxy.setImportParameter("ZZLOESCH", Loeschkennzeichen);
					myProxy.setImportParameter("ZVERKAEUFER", Verkaeufer);
					myProxy.setImportParameter("ZKUNDREF", Kundenreferenz);
					myProxy.setImportParameter("ZKUNDNOTIZ", Kundennotiz);
					
					myProxy.callBapi();
					
					var tempTable = myProxy.getExportTable("EXTAB");
					
					tempTable.Columns["ZZSTATUSUHRZEIT"].MaxLength = 8;
					tempTable.AcceptChanges();
					
					foreach (DataRow dr in tempTable.Rows)
					{
						if (dr["ZZSTATUSDATUM"].ToString().Length == 0)
						{
							dr["ZZSTATUSUHRZEIT"] = "";
						}
						
						tempTable.AcceptChanges();
					}
					
					CreateOutPut(tempTable, appId);
					
				}
				catch (Exception ex)
				{
				    m_intStatus = -9999;
					//ToDo ErrMessage
				    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
				    {
				        case "NO_DATA":
				            m_strMessage = "Es konnte keine Daten ermittelt werden.";
				            break;
				        case "NO_INTERVAL":
				            m_strMessage = "Ungültiger Zeitraum.";
				            break;
				        default:
				            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
				            break;
				    }
				}
				finally
				{
					m_blnGestartet = false;
				}
			}
		}

      public void FillUebersicht(string appId, string sessionId, System.Web.UI.Page page)
		{
			
			m_strClassAndMethod = "Beauftragung.FillUebersicht";
			m_strAppID = appId;
			m_strSessionID = sessionId;
			
          if (!m_blnGestartet)
			{
				m_blnGestartet = true;
				
				try
				{
					
					
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_ZULASSUNGSDATEN_EKFZ", ref m_objApp,ref m_objUser,ref page);
					
					
					myProxy.setImportParameter("KUNNR", Kundennr.PadLeft(10, '0'));
					myProxy.setImportParameter("ZZREFNR", Referenz);
					myProxy.setImportParameter("ZZKENN", Kennzeichen);
					myProxy.setImportParameter("ZZZLDAT_VON", ZuldatVon);
					myProxy.setImportParameter("ZZZLDAT_BIS", ZuldatBis);
					myProxy.setImportParameter("ZZLOESCH", Loeschkennzeichen);
					myProxy.setImportParameter("ZVERKAEUFER", Verkaeufer);
					myProxy.setImportParameter("ZKUNDREF", Kundenreferenz);
					myProxy.setImportParameter("ZKUNDNOTIZ", Kundennotiz);
					
					myProxy.callBapi();
					
					DataTable tempTable = myProxy.getExportTable("EXTAB");
					
					tempTable.Columns["ZZSTATUSUHRZEIT"].MaxLength = 8;
					tempTable.AcceptChanges();
					
					foreach (DataRow dr in tempTable.Rows)
					{
						
						if (dr["ZZSTATUSDATUM"].ToString().Length == 0)
						{
							dr["ZZSTATUSUHRZEIT"] = "";
						}
						
						tempTable.AcceptChanges();
					}
					
					CreateOutPut(tempTable, appId);
					
				}
				catch (Exception ex)
				{
				    m_intStatus = -9999;
					//ToDo ErrMessage
				    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
				    {
				        case "NO_DATA":
				            m_strMessage = "Es konnte keine Daten ermittelt werden.";
				            break;
				        case "NO_INTERVAL":
				            m_strMessage = "Ungültiger Zeitraum.";
				            break;
				        default:
				            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
				            break;
				    }
				}
				finally
				{
					m_blnGestartet = false;
				}
			}
		}

      
      public void FillZulassung(string appId, string sessionId, System.Web.UI.Page page, DateTime datu_ab, DateTime datu_bis)
		{
			
			m_strClassAndMethod = "Beauftragung.FillZulassung";
			m_strAppID = appId;
			m_strSessionID = sessionId;
			m_intStatus = 0;
			m_strMessage = "";
			
          if (!m_blnGestartet)
			{
				m_blnGestartet = true;
				
				try
				{
					
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_ONLSTAT_001",ref m_objApp,ref m_objUser,ref page);
					
					
					myProxy.setImportParameter("I_KREIS", "B");
					myProxy.setImportParameter("I_VKORG", "1010");
					myProxy.setImportParameter("I_VKBUR", "4837");
					
					myProxy.setImportParameter("I_VON", datu_ab.ToString("yyyyMMdd"));
					myProxy.setImportParameter("I_BIS", datu_bis.ToString("yyyyMMdd"));
					
					myProxy.callBapi();
					
					
					
					DataTable tblTemp2 = myProxy.getExportTable("GT_ONL_ZUL");
					
					foreach (DataRow dr in tblTemp2.Rows)
					{
						
						if (dr["ZZSTATUSUHRZEIT"].ToString() == "000000")
						{
							dr["ZZSTATUSUHRZEIT"] = "";
						}
						else
						{
							dr["ZZSTATUSUHRZEIT"] = dr["ZZSTATUSUHRZEIT"].ToString().Substring(0,2) + ":" + dr["ZZSTATUSUHRZEIT"].ToString().Substring(3,2);
						}
						
					}
					
					
					ResultTable = tblTemp2;
					
				}
				catch (Exception ex)
				{
					m_intStatus = -5555;
					if (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) == "NO_DATA")
					{
						m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden.";
					}
					else
					{
						m_intStatus = -9999;
						m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
					}
				}
				finally
				{
					m_blnGestartet = false;
				}
			}
		}


       public bool Save(string strAppId, string strSessionId, System.Web.UI.Page page)
		{
			
			m_strClassAndMethod = "Beauftragung.Save";
			m_strAppID = strAppId;
			m_strSessionID = strSessionId;
			m_intStatus = 0;
			m_strMessage = "";
			
			var success = false;
			var encryptData = "";
			
			
			if (!m_blnGestartet)
			{
				m_blnGestartet = true;
				
				try
				{
					
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_VORERFASSUNG_EKFZ",ref m_objApp,ref m_objUser,ref page);
					
					DataTable belegTable = myProxy.getImportTable("IT_BELEG");
					DataRow belegRow = belegTable.NewRow();
					
					belegRow["VKORG"] = m_objUser.Reference.Substring(0, 4);
					belegRow["VKBUR"] = m_objUser.Reference.Substring(4, 4);
					belegRow["KUNNR"] = Kundennr.PadLeft(10, '0');
					belegRow["ZZKENN"] = Kennzeichen;
					
					if (Zulassungsdatum == null)
					{
						belegRow["ZZZLDAT"] = string.Empty;
					}
					else
					{
						belegRow["ZZZLDAT"] = Zulassungsdatum;
					}
					
					belegRow["USERID"] = m_objUser.UserID.ToString();
					belegRow["USERNAME"] = m_objUser.UserName;
					belegRow["RESERVIERT"] = Reserviert ? "X" : "";
					belegRow["RESERVID"] = Reservierungsnr;
					belegRow["KENNZTYP"] = KennzeichenTyp;
					belegRow["EINKZ"] = Einzelkennzeichen ? "X" : "";
					belegRow["ZZWUNSCH"] = Wunschkennzeichen ? "X" : "";
					belegRow["ZKUNDREF"] = KiReferenz;
					belegRow["ZKUNDNOTIZ"] = Notiz;
					belegRow["ZFEINSTAUB_KZ"] = Feinstaubplakette ? "X" : "";
					belegRow["ZKRAD_KZ"] = Krad ? "X" : "";
					belegRow["ZZFAHRG"] = Bestellnummer;
					belegRow["KREIS"] = StVANr;
					belegRow["MATNR"] = Materialnummer;
					belegRow["ZZHALTER"] = HalterReferenz;
					belegRow["ZVERKAEUFER"] = VerkKuerz;
					belegRow["ZZTEXT"] = Bemerkung;
					belegRow["BANKL"] = BLZ;
					belegRow["BANKN"] = Kontonummer;
					belegRow["XEZER"] = Einzug;
					
					
					int sapId;
					string returnCode = "";
					
					sapId = GiveNewZulassungsID(ref returnCode);
					
					if (returnCode.Length > 0)
					{
						return success;
					}
					
					SetNewZulassungsID(sapId);
					
					belegRow["ID"] = sapId.ToString();
					
					belegTable.Rows.Add(belegRow);
					belegTable.AcceptChanges();
					
					DataTable vorerfassungTable = myProxy.getImportTable("I_VORERFASSUNG_EKFZ_01");
					DataRow vorerfassungRow = vorerfassungTable.NewRow();
					
					vorerfassungRow["ID"] = sapId.ToString();
				    vorerfassungRow["VORG"] = Dienstleistungen.Select("MATNR='" + Materialnummer + "'")[0]["VORG"];
					vorerfassungRow["AKZ"] = Kennzeichen;
					vorerfassungRow["FGNU"] = Fahrgestellnummer;
					vorerfassungRow["ANR"] = HalterAnrede;
					vorerfassungRow["VNAM"] = Haltername1;
					vorerfassungRow["RNAM"] = Haltername2;
					vorerfassungRow["STRN"] = HalterStrasse;
					vorerfassungRow["STRH"] = HalterHausnr;
					vorerfassungRow["STRB"] = HalterHausnrZusatz;
					vorerfassungRow["PLZ"] = HalterPLZ;
					vorerfassungRow["ORT"] = HalterOrt;
					
					if (Zulassungsdatum == null)
					{
						vorerfassungRow["ZUDA"] = System.DBNull.Value;
					}
					else
					{
						vorerfassungRow["ZUDA"] = Zulassungsdatum;
					}
					
					vorerfassungRow["HERS"] = Hersteller;
					vorerfassungRow["TYP"] = Typ;
					vorerfassungRow["VVS"] = VarianteVersion;
					vorerfassungRow["TYPZ"] = TypPruef;
					vorerfassungRow["FGPZ"] = FahrgestellnummerPruef;
					vorerfassungRow["ZZREFERENZCODE"] = Referenz;
					vorerfassungRow["ZZGROSSKUNDNR"] = Grosskundennr;
					vorerfassungRow["ZZEVB"] = EVB;
					vorerfassungRow["BRNR"] = Briefnummer;
					vorerfassungRow["BLZ"] = BLZ;
					vorerfassungRow["KONTO"] = Kontonummer;
					
					if (_naechsteHuNeeded == 'H' || _naechsteHuNeeded == 'G' || _naechsteHuNeeded == 'B')
					{
						vorerfassungRow["NAHU"] = NaechsteHU;
					}
					else
					{
						vorerfassungRow["NAHU"] = System.DBNull.Value;
					}
					
					vorerfassungRow["ARTGE"] = ArtGenehmigung;
					vorerfassungRow["PRUEFORG"] = Prueforganisation;
					vorerfassungRow["NRGUT"] = GutachtenNummer;
					
					if (Geburtstag == null)
					{
						vorerfassungRow["GEBDAT"] = System.DBNull.Value;
					}
					else
					{
						vorerfassungRow["GEBDAT"] = Geburtstag;
					}
					
					vorerfassungRow["GEBORT"] = Geburtsort;
					
					if (_altKennzeichenNeeded == 'P' || _altKennzeichenNeeded == 'O')
					{
						vorerfassungRow["ALT_AKZ"] = AltKennzeichen;
					}
					
					vorerfassungTable.Rows.Add(vorerfassungRow);
					vorerfassungTable.AcceptChanges();
					
					
					//Importparameter
					
					if (NpaUsed)
					{

					    string stringToEncrypt;
						
						stringToEncrypt = Haltername1 + "|";
						stringToEncrypt += Haltername2 + "|";
						stringToEncrypt += HalterStrasse + "|";
						stringToEncrypt += HalterPLZ + "|";
						stringToEncrypt += HalterOrt + "|";
						stringToEncrypt += Geburtstag + "|";
						stringToEncrypt += Geburtsort;
						
						encryptData = EncrData(stringToEncrypt);
						
					}
					
					
					myProxy.setImportParameter("I_STRING", encryptData);
					
					
					myProxy.callBapi();
					
					DataTable errorTable = myProxy.getExportTable("ET_FEHLER");
					
					if (errorTable.Rows.Count > 0)
					{
						ErrorText = errorTable.Rows[0]["FEHLERTEXT"].ToString();
					}
					else
					{
						ErrorText = encryptData;
						success = true;
					}
					
					
				}
				catch (Exception ex)
				{
					m_intStatus = -5555;
					success = false;
					ErrorText = ex.Message;
					if (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) == "NO_DATA")
					{
						m_strMessage = "Keine Daten.";
					}
					else
					{
						m_intStatus = -9999;
						m_strMessage = "Beim Speichern.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
					}
				}
				finally
				{
					m_blnGestartet = false;
				}
			}
			
			return success;
		}

      public bool Save2(string strAppId, string strSessionId, System.Web.UI.Page page)
		{
			
			m_strClassAndMethod = "Beauftragung.Save2";
			m_strAppID = strAppId;
			m_strSessionID = strSessionId;
			m_intStatus = 0;
			m_strMessage = "";
			
			bool success = false;
			string encryptData = "";
			
			
			if (!m_blnGestartet)
			{
				m_blnGestartet = true;
				
				try
				{
					
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_IMPORT_VORERFASSUNG_EKFZ",ref m_objApp,ref m_objUser,ref page);
					
					DataTable belegTable = myProxy.getImportTable("IT_BELEG");
					DataRow belegRow = belegTable.NewRow();
					
					belegRow["VKORG"] = m_objUser.Reference.Substring(0, 4);
					belegRow["VKBUR"] = m_objUser.Reference.Substring(4, 4);
					belegRow["KUNNR"] = Kundennr.PadLeft(10, '0');
					belegRow["ZZKENN"] = Kennzeichen;
					
					if (Zulassungsdatum == null)
					{
						belegRow["ZZZLDAT"] = string.Empty;
					}
					else
					{
						belegRow["ZZZLDAT"] = Zulassungsdatum;
					}
					
					belegRow["USERID"] = m_objUser.UserID.ToString();
					belegRow["USERNAME"] = m_objUser.UserName;
					belegRow["RESERVIERT"] = Reserviert ? "X" : "";
					belegRow["RESERVID"] = Reservierungsnr;
					belegRow["KENNZTYP"] = KennzeichenTyp;
					belegRow["EINKZ"] = Einzelkennzeichen ? "X" : "";
					belegRow["ZZWUNSCH"] = Wunschkennzeichen ? "X" : "";
					belegRow["ZKUNDREF"] = KiReferenz;
					belegRow["ZKUNDNOTIZ"] = Notiz;
					belegRow["ZFEINSTAUB_KZ"] = Feinstaubplakette ? "X" : "";
					belegRow["ZKRAD_KZ"] = Krad ? "X" : "";
					belegRow["ZZFAHRG"] = Bestellnummer;
					belegRow["KREIS"] = StVANr;
					belegRow["MATNR"] = Materialnummer;
					belegRow["ZZHALTER"] = HalterReferenz;
					belegRow["ZVERKAEUFER"] = VerkKuerz;
					belegRow["ZZTEXT"] = Bemerkung;
					belegRow["BANKL"] = BLZ;
					belegRow["BANKN"] = Kontonummer;
					belegRow["XEZER"] = Einzug;
					belegRow["FGNU"] = Fahrgestellnummer;
					belegRow["ANR"] = HalterAnrede;
					belegRow["VNAM"] = Haltername1;
					belegRow["RNAM"] = Haltername2;
					belegRow["STRN"] = HalterStrasse;
					belegRow["STRH"] = HalterHausnr;
					belegRow["STRB"] = HalterHausnrZusatz;
					belegRow["PLZ"] = HalterPLZ;
					belegRow["ORT"] = HalterOrt;
					
					if (Zulassungsdatum == null)
					{
						belegRow["ZUDA"] = System.DBNull.Value;
					}
					else
					{
						belegRow["ZUDA"] = Zulassungsdatum;
					}
					
					belegRow["HERS"] = Hersteller;
					belegRow["TYP"] = Typ;
					belegRow["VVS"] = VarianteVersion;
					belegRow["TYPZ"] = TypPruef;
					belegRow["FGPZ"] = FahrgestellnummerPruef;
					belegRow["ZZREFERENZCODE"] = Referenz;
					belegRow["ZZGROSSKUNDNR"] = Grosskundennr;
					belegRow["ZZEVB"] = EVB;
					belegRow["BRNR"] = Briefnummer;
					belegRow["BLZ"] = BLZ;
					belegRow["KONTO"] = Kontonummer;
					
					if (_naechsteHuNeeded == 'H' || _naechsteHuNeeded == 'G' || _naechsteHuNeeded == 'B')
					{
						belegRow["NAHU"] = NaechsteHU;
					}
					else
					{
						belegRow["NAHU"] = System.DBNull.Value;
					}
					
					belegRow["ARTGE"] = ArtGenehmigung;
					belegRow["PRUEFORG"] = Prueforganisation;
					belegRow["NRGUT"] = GutachtenNummer;
					
					if (Geburtstag == null)
					{
						belegRow["GEBDAT"] = System.DBNull.Value;
					}
					else if (Geburtstag == string.Empty)
					{
						belegRow["GEBDAT"] = System.DBNull.Value;
					}
					else
					{
						belegRow["GEBDAT"] = Geburtstag;
					}
					
					belegRow["GEBORT"] = Geburtsort;
					
					if (_altKennzeichenNeeded == 'P' || _altKennzeichenNeeded == 'O')
					{
						belegRow["ALT_AKZ"] = AltKennzeichen;
					}
					
					belegTable.Rows.Add(belegRow);
					belegTable.AcceptChanges();
					
					//Importparameter
					
					if (NpaUsed)
					{

					    string stringToEncrypt;
						
						stringToEncrypt = Haltername1 + "|";
						stringToEncrypt += Haltername2 + "|";
						stringToEncrypt += HalterStrasse + "|";
						stringToEncrypt += HalterPLZ + "|";
						stringToEncrypt += HalterOrt + "|";
						stringToEncrypt += Geburtstag + "|";
						stringToEncrypt += Geburtsort;
						
						encryptData = EncrData(stringToEncrypt);
						
					}
					
					myProxy.setImportParameter("I_STRING", encryptData);
					
					myProxy.callBapi();
					
					DataTable errorTable = myProxy.getExportTable("ET_FEHLER");
					
					if (errorTable.Rows.Count > 0)
					{
					    ErrorText = errorTable.Rows[0]["FEHLERTEXT"].ToString();
					}
					else
					{
						ErrorText = encryptData;
						success = true;
					}
					
					
				}
				catch (Exception ex)
				{
					m_intStatus = -5555;
					success = false;
					ErrorText =  ex.Message;
					if (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) == "NO_DATA")
					{
						m_strMessage = "Keine Daten.";
					}
					else
					{
						m_intStatus = -9999;
						m_strMessage = "Beim Speichern.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
					}
				}
				finally
				{
					m_blnGestartet = false;
				}
			}
			
			return success;
		}

        public int CheckVin(string fahrgestellnummer, string pruefziffer, System.Web.UI.Page page)
		{
			m_strClassAndMethod = "Beauftragung.CheckVin";
			m_strAppID = AppID;
			m_strSessionID = SessionID;

			if (!m_blnGestartet)
			{
				m_blnGestartet = true;
				
				try
				{
					
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_PRUEF_FIN_001",ref m_objApp,ref m_objUser,ref page);
					
					myProxy.setImportParameter("I_FGNU", fahrgestellnummer);
					myProxy.setImportParameter("I_FGPZ", pruefziffer);
					
					myProxy.callBapi();
					
					return System.Convert.ToInt32(myProxy.getExportParameter("E_STATUS"));
					
				}
				catch (Exception ex)
				{
					m_intStatus = -9999;
					//ToDo ErrMessage
					
					{
						m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
					}
				}
				finally
				{
					m_blnGestartet = false;
				}
			}
			return 0;
		}

       public DataTable FillTypdaten(string hersteller, string typschluessel, string vvs, string pruefziffer, System.Web.UI.Page page)
		{
			m_strClassAndMethod = "Beauftragung.FillTypdaten";
			m_strAppID = AppID;
			m_strSessionID = SessionID;
			
			var tempTable = new DataTable();
			
			if (!m_blnGestartet)
			{
				m_blnGestartet = true;
				
				try
				{
					
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_TYPDATEN_001",ref m_objApp,ref m_objUser,ref page);
					
					myProxy.setImportParameter("I_ZZHERSTELLER_SCH", hersteller);
					myProxy.setImportParameter("I_ZZTYP_SCHL", typschluessel);
					myProxy.setImportParameter("I_ZZVVS_SCHLUESSEL", vvs);
					myProxy.setImportParameter("I_ZZTYP_VVS_PRUEF", pruefziffer);
					
					
					myProxy.callBapi();
					
					_typdatenMessage = myProxy.getExportParameter("E_MESSAGE");
					
					tempTable = myProxy.getExportTable("GT_WEB");
					
					
				}
				catch (Exception ex)
				{
					m_intStatus = -9999;
					//ToDo ErrMessage
					
					{
						m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
					}
				}
				finally
				{
					m_blnGestartet = false;
				}
			}
			
			return tempTable;
			
		}

        public string CheckGrosskundennummer(string kba, string grosskundennummer, System.Web.UI.Page page)
		{
			m_strClassAndMethod = "Beauftragung.CheckGrosskundennummer";
			m_strAppID = AppID;
			m_strSessionID = SessionID;
			string returnValue = "";
			
			if (!m_blnGestartet)
			{
				m_blnGestartet = true;
				
				try
				{
					
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_GROSSKUNDEN_PRUEF",ref m_objApp,ref m_objUser,ref page);
					
					myProxy.setImportParameter("I_ZKBA1", kba);
					myProxy.setImportParameter("I_ZZGROSSKUNDNR", grosskundennummer);
					myProxy.setImportParameter("I_ZKUNNR", Kundennr.PadLeft(10, '0'));
					
					
					
					myProxy.callBapi();
					
					DataTable tempTable = myProxy.getExportTable("GT_OUT");
					
					if (tempTable.Rows.Count > 0)
					{
						returnValue = tempTable.Rows[0]["ZNAME1"].ToString();
					}
					
				}
				catch (Exception ex)
				{
					m_intStatus = -9999;
					//ToDo ErrMessage
					
					{
						m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
					}
					
				}
				finally
				{
					m_blnGestartet = false;
				}
			}
			
			return returnValue;
			
		}

       public string CheckBarcode(string barcode, System.Web.UI.Page page)
		{
			m_strClassAndMethod = "Beauftragung.CheckBarcode";
			m_strAppID = AppID;
			m_strSessionID = SessionID;
			string returnValue = "";
			
			if (!m_blnGestartet)
			{
				m_blnGestartet = true;
				
				try
				{
					
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_STATUS_REFCODE", ref m_objApp,ref m_objUser,ref page);
					
					myProxy.setImportParameter("I_ZZREFERENZCODE", barcode);
					
					myProxy.callBapi();
					
					returnValue = myProxy.getExportParameter("E_RETURNCODE");
					_statustext = myProxy.getExportParameter("E_ZZSTATUS");
					
				}
				catch (Exception ex)
				{
					m_intStatus = -9999;
					//ToDo ErrMessage
					
					{
						m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
					}
					
				}
				finally
				{
					m_blnGestartet = false;
				}
			}
			
			return returnValue;
			
		}


       private int GiveNewZulassungsID(ref string returnStatus)
       {
            returnStatus = "";

			try
			{
				OpenConnection();
				//Save Data
				return DBGiveNewZulassungsID();
			}
			catch (Exception ex)
			{
				returnStatus = ex.Message;
			}
			finally
			{
				CloseConnection();
			}
			return 0;
		}

       private int DBGiveNewZulassungsID()
		{
			var command = new SqlCommand();
			command.Connection = _connection;
			command.CommandType = CommandType.Text;
			command.CommandText = "SELECT PValue FROM Parameters WHERE  (PName = 'HoechsteZulassungsID')";
			return System.Convert.ToInt32( command.ExecuteScalar()) + 1;
       }

       public int SetNewZulassungsID(int idSap)
       {
           var command2 = new SqlCommand();
           OpenConnection();
           command2.Connection = _connection;
           command2.CommandType = CommandType.Text;
           command2.Parameters.Clear();
           command2.CommandText = "SELECT PValue FROM Parameters WHERE  (PName = 'HoechsteZulassungsID')";
           if (idSap > ((System.Int32)command2.ExecuteScalar()))
           {
               command2.CommandText = "UPDATE Parameters SET PValue = " + idSap.ToString() + " WHERE  (PName = 'HoechsteZulassungsID')";
               command2.ExecuteNonQuery();
           }
           CloseConnection();
           return idSap;
       }

       private void OpenConnection()
       {
           _connection = new SqlConnection();
           _connection.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"];
           _connection.Open();
       }

       private void CloseConnection()
       {
           _connection.Close();
           _connection.Dispose();
       }

       public void FillDienstleistungen(string amt, string vkorg, System.Web.UI.Page page)
       {

           _stva = amt;
           _verkaufsorganisation = vkorg;

           if (_verkaufsorganisation != null)
           {
               m_strClassAndMethod = "Beauftragung.FillDienstleistungen";
               m_strAppID = AppID;
               m_strSessionID = SessionID;

               if (!m_blnGestartet)
               {
                   m_blnGestartet = true;
                 
                   try
                   {

                       DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_ONLMAT",ref m_objApp,ref m_objUser,ref page);

                       myProxy.setImportParameter("I_AMT", _stva);
                       myProxy.setImportParameter("I_VKORG", _verkaufsorganisation);

                       myProxy.callBapi();

                       _typdatenMessage = myProxy.getExportParameter("E_SUBRC");
                       _typdatenMessage = myProxy.getExportParameter("E_MESSAGE");


                       _dienstleistungen = myProxy.getExportTable("GT_ONLMAT");



                       if (_dienstleistungen.Rows.Count > 0)
                       {
                           var tblTemp = _dienstleistungen.Clone();
                           var newRow = tblTemp.NewRow();

                           newRow["MATNR"] = "";
                           newRow["MAKTX"] = "--- Auswahl ---";

                           tblTemp.Rows.Add(newRow);

                           foreach (DataRow dr in _dienstleistungen.Rows)
                           {

                               newRow = tblTemp.NewRow();

                               for (int i = 0; i <= dr.Table.Columns.Count - 1; i++)
                               {

                                   newRow[i] = dr[i];

                               }

                               tblTemp.Rows.Add(newRow);
                           }

                           _dienstleistungen = tblTemp;

                       }

                   }
                   catch (Exception ex)
                   {
                       m_intStatus = -9999;
                       //ToDo ErrMessage

                       {
                           m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
                       }
                   }
                   finally
                   {
                       m_blnGestartet = false;
                   }
               }
           }
       }

       public void FillPrueforganisation(System.Web.UI.Page page)
       {

           m_strClassAndMethod = "Beauftragung.FillPrueforganisation";
           m_strAppID = AppID;
           m_strSessionID = SessionID;

           if (!m_blnGestartet)
           {
               m_blnGestartet = true;
              
               try
               {

                   DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_DOMAENEN_WERTE",ref m_objApp,ref m_objUser,ref page);

                   myProxy.setImportParameter("I_DOMNAME", "ZZLD_PRUEFORG");

                   myProxy.callBapi();

                   _typdatenMessage = myProxy.getExportParameter("E_SUBRC");
                   _typdatenMessage = myProxy.getExportParameter("E_MESSAGE");

                   _prueforganisationen = myProxy.getExportTable("GT_WERTE");

                   if (_prueforganisationen.Rows.Count > 0)
                   {
                       var tblTemp = _prueforganisationen.Clone();
                       var newRow = tblTemp.NewRow();

                       newRow["DOMVALUE_L"] = "";
                       newRow["DDTEXT"] = "--- Auswahl ---";

                       tblTemp.Rows.Add(newRow);

                       foreach (DataRow dr in _prueforganisationen.Rows)
                       {

                           newRow = tblTemp.NewRow();

                           for (int i = 0; i <= dr.Table.Columns.Count - 1; i++)
                           {
                               newRow[i] = dr[i];
                           }

                           tblTemp.Rows.Add(newRow);

                       }

                       _prueforganisationen = tblTemp;

                   }

               }
               catch (Exception ex)
               {
                   m_intStatus = -9999;
                   //ToDo ErrMessage

                   {
                       m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
                   }
               }
               finally
               {
                   m_blnGestartet = false;
               }
           }

       }

       public void FillArtGenehmigung(System.Web.UI.Page page)
       {

           m_strClassAndMethod = "Beauftragung.FillArtGenehmigung";
           m_strAppID = AppID;
           m_strSessionID = SessionID;

         
           if (!m_blnGestartet)
           {
               m_blnGestartet = true;
               
               try
               {

                   DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_DOMAENEN_WERTE",ref m_objApp,ref m_objUser,ref page);

                   myProxy.setImportParameter("I_DOMNAME", "ZZLD_ARTGE");

                   myProxy.callBapi();

                   _typdatenMessage = myProxy.getExportParameter("E_SUBRC");
                   _typdatenMessage = myProxy.getExportParameter("E_MESSAGE");

                   _genehmigungArten = myProxy.getExportTable("GT_WERTE");

                   if (_genehmigungArten.Rows.Count > 0)
                   {
                       var tblTemp = _genehmigungArten.Clone();
                       var newRow = tblTemp.NewRow();

                       newRow["DOMVALUE_L"] = "";
                       newRow["DDTEXT"] = "--- Auswahl ---";

                       tblTemp.Rows.Add(newRow);

                       foreach (DataRow dr in _genehmigungArten.Rows)
                       {

                           newRow = tblTemp.NewRow();

                           for (int i = 0; i <= dr.Table.Columns.Count - 1; i++)
                           {
                               newRow[i] = dr[i];
                           }

                           tblTemp.Rows.Add(newRow);

                       }

                       _genehmigungArten = tblTemp;

                   }

               }
               catch (Exception ex)
               {
                   m_intStatus = -9999;
                   //ToDo ErrMessage

                   {
                       m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + ex.Message + ")";
                   }
               }
               finally
               {
                   m_blnGestartet = false;
               }
           }

       }

       private string EncrData(string textToEncrypt)
       {

           string encText ;
           var rd = new RijndaelManaged();

           var md5 = new MD5CryptoServiceProvider();
           byte[] key = md5.ComputeHash(Encoding.UTF8.GetBytes("@S33wolf"));

           md5.Clear();
           rd.Key = key;
           rd.GenerateIV();

           byte[] iv = rd.IV;
           var ms = new MemoryStream();

           ms.Write(iv, 0, iv.Length);

           var cs = new CryptoStream(ms, rd.CreateEncryptor(), CryptoStreamMode.Write);
           byte[] data = Encoding.UTF8.GetBytes(textToEncrypt);

           cs.Write(data, 0, data.Length);
           cs.FlushFinalBlock();

           byte[] encdata = ms.ToArray();
           encText = Convert.ToBase64String(encdata);
           cs.Close();
           rd.Clear();

           return encText;


       }


#endregion
    }
}