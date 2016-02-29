using System;
using System.Collections.Generic;
using System.Data;
using CKG.Base.Common;
using System.Data.SqlClient;
using CKG.Base.Business;
using System.Configuration;

namespace AutohausPortal.lib
{   
    /// <summary>
    /// Klasse, die verschiedenste Listenansichten(Dokumentenanforderung, Zulassungsstatistik) mit Daten aus SAP füllt.
    /// </summary>
    public class ZLD_Suche : CKG.Base.Business.DatenimportBase
    {
        #region "Declarations"

        DataTable m_tblResultRaw;
        String m_strKennzeichen;
        String m_strPLZ;
        String m_strZulassungspartner;
        String m_strZulassungspartnerNr;

        #endregion

        #region "Properties"

        public String Zulassungspartner
        {
            get { return m_strZulassungspartner; }
            set { m_strZulassungspartner = value; }
        }

        public String PLZ
        {
            get { return m_strPLZ; }
            set { m_strPLZ = value; }
        }
        public String ZulassungspartnerNr
        {
            get { return m_strZulassungspartnerNr; }
            set { m_strZulassungspartnerNr = value; }
        }
        public String Kennzeichen
        {
            get { return m_strKennzeichen; }
            set { m_strKennzeichen = value; }
        }
        public DataTable ResultRaw
        {
            get { return m_tblResultRaw; }
        }
        public DataTable Mailings
        {
            get;
            set;
        }

        public String MailBody
        {
            get;
            set;
        }

        public String MailAdress
        {
            get;
            set;
        }

        public String MailAdressCC
        {
            get;
            set;
        }

        public String Betreff
        {
            get;
            set;
        }
        public DataTable KundenDaten
        {
            get;
            set;
        }

        public DataTable Auftragsdaten
        {
            get;
            set;
        }
        public DataTable AuftragsdatenStart
        {
            get;
            set;
        }
        public DataTable Preise
        {
            get;
            set;
        }

        public String ZulVon
        {
            get;
            set;
        }
        public String ZulBis
        {
            get;
            set;
        }
        public String BeaufVon
        {
            get;
            set;
        }
        public String BeaufBis
        {
            get;
            set;
        }
        public String Referenz1
        {
            get;
            set;
        }
        public String Referenz2
        {
            get;
            set;
        }
        public String Referenz3
        {
            get;
            set;
        }
        public String Referenz4
        {
            get;
            set;
        }
        public String Liste
        {
            get;
            set;
        }
        public String Kunnr
        {
            get;
            set;
        }
        public String Kennz
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        public ZLD_Suche(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
        { } 

        #endregion
        
        #region "Methods"

        /// <summary>
        /// Dokumentenanforderung der Zulassungsstellen
        /// Bapi Z_M_BAPIRDZ
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Dokumentenanforderung.aspx</param>
        public void Fill(String strAppID, String strSessionID, System.Web.UI.Page page)
        {

            m_strClassAndMethod = "ZLD_Suche.Fill";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_BAPIRDZ", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("IZKFZKZ", m_strKennzeichen);
                    myProxy.setImportParameter("IPOST_CODE1", m_strPLZ);
                    myProxy.setImportParameter("INAME1", m_strZulassungspartner);
                    myProxy.setImportParameter("IREMARK", m_strZulassungspartnerNr);

                    myProxy.callBapi();

                    m_tblResultRaw = new DataTable();
                    m_tblResultRaw = myProxy.getExportTable("ITAB");
                    CreateOutPut(m_tblResultRaw, strAppID);
                    m_tblResult.Columns.Add("Details", typeof(String));


                    foreach (DataRow tmpRow in m_tblResult.Rows)
                    {
                        tmpRow["Details"] = "Dokumentenanforderung_2.aspx?ID=" + tmpRow["ID"].ToString();
                    }
                    m_tblResult.Columns.Remove("ID");

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
        /// Laden der, in der SQL-Datenbank gespeicherten, Mailtexte eines Kunden
        /// </summary>
        /// <param name="InputVorgang">Vorgangsnummer des Mailtextes</param>
        public void LeseMailTexte(String InputVorgang)
        {
            String strTempVorgang = InputVorgang;

            m_intStatus = 0;
            m_strMessage = "";
            MailAdressCC = "";
            MailAdress = "";
            Mailings = new DataTable();
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection();
            connection.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"].ToString();
            try
            {

                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM vwGetMailTexte WHERE KundenID=@KundenID " +
                                      "AND Vorgangsnummer Like @Vorgangsnummer " +
                                      "AND Aktiv=1", connection);



                adapter.SelectCommand.Parameters.AddWithValue("@KundenID", m_objUser.Customer.CustomerId);
                adapter.SelectCommand.Parameters.AddWithValue("@Vorgangsnummer", strTempVorgang);

                adapter.Fill(Mailings);

                if (Mailings.Rows.Count > 0)
                {
                    foreach (DataRow dRow in Mailings.Rows)
                    {
                        MailBody = dRow["Text"].ToString();
                        Betreff = dRow["Betreff"].ToString();
                        Boolean boolCC = false;
                        Boolean.TryParse(dRow["CC"].ToString(), out boolCC);
                        if (boolCC)
                        {
                            if (MailAdressCC == "")
                            {
                                MailAdressCC = dRow["EmailAdresse"].ToString();
                            }
                            else
                            {
                                MailAdressCC += ";" + dRow["EmailAdresse"].ToString();
                            }
                        }
                        else if (MailAdress == "")
                        {
                            MailAdress = dRow["EmailAdresse"].ToString();
                        }
                        else
                        {
                            MailAdress += ";" + dRow["EmailAdresse"].ToString();
                        }
                    }


                }
                else
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Mailvorlagen für diesen Kunden.";
                }

            }
            catch (Exception ex)
            {
                m_intStatus = 9999;
                m_strMessage = "Fehler beim Laden der Mailadressen: " + ex.Message;
            }
            finally
            {
                connection.Close();

            }


        }

