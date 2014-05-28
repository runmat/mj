using System;
using System.Collections.Generic;
using System.Data;
using CKG.Base.Common;
using CKG.Base.Business;
using System.Configuration;
using System.Data.SqlClient;

namespace AppZulassungsdienst.lib
{
    public class clsDisposition : CKG.Base.Business.BankBase
    {
        public string VkOrg { get; set; }
        public string VkBur { get; set; }
        public string ZulDat { get; set; }
        public DataTable Fahrerliste { get; set; }
        public DataTable Dispositionen { get; set; }

        public clsDisposition(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, String strAppID, String strSessionID, string strFilename, System.Web.UI.Page page)
            : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename )
        {
            if ((objUser != null) && (!String.IsNullOrEmpty(objUser.Reference)))
            {
                if (objUser.Reference.Length > 4)
                {
                    VkOrg = objUser.Reference.Substring(0, 4);
                    VkBur = objUser.Reference.Substring(4);
                }
                else
                {
                    VkOrg = objUser.Reference;
                    VkBur = "";
                }
            }
            else
            {
                VkOrg = "";
                VkBur = "";
            }

            ZulDat = DateTime.Today.AddDays(1).ToString("dd.MM.yyyy");

            FillFahrerliste(strAppID, strSessionID, page);
        }

        private void FillFahrerliste(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Disposition.FillFahrerliste";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    Fahrerliste = new DataTable();
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_MOB_GET_USER", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKORG", VkOrg);
                    myProxy.setImportParameter("I_VKBUR", VkBur);

                    myProxy.callBapi();

                    Fahrerliste = myProxy.getExportTable("GT_USER");

                    // "Leere" Auswahl hinzufügen
                    DataRow aRow = Fahrerliste.NewRow();
                    aRow["MOBUSER"] = "0";
                    aRow["NAME"] = "Bitte wählen Sie einen Fahrer...";
                    Fahrerliste.Rows.Add(aRow);
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

        public override void Show() {}

        public override void Change() {}
        
        /// <summary>
        /// Lädt die aktuellen Zulassungskreise/Dispositionen aus SAP und SQL
        /// </summary>
        /// <param name="strAppID"></param>
        /// <param name="strSessionID"></param>
        /// <param name="page"></param>
        public void LoadDispos(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            string amt = "";

            // Zulassungskreise/Dispositionen aus SAP laden
            LoadDisposFromSap(strAppID, strSessionID, page);
            // Ggf. vorhandene Zuordnungen aus SQL laden und verarbeiten
            Dictionary<string, string> vorhandeneZuordnungen = LoadZuordnungenFromSql();
            foreach (DataRow dRow in Dispositionen.Rows)
            {
                amt = dRow["AMT"].ToString();
                if (vorhandeneZuordnungen.ContainsKey(amt))
                {
                    dRow["MOBUSER"] = vorhandeneZuordnungen[amt];
                    foreach (DataRow fRow in Fahrerliste.Rows)
                    {
                        if (fRow["MOBUSER"].ToString() == vorhandeneZuordnungen[amt])
                        {
                            dRow["NAME"] = fRow["NAME"].ToString();
                            break;
                        }
                    }
                    if (String.IsNullOrEmpty(dRow["NAME"].ToString()))
                    {
                        dRow["NAME"] = dRow["MOBUSER"].ToString();
                    }
                }
            }
        }

        private void LoadDisposFromSap(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Disposition.GetDispositionen";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    Dispositionen = new DataTable();
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_MOB_DISPO_GET_VG", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKORG", VkOrg);
                    myProxy.setImportParameter("I_VKBUR", VkBur);
                    myProxy.setImportParameter("I_ZZZLDAT", ZulDat);

                    myProxy.callBapi();

                    Dispositionen = myProxy.getExportTable("GT_VGANZ");
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

