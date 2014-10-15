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
    public class FinanceAnzeigePruefpunkteDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceAnzeigePruefpunkteDataService
    {
        public PruefpunktSuchparameter Suchparameter { get; set; }

        public List<Pruefpunkt> Pruefpunkte { get { return PropertyCacheGet(() => LoadPruefpunkteFromSap().ToList()); } }

        public FinanceAnzeigePruefpunkteDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new PruefpunktSuchparameter { NurKlaerfaelle = false };
        }

        public void MarkForRefreshPruefpunkte()
        {
            PropertyCacheClear(this, m => m.Pruefpunkte);
        }

        private IEnumerable<Pruefpunkt> LoadPruefpunkteFromSap()
        {
            Z_DPM_READ_PRUEFPUNKTE_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (!String.IsNullOrEmpty(Suchparameter.Kontonummer))
            {
                SAP.SetImportParameter("I_KONTONR", Suchparameter.Kontonummer);
            }
            if (!String.IsNullOrEmpty(Suchparameter.PAID))
            {
                SAP.SetImportParameter("I_PAID", Suchparameter.PAID);
            }

            if (Suchparameter.DatumRange.IsSelected)
            {
                if (Suchparameter.DatumRange.StartDate.HasValue)
                    SAP.SetImportParameter("I_PRUEDAT_VON", Suchparameter.DatumRange.StartDate.Value);

                if (Suchparameter.DatumRange.EndDate.HasValue)
                    SAP.SetImportParameter("I_PRUEDAT_BIS", Suchparameter.DatumRange.EndDate.Value);
            }

            if (Suchparameter.NurKlaerfaelle)
                SAP.SetImportParameter("I_KLAEFALL", "X");

            var sapList = Z_DPM_READ_PRUEFPUNKTE_01.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_READ_PRUEFPUNKTE_01_GT_OUT_To_Pruefpunkt.Copy(sapList);
        }
    }
}
