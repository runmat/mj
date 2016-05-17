using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.Remarketing.Models;
using CkgDomainLogic.Remarketing.Contracts;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Remarketing.Models.AppModelMappings;

namespace CkgDomainLogic.Remarketing.Services
{
    public class BelastungsanzeigenDataServiceSAP : CkgGeneralDataServiceSAP, IBelastungsanzeigenDataService
    {
        public BelastungsanzeigenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public List<Vermieter> GetVermieter()
        {
            Z_DPM_READ_AUFTR6_001.Init(SAP, "I_KUNNR, I_KENNUNG", LogonContext.KundenNr.ToSapKunnr(), "VERMIETER");

            return AppModelMappings.Z_DPM_READ_AUFTR6_001_GT_WEB_To_Vermieter.Copy(Z_DPM_READ_AUFTR6_001.GT_WEB.GetExportListWithExecute(SAP)).ToList();
        }

        public List<Hereinnahmecenter> GetHereinnahmecenter()
        {
            Z_DPM_READ_HC_NR_01.Init(SAP, "I_AG, I_KENNUNG", LogonContext.KundenNr.ToSapKunnr(), "HC");

            return AppModelMappings.Z_DPM_READ_HC_NR_01_GT_WEB_To_Hereinnahmecenter.Copy(Z_DPM_READ_HC_NR_01.GT_WEB.GetExportListWithExecute(SAP)).ToList();
        }

        public List<Belastungsanzeige> GetBelastungsanzeigen(BelastungsanzeigenSelektor selektor)
        {
            Z_DPM_REM_BELASTUNGSANZEIGE_02.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            Z_DPM_REM_BELASTUNGSANZEIGE_02.SetImportParameter_I_KFZKZ(SAP, selektor.Kennzeichen);
            Z_DPM_REM_BELASTUNGSANZEIGE_02.SetImportParameter_I_FIN(SAP, selektor.FahrgestellNr);

            if (!string.IsNullOrEmpty(selektor.Vertragsjahr))
                Z_DPM_REM_BELASTUNGSANZEIGE_02.SetImportParameter_I_VJAHR(SAP, selektor.Vertragsjahr);

            if (!string.IsNullOrEmpty(selektor.Vermieter))
                Z_DPM_REM_BELASTUNGSANZEIGE_02.SetImportParameter_I_AVNR(SAP, selektor.Vermieter);

            if (!string.IsNullOrEmpty(selektor.Hereinnahmecenter))
                Z_DPM_REM_BELASTUNGSANZEIGE_02.SetImportParameter_I_HC(SAP, selektor.Hereinnahmecenter);

            if (selektor.EingangsdatumRange.IsSelected)
            {
                Z_DPM_REM_BELASTUNGSANZEIGE_02.SetImportParameter_I_ERDAT_VON(SAP, selektor.EingangsdatumRange.StartDate);
                Z_DPM_REM_BELASTUNGSANZEIGE_02.SetImportParameter_I_ERDAT_BIS(SAP, selektor.EingangsdatumRange.EndDate);
            }
            
            Z_DPM_REM_BELASTUNGSANZEIGE_02.SetImportParameter_I_STATU(SAP, selektor.Status);

            return AppModelMappings.Z_DPM_REM_BELASTUNGSANZEIGE_02_GT_OUT_To_Belastungsanzeige.Copy(Z_DPM_REM_BELASTUNGSANZEIGE_02.GT_OUT.GetExportListWithExecute(SAP)).ToList();
        }

        public List<Gutachten> GetGutachten(string fahrgestellNr)
        {
            Z_DPM_REM_BELASTUNGSA_IBUTT_02.Init(SAP, "I_KUNNR, I_FIN", LogonContext.KundenNr.ToSapKunnr(), fahrgestellNr);

            return AppModelMappings.Z_DPM_REM_BELASTUNGSA_IBUTT_02_GT_OUT_To_Gutachten.Copy(Z_DPM_REM_BELASTUNGSA_IBUTT_02.GT_OUT.GetExportListWithExecute(SAP)).ToList();
        }

        public string GetReklamationstext(string fahrgestellNr)
        {
            Z_DPM_REM_READ_REKLATEXT.Init(SAP, "I_KUNNR, I_FIN", LogonContext.KundenNr.ToSapKunnr(), fahrgestellNr);

            var sapItems = Z_DPM_REM_READ_REKLATEXT.GT_OUT.GetExportListWithExecute(SAP);

            return string.Join(" ", sapItems.Select(s => s.REKLM));
        }

        public string GetBlockadetext(string fahrgestellNr)
        {
            Z_DPM_REM_READ_BLOCKADETEXT_02.Init(SAP, "I_KUNNR, I_FIN", LogonContext.KundenNr.ToSapKunnr(), fahrgestellNr);

            var sapItems = Z_DPM_REM_READ_BLOCKADETEXT_02.GT_OUT.GetExportListWithExecute(SAP);

            return (sapItems.None() ? null : sapItems.First().BLOCKTEXT);
        }

        public string SetReklamation(SetReklamationModel item)
        {
            Z_DPM_REM_BELASTUNGSA_RBUTT_02.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            Z_DPM_REM_BELASTUNGSA_RBUTT_02.SetImportParameter_I_FIN(SAP, item.FahrgestellNr);
            Z_DPM_REM_BELASTUNGSA_RBUTT_02.SetImportParameter_I_REKLM(SAP, item.Reklamationstext);
            Z_DPM_REM_BELASTUNGSA_RBUTT_02.SetImportParameter_I_SACHB(SAP, item.Sachbearbeiter);
            Z_DPM_REM_BELASTUNGSA_RBUTT_02.SetImportParameter_I_TELNR(SAP, item.Telefon);
            Z_DPM_REM_BELASTUNGSA_RBUTT_02.SetImportParameter_I_EMAIL(SAP, item.Email);

            var erg = Z_DPM_REM_BELASTUNGSA_RBUTT_02.GT_OUT.GetExportListWithExecute(SAP);

            return (SAP.ResultCode != 0 ? SAP.ResultMessage : (erg.None() ? Localize.ErrorsOccuredOnSavingTheClaim : ""));
        }

        public string SetBelastungsanzeigenOffen(List<Belastungsanzeige> items)
        {
            Z_DPM_REM_UPD_STATNEU_BELA_01.Init(SAP, "I_KUNNR, I_WEB_USER", LogonContext.KundenNr.ToSapKunnr(), LogonContext.UserName);

            var sapList = AppModelMappings.Z_DPM_REM_UPD_STATNEU_BELA_01_GT_DAT_From_Belastungsanzeige.CopyBack(items);

            SAP.ApplyImport(sapList);

            SAP.Execute();

            return (SAP.ResultCode != 0 ? SAP.ResultMessage : "");
        }

        public string UpdateBelastungsanzeigen(List<Belastungsanzeige> items)
        {
            Z_DPM_REM_UPD_STAT_BELAST_02.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            var sapList = AppModelMappings.Z_DPM_REM_UPD_STAT_BELAST_02_GT_IN_From_Belastungsanzeige.CopyBack(items);

            SAP.ApplyImport(sapList);

            SAP.Execute();

            return (SAP.ResultCode != 0 ? SAP.ResultMessage : "");
        }
    }
}
