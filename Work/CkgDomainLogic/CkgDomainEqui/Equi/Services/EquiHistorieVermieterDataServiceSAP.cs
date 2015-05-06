using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.Equi.Services
{
    public class EquiHistorieVermieterDataServiceSAP : CkgGeneralDataServiceSAP, IEquiHistorieVermieterDataService
    {
        public EquiHistorieSuchparameter Suchparameter { get; set; }

        public List<EquiHistorieInfoVermieter> HistorieInfos { get { return PropertyCacheGet(() => LoadHistorieInfosFromSap().ToList()); } }

        public EquiHistorieVermieterDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new EquiHistorieSuchparameter();
        }

        public void MarkForRefreshHistorieInfos()
        {
            PropertyCacheClear(this, m => m.HistorieInfos);
        }

        private IEnumerable<EquiHistorieInfoVermieter> LoadHistorieInfosFromSap()
        {
            Z_DPM_FAHRZEUGHISTORIE_AVM.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (!String.IsNullOrEmpty(Suchparameter.Kennzeichen))
                SAP.SetImportParameter("I_LICENSE_NUM", Suchparameter.Kennzeichen.ToUpper());

            if (!String.IsNullOrEmpty(Suchparameter.FahrgestellNr))
                SAP.SetImportParameter("I_CHASSIS_NUM", Suchparameter.FahrgestellNr.ToUpper());

            if (!String.IsNullOrEmpty(Suchparameter.BriefNr))
                SAP.SetImportParameter("I_TIDNR", Suchparameter.BriefNr.ToUpper());

            if (!String.IsNullOrEmpty(Suchparameter.VertragsNr))
                SAP.SetImportParameter("I_REFERENZ1", Suchparameter.VertragsNr.ToUpper());

            var sapList = Z_DPM_FAHRZEUGHISTORIE_AVM.GT_EQUIS.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_EQUIS_To_EquiHistorieInfoVermieter.Copy(sapList);
        }

        public EquiHistorieVermieter GetEquiHistorie(string equiNr, string meldungsNr)
        {
            Z_DPM_FAHRZEUGHISTORIE_AVM.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            SAP.SetImportParameter("I_EQUNR", equiNr);
            SAP.SetImportParameter("I_QMNUM", meldungsNr);

            SAP.Execute();

            var sapItems = Z_DPM_FAHRZEUGHISTORIE_AVM.GT_UEBER.GetExportList(SAP);

            if (sapItems.None())
                return null;

            var hist = AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_UEBER_To_EquiHistorieVermieter.Copy(sapItems).ToList().First();

            hist.HistorieInfo = (AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_EQUIS_To_EquiHistorieInfoVermieter.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_EQUIS.GetExportList(SAP)).ToList().FirstOrDefault() ?? new EquiHistorieInfoVermieter());

            hist.Einsteuerungsdaten = (AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_EINST_To_EquiEinsteuerung.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_EINST.GetExportList(SAP)).ToList().FirstOrDefault() ?? new EquiEinsteuerung());

            hist.Aussteuerungsdaten = (AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_AUSST_To_EquiAussteuerung.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_AUSST.GetExportList(SAP)).ToList().FirstOrDefault() ?? new EquiAussteuerung());

            hist.Typdaten = (AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_TYPEN_To_EquiTypdaten.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_TYPEN.GetExportList(SAP)).ToList().FirstOrDefault() ?? new EquiTypdaten());
            hist.Typdaten.Farbe = hist.Farbe;
            hist.Typdaten.Farbcode = hist.Farbcode;

            hist.LebenslaufZb2 = AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_LLZB2_To_EquiMeldungsdaten.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_LLZB2.GetExportList(SAP)).ToList();

            hist.LebenslaufFsm = AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_LLSCH_To_EquiMeldungsdaten.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_LLSCH.GetExportList(SAP)).ToList();

            hist.InhalteFsm = AppModelMappings.Z_DPM_FAHRZEUGHISTORIE_AVM_GT_TUETE_To_EquiTueteninhalt.Copy(Z_DPM_FAHRZEUGHISTORIE_AVM.GT_TUETE.GetExportList(SAP)).ToList();

            return hist;
        }

        public byte[] GetHistorieAsPdf(string equiNr, string meldungsNr)
        {
            Z_DPM_DRUCK_FZG_HISTORIE_AVM.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            SAP.SetImportParameter("I_EQUNR", equiNr);
            SAP.SetImportParameter("I_QMNUM", meldungsNr);

            SAP.Execute();

            return SAP.GetExportParameterByte("E_PDF");
        }
    }
}
