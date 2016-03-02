using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CkgImportLocalizationResourcesFromDb
{
    public class GeneralConfiguration 
    {
        public string GetConfigVal(string context, string keyName)
        {
            return GetConfigValue(context, keyName);
        }

        public IDictionary<string, string> GetConfigAllServersVals(string context, string connectionString = null, string filterClause = null)
        {
            return GetConfigAllServersValues(context, connectionString, filterClause);
        }

        public void SetConfigVal(string context, string keyName, string value, string connectionString = null)
        {
            SetConfigValue(context, keyName, value, connectionString);
        }

        private const string SQLClause = "Context = @Context AND [Key] = @Key";

        public static string GetConfigValue(string context, string keyName)
        {
            try
            {
                var cnn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);
                var cmd = cnn.CreateCommand();
                cmd.CommandText = "SELECT value FROM Config " +
                                  "WHERE " + SQLClause;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Context", context);
                cmd.Parameters.AddWithValue("@Key", keyName);

                cnn.Open();
                var erg = cmd.ExecuteScalar();
                cnn.Close();

                if (erg != null)
                    return erg.ToString();

                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static IDictionary<string, string> GetConfigAllServersValues(string context, string connectionString = null, string filterClause = null)
        {
            try
            {
                var cnn = new SqlConnection(connectionString ?? ConfigurationManager.AppSettings["Connectionstring"]);
                var cmd = cnn.CreateCommand();
                cmd.CommandText = "SELECT [key], value FROM ConfigAllServers " +
                                  "WHERE Context = @Context" +
                                  (filterClause == null ? "" : " AND " + filterClause);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Context", context);

                cnn.Open();
                var dr = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Load(dr);
                cnn.Close();

                if (dt.Rows.Count > 0)
                    return dt.Rows.OfType<DataRow>().ToDictionary(row => row[0].ToString(), row => row[1].ToString().Replace("******", "seE?Anemone"));

                return new Dictionary<string, string>();
            }
            // ReSharper disable once UnusedVariable
            catch (Exception e)
            {
                return new Dictionary<string, string>();
            }
        }

        public static void SetConfigValue(string context, string keyName, string value, string connectionString = null)
        {
            try
            {
                var cnn = new SqlConnection(connectionString ?? ConfigurationManager.AppSettings["Connectionstring"]);
                cnn.Open();

                var cmdInsert = cnn.CreateCommand();
                cmdInsert.CommandText = "if (not exists(select * from Config where " + SQLClause + ")) " +
                                        "   insert into Config " +
                                        "      (Context, [Key], Value) select " +
                                        "      @Context, @Key, ''";
                cmdInsert.CommandType = CommandType.Text;
                cmdInsert.Parameters.AddWithValue("@Context", context);
                cmdInsert.Parameters.AddWithValue("@Key", keyName);
                cmdInsert.ExecuteNonQuery();


                var cmd = cnn.CreateCommand();
                cmd.CommandText = "update Config " +
                                  "set Value = @Value " +
                                  "WHERE " + SQLClause;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Context", context);
                cmd.Parameters.AddWithValue("@Key", keyName);
                cmd.Parameters.AddWithValue("@Value", value);
                cmd.ExecuteNonQuery();

                cnn.Close();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            // ReSharper disable once UnusedVariable
            catch (Exception e)
            {
            }
        }
    }
}
