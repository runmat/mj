using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Business;
using System.Configuration;
using System.Data.SqlClient;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    public class clsDisposition : SapOrmBusinessBase
    {
        public string UserName { get; set; }
        public string ZulDat { get; set; }
        public List<MobileUser> Fahrerliste { get; set; }
        public List<AmtDispos> Dispositionen { get; set; }

        /// <summary>
        /// 1 = nicht disponiert, 2 = bereits disponiert, 3 = bereits in Arbeit
        /// </summary>
        public string Modus { get; set; }

        public clsDisposition(string userReferenz, string userName)
        {
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);
            UserName = userName;

            ZulDat = DateTime.Today.AddDays(1).ToString("dd.MM.yyyy");
            Modus = "1";

            FillFahrerliste();
        }

        private void FillFahrerliste()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_MOB_GET_USER.Init(SAP, "I_VKORG, I_VKBUR", VKORG, VKBUR);

                    CallBapi();

                    Fahrerliste = AppModelMappings.Z_ZLD_MOB_GET_USER_GT_USER_To_MobileUser.Copy(Z_ZLD_MOB_GET_USER.GT_USER.GetExportList(SAP)).OrderBy(f => f.UserId).ToList();

                    Fahrerliste.Add(new MobileUser
                    {
                        UserId = "0",
                        UserName = "Bitte wählen Sie einen Fahrer..."
                    });
                });
        }
        
        public void LoadDispos()
        {
            // Zulassungskreise/Dispositionen aus SAP laden
            LoadDisposFromSap();
            // Ggf. vorhandene Zuordnungen aus SQL laden und verarbeiten
            Dictionary<string, string> vorhandeneZuordnungen = LoadZuordnungenFromSql();
            foreach (var dispo in Dispositionen)
            {
                if (vorhandeneZuordnungen.ContainsKey(dispo.Amt))
                {
                    dispo.MobileUserId = vorhandeneZuordnungen[dispo.Amt];

                    foreach (var fahrer in Fahrerliste)
                    {
                        if (fahrer.UserId == vorhandeneZuordnungen[dispo.Amt])
                        {
                            dispo.MobileUserName = fahrer.UserName;
                            break;
                        }
                    }
                    if (String.IsNullOrEmpty(dispo.MobileUserName))
                    {
                        dispo.MobileUserName = dispo.MobileUserId;
                    }
                }
            }
        }

        private void LoadDisposFromSap()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_MOB_DISPO_GET_VG.Init(SAP);

                    SAP.SetImportParameter("I_VKORG", VKORG);
                    SAP.SetImportParameter("I_VKBUR", VKBUR);
                    SAP.SetImportParameter("I_ZZZLDAT", ZulDat);
                    SAP.SetImportParameter("I_FUNCTION", Modus);

                    CallBapi();

                    Dispositionen = AppModelMappings.Z_ZLD_MOB_DISPO_GET_VG_GT_VGANZ_To_AmtDispos.Copy(Z_ZLD_MOB_DISPO_GET_VG.GT_VGANZ.GetExportList(SAP)).OrderBy(d => d.Amt).ToList();
                });
        }

        private Dictionary<string, string> LoadZuordnungenFromSql()
        {
            ClearError();

            Dictionary<string, string> erg = new Dictionary<string, string>();

            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

            try
            {
                DataTable tmpTable = new DataTable();

                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();


                command.CommandText = "SELECT Amt, Fahrer FROM dbo.ZLDDisposition " +
                                      "WHERE VkOrg = @VkOrg AND VkBur = @VkBur " +
                                      " ORDER BY Amt";

                command.Parameters.AddWithValue("@VkOrg", VKORG);
                command.Parameters.AddWithValue("@VkBur", VKBUR);

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
                RaiseError(9999, "Fehler beim Laden der Dispositionen aus Sql: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return erg;
        }

        public void SaveDispos(bool inSapSpeichern)
        {
            SaveChangesToSql();

            if (inSapSpeichern)
            {
                SaveDisposToSap();
                if (!ErrorOccured)
                {
                    // Wenn Übernahme nach SAP erfolgreich -> Zuordnungen aus Sql löschen
                    RemoveChangesFromSql(true);
                }
            }
        }

        private void SaveChangesToSql()
        {
            ClearError();

            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

            try
            {
                SqlCommand command = new SqlCommand();

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                foreach (var dispo in Dispositionen)
                {
                    // Insert bzw. Update in Sql-Tabelle
                    command.CommandText = "SELECT Amt FROM dbo.ZLDDisposition " +
                                      "WHERE VkOrg = @VkOrg AND VkBur = @VkBur AND Amt = @Amt ";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@VkOrg", VKORG);
                    command.Parameters.AddWithValue("@VkBur", VKBUR);
                    command.Parameters.AddWithValue("@Amt", dispo.Amt);
                    object tmpErg = command.ExecuteScalar();

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
                    command.Parameters.AddWithValue("@Fahrer", dispo.MobileUserId);
                    command.Parameters.AddWithValue("@VkOrg", VKORG);
                    command.Parameters.AddWithValue("@VkBur", VKBUR);
                    command.Parameters.AddWithValue("@Amt", dispo.Amt);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                RaiseError(9999, "Fehler beim Speichern der Dispositionen in Sql: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void SaveDisposToSap()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_MOB_DISPO_SET_USER.Init(SAP);

                    SAP.SetImportParameter("I_VKORG", VKORG);
                    SAP.SetImportParameter("I_VKBUR", VKBUR);
                    SAP.SetImportParameter("I_ZZZLDAT", ZulDat);
                    SAP.SetImportParameter("I_FUNCTION", Modus);
                    SAP.SetImportParameter("I_DISPO_USER", UserName);

                    // Nur die disponierten Ämter an SAP übergeben
                    var disposToSave = Dispositionen.Where(d => !String.IsNullOrEmpty(d.MobileUserId));
                    SAP.ApplyImport(AppModelMappings.Z_ZLD_MOB_DISPO_SET_USER_GT_VGANZ_From_AmtDispos.CopyBack(disposToSave));

                    CallBapi();
                });
        }

        private void RemoveChangesFromSql(bool nurDisponierte)
        {
            ClearError();

            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

            try
            {
                SqlCommand command = new SqlCommand();

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
                command.Parameters.AddWithValue("@VkOrg", VKORG);
                command.Parameters.AddWithValue("@VkBur", VKBUR);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                RaiseError(9999, "Fehler beim Entfernen der Dispositionen aus Sql: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
