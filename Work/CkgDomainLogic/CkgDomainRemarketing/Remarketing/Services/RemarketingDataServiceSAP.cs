using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.Remarketing.Models;
using CkgDomainLogic.Remarketing.Contracts;
using CkgDomainLogic.General.Services;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Remarketing.Models.AppModelMappings;

namespace CkgDomainLogic.Remarketing.Services
{
    public class RemarketingDataServiceSAP : CkgGeneralDataServiceSAP, IRemarketingDataService
    {
        public RemarketingDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public List<Vermieter> GetVermieter()
        {
            Z_DPM_READ_AUFTR6_001.Init(SAP, "I_KUNNR, I_KENNUNG", LogonContext.KundenNr.ToSapKunnr(), "VERMIETER");

            return AppModelMappings.Z_DPM_READ_AUFTR6_001_GT_WEB_To_Vermieter.Copy(Z_DPM_READ_AUFTR6_001.GT_WEB.GetExportListWithExecute(SAP)).ToList();
        }

        public List<Hereinnahmecenter> GetHereinnahmecenter()
        {
            Z_DPM_READ_HC_NR_01.Init(SAP, "I_AG, I_KENNUNG", LogonContext.KundenNr.ToSapKunnr(), "HC");

            return AppModelMappings.Z_DPM_READ_HC_NR_01_GT_WEB_To_Hereinnahmecenter.Copy(Z_DPM_READ_HC_NR_01.GT_WEB.GetExportListWithExecute(SAP)).ToList();
        }
    }
}