        /// <summary>
        /// Laden der Zulassungsstatistik des Webusers
        /// Bapi Z_ZLD_AH_ZULLISTE
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Zulassungsstatistik.aspx</param>
        /// <param name="VKORG">Verkaufsorganisation</param>
        /// <param name="VKBUR">Verkaufsbüro</param>
        public void FillStatistik(String strAppID, String strSessionID, System.Web.UI.Page page, String VKORG, String VKBUR)
        {

            m_strClassAndMethod = "ZLD_Suche.FillStatistik";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_ZULLISTE", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKORG", VKORG);
                    myProxy.setImportParameter("I_VKBUR", VKBUR);
                    if (Kunnr.Length > 1)
                    {
                        myProxy.setImportParameter("I_KUNNR", Kunnr.PadLeft(10, '0'));
                    }
                    else 
                    {
                        myProxy.setImportParameter("I_KUNNR", "");
                        myProxy.setImportParameter("I_GRUPPE", m_objUser.Groups[0].GroupName);
                    }
                                     
                    
                    myProxy.setImportParameter("I_ZZREFNR1", Referenz1);
                    myProxy.setImportParameter("I_ZZREFNR2", Referenz2);
                    myProxy.setImportParameter("I_ZZREFNR3", Referenz3);
                    myProxy.setImportParameter("I_ZZREFNR4", Referenz4);
                    myProxy.setImportParameter("I_ZZZLDAT_VON", ZulVon);
                    myProxy.setImportParameter("I_ZZZLDAT_BIS", ZulBis);
                    myProxy.setImportParameter("I_ERDAT_VON", BeaufVon);
                    myProxy.setImportParameter("I_ERDAT_BIS", BeaufBis);
                    myProxy.setImportParameter("I_ZZKENN", Kennz);
                    myProxy.setImportParameter("I_LISTE", Liste);
                    myProxy.callBapi();


                    KundenDaten = myProxy.getExportTable("GT_KUN");
                    Auftragsdaten = myProxy.getExportTable("GT_OUT");

                    bool blnNoPrices = m_objUser.Organization.OrganizationName.ToUpper().Contains(("NOPRICES"));

