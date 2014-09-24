﻿using System;
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
    public class FinancePruefschritteDataServiceSAP : CkgGeneralDataServiceSAP, IFinancePruefschritteDataService
    {
        public PruefschrittSuchparameter Suchparameter { get; set; }

        public List<Pruefschritt> Pruefschritte { get { return PropertyCacheGet(() => LoadPruefschritteFromSap().ToList()); } }

        public FinancePruefschritteDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new PruefschrittSuchparameter();
        }

        public void MarkForRefreshPruefschritte()
        {
            PropertyCacheClear(this, m => m.Pruefschritte);
        }

        private IEnumerable<Pruefschritt> LoadPruefschritteFromSap()
        {
            Z_DPM_READ_PRUEFSCHRITTE_03.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (!String.IsNullOrEmpty(Suchparameter.Kontonummer))
            {
                SAP.SetImportParameter("I_KONTONR", Suchparameter.Kontonummer);
            }
            if (!String.IsNullOrEmpty(Suchparameter.PAID))
            {
                SAP.SetImportParameter("I_PAID", Suchparameter.PAID);
            }

            var sapList = Z_DPM_READ_PRUEFSCHRITTE_03.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_READ_PRUEFSCHRITTE_03_GT_OUT_To_Pruefschritt.Copy(sapList);
        }
    }
}
