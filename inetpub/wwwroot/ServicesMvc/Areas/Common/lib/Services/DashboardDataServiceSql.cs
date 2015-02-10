// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Contracts;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class DashboardDataServiceSql : IDashboardDataService 
    {
        public IEnumerable<IDashboardItem> GetDashboardItems()
        {
            var ct = CreateDbContext();

            return ct.GetDashboardItems();
        }

        public IEnumerable<IDashboardItemUser> GetDashboardItemsUser()
        {
            return null;
        }

        public void SaveDashboardItemsUser(IEnumerable<IDashboardItemUser> userItems)
        {
            
        }

        private static DashboardSqlDbContext CreateDbContext()
        {
            return new DashboardSqlDbContext();
        }
    }
}
