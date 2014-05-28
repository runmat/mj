using System;
using CKG.Base.Common;
using System.Data;
using CKG.Base.Business;
using System.Configuration;

namespace AppZulassungsdienst.lib 
{
    /// <summary>
    /// Klasse zum Laden der Stammdaten aus SAP.
    /// </summary>
	public class ZLDCommon: DatenimportBase
    {
        #region Properties

        /// <summary>
        /// Kundenstammtabelle
        /// </summary>
        public DataTable tblKundenStamm
        {
            get;
            set;
        }
        /// <summary>
        /// Stammdatentabelle Zulassungskreise
        /// </summary>
        public DataTable tblStvaStamm
        {
            get;
            set;
        }
        /// <summary>
        /// Stammdatentabelle Sonderkreisen 
        /// z.B. HH2(Städte mit mehreren Zulassungsdienststellen)
        /// </summary>
        public DataTable tblSonderStva
        {
            get;
            set;
        }
        /// <summary>
        /// Stammdatentabelle mit Dienstleistungen und Materialien
        /// </summary>
        public DataTable tblMaterialStamm
        {
            get;
            set;
        }
        /// <summary>
        /// Stammdatentabelle Dienstleistungen/Materialien ohne 
        /// Materialnummer im Materialtext
        /// </summary>
        public DataTable tblMaterialtextohneMatNr
        {
            get;
            set;
        }
        /// <summary>
        /// Stamdatentabelle Kennzeichengrössen
        /// </summary>
        public DataTable tblKennzGroesse
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
        /// Adresstaden einer Filiale
        /// </summary>
        public DataTable AdresseFiliale
        {
            get;
            set;
        }
        /// <summary>
        /// Name des Kunden
        /// </summary>
        public String Kundename
        {
            get;
            set;
        }
        /// <summary>
        /// "Nickname" des Kunden
        /// </summary>
        public String Nickname
        {
            get;
            set;
        }
        /// <summary>
        /// Tabelle der Gruppen zur Gruppenart 
        /// </summary>
        public DataTable tblGruppeTouren
        {
            get;
            set;
        }
        /// <summary>
        /// Tabelle der Kunden zur Gruppe
        /// </summary>
        public DataTable tblKundeGruppe
        {
            get;
            set;
        }
        /// <summary>
        /// Tabelle der Gruppe zur Gruppenart für Selektionen
        /// </summary>
        public DataTable tblTourenforSelection
        {
            get;
            set;
        }
        /// <summary>
        /// Tabelle der Kunden zur Gruppefür Selektionen
        /// </summary>
        public DataTable tblKdGruppeforSelection
        {
            get;
            set;
        }
        /// <summary>
        /// Bezeichnung der Gruppe oder der Tour
        /// </summary>
        public String Bezeichnung
        {
            get;
            set;
        }
        /// <summary>
        /// Gruppen- bzw. TourID
        /// </summary>
        public String GroupOrTourID 
        {
            get;
            set;
        }
        /// <summary>
        /// SWIFT-BIC des Kunden
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
        /// Name der Bank
        /// </summary>
        public String Bankname
        {
            get;
            set;
        }
        /// <summary>
        /// Bankschlüssel
        /// </summary>
        public String Bankschluessel
        {
            get;
            set;
        }
        /// <summary>
        /// Bankleitzahl des Kunden
        /// </summary>
        public String BLZ
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
        /// Rückgabe der Kunden zur Kundengruppe eines Kunden
        /// </summary>
        public DataTable tblAHKundenStamm
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="objUser">Webuserobjekt</param>
        /// <param name="objApp">Applikationsobjekt</param>
        /// 
        public ZLDCommon(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp)
			: base(ref objUser, objApp, "")
		{
            tblKundenStamm = new DataTable();
            tblStvaStamm = new DataTable();
            tblMaterialStamm = new DataTable();
            tblMaterialtextohneMatNr = new DataTable();
            tblKennzGroesse = new DataTable();
		}

        /// <summary>
        /// Laden der Stammdaten der jeweiligen Gruppe aus SAP. Bapi: Z_ZLD_EXPORT_KUNDE_MAT
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">alle Erfassungsmasken</param>
        public void getSAPDatenStamm(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "ZLDCommon.getSAPDatenStamm";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_KUNDE_MAT", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKORG", VKORG);
                    myProxy.setImportParameter("I_VKBUR", VKBUR);

                    myProxy.callBapi();

