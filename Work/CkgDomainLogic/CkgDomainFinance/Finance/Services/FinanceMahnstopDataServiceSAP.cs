using System;
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
    public class FinanceMahnstopDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceMahnstopDataService
    {
        public MahnstopSuchparameter Suchparameter { get; set; }

        public List<Mahnstop> Mahnstops { get { return PropertyCacheGet(() => LoadMahnstopsFromSap().ToList()); } }

        public FinanceMahnstopDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new MahnstopSuchparameter();
        }

        public void MarkForRefreshMahnstops()
        {
            PropertyCacheClear(this, m => m.Mahnstops);
        }

        private IEnumerable<Mahnstop> LoadMahnstopsFromSap()
        {
            Z_DPM_READ_MAHN_EQSTL_02.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            if (!String.IsNullOrEmpty(Suchparameter.PAID))
                SAP.SetImportParameter("I_PAID", Suchparameter.PAID);

            if (!String.IsNullOrEmpty(Suchparameter.Kontonummer))
                SAP.SetImportParameter("I_KONTONR", Suchparameter.Kontonummer);

            if (Suchparameter.DatumRange.IsSelected)
            {
                if (Suchparameter.DatumRange.StartDate.HasValue)
                    SAP.SetImportParameter("I_STOPDAT_VON", Suchparameter.DatumRange.StartDate.Value);

                if (Suchparameter.DatumRange.EndDate.HasValue)
                    SAP.SetImportParameter("I_STOPDAT_BIS", Suchparameter.DatumRange.EndDate.Value);
            }

            var sapList = Z_DPM_READ_MAHN_EQSTL_02.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_READ_MAHN_EQSTL_02_GT_OUT_To_Mahnstop.Copy(sapList);
        }

        public string SaveMahnstops(List<Mahnstop> mahnstops)
        {
            Z_DPM_SAVE_MAHN_EQSTL_01.Init(SAP, "I_AG, I_USER", LogonContext.KundenNr.ToSapKunnr(), LogonContext.UserName);

            var vList = AppModelMappings.Z_DPM_SAVE_MAHN_EQSTL_01_GT_IN_From_Mahnstop.CopyBack(mahnstops).ToList();
            SAP.ApplyImport(vList);

            SAP.Execute();

            if (SAP.ResultCode != 0)
                return Localize.SaveFailed + ": " + SAP.ResultMessage;

            return "";
        }
    }
}
