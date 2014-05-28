using System;
using System.Data;
using CKG.Base.Common;
using System.Data.SqlClient;
using CKG.Base.Business;
using System.Configuration;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Klasse für die Zulassungsdienstsuche und Lesen von E-Mail-Texten aus der SQL-DB.
    /// </summary>
    public class ZLD_Suche: CKG.Base.Business.DatenimportBase
    {

        #region "Properties"
        /// <summary>
        /// Selektionsparameter Zulassungspartner
        /// </summary>
        public String Zulassungspartner
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionsparameter Postleitzahl
        /// </summary>
        public String PLZ
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionsparameter Zulassungspartnernummer
        /// </summary>
        public String ZulassungspartnerNr
        {
            get;
            set;
        }
        /// <summary>
        /// Selektionsparameter Kreiskennzeichen
        /// </summary>
        public String Kennzeichen
        {
            get;
            set;
        }
        /// <summary>
        /// Rückgabetabelle Zulassungsdienstsuche
        /// </summary>
        public DataTable ResultRaw
        {
            get;
            set;
        }
        /// <summary>
        /// Rückgabetabelle mit den angelegten Mailtexten
        /// </summary>
        public DataTable Mailings
        {
            get;
            set;
        }
        /// <summary>
        /// Rückgabe Mailbody
        /// </summary>
        public String MailBody
        {
            get;
            set;
        }
        /// <summary>
        /// Rückgabe E-Mail-Adresse
        /// </summary>
        public String MailAdress
        {
            get;
            set;
        }
        /// <summary>
        /// Rückgabe E-Mail-Adresse-CC
        /// </summary>
        public String MailAdressCC
        {
            get;
            set;
        }
        /// <summary>
        /// Rückgabe Mail-Betreff
        /// </summary>
        public String Betreff
        {
            get;
            set;
        }
        #endregion
        #region "Methods"
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="objUser">Webuserobjekt</param>
        /// <param name="objApp">Applikationsobjekt</param>
        /// <param name="strFilename">Filename</param>
        public ZLD_Suche(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
        {}
        /// <summary>
        /// Funktion für die Zulassungsdienstsuche in SAP. Bapi: Z_M_BAPIRDZ
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Report33ZLD.aspx</param>
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
                  
                    myProxy.setImportParameter("IZKFZKZ", Kennzeichen);
                    myProxy.setImportParameter("IPOST_CODE1", PLZ);
                    myProxy.setImportParameter("INAME1", Zulassungspartner);
                    myProxy.setImportParameter("IREMARK", ZulassungspartnerNr);

                    myProxy.callBapi();

                    ResultRaw = new DataTable();
                    ResultRaw = myProxy.getExportTable("ITAB");
                    CreateOutPut(ResultRaw, strAppID);
                    m_tblResult.Columns.Add("Details", typeof(String));


                    foreach (DataRow tmpRow in m_tblResult.Rows)
	                    {
                		     tmpRow["Details"] = "Report30ZLD_2.aspx?ID=" + tmpRow["ID"].ToString();
	                    }
                    m_tblResult.Columns.Remove("ID");

                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -9999;
                            m_strMessage = m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            WriteLogEntry(false, "ZKFZKZ=" + Kennzeichen + ", POST_CODE1=" + PLZ + ", NAME1=" + Zulassungspartner + ", REMARK=" + ZulassungspartnerNr + ", " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message), ref m_tblResult, false);
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        /// <summary>
        /// Lesen von definierten E-Mailtexten aus der SQL-Datenbank
        /// </summary>
        /// <param name="InputVorgang">Vorgang</param>
        public void LeseMailTexte(String InputVorgang)
        {
            String strTempVorgang = InputVorgang;

            m_intStatus = 0;
            m_strMessage = "";
            MailAdressCC = "";
            MailAdress = "";
            Mailings = new DataTable();
            System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection();
            connection.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"];
            try
            {

                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM vwGetMailTexte WHERE KundenID=@KundenID " +
                                      "AND Vorgangsnummer Like @Vorgangsnummer " +
                                      "AND Aktiv=1",connection);



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
        
        #endregion
    }
}