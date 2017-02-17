using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;

namespace MyBoss
{
    public partial class App 
    {
        private const string DbCategoryID = "ResAhw";
        private const string DbUserName = "JenzenMvc";
        private const int DisableRebootDelaySecondsSinceLastSystemStartup = 300;  // 5 minutes

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length > 0 && e.Args[0].ToLower() == "reboot")
            {
                CheckForReboot();
                ApplicationQuit();

                return;
            }

            base.OnStartup(e);
        }

        static void CheckForReboot()
        {
            var secondsSinceLastSystemStartup = GetSecondsSinceLastSystemStartup();
            if (secondsSinceLastSystemStartup < DisableRebootDelaySecondsSinceLastSystemStartup)
                return;

            var rebootRequestAvailable = (GetResAhwValueFromDb() == "1");
            if (!rebootRequestAvailable)
                return;

            ResetResAhwValueToDb();
            SystemReboot();
        }

        private void ApplicationQuit()
        {
            // application shutdown only
            Shutdown(0);
        }

        static void SystemReboot()
        {
            Process.Start("shutdown.exe", "-r -f -t 00");
        }

        static int GetSecondsSinceLastSystemStartup()
        {
            var t = TimeSpan.FromMilliseconds(Environment.TickCount);
            return (int)t.TotalSeconds;
        }

        static string GetResAhwValueFromDb()
        {
            try
            {
                var cnn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);
                var cmd = cnn.CreateCommand();
                cmd.CommandText = $"SELECT SettingsValue FROM CategorySettingsWebUser WHERE CategoryID = '{DbCategoryID}' and UserName = '{DbUserName}'";
                cmd.CommandType = CommandType.Text;

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

        static void ResetResAhwValueToDb()
        {
            try
            {
                var cnn = new SqlConnection(ConfigurationManager.AppSettings["Connectionstring"]);
                var cmd = cnn.CreateCommand();
                cmd.CommandText = $"update CategorySettingsWebUser set SettingsValue='0' WHERE CategoryID = '{DbCategoryID}' and UserName = '{DbUserName}'";
                cmd.CommandType = CommandType.Text;

                cnn.Open();
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