                    tblKundenStamm = myProxy.getExportTable("GT_EX_KUNDE");
                    foreach (DataRow drow in tblKundenStamm.Rows)
                    {
                        drow["KUNNR"] = drow["KUNNR"].ToString().TrimStart('0');
                        drow["NAME1"] = drow["NAME1"] + " ~ " + drow["KUNNR"].ToString();
                        if (drow["EXTENSION1"].ToString().Length > 0) 
                        {
                            drow["NAME1"] = drow["NAME1"] + " / " + drow["EXTENSION1"].ToString();
                        }
                    }

                    DataRow dr = tblKundenStamm.NewRow();
                    dr["KUNNR"] = "0";
                    dr["NAME1"] = " - keine Auswahl - ";
                    dr["INAKTIV"] = "";
                    tblKundenStamm.Rows.Add(dr);
                    tblMaterialtextohneMatNr = new DataTable();

                    tblMaterialtextohneMatNr = myProxy.getExportTable("GT_EX_MATERIAL");
                    tblMaterialStamm = tblMaterialtextohneMatNr.Copy();
                    foreach (DataRow drow in tblMaterialStamm.Rows)
                    {
                        drow["MATNR"] = drow["MATNR"].ToString().TrimStart('0');
                        drow["MAKTX"] = drow["MAKTX"].ToString() + " ~ " + drow["MATNR"];
                    }

                    if (tblMaterialStamm.Rows.Count == 0)
                    {
                        m_intStatus = -5555;
                        m_strMessage = "Keine Materialdaten gefunden!";
                    }

