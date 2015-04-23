using System;
using System.Data;
using System.Data.SqlClient;
using CKG.Base.Business;
using System.Configuration;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    public class ZLD_Suche: SapOrmBusinessBase
    {
        #region "Properties"

        public DataTable tblResultRaw { get; set; }
        public DataTable Mailings { get; set; }
        public DataTable tblResult { get; private set; }

        public String Zulassungspartner { get; set; }
        public String PLZ { get; set; }
        public String ZulassungspartnerNr { get; set; }
        public String Kennzeichen { get; set; }
        public String MailBody { get; set; }
        public String MailAdress { get; set; }
        public String MailAdressCC { get; set; }
        public String Betreff { get; set; }    

        #endregion

        #region "Methods"

        public void Fill()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_M_BAPIRDZ.Init(SAP);

                    SAP.SetImportParameter("IZKFZKZ", Kennzeichen);
                    SAP.SetImportParameter("IPOST_CODE1", PLZ);
                    SAP.SetImportParameter("INAME1", Zulassungspartner);
                    SAP.SetImportParameter("IREMARK", ZulassungspartnerNr);

                    CallBapi();

                    tblResultRaw = SAP.GetExportTable("ITAB");

                    tblResult = tblResultRaw.Copy();
                    tblResult.Columns.Add("Details", typeof(String));
                    foreach (DataRow tmpRow in tblResult.Rows)
                    {
                        tmpRow["Details"] = "Report30ZLD_2.aspx?ID=" + tmpRow["ID"].ToString();
                    }
                    tblResult.Columns.Remove("ID");
                });
        }

        public void LeseMailTexte(int customerId, string InputVorgang)
        {
            String strTempVorgang = InputVorgang;

            ClearError();
            
            MailAdressCC = "";
            MailAdress = "";
            Mailings = new DataTable();
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM vwGetMailTexte WHERE KundenID=@KundenID " +
                                      "AND Vorgangsnummer Like @Vorgangsnummer " +
                                      "AND Aktiv=1",connection);

                adapter.SelectCommand.Parameters.AddWithValue("@KundenID", customerId);
                adapter.SelectCommand.Parameters.AddWithValue("@Vorgangsnummer", strTempVorgang);

                adapter.Fill(Mailings);

                if (Mailings.Rows.Count > 0)
                {
                    foreach (DataRow dRow in Mailings.Rows)
                    {
                        MailBody = dRow["Text"].ToString();
                        Betreff = dRow["Betreff"].ToString();
                        Boolean boolCC;
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
                    RaiseError(-9999, "Keine Mailvorlagen für diesen Kunden.");
                }
            }
            catch (Exception ex)
            {
                RaiseError(9999, "Fehler beim Laden der Mailadressen: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        
        #endregion
    }
}