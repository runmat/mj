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
    public class KlaerfaelleVhcDataServiceSAP : CkgGeneralDataServiceSAP, IKlaerfaelleVhcDataService
    {
        public List<KlaerfallVhc> KlaerfaelleVhc { get { return PropertyCacheGet(() => LoadKlaerfaelleVhcFromSap().ToList()); } }

        public KlaerfaelleVhcDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshKlaerfaelleVhc()
        {
            PropertyCacheClear(this, m => m.KlaerfaelleVhc);
        }

        private IEnumerable<KlaerfallVhc> LoadKlaerfaelleVhcFromSap()
        {
            var sapList = Z_M_VHC_KLAERFAELLE_001.GT_WEB.GetExportListWithInitExecute(SAP, "I_KONZS", LogonContext.KundenNr.ToSapKunnr());

            return AppModelMappings.Z_M_VHC_KLAERFAELLE_001_GT_WEB_To_KlaerfallVhc.Copy(sapList);
        }
    }
}