                    List<DataRow> KunnrToDel = new List<DataRow>();
                    foreach (DataRow aRow in Auftragsdaten.Rows)
                    {
                        aRow["KUNNR"] = aRow["KUNNR"].ToString().TrimStart('0');
                        aRow["ZULBELN"] = aRow["ZULBELN"].ToString().TrimStart('0');
                        // Stati für Anzeige aufbereiten (aus SAP kommen nur 1-stellige Werte, alle anderen müssen hier übersetzt werden)
                        if (aRow["BEB_STATUS"] != null)
                        {
                            string beb_stat = aRow["BEB_STATUS"].ToString();
                            switch (beb_stat)
                            {
                                case "4":
                                    aRow["BEB_STATUS"] = "DIS";
                                    break;
                                case "5":
                                    aRow["BEB_STATUS"] = "GO";
                                    break;
                                case "7":
                                    aRow["BEB_STATUS"] = "AR";
                                    break;
                            }
                        }
                    }
                    foreach (DataRow kRow in KundenDaten.Rows)
                    {
                        double sum = 0;
                        double sumGB = 0;
                        double sumST = 0;
                        double sumKZ = 0;
                        double tempDoublewert = 0;

                        if (KundenDaten.Rows.Count > 1)
                        {
                            if (Auftragsdaten.Select("KUNNR='" + kRow["KUNNR"].ToString().TrimStart('0') + "'").Length > 0)
                            {
                                Int32 iRowIndex = 0;
                                foreach (DataRow aRow in Auftragsdaten.Rows)
                                {
                                    aRow["KUNNR"] = aRow["KUNNR"].ToString().TrimStart('0');
                                    aRow["ZULBELN"] = aRow["ZULBELN"].ToString().TrimStart('0');
                                    if (aRow["KUNNR"].ToString() == kRow["KUNNR"].ToString().TrimStart('0'))
                                    {
                                        string bebStatus = aRow["BEB_STATUS"].ToString();

                                        if (blnNoPrices)
                                        {
                                            // Für Benutzer, deren Organisation das Tag "NoPrices" enthält, die Preise ausblenden
                                            aRow["PREIS_DL"] = DBNull.Value;
                                            aRow["PREIS_GB"] = DBNull.Value;
                                            aRow["PREIS_ST"] = DBNull.Value;
                                            aRow["PREIS_KZ"] = DBNull.Value;
                                        }
                                        else if (bebStatus == "AR")
                                        {
                                            // wenn abgerechnet, alle Preise anzeigen
                                            tempDoublewert = double.Parse(aRow["PREIS_DL"].ToString());
                                            aRow["PREIS_DL"] = tempDoublewert.ToString("F2");
                                            sum += tempDoublewert;
                                            tempDoublewert = double.Parse(aRow["PREIS_GB"].ToString());
                                            aRow["PREIS_GB"] = tempDoublewert.ToString("F2");
                                            sumGB += tempDoublewert;
                                            tempDoublewert = double.Parse(aRow["PREIS_ST"].ToString());
                                            aRow["PREIS_ST"] = tempDoublewert.ToString("F2");
                                            sumST += tempDoublewert;
                                            tempDoublewert = double.Parse(aRow["PREIS_KZ"].ToString());
                                            aRow["PREIS_KZ"] = tempDoublewert.ToString("F2");
                                            sumKZ += tempDoublewert;
                                        }
                                        else if (bebStatus == "D")
                                        {
                                            // wenn durchgeführt, Gebühren anzeigen
                                            aRow["PREIS_DL"] = DBNull.Value;
                                            tempDoublewert = double.Parse(aRow["PREIS_GB"].ToString());
                                            aRow["PREIS_GB"] = tempDoublewert.ToString("F2");
                                            sumGB += tempDoublewert;
                                            aRow["PREIS_ST"] = DBNull.Value;
                                            aRow["PREIS_KZ"] = DBNull.Value;
                                        }
                                        else
                                        {
                                            // wenn weder durchgeführt noch abgerechnet, keine Preise anzeigen
                                            aRow["PREIS_DL"] = DBNull.Value;
                                            aRow["PREIS_GB"] = DBNull.Value;
                                            aRow["PREIS_ST"] = DBNull.Value;
                                            aRow["PREIS_KZ"] = DBNull.Value;
                                        }

                                        if (aRow["VE_ERZEIT"] != DBNull.Value)
                                        {
                                            string erfassungszeit = aRow["VE_ERZEIT"].ToString();
                                            if ((!String.IsNullOrEmpty(erfassungszeit)) && (erfassungszeit.Length == 6))
                                            {
                                                aRow["VE_ERZEIT"] = erfassungszeit.Substring(0, 2) + ":" + erfassungszeit.Substring(2, 2) + ":" + erfassungszeit.Substring(4, 2);
                                            }
                                        }
                                    }

                                    iRowIndex++;
                                }
                                DataRow NewRow = Auftragsdaten.NewRow();
                                NewRow["KUNNR"] = kRow["KUNNR"].ToString().TrimStart('0');
                                NewRow["MAKTX"] = "Gesamt: ";
                                NewRow["PREIS_DL"] = sum.ToString("F2");
                                NewRow["PREIS_GB"] = sumGB.ToString("F2");
                                NewRow["PREIS_ST"] = sumST.ToString("F2");
                                NewRow["PREIS_KZ"] = sumKZ.ToString("F2");
                                Auftragsdaten.Rows.InsertAt(NewRow, iRowIndex + 1);
                            }
                            else
                            {
                                KunnrToDel.Add(kRow);
                            }
                        }
                        else if (KundenDaten.Rows.Count == 1)
                        {
                            Int32 iRowIndex = 0;
                            foreach (DataRow aRow in Auftragsdaten.Select("KUNNR='" + kRow["KUNNR"].ToString().TrimStart('0') + "'"))
                            {
                                if (aRow["KUNNR"].ToString() == kRow["KUNNR"].ToString().TrimStart('0'))
                                {
                                    if (aRow["BEB_STATUS"].ToString() == "O")
                                    {
                                        aRow["PREIS_DL"] = DBNull.Value;
                                        aRow["PREIS_GB"] = DBNull.Value;
                                        aRow["PREIS_ST"] = DBNull.Value;
                                        aRow["PREIS_KZ"] = DBNull.Value;
                                    }
                                    else if (aRow["BEB_STATUS"].ToString() == "AR")
                                    {
                                        tempDoublewert = double.Parse(aRow["PREIS_DL"].ToString());
                                        aRow["PREIS_DL"] = tempDoublewert.ToString("F2");
                                        sum += tempDoublewert;
                                        tempDoublewert = double.Parse(aRow["PREIS_GB"].ToString());
                                        aRow["PREIS_GB"] = tempDoublewert.ToString("F2");
                                        sumGB += tempDoublewert;
                                        tempDoublewert = double.Parse(aRow["PREIS_ST"].ToString());
                                        aRow["PREIS_ST"] = tempDoublewert.ToString("F2");
                                        sumST += tempDoublewert;
                                        tempDoublewert = double.Parse(aRow["PREIS_KZ"].ToString());
                                        aRow["PREIS_KZ"] = tempDoublewert.ToString("F2");
                                        sumKZ += tempDoublewert;
                                    }
                                    else
                                    {
                                        // Wenn noch nicht abgerechnet, nur Gebühren anzeigen
                                        aRow["PREIS_DL"] = DBNull.Value;
                                        tempDoublewert = double.Parse(aRow["PREIS_GB"].ToString());
                                        aRow["PREIS_GB"] = tempDoublewert.ToString("F2");
                                        sumGB += tempDoublewert;
                                        aRow["PREIS_ST"] = DBNull.Value;
                                        aRow["PREIS_KZ"] = DBNull.Value;
                                    }
                                    if (aRow["VE_ERZEIT"] != DBNull.Value)
                                    {
                                        string erfassungszeit = aRow["VE_ERZEIT"].ToString();
                                        if ((!String.IsNullOrEmpty(erfassungszeit)) && (erfassungszeit.Length == 6))
                                        {
                                            aRow["VE_ERZEIT"] = erfassungszeit.Substring(0, 2) + ":" + erfassungszeit.Substring(2, 2) + ":" + erfassungszeit.Substring(4, 2);
                                        }
                                    }
                                }
                                
                                iRowIndex++;    
                            }
                            DataRow NewRow = Auftragsdaten.NewRow();
                            NewRow["KUNNR"] = kRow["KUNNR"].ToString().TrimStart('0');
                            NewRow["MAKTX"] = "Gesamt: ";
                            NewRow["PREIS_DL"] = sum.ToString("F2");
                            NewRow["PREIS_GB"] = sumGB.ToString("F2");
                            NewRow["PREIS_ST"] = sumST.ToString("F2");
                            NewRow["PREIS_KZ"] = sumKZ.ToString("F2");
                            Auftragsdaten.Rows.InsertAt(NewRow, iRowIndex + 1);
                        }
                        kRow["KUNNR"] = kRow["KUNNR"].ToString().TrimStart('0');
                        kRow["NAME1"] = kRow["NAME1"] + " ~ " + kRow["KUNNR"].ToString();
                    }

