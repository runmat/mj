using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using GeneralTools.Log.Models.MultiPlatform;
using GeneralTools.Models;

namespace LogMaintenance.Services
{
    public class BusinessDataCopyService
    {
        private static Action<string> _infoMessageAction;

        public static void CopyToLogsDb(Action<string> infoMessageAction)
        {
            _infoMessageAction = infoMessageAction;

            CopyToLogsDbForServer("Dev");
            CopyToLogsDbForServer("Test");
            CopyToLogsDbForServer("Prod");
        }

        private static void CopyToLogsDbForServer(string serverType = null) 
        {
            if (serverType.IsNullOrEmpty())
                return;

            CopyToLogsDb<MpWebUser>(serverType);
            CopyToLogsDb<MpCustomer>(serverType);
            CopyToLogsDb<MpApplicationTranslated>(serverType);
            
            Alert("");
        }

        private static void CopyToLogsDb<T>(string serverType) where T : class, new()
        {
            var tableName = typeof (T).Name;
            var tableAttribute = typeof (T).GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault();
            if (tableAttribute != null)
                tableName = tableAttribute.Name;

            var businessConnectionString = string.Format("Source{0}", serverType);
            var logsConnectionString = string.Format("Logs{0}", serverType);

            var businessDbContext = new MultiDbPlatformContext(businessConnectionString);
            var logsDbContext = new MultiDbPlatformContext(logsConnectionString);

            logsDbContext.Database.ExecuteSqlCommand("delete from " + tableName);
            logsDbContext.SaveChanges();

            var businessData = businessDbContext.GetData<T>(tableName);
            if (businessData.None())
                return;

            businessData.ToList().ForEach(m => logsDbContext.AddData(ModelMapping.Copy(m)));
            logsDbContext.SaveChanges();

            Alert(string.Format("{0}-Server: Successfully copied data for '{1}' !", serverType, tableName));
        }

        private static void Alert(string info)
        {
            if (_infoMessageAction != null)
                _infoMessageAction(info);
        }
    }
}
