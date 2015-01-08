using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using CkgDomainLogic.UserReporting.Models;
using GeneralTools.Log.Models.MultiPlatform;
using GeneralTools.Models;

namespace CkgDomainLogic.UserReporting.Services
{
    public class UserReportingDbContext : DbContext
    {
        protected string ConnectionString { get; private set; }

        public UserReportingDbContext(string connectionString)
            : base(connectionString)
        {
            ConnectionString = connectionString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<UserReportingDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        #region User-Auswertung

        public List<MpCustomer> GetCustomerList()
        {
            return Database.SqlQuery<MpCustomer>("SELECT * FROM Customer").OrderBy(c => c.CustomerDropdownName).ToList();
        }

        public List<UserInfo> GetUserList(MpCustomer kunde, bool nurNeueUser = false, bool nurGeloeschteUser = false, DateRange zeitraum = null)
        {
            bool mitZeitraum = (zeitraum != null && zeitraum.IsSelected);

            if (kunde != null && kunde.CustomerID > 0)
            {
                if (nurNeueUser && mitZeitraum)
                    return Database.SqlQuery<UserInfo>("SELECT * FROM vwWebUserWebMember WHERE CustomerID = {0} AND CreateDate BETWEEN {1} AND {2}", kunde.CustomerID, zeitraum.StartDate, zeitraum.EndDate).ToList();

                if (nurGeloeschteUser)
                {
                    if (mitZeitraum)
                        return Database.SqlQuery<UserInfo>("SELECT * FROM WebUserHistory WHERE Deleted = 1 AND Customername = {0} AND DeleteDate BETWEEN {1} AND {2}", kunde.Customername, zeitraum.StartDate, zeitraum.EndDate).ToList();

                    return Database.SqlQuery<UserInfo>("SELECT * FROM WebUserHistory WHERE Deleted = 1 AND Customername = {0}", kunde.Customername).ToList();
                }

                return Database.SqlQuery<UserInfo>("SELECT * FROM vwWebUserWebMember WHERE CustomerID = {0}", kunde.CustomerID).ToList();
            }

            if (nurNeueUser && mitZeitraum)
                return Database.SqlQuery<UserInfo>("SELECT * FROM vwWebUserWebMember WHERE CreateDate BETWEEN {0} AND {1}", zeitraum.StartDate, zeitraum.EndDate).ToList();

            if (nurGeloeschteUser)
            {
                if (mitZeitraum)
                    return Database.SqlQuery<UserInfo>("SELECT * FROM WebUserHistory WHERE Deleted = 1 AND DeleteDate BETWEEN {0} AND {1}", zeitraum.StartDate, zeitraum.EndDate).ToList();

                return Database.SqlQuery<UserInfo>("SELECT * FROM WebUserHistory WHERE Deleted = 1").ToList();
            }
                
            return Database.SqlQuery<UserInfo>("SELECT * FROM vwWebUserWebMember").ToList();
        }

        #endregion
    }
}
