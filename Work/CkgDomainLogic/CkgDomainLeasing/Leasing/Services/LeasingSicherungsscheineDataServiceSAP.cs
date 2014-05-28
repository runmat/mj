using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Leasing.Models.AppModelMappings;

namespace CkgDomainLogic.Leasing.Services
{
    public class LeasingSicherungsscheineDataService : CkgGeneralDataServiceSAP, ILeasingSicherungsscheineDataService
    {
        public List<Sicherungsschein> Sicherungsscheine { get { return PropertyCacheGet(() => LoadSicherungsscheineFromSap().ToList()); } }

        public LeasingSicherungsscheineDataService(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshSicherungsscheine()
        {
            PropertyCacheClear(this, m => m.Sicherungsscheine);
        }

        private IEnumerable<Sicherungsschein> LoadSicherungsscheineFromSap()
        {
            Z_DPM_SIS_BESTAND.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            var sapList = Z_DPM_SIS_BESTAND.GT_WEB.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_SIS_BESTAND_GT_WEB_To_Sicherungsschein.Copy(sapList);
        }

    }
}
