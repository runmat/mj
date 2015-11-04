using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CkgDomainLogic.General.Services
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
    }
}
