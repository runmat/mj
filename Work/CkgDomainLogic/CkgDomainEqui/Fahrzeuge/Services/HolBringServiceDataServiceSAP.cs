// ReSharper disable RedundantUsingDirective
// ReSharper disable AccessToForEachVariableInClosure

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Database.Services;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class HolBringServiceDataServiceSAP : CkgGeneralDataServiceSAP, IHolBringServiceDataService
    {
        public string Test()
        {
            throw new NotImplementedException();
        }

        public List<Domaenenfestwert> GetFahrzeugarten { get { return PropertyCacheGet(() => LoadFahrzeugartenFromSap().ToList()); } }
        public List<Domaenenfestwert> GetUsers { get { return PropertyCacheGet(() => LoadUserList().ToList()); } }

        public string GetUsername { get { return (LogonContext).User.Username; } }
        public string GetUserTel { get { return (LogonContext).UserInfo.Telephone; } }

        //public string GetUsername()
        //{
        //    // return ((ILogonContextDataService)LogonContext).Customer.AccountingArea.ToString();
        //    //var iGroup = ((ILogonContextDataService)LogonContext).Organization.OrganizationName;
        //    //var iVkOrg = ((ILogonContextDataService)LogonContext).Customer.AccountingArea.ToString();
        //    //var iVkBur = ((ILogonContextDataService)LogonContext).Organization.OrganizationReference2;
        //    var asdf = (LogonContext).UserInfo.Telephone;
        //    // return (LogonContext).Customer.Customername;
        //    return (LogonContext).User.Username;
        //}

        public HolBringServiceDataServiceSAP(ISapDataService sap)
            :base(sap)
        {
        }

        private IEnumerable<Domaenenfestwert> LoadFahrzeugartenFromSap()
        {
            var sapList = Z_DPM_DOMAENENFESTWERTE.GT_WEB.GetExportListWithInitExecute(SAP, "DOMNAME, DDLANGUAGE, SORTIEREN", "ZZLD_FAHRZ_ART", "DE", "X");

            return DomainCommon.Models.AppModelMappings.Z_DPM_DOMAENENFESTWERTE_GT_WEB_To_Domaenenfestwert.Copy(sapList);
        }

        public IEnumerable<Domaenenfestwert> LoadUserList()
        {
            using (var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], LogonContext.UserName))
            {
                var test = dbContext.UserGroupsOfCurrentCustomer;
                return null;
            }
        }
    }
}
