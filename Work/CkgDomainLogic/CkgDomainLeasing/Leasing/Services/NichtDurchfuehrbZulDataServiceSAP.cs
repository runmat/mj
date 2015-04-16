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
    public class NichtDurchfuehrbZulDataServiceSAP : CkgGeneralDataServiceSAP, INichtDurchfuehrbZulDataService
    {
        public List<NichtDurchfuehrbareZulassung> NichtDurchfuehrbareZulassungen { get { return PropertyCacheGet(() => LoadNichtDurchfuehrbareZulassungenFromSap().ToList()); } }

        public NichtDurchfuehrbZulDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshNichtDurchfuehrbareZulassungen()
        {
            PropertyCacheClear(this, m => m.NichtDurchfuehrbareZulassungen);
        }

        private IEnumerable<NichtDurchfuehrbareZulassung> LoadNichtDurchfuehrbareZulassungenFromSap()
        {
            Z_M_KLAERFAELLEVW.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            return AppModelMappings.Z_M_KLAERFAELLEVW_GT_WEB_To_NichtDurchfuehrbareZulassung.Copy(Z_M_KLAERFAELLEVW.GT_WEB.GetExportListWithExecute(SAP));
        }
    }
}
