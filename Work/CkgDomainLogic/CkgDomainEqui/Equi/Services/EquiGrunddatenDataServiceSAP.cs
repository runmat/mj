using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Equi.Models.AppModelMappings;

namespace CkgDomainLogic.Equi.Services
{
    public class EquiGrunddatenDataServiceSAP : CkgGeneralDataServiceSAP, IEquiGrunddatenDataService
    {
        public EquiGrunddatenDataServiceSAP(ISapDataService sap)
            :base(sap)
        {
        }

        public List<SelectItem> GetZielorte()
        {
            return GetAuftr006Data("ZIELORT");
        }

        public List<SelectItem> GetStandorte()
        {
            return GetAuftr006Data("STANDORT");
        }

        public List<SelectItem> GetBetriebsnummern()
        {
            return GetAuftr006Data("BETRIEBNR");
        }

        private List<SelectItem> GetAuftr006Data(string kennung)
        {
            Z_DPM_READ_AUFTR_006.Init(SAP, "I_KUNNR, I_KENNUNG", LogonContext.KundenNr.ToSapKunnr(), kennung);

            return AppModelMappings.Z_DPM_READ_AUFTR_006_GT_OUT_To_SelectItem.Copy(Z_DPM_READ_AUFTR_006.GT_OUT.GetExportListWithExecute(SAP)).ToList();
        }

        public List<EquiGrunddaten> GetEquis(EquiGrunddatenSelektor suchparameter)
        {
            Z_DPM_CD_READ_GRUEQUIDAT_02.Init(SAP, "I_AG", LogonContext.KundenNr.PadLeft(10, '0'));

            if (suchparameter.ErstzulassungsDatumRange.IsSelected)
            {
                SAP.SetImportParameter("I_REPLA_DATE_VON", suchparameter.ErstzulassungsDatumRange.StartDate);
                SAP.SetImportParameter("I_REPLA_DATE_BIS", suchparameter.ErstzulassungsDatumRange.EndDate);
            }
            if (suchparameter.AbmeldeDatumRange.IsSelected)
            {
                SAP.SetImportParameter("I_DAT_ABM_AUFTR_VON", suchparameter.AbmeldeDatumRange.StartDate);
                SAP.SetImportParameter("I_DAT_ABM_AUFTR_BIS", suchparameter.AbmeldeDatumRange.EndDate);
            }
            if (suchparameter.ErfassungsDatumRange.IsSelected)
            {
                SAP.SetImportParameter("I_ERDAT_VON", suchparameter.ErfassungsDatumRange.StartDate);
                SAP.SetImportParameter("I_ERDAT_BIS", suchparameter.ErfassungsDatumRange.EndDate);
            }

            if (suchparameter.FahrzeugeMitZulassungsStatus == "VehiclesOnlyLicensed")
                SAP.SetImportParameter("I_NUR_ZUGEL_FZG", "X");

            if (suchparameter.FahrzeugeMitZulassungsStatus == "VehiclesOnlyUnlicensed")
                SAP.SetImportParameter("I_NUR_UNZUGEL_FZG", "X");

            if (suchparameter.NurAbgemeldeteFahrzeuge)
                SAP.SetImportParameter("I_NUR_ABGEM_FZG", "X");

            // Standorte
            if (suchparameter.Standorte.AnyAndNotNull())
            {
                var standortList = AppModelMappings.Z_DPM_CD_READ_GRUEQUIDAT_02_GT_STORT_From_SelectItem.CopyBack(suchparameter.Standorte.Select(e => new SelectItem { Key = e })).ToList();
                SAP.ApplyImport(standortList);
            }
            // Betriebe
            if (suchparameter.Betriebsnummern.AnyAndNotNull())
            {
                var betriebList = AppModelMappings.Z_DPM_CD_READ_GRUEQUIDAT_02_GT_BETRIEB_From_SelectItem.CopyBack(suchparameter.Betriebsnummern.Select(e => new SelectItem { Key = e })).ToList();
                SAP.ApplyImport(betriebList);
            }
            // Zielorte
            if (suchparameter.Zielorte.AnyAndNotNull())
            {
                var zielortList = AppModelMappings.Z_DPM_CD_READ_GRUEQUIDAT_02_GT_ZIELORT_From_SelectItem.CopyBack(suchparameter.Zielorte.Select(e => new SelectItem { Key = e })).ToList();
                SAP.ApplyImport(zielortList);
            }

            // Fahrgestellnummern
            if (suchparameter.Fahrgestellnummern.AnyAndNotNull())
            {
                var fin17List = AppModelMappings.Z_DPM_CD_READ_GRUEQUIDAT_02_GT_FIN_17_To_Fahrgestellnummer.CopyBack(suchparameter.Fahrgestellnummern).ToList();
                SAP.ApplyImport(fin17List);
            }
            // Fahrgestellnummern (10-stellig)
            if (suchparameter.Fahrgestellnummern10.AnyAndNotNull())
            {
                var fin10List = AppModelMappings.Z_DPM_CD_READ_GRUEQUIDAT_02_GT_FIN_10_To_Fahrgestellnummer10.CopyBack(suchparameter.Fahrgestellnummern10).ToList();
                SAP.ApplyImport(fin10List);
            }

            SAP.Execute();

            var sapItemsEquis = Z_DPM_CD_READ_GRUEQUIDAT_02.GT_OUT.GetExportList(SAP);
            var webItemsEquis = AppModelMappings.Z_DPM_CD_READ_GRUEQUIDAT_02_GT_OUT_To_GrunddatenEqui.Copy(sapItemsEquis).OrderBy(w => w.Fahrgestellnummer).ToList();

            return webItemsEquis;
        }
    }
}
