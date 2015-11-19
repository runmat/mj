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
    public class FinanceVersendungenDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceVersendungenDataService
    {
        public FinanceVersendungenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void GetVersendungenFromSap(VersendungenSuchparameter suchparameter, out List<Versendung> versendungen, out List<VersendungSummiert> versendungenSummiert)
        {
            Z_DPM_EXP_VERS_AUSWERTUNG_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (suchparameter.ImportdatumRange.IsSelected && suchparameter.ImportdatumRange.StartDate.HasValue && suchparameter.ImportdatumRange.EndDate.HasValue)
            {
                SAP.SetImportParameter("I_DAT_ANGEL_VON", suchparameter.ImportdatumRange.StartDate);
                SAP.SetImportParameter("I_DAT_ANGEL_BIS", suchparameter.ImportdatumRange.EndDate);
            }

            if (suchparameter.VerarbeitungsdatumRange.IsSelected && suchparameter.VerarbeitungsdatumRange.StartDate.HasValue && suchparameter.VerarbeitungsdatumRange.EndDate.HasValue)
            {
                SAP.SetImportParameter("I_DAT_VERSAUFTR_VON", suchparameter.VerarbeitungsdatumRange.StartDate);
                SAP.SetImportParameter("I_DAT_VERSAUFTR_BIS", suchparameter.VerarbeitungsdatumRange.EndDate);
            }

            if (suchparameter.Vertragsart != "ALLE")
                SAP.SetImportParameter("I_VERT_ART", suchparameter.Vertragsart);

            if (suchparameter.Versandart != "ALLE")
                SAP.SetImportParameter("I_VERSANDART", suchparameter.Versandart);

            if (!string.IsNullOrEmpty(suchparameter.HaendlerNr))
                SAP.SetImportParameter("I_KUNNR_BEIM_AG", suchparameter.HaendlerNr);

            SAP.Execute();

            versendungen = AppModelMappings.Z_DPM_EXP_VERS_AUSWERTUNG_01_GT_OUT_To_Versendung.Copy(Z_DPM_EXP_VERS_AUSWERTUNG_01.GT_OUT.GetExportList(SAP)).ToList();
            versendungenSummiert = AppModelMappings.Z_DPM_EXP_VERS_AUSWERTUNG_01_GT_OUT_SUM_To_VersendungSummiert.Copy(Z_DPM_EXP_VERS_AUSWERTUNG_01.GT_OUT_SUM.GetExportList(SAP)).ToList();
        }
    }
}
