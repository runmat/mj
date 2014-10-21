using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Equi.Models.AppModelMappings;

namespace CkgDomainLogic.Equi.Services
{
    public class BriefbestandVhcDataServiceSAP : CkgGeneralDataServiceSAP, IBriefbestandVhcDataService
    {
        public List<FahrzeugbriefVhc> FahrzeugbriefeVhc { get { return PropertyCacheGet(() => LoadFahrzeugbriefeVhcFromSap().ToList()); } }

        public BriefbestandVhcDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshFahrzeugbriefeVhc()
        {
            PropertyCacheClear(this, m => m.FahrzeugbriefeVhc);
        }

        private IEnumerable<FahrzeugbriefVhc> LoadFahrzeugbriefeVhcFromSap()
        {
            var sapList = Z_M_VHC_ZBII_BESTAND_001.GT_WEB.GetExportListWithInitExecute(SAP, "I_KONZS", LogonContext.KundenNr.ToSapKunnr());

            return AppModelMappings.Z_M_VHC_ZBII_BESTAND_001_GT_WEB_To_FahrzeugbriefVhc.Copy(sapList);
        }
    }
}
