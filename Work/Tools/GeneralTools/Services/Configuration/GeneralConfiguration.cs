using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GeneralTools.Services
{
    public class GeneralConfiguration
    {
        public static string GetConfigValue(string context, string keyName)
        {
            try
            {
                var cnn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);
                var cmd = cnn.CreateCommand();
                cmd.CommandText = "SELECT value FROM Config " +
                                  "WHERE Context = @Context AND [Key] = @Key";
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

        public static IDictionary<string, string> GetConfigAllServersValues(string context, string connectionString = null)
        {
            try
            {
                var cnn = new SqlConnection(connectionString ?? ConfigurationManager.AppSettings["Connectionstring"]);
                var cmd = cnn.CreateCommand();
                cmd.CommandText = "SELECT [key], value FROM ConfigAllServers " +
                                  "WHERE Context = @Context";
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
    }
}
