using System.Collections.Generic;
using System.Configuration;
using CkgDomainLogic.UserReporting.Contracts;
using CkgDomainLogic.UserReporting.Models;
using GeneralTools.Log.Models.MultiPlatform;

namespace CkgDomainLogic.UserReporting.Services
{
    public class UserReportingDataServiceSql : IUserReportingDataService 
    {
        private UserReportingDbContext _dbContext;

        public UserReportingDataServiceSql()
        {
            _dbContext = new UserReportingDbContext(ConfigurationManager.AppSettings["Connectionstring"]);
        }

        public List<MpCustomer> GetCustomerList() { return _dbContext.GetCustomerList(); } 

        public List<UserInfo> GetWebUsers(WebUserSuchparameter suchparameter)
        {
            List<UserInfo> liste = new List<UserInfo>();

            switch (suchparameter.UserSelection)
            {
                case "A":
                    liste = _dbContext.GetUserList(suchparameter.Customer);
                    break;

                case "N":
                    liste = _dbContext.GetUserList(suchparameter.Customer, true, false, suchparameter.DatumRange);
                    break;

                case "G":
                    liste = _dbContext.GetUserList(suchparameter.Customer, false, true, suchparameter.DatumRange);
                    break;
            }

            return liste;
        }
    }
}
