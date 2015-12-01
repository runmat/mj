using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace GeneralTools.Services
{
    public class ApplicationConfiguration : IApplicationConfigurationProvider, ICustomerConfigurationProvider
    {
        public ILogonContextProvider LogonContextProvider { get; set; }

        public string GetApplicationConfigVal(string keyName, string appID, int customerID = 0, int groupID = 0)
        {
            return GetApplicationConfigValue(keyName, appID, customerID, groupID);
        }

        public void SetApplicationConfigVal(string keyName, string value, string appID, int customerID = 0, int groupID = 0, string connectionString = null)
        {
            SetApplicationConfigValue(keyName, value, appID, customerID, groupID, connectionString);
        }

        public string GetCustomerConfigVal(string keyName, int customerId)
        {
            return GetApplicationConfigValue(keyName, "0", customerId);
        }

        public void SetCustomerConfigVal(string keyName, string value, int customerId, string connectionString = null)
        {
            SetApplicationConfigValue(keyName, value, "0", customerId, 0, connectionString);
        }

        public string GetCurrentCustomerConfigVal(string keyName)
        {
            var appID = LogonContextProvider.GetLogonContext().AppID;
            return GetCustomerConfigVal(keyName, LogonContextProvider.GetLogonContext().CustomerID);
        }

        public string GetCurrentBusinessCustomerConfigVal(string keyName)
        {
            var appID = LogonContextProvider.GetLogonContext().AppID;
            return GetCustomerConfigVal(keyName, LogonContextProvider.GetLogonContext().KundenNr.ToInt());
        }

        public void SetCurrentCustomerConfigVal(string keyName, string value, string connectionString = null)
        {
            SetCustomerConfigVal(keyName, value, LogonContextProvider.GetLogonContext().CustomerID, connectionString);
        }

        public void SetCurrentBusinessCustomerConfigVal(string keyName, string value, string connectionString = null)
        {
            SetCustomerConfigVal(keyName, value, LogonContextProvider.GetLogonContext().KundenNr.ToInt(), connectionString);
        }

        private const string SQLClause = "AppID = @AppID AND ConfigKey = @ConfigKey AND CustomerID IN (1, @CustomerID) AND GroupID IN (0, @GroupID)";

        public static string GetApplicationConfigValue(string keyName, string appID, int customerID = 0, int groupID = 0)
        {
            try
            {
                var cnn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);
                var cmd = cnn.CreateCommand();
                cmd.CommandText = "SELECT * FROM ApplicationConfig " +
                                  "WHERE " + SQLClause;
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

        public static void SetApplicationConfigValue(string keyName, string value, string appID, int customerID = 0, int groupID = 0, string connectionString = null)
        {
            try
            {
                var cnn = new SqlConnection(connectionString ?? ConfigurationManager.AppSettings["Connectionstring"]);
                cnn.Open();

                var cmdInsert = cnn.CreateCommand();
                cmdInsert.CommandText = "if (not exists(select * from ApplicationConfig where " + SQLClause + ")) " +
                                        "   insert into ApplicationConfig " +
                                        "      (AppID, ConfigKey, CustomerID, GroupID, ConfigType) select " + 
                                        "      @AppID, @ConfigKey, @CustomerID, @GroupID, 'string'";
                cmdInsert.CommandType = CommandType.Text;
                cmdInsert.Parameters.AddWithValue("@AppID", appID);
                cmdInsert.Parameters.AddWithValue("@ConfigKey", keyName);
                cmdInsert.Parameters.AddWithValue("@CustomerID", customerID);
                cmdInsert.Parameters.AddWithValue("@GroupID", groupID);
                cmdInsert.ExecuteNonQuery();


                var cmd = cnn.CreateCommand();
                cmd.CommandText = "update ApplicationConfig " +
                                  "set ConfigValue = @ConfigValue " +
                                  "WHERE " + SQLClause;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@AppID", appID);
                cmd.Parameters.AddWithValue("@ConfigKey", keyName);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters.AddWithValue("@GroupID", groupID);
                cmd.Parameters.AddWithValue("@ConfigValue", value);
                cmd.ExecuteNonQuery();

                cnn.Close();
            }
            catch (Exception exception)
            {
            }
        }
    }
}
