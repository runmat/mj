using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using CkgDomainLogic.Logs.Models;
using GeneralTools.Log.Models.MultiPlatform;

namespace CkgDomainLogic.Logs.Services
{
    public class LogsSqlDbContext : DbContext
    {
        protected string ConnectionString { get; private set; }

        public DbSet<SapLogItem> SapLogItems { get; set; }

        public DbSet<MpApplicationTranslated> MpApplicationsTranslated { get; set; }

        public DbSet<MpWebUser> MpWebUsers { get; set; }

        public DbSet<MpCustomer> MpCustomers { get; set; }


        public IEnumerable<SapLogItem> GetSapLogItems(SapLogItemSelector sapLogItemSelector)
        {
            return Database.SqlQuery<SapLogItem>(sapLogItemSelector.GetSqlSelectStatement());
        }

        public SapLogItemDetailed GetSapLogItemDetailed(int id)
        {
            return Database.SqlQuery<SapLogItemDetailed>(string.Format("select Id, ImportParameters, ImportTables, ExportParameters, ExportTables from SapBapi where Id = {0}", id)).FirstOrDefault();
        }
        
        
        public IEnumerable<PageVisitLogItem> GetPageVisitLogItems(PageVisitLogItemSelector pageVisitLogItemSelector)
        {
            return Database.SqlQuery<PageVisitLogItem>(pageVisitLogItemSelector.GetSqlSelectStatement());
        }


        public IEnumerable<WebServiceTrafficLogItem> GetWebServiceTrafficLogItems(WebServiceTrafficLogItemSelector webServiceTrafficLogItemSelector)
        {
            return Database.SqlQuery<WebServiceTrafficLogItem>(webServiceTrafficLogItemSelector.GetSqlSelectStatement());
        }

        public IEnumerable<WebServiceTrafficLogTable> GetWebServiceTrafficLogTables()
        {
            return Database.SqlQuery<WebServiceTrafficLogTable>("SELECT DISTINCT TABLE_NAME, TABLE_COMMENT FROM information_schema.tables WHERE TABLE_NAME LIKE 'WebServiceTraffic%' AND IFNULL(TABLE_COMMENT, '') <> ''");
        }


        public LogsSqlDbContext(string connectionString) 
            : base(connectionString)
        {
            ConnectionString = connectionString; 
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<LogsSqlDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
