using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace CKGDatabaseAdminLib
{
    public static class DataBaseContextHelper
    {
        public static Dictionary<string, DatabaseContext> DatabaseContexts { get; private set; }

        public static void Init(IEnumerable<string> connections)
        {
            DatabaseContexts = new Dictionary<string, DatabaseContext>();

            var sectionData = (NameValueCollection)ConfigurationManager.GetSection("dbConnections");

            foreach (var conn in connections)
            {
                DatabaseContexts.Add(conn, new DatabaseContext(sectionData.Get(conn)));
            }
        }
    }
}
