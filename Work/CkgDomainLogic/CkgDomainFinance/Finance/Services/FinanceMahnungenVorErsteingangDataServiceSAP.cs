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
    public class FinanceMahnungenVorErsteingangDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceMahnungenVorErsteingangDataService
    {
        public MahnungVorErsteingangSuchparameter Suchparameter { get; set; }

        public List<Mahnung> Mahnungen { get { return PropertyCacheGet(() => LoadMahnungenFromSap().ToList()); } }

        public FinanceMahnungenVorErsteingangDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new MahnungVorErsteingangSuchparameter();
        }

        public void MarkForRefreshMahnungen()
        {
            PropertyCacheClear(this, m => m.Mahnungen);
        }

        private IEnumerable<Mahnung> LoadMahnungenFromSap()
        {
            Z_DPM_EXP_MAHN_ERSTEINGANG.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            if (Suchparameter.Vertragsart != "alle")
            {
                SAP.SetImportParameter("I_ZVERT_ART", Suchparameter.Vertragsart);
            }

            if (Suchparameter.Mahnstufe1)
                SAP.SetImportParameter("I_MAHNSTUFE1", "X");

            if (Suchparameter.Mahnstufe2)
                SAP.SetImportParameter("I_MAHNSTUFE2", "X");

            if (Suchparameter.Mahnstufe3)
                SAP.SetImportParameter("I_MAHNSTUFE3", "X");

            if (Suchparameter.MahnsperreGesetzt)
                SAP.SetImportParameter("I_MASPER_GES", "X");

            var sapList = Z_DPM_EXP_MAHN_ERSTEINGANG.GT_WEB.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_EXP_MAHN_ERSTEINGANG_GT_WEB_To_Mahnung.Copy(sapList);
        }
    }
}
