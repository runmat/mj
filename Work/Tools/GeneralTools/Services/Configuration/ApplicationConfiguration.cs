using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using GeneralTools.Contracts;

namespace GeneralTools.Services
{
    public class ApplicationConfiguration : IApplicationConfigurationProvider, ICustomerConfigurationProvider
    {
        public ILogonContextProvider LogonContextProvider { get; set; }

        public string GetApplicationConfigVal(string keyName, string appID, int customerID = 0, int groupID = 0)
        {
            return GetApplicationConfigValue(keyName, appID, customerID, groupID);
        }

        public string GetCustomerConfigVal(string keyName, int customerId)
        {
            return GetApplicationConfigValue(keyName, "0", customerId);
        }

        public string GetCurrentCustomerConfigVal(string keyName)
        {
            return GetApplicationConfigValue(keyName, "0", LogonContextProvider.GetLogoncontext().CustomerID);
        }

        public static string GetApplicationConfigValue(string keyName, string appID, int customerID = 0, int groupID = 0)
        {
            try
            {
                var cnn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);
                var cmd = cnn.CreateCommand();
                cmd.CommandText = "SELECT * FROM ApplicationConfig " +
                                  "WHERE AppID = @AppID AND ConfigKey = @ConfigKey AND CustomerID IN (1, @CustomerID) AND GroupID IN (0, @GroupID)";
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

                    if (customerID > 1 && custRows.Length > 0)
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
                        // generelle Einstellung (Firma 1)
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