                    foreach (DataRow RowToDel in KunnrToDel)
                    {
                        KundenDaten.Rows.Remove(RowToDel);
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
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Laden der offenen Aufträge für die Startseite
        /// </summary>
        /// <param name="strAppID">AppID = 9999 hart gesetzt, Startseite hat keine Application-ID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Selection.aspx</param>
        /// <param name="VKORG">Verkaufsorganisation</param>
        /// <param name="VKBUR">Verkaufsbüro</param>
        public void FillStartPage(String strAppID, String strSessionID, System.Web.UI.Page page, String VKORG, String VKBUR)
        {

            m_strClassAndMethod = "ZLD_Suche.FillStatistik";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_MEINE_AUFTRAEGE", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKORG", VKORG);
                    myProxy.setImportParameter("I_VKBUR", VKBUR);
                    myProxy.setImportParameter("I_GRUPPE", m_objUser.Groups[0].GroupName);
                    myProxy.setImportParameter("I_ERNAM", m_objUser.UserName);
                    myProxy.callBapi();

                    AuftragsdatenStart = myProxy.getExportTable("GT_OUT");
                    foreach (DataRow aRow in AuftragsdatenStart.Rows)
                    {
                        aRow["KUNNR"] = aRow["KUNNR"].ToString().TrimStart('0');
                        aRow["ZULBELN"] = aRow["ZULBELN"].ToString().TrimStart('0');
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
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        #endregion
    }
}