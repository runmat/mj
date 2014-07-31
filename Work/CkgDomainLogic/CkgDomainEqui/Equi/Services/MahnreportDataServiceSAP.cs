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
    public class MahnreportDataServiceSAP : CkgGeneralDataServiceSAP, IMahnreportDataService
    {
        public List<EquiMahn> Fahrzeuge { get { return PropertyCacheGet(() => LoadFahrzeugeFromSap().ToList()); } }

        public MahnreportDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshFahrzeuge()
        {
            PropertyCacheClear(this, m => m.Fahrzeuge);
        }

        private IEnumerable<EquiMahn> LoadFahrzeugeFromSap()
        {
            var sapList = Z_DPM_READ_EQUI_MAHN_01.GT_OUT.GetExportListWithInitExecute(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            return AppModelMappings.Z_DPM_READ_EQUI_MAHN_01_GT_OUT_To_EquiMahn.Copy(sapList);
        }
    }
}