                    dr = tblMaterialStamm.NewRow();
                    dr["MATNR"] = "0";
                    dr["MAKTX"] = " - keine Auswahl - ";
                    dr["INAKTIV"] = "";
                    tblMaterialStamm.Rows.Add(dr);
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
       /// Laden der Stammdaten der jeweiligen Autohausgruppe aus SAP. Bapi: Z_ZLD_AH_KUNDE_ZU_GRUPPE
       /// </summary>
       /// <param name="strAppID">AppID</param>
       /// <param name="strSessionID">SessionID</param>
       /// <param name="page">ChangeZLDNach.aspx, AHVersandChange.aspx</param>
       /// <param name="Kunnr">Kundennummer</param>
        public void getSAPAHDatenStamm(String strAppID, String strSessionID, System.Web.UI.Page page, String Kunnr)
        {
           m_strClassAndMethod = "ZLDCommon.getSAPAHDatenStamm";
           m_strAppID = strAppID;
           m_strSessionID = strSessionID;
           m_intStatus = 0;
           m_strMessage = String.Empty;

           if (m_blnGestartet == false)
           {
               m_blnGestartet = true;
               try
               {
                   DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_KUNDE_ZU_GRUPPE", ref m_objApp, ref m_objUser, ref page);

                   myProxy.setImportParameter("I_VKORG", VKORG);
                   myProxy.setImportParameter("I_VKBUR", VKBUR);
                   myProxy.setImportParameter("I_KUNNR", Kunnr.PadLeft(10,'0'));

                   myProxy.callBapi();

                   tblAHKundenStamm = myProxy.getExportTable("GT_DEB");
                   foreach (DataRow drow in tblAHKundenStamm.Rows)
                   {
                       drow["KUNNR"] = drow["KUNNR"].ToString().TrimStart('0');
                       drow["NAME1"] = drow["NAME1"] + " ~ " + drow["KUNNR"].ToString();
                       if (drow["EXTENSION1"].ToString().Length > 0)
                       {
                           drow["NAME1"] = drow["NAME1"] + " / " + drow["EXTENSION1"].ToString();
                       }
                   }

                   DataRow dr = tblAHKundenStamm.NewRow();
                   dr["KUNNR"] = "0";
                   dr["NAME1"] = " - keine Auswahl - ";
                   tblAHKundenStamm.Rows.Add(dr);
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
       /// Laden der Zulassungsstellen aus SAP. Bapi: Z_ZLD_EXPORT_ZULSTEL
       /// </summary>
       /// <param name="strAppID"></param>
       /// <param name="strSessionID"></param>
       /// <param name="page"></param>
        public void getSAPZulStellen(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "ZLDCommon.getSAPZulStellen";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_ZULSTEL", ref m_objApp, ref m_objUser, ref page);

                    myProxy.callBapi();

                    tblStvaStamm = myProxy.getExportTable("GT_EX_ZULSTELL");
                    tblSonderStva = myProxy.getExportTable("GT_SONDER");

                    if (tblStvaStamm.Rows.Count == 0)
                    {
                        m_intStatus = -5555;
                        m_strMessage = "Keine STVA-Daten gefunden!";
                    }
                    tblStvaStamm.Columns.Add("KREISTEXT", typeof(String));
                    DataRow dr = tblStvaStamm.NewRow();

                    dr["KREISKZ"] = "";
                    dr["KREISBEZ"] = " - keine Auswahl - ";
                    tblStvaStamm.Rows.Add(dr);
                    foreach (DataRow row in tblStvaStamm.Rows)
                    {
                        row["KREISTEXT"] = row["KREISKZ"].ToString().PadRight(4, '.') + row["KREISBEZ"].ToString();
                    }

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
        /// Laden der in der SQL-Tabelle hinterlegten Kennzeichengösse pro Materialnummer
        /// </summary>
        public void LadeKennzeichenGroesse()
        {
            m_intStatus = 0;
            m_strMessage = "";
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection();
            connection.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"];
            try
            {
                tblKennzGroesse = new DataTable();

                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();

                command.CommandText = "SELECT  dbo.KennzeichGroesse.ID, dbo.MaterialKennzGroesse.Matnr, dbo.MaterialKennzGroesse.Kennzart, dbo.KennzeichGroesse.Groesse" +
                        " FROM dbo.MaterialKennzGroesse INNER JOIN" +
                        " dbo.KennzeichGroesse ON dbo.MaterialKennzGroesse.Matnr = dbo.KennzeichGroesse.Matnr";

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                adapter.SelectCommand = command;
                adapter.Fill(tblKennzGroesse);

                if (tblKennzGroesse.Rows.Count == 0)
                {
                    m_intStatus = 9999;
                    m_strMessage = "Keine Daten gefunden!";
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
        /// Prüfung ob das angebene Material in der Stammtabelle Gebührenpflichtig ist.
        /// </summary>
        /// <param name="matnr">Materialnummer</param>
        /// <returns>Ja/Nein</returns>
        public Boolean proofGebPflicht(String matnr)
        {
            Boolean GebPflicht = false;
            DataRow[] MatRow = tblMaterialStamm.Select("MATNR = " + matnr.PadLeft(18, '0'));

            if (MatRow.Length == 1)
            {
                if (MatRow[0]["ZZGEBPFLICHT"].ToString() == "X")
                {
                    GebPflicht = true;
                }
            }
            return GebPflicht;
        }   

        /// <summary>
        ///  Adresse des durchzuführenden Zulassungsdienstes. Bapi: Z_ZLD_EXPORT_FILIAL_ADRESSE
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">ChangeZLDVorVersand_2.aspx</param>
        public void getFilialadresse(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "VoerfZLD.getFilialadresse";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_EXPORT_FILIAL_ADRESSE", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKORG", VKORG);
                    myProxy.setImportParameter("I_VKBUR", VKBUR);

                    myProxy.callBapi();

                    AdresseFiliale = myProxy.getExportTable("ES_FIL_ADRS");

                    m_strMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    if (m_strMessage.Length > 0)
                    {
                        m_intStatus = -9999;
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
        /// Nickname bzw. Namenszusatz eines Kunden laden. Bapi: Z_ZLD_GET_NICKNAME
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Nickname.aspx</param>
        /// <param name="SearchKunnr">Kundennummer</param>
        public void GetKundeNickname(String strAppID, String strSessionID, System.Web.UI.Page page, String SearchKunnr)
        {
            m_strClassAndMethod = "ZLDCommon.getKundeNickname";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_GET_NICKNAME", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKBUR", VKBUR);
                    myProxy.setImportParameter("I_KUNNR", SearchKunnr.PadLeft(10, '0'));

                    myProxy.callBapi();

                    String Name1 = myProxy.getExportParameter("E_NAME1").ToString();
                    String Name2 = myProxy.getExportParameter("E_NAME1").ToString();
                    String Extension = myProxy.getExportParameter("E_EXTENSION1").ToString();
                    Nickname = myProxy.getExportParameter("E_NICK_NAME").ToString();
                                        Kundename = Name1 + " ~ " + Name2;
                    if (Extension.Length > 0)
                    {
                        Kundename += " / " + Extension;
                    }

                    m_strMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    if (m_strMessage.Length > 0)
                    {
                        m_intStatus = -9999;
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
        /// Nickname bzw. Namenszusatz eines Kunden speichern oder löschen. Bapi: Z_ZLD_SET_NICKNAME
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Nickname.aspx</param>
        /// <param name="SearchKunnr">Kundennummer</param>
        /// <param name="Delete">Löschkennzeichen</param>
        public void SetKundeNickname(String strAppID, String strSessionID, System.Web.UI.Page page, String SearchKunnr, String Delete)
        {
            m_strClassAndMethod = "ZLDCommon.SetKundeNickname";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_SET_NICKNAME", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKBUR", VKBUR);
                    myProxy.setImportParameter("I_KUNNR", SearchKunnr.PadLeft(10,'0'));
                    myProxy.setImportParameter("I_NICK_NAME", Nickname);
                    myProxy.setImportParameter("I_DELETE", Delete);

                    myProxy.callBapi();

                    m_strMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    if (m_strMessage.Length > 0)
                    {
                        m_intStatus = -9999;
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
        /// Laden der Kundengruppen oder Touren der Filiale. Bapi: Z_ZLD_GET_GRUPPE
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Kundengruppe.aspx, Neukundenanlage</param>
        /// <param name="GroupArt">"K" für Kundengruppe, "T" für Touren</param>
        public void GetGruppen_Touren(String strAppID, String strSessionID, System.Web.UI.Page page, String GroupArt)
        {
            m_strClassAndMethod = "ZLDCommon.GetGruppen_Touren";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_GET_GRUPPE", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKBUR", VKBUR);
                    myProxy.setImportParameter("I_GRUPART", GroupArt);

                    myProxy.callBapi();

                    tblGruppeTouren = myProxy.getExportTable("GT_GRUPPE");

                    foreach (DataRow drow in tblGruppeTouren.Rows)
                    {
                        drow["GRUPPE"] = drow["GRUPPE"].ToString().TrimStart('0');
                    }

                    DataRow dRow;
                    if (GroupArt == "T")
                    {
                        tblTourenforSelection = tblGruppeTouren.Copy();

                        dRow = tblTourenforSelection.NewRow();
                        dRow["GRUPPE"] = "0";
                        dRow["VKBUR"] = VKBUR;
                        dRow["GRUPART"] = GroupArt;
                        if (tblTourenforSelection.Rows.Count > 0)
                        { dRow["BEZEI"] = " - keine Auswahl - "; }
                        else
                        { dRow["BEZEI"] = " - keine Touren gepflegt - "; }
                      
                        tblTourenforSelection.Rows.Add(dRow);
                    }
                    else if(GroupArt == "K")
                    {
                        tblKdGruppeforSelection = tblGruppeTouren.Copy();
                        dRow = tblKdGruppeforSelection.NewRow();
                        dRow["GRUPPE"] = "0";
                        dRow["VKBUR"] = VKBUR;
                        dRow["GRUPART"] = GroupArt;
                        if (tblKdGruppeforSelection.Rows.Count > 0)
                        { dRow["BEZEI"] = " - keine Auswahl - "; }
                        else
                        { dRow["BEZEI"] = " - keine Gruppen gepflegt - "; }
                        tblKdGruppeforSelection.Rows.Add(dRow);
                    }

                    m_strMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    if (m_strMessage.Length > 0)
                    {
                        m_intStatus = -9999;
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
        /// Laden der Zuordnung Gruppe zu Kunden oder Kunden zur Gruppe. Bapi: Z_ZLD_GET_GRUPPE_KDZU
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Touren.aspx</param>
        public void GetKunden_TourenZuordnung(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "ZLDCommon.GetKunden_TourenZuordnung";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_GET_GRUPPE_KDZU", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_GRUPPE", GroupOrTourID);

                    myProxy.callBapi();

                    tblKundeGruppe = myProxy.getExportTable("GT_KDZU");

                    foreach (DataRow dRow in tblKundeGruppe.Rows) 
                    {
                        String Name1 = dRow["NAME1"].ToString();
                        String KUNNR = dRow["KUNNR"].ToString();
                        String Extension = dRow["EXTENSION1"].ToString();
                        Kundename = Name1 + " ~ " + KUNNR.TrimStart('0');
                        if (Extension.Length > 0)
                        {
                            Kundename += " / " + Extension;
                        }
                        dRow["NAME1"] = Kundename;
                    }
                   
                    m_strMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    if (m_strMessage.Length > 0)
                    {
                        m_intStatus = -9999;
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
       /// Gruppe oder Tour speichern, aktulisieren oder löschen. Bapi: Z_ZLD_SET_GRUPPE
       /// </summary>
       /// <param name="strAppID">AppID</param>
       /// <param name="strSessionID">SessionID</param>
       /// <param name="page">Touren.aspx</param>
       /// <param name="GroupArt">"K" für Kundengruppe, "T" für Touren</param>
       /// <param name="Action">I = insert, C = change, D = delete</param>
        public void SetKunden_Touren(String strAppID, String strSessionID, System.Web.UI.Page page, String GroupArt, String Action)
        {
            m_strClassAndMethod = "ZLDCommon.SetKunden_Touren";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_SET_GRUPPE", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKBUR", VKBUR);
                    myProxy.setImportParameter("I_GRUPPE", GroupOrTourID);
                    myProxy.setImportParameter("I_GRUPART", GroupArt);
                    myProxy.setImportParameter("I_BEZEI",Bezeichnung);
                    myProxy.setImportParameter("I_FUNC", Action);

                    myProxy.callBapi();

                    m_strMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    if (m_strMessage.Length > 0)
                    {
                        m_intStatus = -9999;
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
       /// Anlegen / löschen der Zuordnung  Kunden zu Gruppe oder Tour. Bapi: Z_ZLD_SET_GRUPPE_KDZU
       /// </summary>
       /// <param name="strAppID">AppID</param>
       /// <param name="strSessionID">SessionID</param>
       /// <param name="page">Touren.aspx</param>
       /// <param name="Kunnr">Kundennummer</param>
       /// <param name="Action">I = insert, D = delete</param>
        public void SetKunden_TourenZuordnung(String strAppID, String strSessionID, System.Web.UI.Page page, String Kunnr, String Action)
        {
            m_strClassAndMethod = "ZLDCommon.SetGruppen_Touren";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_SET_GRUPPE_KDZU", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_GRUPPE", GroupOrTourID);
                    myProxy.setImportParameter("I_KUNNR", Kunnr);
                    myProxy.setImportParameter("I_FUNC", Action);

                    myProxy.callBapi();

                    m_strMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    if (m_strMessage.Length > 0)
                    {
                        m_intStatus = -9999;
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
        /// IBAN prüfen und daraus Bankinfos ermitteln. Bapi: Z_FI_CONV_IBAN_2_BANK_ACCOUNT
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page"></param>
        public void ProofIBAN(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Neukunde.ProofIBAN";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_FI_CONV_IBAN_2_BANK_ACCOUNT", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_IBAN", IBAN);

                    myProxy.callBapi();

                    Bankname = myProxy.getExportParameter("E_BANKA");
                    Bankschluessel = myProxy.getExportParameter("E_BANK_NUMBER");
                    SWIFT = myProxy.getExportParameter("E_SWIFT");
                    Kontonr = myProxy.getExportParameter("E_BANK_ACCOUNT");

                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    m_strMessage = sapMessage;
                    if (m_strMessage.Length > 0)
                    {
                        m_strMessage = "IBAN fehlerhaft: " + sapMessage;
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -9999;
                            m_strMessage = m_strMessage = "Fehler bei der IBAN-Prüfung: " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Prüft ob der mitgegebene String ein numerischer Wert ist.
        /// </summary>
        /// <param name="val">numerischer String</param>
        /// <returns>true bei numerisch, false bei nicht numerisch</returns>
        public static bool IsNumeric(string val)
        {
            try
            {
                Convert.ToInt32(val);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Prüft ob der mitgegebene String dezimal-Wert ist.
        /// </summary>
        /// <param name="val">String dezimal</param>
        /// <returns>true bei dezimal, false bei nicht dezimal</returns>
        public static bool IsDecimal(string val)
        {
            try
            {
                Convert.ToDecimal(val);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Prüft ob der mitgegebene String ein Datum ist
        /// </summary>
        /// <param name="val">Date String</param>
        /// <returns>true bei Date, false bei nicht Date</returns>
        public static bool IsDate(String val)
        {
            bool result;
            DateTime myDT;

            try
            {
                result = DateTime.TryParse(val, out myDT);

            }
            catch (FormatException e)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Wandelt Boolean-Wert in X oder "" für die
        /// Datenaufbereitung SAP um.
        /// </summary>
        /// <param name="val">Boolean-Wert</param>
        /// <returns>X bei true, "" bei false</returns>
        public static String BoolToX(Boolean? val)
        {
            return ((val.HasValue && val.Value) ? "X" : "");
        }

        /// <summary>
        /// Wandelt Boolean-Wert in X oder "" für die
        /// Datenaufbereitung SAP um.
        /// </summary>
        /// <param name="val">Boolean-Wert</param>
        /// <returns>X bei true, "" bei false</returns>
        public static String BoolToX(Boolean val)
        {
            return (val ? "X" : "");
        }

        /// <summary>
        /// Wandelt X bzw. " " in boolean-Wert um
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Boolean XToBool(String val)
        {
            return (val == "X");
        }

        /// <summary>
        /// Helper.
        /// wandelt das benutzte Kurzdatum z.B. 010112 in 01.01.2012
        /// </summary>
        /// <param name="dat">Kurzdatum</param>
        /// <returns>norm. Datum</returns>
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

        #endregion
    }
}