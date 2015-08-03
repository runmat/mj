using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CkgDomainLogic.General.Database.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.General.Services
{
    public class DashboardSqlDbContext : DbContext
    {
        public DbSet<DashboardItemUser> DashboardItemsUser { get; set; }

        public DashboardSqlDbContext()
            : base(ConfigurationManager.AppSettings["Connectionstring"])
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            System.Data.Entity.Database.SetInitializer<DashboardSqlDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public IEnumerable<DashboardItem> GetDashboardItems()
        {
            return Database.SqlQuery<DashboardItem>("SELECT * FROM DashboardItem where ChartJsonOptions is not null order by Sort");
        }


        public DashboardItemUser GetDashboardItemUser(string userName)
        {
            return DashboardItemsUser.FirstOrDefault(item => item.UserName == userName) 
                    ?? DashboardItemsUser.Add(new DashboardItemUser { UserName = userName });
        }

        public IEnumerable<DashboardItemUser> DashboardItemsUserGet(string userName)
        {
            var item = GetDashboardItemUser(userName);

            if (item.ItemsXml == null)
                return new List<DashboardItemUser>();

            return XmlService.XmlDeserializeFromString<List<DashboardItemUser>>(item.ItemsXml);
        }

        public void DashboardItemsUserSave(string userName, IEnumerable<DashboardItemUser> userItems)
        {
            var item = GetDashboardItemUser(userName);

            item.ItemsXml = XmlService.XmlSerializeToString(userItems);
            SaveChanges();
        }
    }
}
