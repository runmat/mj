using System;
using CKG.Base.Common;
using System.Data;
using CKG.Base.Business;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Klasse für die Versandzulassungen.
    /// </summary>
	public class VorVersand : CKG.Base.Business.BankBase
	{

		#region "Properties"

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
        /// Selektionsparameter Zulassungsdatum von
        /// </summary>
		public String SelDatum
		{
			get;
			set;
		}
        /// <summary>
        /// Selektionsparameter Zulassungsdatum bis
        /// </summary>
		public String SelDatumBis
		{
			get;
			set;
		}
        /// <summary>
        /// Selektionsparameter SAPID(ZULBELN)
        /// </summary>
		public String SelID
		{
			get;
			set;
		}
        /// <summary>
        /// Selektionsparameter Kundennummer
        /// </summary>
		public String SelKunde
		{
			get;
			set;
		}
        /// <summary>
        /// Selektionsparameter Kreiskennzeichen
        /// </summary>
		public String SelKreis
		{
			get;
			set;
		}
        /// <summary>
        /// Selektionsparameter Lieferant/ausführende Filiale
        /// </summary>
		public String SelLief
		{
			get;
			set;
		}
        /// <summary>
        /// Selektionsparameter Status Versanzulassung
        /// </summary>
		public String SelStatus
		{
			get;
			set;
		}
        /// <summary>
        /// Rückgabetabelle Filialen/Dienstleister, die 
        /// für selektierten Kreis "zuständig" sind
        /// </summary>
		public DataTable BestLieferanten
		{
			get;
			set;
		}
        /// <summary>
        /// Rückgabetabelle der selektierten Versandzulassungen
        /// </summary>
		public DataTable Liste
		{
			get;
			set;
		}
        /// <summary>
        /// Aufbereitete Tabelle für den Exceldownload
        /// </summary>
		public DataTable ExcelListe
		{
			get;
			set;
		}
		#endregion
        /// <summary>
        /// Kontruktor
        /// </summary>
        /// <param name="objUser">Userobjekt</param>
        /// <param name="objApp">Applikationsobjekt</param>
        /// <param name="strAppID">AppID</param>
        /// /// <param name="strSessionID">SessionID</
		public VorVersand(ref CKG.Base.Kernel.Security.User objUser, ref CKG.Base.Kernel.Security.App objApp, String strAppID, String strSessionID)
			: base(ref objUser, ref objApp, strAppID, strSessionID, "")
		{}
        /// <summary>
        /// Overrides Change() Base.Business.BankBase
        /// </summary>
		public override void Change() 
		{ 
		}
        /// <summary>
        /// Overrides Show() Base.Business.BankBase
        /// </summary>
		public override void Show()
		{
		}
        /// <summary>
        /// Liefert aus SAP den am nahe gelegensten ZLD oder ext. Dienstleister
        /// nach KreisKZ (Versandzulassunng). Bapi: Z_ZLD_EXPORT_INFOPOOL
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ChangeStatusVersand.aspx</param>
		public void getBestLieferant(String strAppID, String strSessionID, System.Web.UI.Page page)
		{

			m_strClassAndMethod = "VoerfZLD.getBestLieferant";
			m_strAppID = strAppID;
			m_strSessionID = strSessionID;
			m_intStatus = 0;
			m_strMessage = String.Empty;

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
							m_intStatus = -9999;
							m_strMessage = m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
							break;
					}
				}
				finally { m_blnGestartet = false; }
			}
		}

        /// <summary>
        /// Selektion und Anzeige der Versandzulassungen(Übersicht Versandzulassungen/ RE-Prüfung). 
        /// Bapi:Z_ZLD_EXPORT_VZOZUERL
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ChangeStatusVersandListe.aspx</param>
        /// <param name="tblKundenStamm">Kundenstammtabelle</param>
        public void FillVersanZul(String strAppID, String strSessionID, System.Web.UI.Page page, DataTable tblKundenStamm)
		{

			m_strClassAndMethod = "VorVersand.FillVersanZul";
			m_strAppID = strAppID;
			m_strSessionID = strSessionID;
			m_intStatus = 0;
			m_strMessage = String.Empty;
			DataTable tmpListe = new DataTable();
			if (m_blnGestartet == false)
			{
				m_blnGestartet = true;
				try
				{
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_VZOZUERL", ref m_objApp, ref m_objUser, ref page);

					if (SelKunde.Length > 0) { SelKunde = SelKunde.PadLeft(10, '0'); }

					myProxy.setImportParameter("I_KUNNR",SelKunde);
					myProxy.setImportParameter("I_VKBUR", VKBUR);
					myProxy.setImportParameter("I_VKORG", VKORG);
					myProxy.setImportParameter("I_KREISKZ", SelKreis);
					myProxy.setImportParameter("I_ZZZLDAT_VON", SelDatum);
					myProxy.setImportParameter("I_ZZZLDAT_BIS", SelDatumBis);
                    if (SelID.Length> 0) myProxy.setImportParameter("I_ZULBELN", SelID.PadLeft(10, '0'));
                    else myProxy.setImportParameter("I_ZULBELN", "");
					myProxy.setImportParameter("I_STATUS", "");
                    myProxy.setImportParameter("I_LISTE", SelStatus);

					myProxy.callBapi();

					 tmpListe = myProxy.getExportTable("GT_EX_ZUERL");
					tmpListe.Columns.Add("KUNNAME", typeof(String));
					tmpListe.Columns.Add("GebPreis", typeof(String));
					tmpListe.Columns.Add("PreisKZ", typeof(String));
					tmpListe.Columns.Add("Steuer", typeof(String));
					tmpListe.Columns.Add("WBKunde", typeof(String));
					foreach (DataRow item in tmpListe.Rows)
					{
						if (item["VZB_STATUS"].ToString() == "VD")
						{ item["WBKunde"] = "J"; }
						else { item["WBKunde"] = "N"; }

                        item["ZULPOSNR"] = item["ZULPOSNR"].ToString().TrimStart('0');
						item["GebPreis"] = "0,00";	
						item["PreisKZ"] = "0,00";		
						item["Steuer"] = "0,00";
						item["PREIS"] = String.Format("{0:0.00}", item["PREIS"]);
						item["KBETR"] = String.Format("{0:0.00}", item["KBETR"]);
						
						if (item["WEBMTART"].ToString() == "D") 
						{
							DataRow[] SelItem = tmpListe.Select("ZULBELN='" + item["ZULBELN"].ToString() + "' AND UEPOS ='" + item["ZULPOSNR"].ToString().PadLeft(6,'0') + "'");
							if (SelItem.Length > 0)
							{
								for (int i = 0; i < SelItem.Length; i++)
								{
									if (SelItem[i]["WEBMTART"].ToString() == "G")
									{
										item["GebPreis"] = String.Format("{0:0.00}", SelItem[i]["PREIS"]);
										
									}
									if (SelItem[i]["WEBMTART"].ToString() == "K")
									{
										item["PreisKZ"] = String.Format("{0:0.00}", SelItem[i]["PREIS"]);
									}
									if (SelItem[i]["WEBMTART"].ToString() == "S")
									{
										item["Steuer"] = String.Format("{0:0.00}", SelItem[i]["PREIS"]);
									}
								}

							}
							item["KUNNR"] = item["KUNNR"].ToString().TrimStart('0');
							item["ZULBELN"] = item["ZULBELN"].ToString().TrimStart('0');
							SelItem = tblKundenStamm.Select("KUNNR='" + item["KUNNR"].ToString() + "'");
							if (SelItem.Length > 0) 
							{
								for (int i = 0; i < SelItem.Length; i++)
								{

									item["KUNNAME"] = SelItem[i]["NAME1"].ToString().Split('~')[0].ToString();
								}
							}
							if (item["ZL_LIFNR"].ToString().Length>0) 
							
							{

								item["ZL_LIFNR"] = item["ZL_LIFNR"].ToString().TrimStart('0');
								if (item["ZL_LIFNR"].ToString().Substring(0,2) == "56")
								{
									item["KBETR"] = 0;
								}
							}
						}
					}

					for (int i = tmpListe.Rows.Count - 1; i > 0; i--)
					{
						if (tmpListe.Rows[i]["WEBMTART"].ToString() != "D")
						{
							tmpListe.Rows.RemoveAt(i);
						}
					}
					Liste = CreateOutPut(tmpListe, strAppID);
					ExcelListe = CreateOutPut(tmpListe, strAppID);
					ExcelListe.Columns.Remove("Belegnr");
					ExcelListe.Columns.Remove("ZULPOSNR");
					ExcelListe.Columns.Remove("BLTYP");
					ExcelListe.Columns["LoeschKZ"].ColumnName="L/OK";


						WriteLogEntry(true, "Selektion Versanzulassungen: Kunnr:" + SelKunde + ", Filiale:" + VKBUR
									   + ", Kreis:" + SelKreis + ", Zul.-Datum von:" + SelDatum + ", Zul.-Datum bis:" + SelDatum
									   + ", ID:" + SelID + ", Status:" + SelStatus, ref tmpListe);
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
							WriteLogEntry(false, "Selektion Versanzulassungen: Kunnr:" + SelKunde + ", Filiale:" + VKBUR
					   + ", Kreis:" + SelKreis + ", Zul.-Datum von:" + SelDatum + ", Zul.-Datum bis:" + SelDatum
					   + ", ID:" + SelID + ", Status:" + SelStatus, ref tmpListe);
				}
				finally { m_blnGestartet = false; }
			}
		}

        /// <summary>
        /// Status der Versandzulassung an SAP übergeben und speichern.
        /// Bapi: Z_ZLD_CHANGE_VZOZUERL
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ChangeStatusVersandListe.aspx</param>
        /// <param name="sID">ZULBELN</param>
        /// <param name="sPosNr">ZULPOSNR</param>
        /// <param name="sStatus">STATUS</param>
        /// <param name="sLoesch">LOEKZ</param>
        /// <param name="UpdateListe">UpdateListe</param>
        public void UpdateStatus(String strAppID, String strSessionID, System.Web.UI.Page page, String sID,
									String sPosNr, String sStatus, String sLoesch, DataTable UpdateListe)
		{

			m_strClassAndMethod = "VorVersand.UpdateStatus";
			m_strAppID = strAppID;
			m_strSessionID = strSessionID;
			m_intStatus = 0;
			m_strMessage = String.Empty;
			DataTable SapTable = new DataTable();
			if (m_blnGestartet == false)
			{
				m_blnGestartet = true;
				try
				{
					BestLieferanten = new DataTable();
					DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_CHANGE_VZOZUERL", ref m_objApp, ref m_objUser, ref page);

					SapTable = myProxy.getImportTable("GT_IMP_VZOZUERL");

					DataRow NewRow ;
					if (UpdateListe == null)
					{
						NewRow = SapTable.NewRow();
						NewRow["ZULBELN"] = sID.PadLeft(10, '0');
						NewRow["ZULPOSNR"] = sPosNr.PadLeft(6, '0');
						NewRow["STATUS"] = sStatus;
						NewRow["LOEKZ"] = sLoesch;
						if (sStatus == "S")
						{
							NewRow["VZERDAT"] = DateTime.Now.ToShortDateString();
						}
						SapTable.Rows.Add(NewRow);
					}
					else 
					{
						foreach (DataRow mRow in UpdateListe.Rows)
						{
							NewRow = SapTable.NewRow();
							NewRow["ZULBELN"] = mRow["ZULBELN"].ToString().PadLeft(10, '0');
							NewRow["ZULPOSNR"] = mRow["ZULPOSNR"].ToString().PadLeft(6, '0');
							NewRow["STATUS"] = mRow["STATUS"].ToString();
							NewRow["LOEKZ"] = mRow["LOEKZ"].ToString();
                            if (mRow["STATUS"].ToString() == "S")
							{
								NewRow["VZERDAT"] = DateTime.Now.ToShortDateString();
							}   
							SapTable.Rows.Add(NewRow);
						}
					}
					
					if (SapTable.Rows.Count > 0) { 
						myProxy.callBapi();

						Int32 subrc = 0;
                        if (ZLDCommon.IsNumeric(myProxy.getExportParameter("E_SUBRC").ToString()))
						{
						 Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);                       
						}

						String sapMessage;
						sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
						m_intStatus = subrc;
						m_strMessage = sapMessage;
					}

					if (m_intStatus == 0)
					{
						WriteLogEntry(true, "Statusänderung Versandzulassung", ref SapTable);
					}
					else
					{
						WriteLogEntry(false, "Statusänderung Versandzulassung. Fehler: " + m_strMessage, ref SapTable);
					}
				}
				catch (Exception ex)
				{
					switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
					{
						default:
							m_intStatus = -9999;
							m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
							WriteLogEntry(false, "Statusänderung Versandzulassung. Fehler: " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message), ref SapTable);
							break;
					}
				}
				finally { m_blnGestartet = false; }
			}
		}

	}
}
