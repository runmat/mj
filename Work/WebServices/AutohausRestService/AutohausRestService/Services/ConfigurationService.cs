using System.Configuration;

namespace AutohausRestService.Services
{
    public class ConfigurationService
    {
        public static string DbConnection { get { return ConfigurationManager.AppSettings["Connectionstring"]; } }

        public static string LogTableName { get { return ConfigurationManager.AppSettings["LogTableName"]; } }
    }
}