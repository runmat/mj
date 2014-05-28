using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Leasing.Models.AppModelMappings;

namespace CkgDomainLogic.Leasing.Services
{
    public class LeasingKlaerfaelleDataServiceSAP : CkgGeneralDataServiceSAP, ILeasingKlaerfaelleDataService
    {
        public KlaerfallSuchparameter Suchparameter { get; set; }
        public List<Klaerfall> Klaerfaelle { get { return PropertyCacheGet(() => LoadKlaerfaelleFromSap().ToList()); } }

        public LeasingKlaerfaelleDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new KlaerfallSuchparameter();
        }

        public void MarkForRefreshKlaerfaelle()
        {
            PropertyCacheClear(this, m => m.Klaerfaelle);
        }

        private IEnumerable<Klaerfall> LoadKlaerfaelleFromSap()
        {
            Z_DPM_SIXT_PG_KLAERFALL.Init(SAP, "KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (Suchparameter.Klaerfaelle)
            {
                SAP.SetImportParameter("KLAERFALL", "X");
            }

            if (Suchparameter.AuswahlFehlendeDaten == 1)
            {
                SAP.SetImportParameter("EMAIL", "X");
            }
            else if (Suchparameter.AuswahlFehlendeDaten == 2)
            {
                SAP.SetImportParameter("ANNAHME", "X");
            }

            if (!String.IsNullOrEmpty(Suchparameter.Leasingvertragsnummer))
            {
                SAP.SetImportParameter("LVTNR", Suchparameter.Leasingvertragsnummer);
            }

            if (!String.IsNullOrEmpty(Suchparameter.Kundennummer))
            {
                SAP.SetImportParameter("KUNUM", Suchparameter.Kundennummer);
            }

            var sapList = Z_DPM_SIXT_PG_KLAERFALL.GT_WEB.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_SIXT_PG_KLAERFALL_GT_WEB_To_Klaerfall.Copy(sapList);
        }
    }
}
