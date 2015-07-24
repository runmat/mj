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
        public KlaerfaelleVhcSuchparameter Suchparameter { get; set; }

        public List<KlaerfallVhc> KlaerfaelleVhc { get { return PropertyCacheGet(() => LoadKlaerfaelleVhcFromSap().ToList()); } }

        public KlaerfaelleVhcDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new KlaerfaelleVhcSuchparameter { Auswahl = "K" };
        }

        public void MarkForRefreshKlaerfaelleVhc()
        {
            PropertyCacheClear(this, m => m.KlaerfaelleVhc);
        }

        private IEnumerable<KlaerfallVhc> LoadKlaerfaelleVhcFromSap()
        {
            if (Suchparameter.Auswahl == "D")
                return AppModelMappings.Z_DPM_FFD_DATEN_OHNE_DOKUMENTE_GT_WEB_To_KlaerfallVhc.Copy(Z_DPM_FFD_DATEN_OHNE_DOKUMENTE.GT_WEB.GetExportListWithInitExecute(SAP, "I_KUNNR_AG, I_TAGE", LogonContext.KundenNr.ToSapKunnr(), 3));
            
            return AppModelMappings.Z_M_VHC_KLAERFAELLE_001_GT_WEB_To_KlaerfallVhc.Copy(Z_M_VHC_KLAERFAELLE_001.GT_WEB.GetExportListWithInitExecute(SAP, "I_KONZS, I_VKORG", LogonContext.KundenNr.ToSapKunnr(), "1510"));
        }
    }
}
