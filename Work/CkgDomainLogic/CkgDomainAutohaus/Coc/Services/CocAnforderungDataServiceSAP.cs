using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.General.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Autohaus.Models.AppModelMappings;

namespace CkgDomainLogic.Autohaus.Services
{
    public class CocAnforderungDataServiceSAP : CkgGeneralDataServiceSAP, ICocAnforderungDataService
    {
        public CocAnforderungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public List<Hersteller> HerstellerGesamtliste { get { return PropertyCacheGet(() => LoadHerstellerFromSap().ToList()); } }

        private IEnumerable<Hersteller> LoadHerstellerFromSap()
        {
            Z_DPM_READ_ZDAD_AUFTR_006.Init(SAP, "I_KENNUNG", "HERSTELLER");

            return AppModelMappings.Z_DPM_READ_ZDAD_AUFTR_006_GT_WEB_To_Hersteller.Copy(Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB.GetExportListWithExecute(SAP));
        }

        public string GetEmpfaengerEmailAdresse()
        {
            Z_DPM_READ_ZDAD_AUFTR_006.Init(SAP, "I_KENNUNG", "COC-ANFORDERUNG");

            SAP.Execute();

            if (SAP.ResultCode == 0)
            {
                var sapItem = Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB.GetExportList(SAP).FirstOrDefault();

                if (sapItem != null)
                    return sapItem.EMAIL;
            }

            return "";
        }
    }
}
