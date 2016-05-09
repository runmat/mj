using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Business;
using System.Configuration;
using System.Data.SqlClient;
using GeneralTools.Models;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    public class clsDisposition : SapOrmBusinessBase
    {
        public string UserName { get; set; }
        public string ZulDat { get; set; }
        public bool AlleAemter { get; set; }
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
            var vorhandeneZuordnungen = LoadZuordnungenFromSql();
            foreach (var dispo in Dispositionen.Where(d => vorhandeneZuordnungen.ContainsKey(d.Amt)))
            {
                var dispoSql = vorhandeneZuordnungen[dispo.Amt];

                dispo.MobileUserId = dispoSql.MobileUserId;

                var fahrer = Fahrerliste.FirstOrDefault(f => f.UserId == dispo.MobileUserId);
                if (fahrer != null && !string.IsNullOrEmpty(fahrer.UserName))
                    dispo.MobileUserName = fahrer.UserName;
                else
                    dispo.MobileUserName = dispo.MobileUserId;

                dispo.Vorschuss = dispoSql.Vorschuss;
                dispo.VorschussBetrag = dispoSql.VorschussBetrag;
            }
        }

        private void LoadDisposFromSap()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_MOB_DISPO_GET_VG.Init(SAP);

                    Z_ZLD_MOB_DISPO_GET_VG.SetImportParameter_I_VKORG(SAP, VKORG);
                    Z_ZLD_MOB_DISPO_GET_VG.SetImportParameter_I_VKBUR(SAP, VKBUR);
                    Z_ZLD_MOB_DISPO_GET_VG.SetImportParameter_I_ZZZLDAT(SAP, ZulDat.ToNullableDateTime("dd.MM.yyyy"));
                    Z_ZLD_MOB_DISPO_GET_VG.SetImportParameter_I_FUNCTION(SAP, Modus);
                    Z_ZLD_MOB_DISPO_GET_VG.SetImportParameter_I_ALLE_AEMTER(SAP, AlleAemter.BoolToX());

                    CallBapi();

                    Dispositionen = AppModelMappings.Z_ZLD_MOB_DISPO_GET_VG_GT_VGANZ_To_AmtDispos.Copy(Z_ZLD_MOB_DISPO_GET_VG.GT_VGANZ.GetExportList(SAP)).OrderBy(d => d.Amt).ToList();
                });
        }

        private Dictionary<string, AmtDisposSql> LoadZuordnungenFromSql()
        {
            ClearError();

            var erg = new Dictionary<string, AmtDisposSql>();

            var connection = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

            try
            {
                var tmpTable = new DataTable();

                connection.Open();

                var command = connection.CreateCommand();
                var adapter = new SqlDataAdapter();

                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT Amt, Fahrer, ISNULL(Vorschuss, 0) AS Vorschuss, ISNULL(VorschussBetrag, 0) AS VorschussBetrag FROM dbo.ZLDDisposition " +
                                      "WHERE VkOrg = @VkOrg AND VkBur = @VkBur AND Modus = @Modus AND Datum = @Datum " +
                                      "ORDER BY Amt";

                command.Parameters.AddWithValue("@VkOrg", VKORG);
                command.Parameters.AddWithValue("@VkBur", VKBUR);
                command.Parameters.AddWithValue("@Modus", Modus);
                command.Parameters.AddWithValue("@Datum", ZulDat);

                adapter.SelectCommand = command;
                adapter.Fill(tmpTable);

                foreach (DataRow dRow in tmpTable.Rows)
                {
                    erg.Add(dRow["Amt"].ToString(), new AmtDisposSql(dRow["Amt"].ToString(), dRow["Fahrer"].ToString(), (bool)dRow["Vorschuss"], (decimal?)dRow["VorschussBetrag"]));
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
                    CleanUpDisposInSql();
                    Dispositionen.RemoveAll(d => !string.IsNullOrEmpty(d.MobileUserId) && string.IsNullOrEmpty(d.SaveError));
                }
            }
        }

        private void SaveChangesToSql()
        {
            ClearError();

            var connection = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

            try
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandType = CommandType.Text;

                foreach (var dispo in Dispositionen)
                {
                    // Insert bzw. Update in Sql-Tabelle
                    command.CommandText = "SELECT Amt FROM dbo.ZLDDisposition " +
                                      "WHERE VkOrg = @VkOrg AND VkBur = @VkBur AND Amt = @Amt AND Modus = @Modus AND Datum = @Datum ";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@VkOrg", VKORG);
                    command.Parameters.AddWithValue("@VkBur", VKBUR);
                    command.Parameters.AddWithValue("@Amt", dispo.Amt);
                    command.Parameters.AddWithValue("@Modus", Modus);
                    command.Parameters.AddWithValue("@Datum", ZulDat);
                    var tmpErg = command.ExecuteScalar();

                    if (tmpErg == null)
                    {
                        command.CommandText = "INSERT INTO dbo.ZLDDisposition " +
                                      "(Fahrer, Vorschuss, VorschussBetrag, VkOrg, VkBur, Amt, Modus, Datum) VALUES (@Fahrer, @Vorschuss, @VorschussBetrag, @VkOrg, @VkBur, @Amt, @Modus, @Datum) ";
                    }
                    else
                    {
                        command.CommandText = "UPDATE dbo.ZLDDisposition " +
                                      "SET Fahrer = @Fahrer, Vorschuss = @Vorschuss, VorschussBetrag = @VorschussBetrag WHERE VkOrg = @VkOrg AND VkBur = @VkBur AND Amt = @Amt AND Modus = @Modus AND Datum = @Datum ";
                    }
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Fahrer", dispo.MobileUserId);
                    command.Parameters.AddWithValue("@Vorschuss", dispo.Vorschuss);
                    command.Parameters.AddWithValue("@VorschussBetrag", dispo.VorschussBetrag);
                    command.Parameters.AddWithValue("@VkOrg", VKORG);
                    command.Parameters.AddWithValue("@VkBur", VKBUR);
                    command.Parameters.AddWithValue("@Amt", dispo.Amt);
                    command.Parameters.AddWithValue("@Modus", Modus);
                    command.Parameters.AddWithValue("@Datum", ZulDat);
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

                    Z_ZLD_MOB_DISPO_SET_USER.SetImportParameter_I_VKORG(SAP, VKORG);
                    Z_ZLD_MOB_DISPO_SET_USER.SetImportParameter_I_VKBUR(SAP, VKBUR);
                    Z_ZLD_MOB_DISPO_SET_USER.SetImportParameter_I_ZZZLDAT(SAP, ZulDat.ToNullableDateTime("dd.MM.yyyy"));
                    Z_ZLD_MOB_DISPO_SET_USER.SetImportParameter_I_FUNCTION(SAP, Modus);
                    Z_ZLD_MOB_DISPO_SET_USER.SetImportParameter_I_DISPO_USER(SAP, UserName);
                    Z_ZLD_MOB_DISPO_SET_USER.SetImportParameter_I_ALLE_AEMTER(SAP, AlleAemter.BoolToX());

                    // Nur die disponierten Ämter an SAP übergeben
                    var disposToSave = Dispositionen.Where(d => !string.IsNullOrEmpty(d.MobileUserId)).ToList();
                    SAP.ApplyImport(AppModelMappings.Z_ZLD_MOB_DISPO_SET_USER_GT_VGANZ_From_AmtDispos.CopyBack(disposToSave));

                    CallBapi();

                    var sapItems = Z_ZLD_MOB_DISPO_SET_USER.GT_VGANZ.GetExportList(SAP);

                    foreach (var dispo in disposToSave)
                    {
                        var savedDispo = sapItems.FirstOrDefault(s => s.AMT == dispo.Amt);

                        dispo.SaveError = (savedDispo != null && savedDispo.SUBRC != 0 ? savedDispo.MESSAGE : "");
                    }
                });
        }

        private void CleanUpDisposInSql()
        {
            ClearError();

            var connection = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);

            try
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandType = CommandType.Text;
                command.CommandText = "DELETE FROM dbo.ZLDDisposition " +
                                    "WHERE VkOrg = @VkOrg AND VkBur = @VkBur AND ISNULL(Fahrer,'') <> '' AND Modus = @Modus AND Datum = @Datum";

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@VkOrg", VKORG);
                command.Parameters.AddWithValue("@VkBur", VKBUR);
                command.Parameters.AddWithValue("@Modus", Modus);
                command.Parameters.AddWithValue("@Datum", ZulDat);
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
