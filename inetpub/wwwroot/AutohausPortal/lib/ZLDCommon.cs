using System;
using CKG.Base.Common;
using System.Data;
using CKG.Base.Business;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AutohausPortal.lib
{
    /// <summary>
    /// Allgemeine Klasse in der Stammdaten geladen werden.
    /// </summary>
    public class ZLDCommon : DatenimportBase
    {
        #region Properties

        /// <summary>
        /// Tabelle Kundenstamm aus SAP.
        /// </summary>
        public DataTable tblKundenStamm
        {
            get;
            set;
        }
        /// <summary>
        /// Tabelle Kreiskennzeichen(StVa) aus SAP.
        /// </summary>
        public DataTable tblStvaStamm
        {
            get;
            set;
        }
        /// <summary>
        /// Tabelle Sonderkreiskennzeichen z.b. HH1 aus SAP.
        /// </summary>
        public DataTable tblSonderStva
        {
            get;
            set;
        }
        /// <summary>
        /// Tabelle Kennzeichengroessen aus SQL-DB
        /// </summary>
        public DataTable tblKennzGroesse
        {
            get;
            set;
        }
        /// <summary>
        /// Fahrzeugarten (aus SAP)
        /// </summary>
        public DataTable tblFahrzeugarten
        {
            get;
            set;
        }
        /// <summary>
        /// Verkaufsorganisation (1010).
        /// </summary>
        public String VKORG
        {
            get;
            set;
        }
        /// <summary>
        /// Verkaufsbüro (4400).
        /// </summary>
        public String VKBUR
        {
            get;
            set;
        }
        /// <summary>
        /// Tabelle Adresse der Filiale aus SAP
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
        /// IBAN des Kunden.
        /// </summary>
        public String IBAN
        {
            get;
            set;
        }
        /// <summary>
        /// SWIFT-BIC der Bank des Kunden.
        /// </summary>
        public String SWIFT
        {
            get;
            set;
        }
        /// <summary>
        /// Name der Bank des Kunden.
        /// </summary>
        public String Bankname
        {
            get;
            set;
        }
        /// <summary>
        /// Bankschlüssel (SAP) der Bank
        /// </summary>
        public String Bankkey
        {
            get;
            set;
        }
        /// <summary>
        /// Kontonr
        /// </summary>
        public String Kontonr
        {
            get;
            set;
        }

        #endregion

        #region Contructor
        /// <summary>
        /// Kontruktor
        /// </summary>
        /// <param name="objUser">User-Object</param>
        /// <param name="objApp">Applikations-Objekt</param>
        public ZLDCommon(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp)
            : base(ref objUser, objApp, "")
        {

            tblKundenStamm = new DataTable();
            tblStvaStamm = new DataTable();
            tblKennzGroesse = new DataTable();
            tblFahrzeugarten = new DataTable();
        } 
        #endregion

        #region Methods

        /// <summary>
        /// Stammdaten initialisieren
        /// </summary>
        /// <param name="strAppID"></param>
        /// <param name="strSessionID"></param>
        /// <param name="page"></param>
        /// <returns>true, wenn keine Fehler aufgetreten sind, also Status = 0 ist</returns>
        public bool Init(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            this.VKBUR = m_objUser.Reference.Substring(4, 4);
            this.VKORG = m_objUser.Reference.Substring(0, 4);

            this.getSAPDatenStamm(strAppID, strSessionID, page);
            if (this.Status != 0)
            {
                return false;
            }

            this.getSAPZulStellen(strAppID, strSessionID, page);
            this.LadeKennzeichenGroesse();
            this.getSAPFahrzeugarten(strAppID, strSessionID, page);

            return (this.Status == 0);
        }

        /// <summary>
        /// Laden der Stammdaten der jeweiligen Gruppe aus SAP
        /// Bapi Z_ZLD_AH_KUNDE_MAT
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page"></param>
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
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_AH_KUNDE_MAT", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKORG", VKORG);
                    myProxy.setImportParameter("I_VKBUR", VKBUR);
                    myProxy.setImportParameter("I_GRUPPE", m_objUser.Groups[0].GroupName);
                    myProxy.callBapi();

                    tblKundenStamm = myProxy.getExportTable("GT_DEB");
                    foreach (DataRow drow in tblKundenStamm.Rows)
                    {
                        drow["KUNNR"] = drow["KUNNR"].ToString().TrimStart('0');
                        drow["NAME1"] = drow["NAME1"] + " ~ " + drow["KUNNR"].ToString();
                    }

                    if (tblKundenStamm.Rows.Count == 0)
                    {
                        m_intStatus = -5555;
                        m_strMessage = "Keine Kundendaten gefunden!";
                    }

                    if (tblKundenStamm.Rows.Count > 1)
                    {
                        DataRow dr = tblKundenStamm.NewRow();
                        dr["KUNNR"] = "0";
                        dr["NAME1"] = " - keine Auswahl - ";
                        tblKundenStamm.Rows.Add(dr);                   
                    }
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
        /// Laden der Zulassungsstellen aus SAP
        /// Bapi Z_ZLD_EXPORT_ZULSTEL
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
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
                        if (!IsNumeric(row["KREISKZ"].ToString()))
                        {
                            row["KREISTEXT"] = row["KREISKZ"].ToString().PadRight(4, '.') + row["KREISBEZ"].ToString();
                        }
                        else { row["KREISTEXT"] = row["KREISBEZ"].ToString(); }
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
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
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
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

            try
            {
                tblKennzGroesse = new DataTable();

                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();


                command.CommandText = "SELECT  dbo.KennzeichGroesse.ID, dbo.MaterialKennzGroesse.Matnr, dbo.MaterialKennzGroesse.Kennzart, dbo.KennzeichGroesse.Groesse" +
                      " FROM dbo.MaterialKennzGroesse INNER JOIN" +
                      " dbo.KennzeichGroesse ON dbo.MaterialKennzGroesse.Matnr = dbo.KennzeichGroesse.Matnr " +
                      " ORDER BY dbo.KennzeichGroesse.Position";



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
        /// IBAN prüfen und daraus Bankinfos ermitteln. Bapi: Z_FI_CONV_IBAN_2_BANK_ACCOUNT
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page"></param>
        public void ProofIBAN(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "ZLDCommon.ProofIBAN";
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
                    Bankkey = myProxy.getExportParameter("E_BANK_NUMBER");
                    Kontonr = myProxy.getExportParameter("E_BANK_ACCOUNT");
                    SWIFT = myProxy.getExportParameter("E_SWIFT");

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
        /// Laden der Fahrzeugarten aus SAP
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page"></param>
        public void getSAPFahrzeugarten(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "ZLDCommon.getSAPFahrzeugarten";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_DOMAENEN_WERTE", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_DOMNAME", "ZZLD_FAHRZ_ART");

                    myProxy.callBapi();

                    tblFahrzeugarten = myProxy.getExportTable("GT_WERTE");
                    tblFahrzeugarten.Rows.Add(0, "Fahrzeugart");
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            m_intStatus = -5555;
                            m_strMessage = "Keine Daten gefunden(Fahrzeugarten).";
                            break;
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
        /// Lädt Anzahl der angelegten Aufträge (für Anzeige in der Masterpage), statisch
        /// </summary>
        /// <param name="usr"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        private static string ermittleAnzahlAuftraege(CKG.Base.Kernel.Security.User usr, SqlConnection conn)
        {
            string menge;

            try
            {
                bool blnSendForAll = usr.Organization.OrganizationName.ToUpper().Contains(("SENDFORALL"));

                SqlCommand command = new SqlCommand();

                if (blnSendForAll)
                {
                    // Für Benutzer, deren Organisation das Tag "SendForAll" enthält, alle erfassten Aufträge des VkBurs bzw. der Gruppe anzeigen
                    command.CommandText = "SELECT COUNT(*) FROM  dbo.ZLDKopfTabelle " +
                        " INNER JOIN dbo.WebMember ON dbo.ZLDKopfTabelle.id_user = dbo.WebMember.UserID " +
                        " WHERE     (Filiale = @filiale) AND (GroupID = @GroupID) AND (abgerechnet = 0)";
                    command.Parameters.Add("@filiale", usr.Reference);
                    command.Parameters.Add("@GroupID", usr.GroupID);
                }
                else
                {
                    command.CommandText = "SELECT COUNT(*) FROM  dbo.ZLDKopfTabelle " +
                                    "WHERE     (id_user = @id_user) AND (abgerechnet = 0)";
                    command.Parameters.Add("@id_user", usr.UserID);
                }

                command.Connection = conn;
                menge = command.ExecuteScalar().ToString();
            }
            catch (Exception)
            {
                menge = "0";
                throw;
            }

            return menge;
        }

        /// <summary>
        /// Lädt Anzahl der angelegten Aufträge (für Anzeige in der Masterpage), ohne Parameter
        /// </summary>
        /// <returns></returns>
        public string getAnzahlAuftraege()
        {
            string menge;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"].ToString()))
                {
                    conn.Open();

                    menge = ermittleAnzahlAuftraege(m_objUser, conn);

                    conn.Close();
                }
            }
            catch (Exception)
            {
                menge = "0";
                throw;
            }

            return menge;
        }

        /// <summary>
        /// Lädt Anzahl der angelegten Aufträge (für Anzeige in der Masterpage), statisch, mit Übergabe von User und Connection
        /// </summary>
        /// <param name="usr"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static string getAnzahlAuftraege(CKG.Base.Kernel.Security.User usr, SqlConnection conn)
        {
            return ermittleAnzahlAuftraege(usr, conn);
        }

        /// <summary>
        /// Format- und Zulassungskreisprüfung eines Kennzeichens
        /// </summary>
        /// <param name="kennzeichen"></param>
        /// <returns>true, wenn Kennzeichen ok</returns>
        public bool checkKennzeichenformat(string kennzeichen)
        {
            if (String.IsNullOrEmpty(kennzeichen))
            {
                return false;
            }

            // Formatprüfung mit Regular Expression
            Regex expr = new Regex("[A-Z]{1,3}-[A-Z]{1,2}[0-9]{1,4}");
            if (!expr.IsMatch(kennzeichen))
            {
                return false;
            }

            // Prüfung Zulassungskreis
            string kreis = kennzeichen.Split('-')[0];
            DataRow[] kennzStamm = tblStvaStamm.Select("KREISKZ = '" + kreis + "'");
            if (kennzStamm.Length == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Füllen eines Javascript-Array mit Sonderkennzeichen
        /// </summary>
        public string GetSonderStvaJsArray()
        {
            System.Text.StringBuilder javaScript = new System.Text.StringBuilder();

            for (int i = 0; i < tblSonderStva.Rows.Count; i++)
            {
                if (i == 0)
                {
                    javaScript.Append("var ArraySonderStva = \n[\n");
                }

                DataRow dataRow = tblSonderStva.Rows[i];

                for (int j = 0; j < dataRow.Table.Columns.Count; j++)
                {
                    if (j == 0)
                    {
                        javaScript.Append(" [ ");
                    }

                    javaScript.Append("'" + dataRow[j].ToString().Trim() + "'");

                    if ((j + 1) == dataRow.Table.Columns.Count)
                    {
                        javaScript.Append(" ]");
                    }
                    else
                    {
                        javaScript.Append(",");
                    }
                }

                if ((i + 1) == tblSonderStva.Rows.Count)
                {
                    javaScript.Append("\n];\n");
                }
                else
                {
                    javaScript.Append(",\n");
                }
            }

            return javaScript.ToString();
        }

        /// <summary>
        /// Mailempfänger aus SQL-DB lesen
        /// </summary>
        /// <param name="InputVorgang"></param>
        /// <param name="MailAdress"></param>
        /// <param name="kundenId"></param>
        public static void LeseMailEmpfaenger(string InputVorgang, ref string MailAdress, string kundenId = null)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    MailAdress = "";
                    DataTable Mailings = new DataTable();

                    connection.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"].ToString();

                    using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM vwGetMailTexte WHERE KundenID=@KundenID " + "AND Vorgangsnummer Like @Vorgangsnummer " + "AND Aktiv=1", connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@KundenID", (kundenId ?? "1"));
                        adapter.SelectCommand.Parameters.AddWithValue("@Vorgangsnummer", InputVorgang);
                        adapter.Fill(Mailings);
                    }           

                    foreach (DataRow dRow in Mailings.Rows)
                    {
                        if (String.IsNullOrEmpty(MailAdress))
                        {
                            MailAdress = dRow["EmailAdresse"].ToString();
                        }
                        else
                        {
                            MailAdress += ";" + dRow["EmailAdresse"].ToString();
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        #endregion

        #region Helper
        /// <summary>
        /// Überprüft ob der eingegene Wert numerisch ist.
        /// </summary>
        /// <param name="Value">Eingabe</param>
        /// <returns>bool</returns>
        public static bool IsNumeric(string Value)
        {
            try
            {
                int dummy;
                return Int32.TryParse(Value, out dummy);
            }
            catch
            {
                return false;
            }
        } 
        #endregion
    }
}