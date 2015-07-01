﻿// ReSharper disable RedundantUsingDirective
// ReSharper disable AccessToForEachVariableInClosure

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.Services;
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
        public List<Domaenenfestwert> GetFahrzeugarten { get { return PropertyCacheGet(() => LoadFahrzeugartenFromSap().ToList()); } }
        public List<Domaenenfestwert> GetUsers { get { return PropertyCacheGet(() => LoadUserList().ToList()); } }

        public List<Kunde> Kunden { get { return PropertyCacheGet(() => LoadKundenFromSap().ToList()); } }

        public string GetUsername { get { return (LogonContext).User.Username; } }
        public string GetUserTel { get { return (LogonContext).UserInfo.Telephone; } }

        public IEnumerable<Kunde> LoadKundenFromSap()
        {
            Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.Init(SAP);

            var orgRef = ((ILogonContextDataService)LogonContext).Organization.OrganizationReference;

            SAP.SetImportParameter("I_KUNNR", (string.IsNullOrEmpty(orgRef) ? LogonContext.KundenNr.ToSapKunnr() : orgRef.ToSapKunnr()));
            SAP.SetImportParameter("I_VKORG", ((ILogonContextDataService)LogonContext).Customer.AccountingArea.ToString());
            SAP.SetImportParameter("I_VKBUR", ((ILogonContextDataService)LogonContext).Organization.OrganizationReference2);
            SAP.SetImportParameter("I_SPART", "01");

            var sapList = Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB.GetExportListWithExecute(SAP);

            return Autohaus.Models.AppModelMappings.Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE_GT_DEB_To_Kunde.Copy(sapList).OrderBy(k => k.Name1);
        }

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
