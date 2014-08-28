using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GeneralTools.Services
{
    public class ApplicationConfiguration
    {
        public static string GetApplicationConfigValue(string keyName, string appID, int customerID = 0, int groupID = 0)
        {
            try
            {
                var cnn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);
                var cmd = cnn.CreateCommand();
                cmd.CommandText = "SELECT * FROM ApplicationConfig " +
                                  "WHERE AppID = @AppID AND ConfigKey = @ConfigKey AND (CustomerID = @CustomerID OR CustomerID = 0) AND (GroupID = @GroupID OR GroupID = 0)";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@AppID", appID);
                cmd.Parameters.AddWithValue("@ConfigKey", keyName);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters.AddWithValue("@GroupID", groupID);

                cnn.Open();
                var table = new DataTable();
                table.Load(cmd.ExecuteReader());
                cnn.Close();

                if (table.Rows.Count > 0)
                {
                    var custRows = table.Select("CustomerID = " + customerID);

                    if (customerID != 0 && custRows.Length > 0)
                    {
                        // kundenspezifische Einstellung
                        var grpRows = table.Select("CustomerID = " + customerID + " AND GroupID = " + groupID);

                        if (groupID != 0 && grpRows.Length > 0)
                        {
                            // gruppenspezifische Einstellung
                            return grpRows[0]["ConfigValue"].ToString();
                        }
                        else
                        {
                            // generelle Kunden-Einstellung (GroupID 0)
                            return custRows[0]["ConfigValue"].ToString();
                        }
                    }
                    else
                    {
                        // generelle Einstellung (CustomerID 0)
                        return table.Rows[0]["ConfigValue"].ToString();
                    }
                }

                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
