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
    public class FinanceTempZb2VersandDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceTempZb2VersandDataService
    {
        public List<TempZb2Versand> TempZb2Versands { get { return PropertyCacheGet(() => LoadTempZb2VersandFromSap().ToList()); } }

        public FinanceTempZb2VersandDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshTempZb2Versand()
        {
            PropertyCacheClear(this, m => m.TempZb2Versands);
        }

        private IEnumerable<TempZb2Versand> LoadTempZb2VersandFromSap()
        {
            Z_M_BRIEF_TEMP_VERS_MAHN_001.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            var sapList = Z_M_BRIEF_TEMP_VERS_MAHN_001.GT_WEB.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_EXP_MAHN_ERSTEINGANG_GT_WEB_To_TempZb2Versand.Copy(sapList);
        }       
    }    
}
