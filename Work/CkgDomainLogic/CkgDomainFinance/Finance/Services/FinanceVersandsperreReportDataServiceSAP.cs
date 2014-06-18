using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Finance.Models.AppModelMappings;

namespace CkgDomainLogic.Finance.Services
{
    public class FinanceVersandsperreReportDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceVersandsperreReportDataService
    {
        public List<VorgangVersandsperre> Vorgaenge { get { return PropertyCacheGet(() => LoadVorgaengeFromSap().ToList()); } }

        public FinanceVersandsperreReportDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshVorgaenge()
        {
            PropertyCacheClear(this, m => m.Vorgaenge);
        }

        private IEnumerable<VorgangVersandsperre> LoadVorgaengeFromSap()
        {
            var sapList = Z_DPM_LIST_ZZAKTSPERRE.GT_WEB.GetExportListWithInitExecute(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            return AppModelMappings.Z_DPM_LIST_ZZAKTSPERRE_GT_WEB_To_VorgangVersandsperre.Copy(sapList);
        }
    }
}
