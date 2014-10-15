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
    public class FinanceMahnstufenDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceMahnstufenDataService
    {
        public MahnungSuchparameter Suchparameter { get; set; }

        public List<Mahnung> Mahnungen { get { return PropertyCacheGet(() => LoadMahnungenFromSap().ToList()); } }

        public FinanceMahnstufenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new MahnungSuchparameter();
        }

        public void MarkForRefreshMahnungen()
        {
            PropertyCacheClear(this, m => m.Mahnungen);
        }

        private IEnumerable<Mahnung> LoadMahnungenFromSap()
        {
            Z_DPM_READ_STL_MAHNUNGEN_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (!string.IsNullOrEmpty(Suchparameter.Fahrgestellnummer))
                SAP.SetImportParameter("I_CHASSIS_NUM", Suchparameter.Fahrgestellnummer);

            if (Suchparameter.VersanddatumRange.IsSelected)
            {
                if (Suchparameter.VersanddatumRange.StartDate.HasValue)
                    SAP.SetImportParameter("I_ZZTMPDT_VON", Suchparameter.VersanddatumRange.StartDate.Value);

                if (Suchparameter.VersanddatumRange.EndDate.HasValue)
                    SAP.SetImportParameter("I_ZZTMPDT_BIS", Suchparameter.VersanddatumRange.EndDate.Value);
            }

            if (Suchparameter.Mahndatum.HasValue)
                SAP.SetImportParameter("I_ZZMADAT", Suchparameter.Mahndatum.Value);

            if (!string.IsNullOrEmpty(Suchparameter.Mahnstufe))
                SAP.SetImportParameter("I_ZZMAHNS", Suchparameter.Mahnstufe);

            if (Suchparameter.Mahnsperre)
                SAP.SetImportParameter("I_ZZMANSP", "X");

            var sapList = Z_DPM_READ_STL_MAHNUNGEN_01.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_READ_STL_MAHNUNGEN_01_GT_OUT_To_Mahnung.Copy(sapList);
        }
    }
}
