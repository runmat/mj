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
    public class CarporterfassungDataServiceSAP : CkgGeneralDataServiceSAP, ICarporterfassungDataService
    {
        public CarporterfassungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public CarporterfassungModel LoadFahrzeugdaten(string kennzeichen, string bestandsnummer, string fin)
        {
            Z_DPM_READ_MEL_CARP_01.Init(SAP, "I_AG, I_CARPORT_ID_AG", LogonContext.KundenNr.ToSapKunnr(), LogonContext.User.Reference);

            if (!String.IsNullOrEmpty(kennzeichen))
                SAP.SetImportParameter("I_LICENSE_NUM", kennzeichen);

            if (!String.IsNullOrEmpty(bestandsnummer))
                SAP.SetImportParameter("I_MVA_NUMMER", bestandsnummer);

            if (!String.IsNullOrEmpty(fin))
                SAP.SetImportParameter("I_CHASSIS_NUM", fin);

            return AppModelMappings.Z_DPM_READ_MEL_CARP_01_GT_TAB_To_CarporterfassungModel.Copy(Z_DPM_READ_MEL_CARP_01.GT_TAB.GetExportListWithExecute(SAP).FirstOrDefault());
        }

        public List<CarporterfassungModel> SaveFahrzeuge(List<CarporterfassungModel> items)
        {
            Z_DPM_IMP_CARPORT_MELD_01.Init(SAP, "I_WEB_USER", LogonContext.UserName);

            var sapItems = AppModelMappings.Z_DPM_IMP_CARPORT_MELD_01_GT_WEB_From_CarporterfassungModel.CopyBack(items);
            SAP.ApplyImport(sapItems);

            return AppModelMappings.Z_DPM_IMP_CARPORT_MELD_01_GT_WEB_To_CarporterfassungModel.Copy(Z_DPM_IMP_CARPORT_MELD_01.GT_WEB.GetExportListWithExecute(SAP)).ToList();
        }

        public CarportInfo GetCarportInfo(string carportId)
        {
            Z_DPM_READ_AUFTR_006.Init(SAP, "I_KUNNR, I_KENNUNG", LogonContext.KundenNr.ToSapKunnr(), carportId);

            return AppModelMappings.Z_DPM_READ_AUFTR_006_GT_OUT_To_CarportInfo.Copy(Z_DPM_READ_AUFTR_006.GT_OUT.GetExportListWithExecute(SAP)).FirstOrDefault();
        }
    }
}
