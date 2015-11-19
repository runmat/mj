using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using GeneralTools.Contracts;

namespace GeneralTools.Services
{
    public class GeneralConfiguration : IGeneralConfigurationProvider
    {
        public string GetConfigVal(string context, string keyName)
        {
            return GetConfigValue(context, keyName);
        }

        public void SetConfigVal(string context, string keyName, string value)
        {
            SetConfigValue(context, keyName, value);
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

        public static void SetConfigValue(string context, string keyName, string value)
        {
            try
            {
                var cnn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);
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
            catch (Exception exception)
            {
            }
        }
    }
}
