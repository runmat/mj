using System.Collections.Generic;
using CkgDomainLogic.UserReporting.Models;
using GeneralTools.Log.Models.MultiPlatform;

namespace CkgDomainLogic.UserReporting.Contracts
{
    public interface IUserReportingDataService
    {
        List<MpCustomer> GetCustomerList();

        List<UserInfo> GetWebUsers(WebUserSuchparameter suchparameter);
    }
}
