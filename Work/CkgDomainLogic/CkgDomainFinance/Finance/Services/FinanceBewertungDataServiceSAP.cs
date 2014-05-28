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
    public class FinanceBewertungDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceBewertungDataService
    {
        public VorgangSuchparameter Suchparameter { get; set; }
        public List<VorgangInfo> Vorgaenge { get { return PropertyCacheGet(() => LoadVorgaengeFromSap().ToList()); } }

        public FinanceBewertungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new VorgangSuchparameter();
        }

        public void MarkForRefreshVorgaenge()
        {
            PropertyCacheClear(this, m => m.Vorgaenge);
        }

        private IEnumerable<VorgangInfo> LoadVorgaengeFromSap()
        {
            Z_DPM_VERTRAGSBESTAND_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (Suchparameter.Vertragsart != "alle")
            {
                SAP.SetImportParameter("I_VERT_ART", Suchparameter.Vertragsart);
            }
            if (!String.IsNullOrEmpty(Suchparameter.Kontonummer))
            {
                SAP.SetImportParameter("I_KONTONR", Suchparameter.Kontonummer);
            }
            if (!String.IsNullOrEmpty(Suchparameter.CIN))
            {
                SAP.SetImportParameter("I_CIN", Suchparameter.CIN);
            }
            if (!String.IsNullOrEmpty(Suchparameter.PAID))
            {
                SAP.SetImportParameter("I_PAID", Suchparameter.PAID);
            }

            var sapList = Z_DPM_VERTRAGSBESTAND_01.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_VERTRAGSBESTAND_01_GT_OUT_To_VorgangInfo.Copy(sapList);
        }

        public string SaveBewertung(VorgangBewertung vorg)
        {
            var erg = "";
            var vorgaenge = new List<VorgangBewertung> { vorg };

            Z_DPM_SAVE_WERTE_GUTA_VERW_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (vorg.Erfassungstyp == Erfassungsart.Gutachterwert)
            {
                SAP.SetImportParameter("I_GUTA", "X");
            }
            else
            {
                SAP.SetImportParameter("I_VERWERT", "X");
            }

            var vList = AppModelMappings.Z_DPM_SAVE_WERTE_GUTA_VERW_01_GT_IN_From_VorgangBewertung.CopyBack(vorgaenge).ToList();
            SAP.ApplyImport(vList);

            SAP.Execute();

            var ergList = Z_DPM_SAVE_WERTE_GUTA_VERW_01.GT_OUT.GetExportList(SAP);
            if ((ergList.Count > 0) && (!String.IsNullOrEmpty(ergList[0].BEM)))
            {
                erg = ergList[0].BEM;
            }

            return erg;
        }
    }
}
