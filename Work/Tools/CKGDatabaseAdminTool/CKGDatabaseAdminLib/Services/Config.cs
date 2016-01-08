using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using GeneralTools.Services;

namespace CKGDatabaseAdminLib.Services
{
    public class Config
    {
        private static readonly string MasterConnectionString = ((NameValueCollection)ConfigurationManager.GetSection("dbConnections"))["DAD Test (VMS026)"];

        public static NameValueCollection GetAllDbConnections()
        {
            var connectionStringDict = GeneralConfiguration.GetConfigAllServersValues("ConnectionString", MasterConnectionString);

            var coll = new NameValueCollection();
            connectionStringDict.ToList().ForEach(e => coll.Add(e.Key, e.Value));

            return coll;
        }
    }
}
