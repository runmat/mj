using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeuge.Models.AppModelMappings;

namespace CkgDomainLogic.Fahrzeuge.Services
{
    public class FahrzeugzulaeufeDataServiceSAP : CkgGeneralDataServiceSAP, IFahrzeugzulaeufeDataService
    {
        public FahrzeugzulaeufeSelektor Suchparameter { get; set; }

        public List<Hersteller> HerstellerListe { get { return PropertyCacheGet(() => LoadFahrzeugherstellerFromSap().ToList()); } }

        public List<Fahrzeugzulauf> Fahrzeugzulaeufe { get { return PropertyCacheGet(() => LoadFahrzeugzulaeufeFromSap().ToList()); } }

        public FahrzeugzulaeufeDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new FahrzeugzulaeufeSelektor();
        }

        public void MarkForRefreshFahrzeugzulaeufe()
        {
            PropertyCacheClear(this, m => m.Fahrzeugzulaeufe);
        }

        public void MarkForRefreshHerstellerListe()
        {
            PropertyCacheClear(this, m => m.HerstellerListe);
        }

        private IEnumerable<Hersteller> LoadFahrzeugherstellerFromSap()
        {
            Z_M_HERSTELLERGROUP.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            return AppModelMappings.Z_M_HERSTELLERGROUP_T_HERST_To_Hersteller.Copy(Z_M_HERSTELLERGROUP.T_HERST.GetExportListWithExecute(SAP));
        }

        private IEnumerable<Fahrzeugzulauf> LoadFahrzeugzulaeufeFromSap()
        {
            Z_M_EC_AVM_ZULAUF.Init(SAP);

            if (Suchparameter.DatumRange.IsSelected)
            {
                if (Suchparameter.DatumRange.StartDate.HasValue)
                    SAP.SetImportParameter("ZUL_DAT", Suchparameter.DatumRange.StartDate.Value);

                if (Suchparameter.DatumRange.EndDate.HasValue)
                    SAP.SetImportParameter("ZUL_BIS", Suchparameter.DatumRange.EndDate.Value);
            }

            if (!String.IsNullOrEmpty(Suchparameter.HerstellerSchluessel))
                SAP.SetImportParameter("I_HERSTNR", Suchparameter.HerstellerSchluessel);

            return AppModelMappings.Z_M_EC_AVM_ZULAUF_GT_WEB_To_Fahrzeugzulauf.Copy(Z_M_EC_AVM_ZULAUF.GT_WEB.GetExportListWithExecute(SAP)).OrderBy(f => f.Hersteller).ThenBy(f => f.Modell).ThenBy(f => f.FahrgestellNr);
        }
    }
}
