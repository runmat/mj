using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.Insurance.Services
{
    public class BestandsdatenDataServiceSap : CkgGeneralDataServiceSAP, IBestandsdatenDataService
    {
        public bool NurDatenDerFiliale { get; set; }

        public List<BestandsdatenModel> Bestandsdaten { get { return PropertyCacheGet(() => LoadBestandsdatenFromSap().ToList()); } }

        public BestandsdatenDataServiceSap(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshBestandsdaten()
        {
            PropertyCacheClear(this, m => m.Bestandsdaten);
        }

        private IEnumerable<BestandsdatenModel> LoadBestandsdatenFromSap()
        {
            Z_DPM_ASSIST_READ_BESTAND_01.Init(SAP, "I_KUNNR_AH", LogonContext.KundenNr.ToSapKunnr());

            if (NurDatenDerFiliale)
            {
                SAP.SetImportParameter("I_KUNNR_FIL", (LogonContext as ILogonContextDataService).User.Reference.NotNullOrEmpty().ToSapKunnr());
            }

            var sapList = Z_DPM_ASSIST_READ_BESTAND_01.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_ASSIST_READ_BESTAND_01_GT_OUT_To_BestandsdatenModel.Copy(sapList);
        }
    }
}
