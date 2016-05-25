using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.Remarketing.Models;
using CkgDomainLogic.Remarketing.Contracts;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Remarketing.Models.AppModelMappings;

namespace CkgDomainLogic.Remarketing.Services
{
    public class GemeldeteVorschaedenDataServiceSAP : RemarketingDataServiceSAP, IGemeldeteVorschaedenDataService
    {
        public GemeldeteVorschaedenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public List<Schadensmeldung> GetGemeldeteVorschaeden(GemeldeteVorschaedenSelektor selektor)
        {
            Z_DPM_REM_SCHADENSBERICHT_02.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            if (!string.IsNullOrEmpty(selektor.Vertragsjahr))
                Z_DPM_REM_SCHADENSBERICHT_02.SetImportParameter_I_VJAHR(SAP, selektor.Vertragsjahr);

            if (!string.IsNullOrEmpty(selektor.InventarNr))
                Z_DPM_REM_SCHADENSBERICHT_02.SetImportParameter_I_INVENTAR(SAP, selektor.InventarNr);

            if (!string.IsNullOrEmpty(selektor.Vermieter))
                Z_DPM_REM_SCHADENSBERICHT_02.SetImportParameter_I_AVNR(SAP, selektor.Vermieter);

            if (selektor.SchadensdatumRange.IsSelected)
            {
                Z_DPM_REM_SCHADENSBERICHT_02.SetImportParameter_I_SCHAD_DAT_VON(SAP, selektor.SchadensdatumRange.StartDate);
                Z_DPM_REM_SCHADENSBERICHT_02.SetImportParameter_I_SCHAD_DAT_BIS(SAP, selektor.SchadensdatumRange.EndDate);
            }

            if (!string.IsNullOrEmpty(selektor.FahrgestellNr) || !string.IsNullOrEmpty(selektor.Kennzeichen))
            {
                var sapList = new List<Z_DPM_REM_SCHADENSBERICHT_02.GT_FIN_IN> { new Z_DPM_REM_SCHADENSBERICHT_02.GT_FIN_IN { FAHRGNR = selektor.FahrgestellNr, KENNZ = selektor.Kennzeichen } };
                SAP.ApplyImport(sapList);
            }

            return AppModelMappings.Z_DPM_REM_SCHADENSBERICHT_02_GT_OUT_To_Schadensmeldung.Copy(Z_DPM_REM_SCHADENSBERICHT_02.GT_OUT.GetExportListWithExecute(SAP)).ToList();
        }

        public string UpdateVorschaden(EditVorschadenModel item)
        {
            Z_DPM_REM_AEND_SCHADEN_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            var sapList = AppModelMappings.Z_DPM_REM_AEND_SCHADEN_01_GT_IN_From_EditVorschadenModel.CopyBack(new List<EditVorschadenModel> { item });

            SAP.ApplyImport(sapList);

            var errItems = Z_DPM_REM_AEND_SCHADEN_01.GT_ERR.GetExportListWithExecute(SAP);

            return (SAP.ResultCode != 0 ? SAP.ResultMessage : (errItems.Any() ? errItems.First().ZBEM : ""));
        }
    }
}