        private Dictionary<string, string> LoadZuordnungenFromSql()
        {
            Dictionary<string, string> erg = new Dictionary<string, string>();
            m_intStatus = 0;
            m_strMessage = String.Empty;

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"];

            try
            {
                DataTable tmpTable = new DataTable();

                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();


                command.CommandText = "SELECT Amt, Fahrer FROM dbo.ZLDDisposition " +
                                      "WHERE VkOrg = @VkOrg AND VkBur = @VkBur " +
                                      " ORDER BY Amt";

                command.Parameters.AddWithValue("@VkOrg", VkOrg);
                command.Parameters.AddWithValue("@VkBur", VkBur);

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                adapter.SelectCommand = command;
                adapter.Fill(tmpTable);

                foreach (DataRow dRow in tmpTable.Rows)
                {
                    erg.Add(dRow["Amt"].ToString(), dRow["Fahrer"].ToString());
                }
            }
            catch (Exception ex)
            {
                m_intStatus = 9999;
                m_strMessage = "Fehler beim Laden der Dispositionen aus Sql: " + ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return erg;
        }

        /// <summary>
        /// Speichert die Dispositionen in Sql und ggf. SAP
        /// </summary>
        /// <param name="strAppID"></param>
        /// <param name="strSessionID"></param>
        /// <param name="page"></param>
        /// <param name="inSapSpeichern"></param>
        public void SaveDispos(String strAppID, String strSessionID, System.Web.UI.Page page, bool inSapSpeichern)
        {
            SaveChangesToSql();
            if (inSapSpeichern)
            {
                SaveDisposToSap(strAppID, strSessionID, page);
                if (m_intStatus == 0)
                {
                    // Wenn Übernahme nach SAP erfolgreich -> Zuordnungen aus Sql löschen
                    RemoveChangesFromSql(true);
                }
            }
        }

        private void SaveChangesToSql()
        {
            m_intStatus = 0;
            m_strMessage = String.Empty;
            object tmpErg = null;

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"];

            try
            {
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                foreach (DataRow dRow in Dispositionen.Rows)
                {
                    // Insert bzw. Update in Sql-Tabelle
                    command.CommandText = "SELECT Amt FROM dbo.ZLDDisposition " +
                                      "WHERE VkOrg = @VkOrg AND VkBur = @VkBur AND Amt = @Amt ";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@VkOrg", VkOrg);
                    command.Parameters.AddWithValue("@VkBur", VkBur);
                    command.Parameters.AddWithValue("@Amt", dRow["AMT"].ToString());
                    tmpErg = command.ExecuteScalar();

                    if (tmpErg == null)
                    {
                        command.CommandText = "INSERT INTO dbo.ZLDDisposition " +
                                      "(Fahrer, VkOrg, VkBur, Amt) VALUES (@Fahrer, @VkOrg, @VkBur, @Amt) ";
                    }
                    else
                    {
                        command.CommandText = "UPDATE dbo.ZLDDisposition " +
                                      "SET Fahrer = @Fahrer WHERE VkOrg = @VkOrg AND VkBur = @VkBur AND Amt = @Amt ";
                    }
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Fahrer", dRow["MOBUSER"].ToString());
                    command.Parameters.AddWithValue("@VkOrg", VkOrg);
                    command.Parameters.AddWithValue("@VkBur", VkBur);
                    command.Parameters.AddWithValue("@Amt", dRow["AMT"].ToString());
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                m_intStatus = 9999;
                m_strMessage = "Fehler beim Speichern der Dispositionen in Sql: " + ex.Message;
            }
            finally
            {
                connection.Close();
            }
        }

        private void SaveDisposToSap(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Dispositionen.SetDispositionen";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            DataTable tblSAP;
            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_MOB_DISPO_SET_USER", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKORG", VkOrg);
                    myProxy.setImportParameter("I_VKBUR", VkBur);
                    myProxy.setImportParameter("I_ZZZLDAT", ZulDat);

                    tblSAP = myProxy.getImportTable("GT_VGANZ");

                    foreach (DataRow tmpRow in Dispositionen.Rows)
                    {
                        // Nur die disponierten Ämter an SAP übergeben
                        if (!String.IsNullOrEmpty(tmpRow["MOBUSER"].ToString()))
                        {
                            DataRow tmpSAPRow = tblSAP.NewRow();
                            tmpSAPRow["AMT"] = tmpRow["AMT"].ToString();
                            tmpSAPRow["KREISBEZ"] = tmpRow["KREISBEZ"].ToString();
                            tmpSAPRow["VG_ANZ"] = tmpRow["VG_ANZ"].ToString();
                            tmpSAPRow["MOBUSER"] = tmpRow["MOBUSER"].ToString().ToUpper();
                            tmpSAPRow["NAME"] = tmpRow["NAME"].ToString();
                            tblSAP.Rows.Add(tmpSAPRow);
                        }
                    }

                    myProxy.callBapi();

                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
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
                            m_strMessage = m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }

        private void RemoveChangesFromSql(bool nurDisponierte)
        {
            m_intStatus = 0;
            m_strMessage = String.Empty;

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.AppSettings["Connectionstring"];

            try
            {
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                command.CommandText = "DELETE FROM dbo.ZLDDisposition " +
                                    "WHERE VkOrg = @VkOrg AND VkBur = @VkBur ";
                if (nurDisponierte)
                {
                    command.CommandText += "AND ISNULL(Fahrer,'') <> '' ";
                }
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@VkOrg", VkOrg);
                command.Parameters.AddWithValue("@VkBur", VkBur);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                m_intStatus = 9999;
                m_strMessage = "Fehler beim Entfernen der Dispositionen aus Sql: " + ex.Message;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
