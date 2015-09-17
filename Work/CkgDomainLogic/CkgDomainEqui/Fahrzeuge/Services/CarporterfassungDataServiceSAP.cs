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

        public CarporterfassungModel LoadFahrzeugdaten(string kennzeichen)
        {
            Z_DPM_READ_MEL_CARP_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            SAP.SetImportParameter("I_LICENSE_NUM", kennzeichen);
            SAP.SetImportParameter("I_CARPORT_ID_AG", LogonContext.User.Reference);

            return AppModelMappings.Z_DPM_READ_MEL_CARP_01_GT_TAB_To_CarporterfassungModel.Copy(Z_DPM_READ_MEL_CARP_01.GT_TAB.GetExportListWithExecute(SAP).FirstOrDefault());
        }

        public List<CarporterfassungModel> SaveFahrzeuge(List<CarporterfassungModel> items)
        {
            Z_DPM_IMP_CARPORT_MELD_01.Init(SAP, "I_WEB_USER", LogonContext.UserName);

            var sapItems = AppModelMappings.Z_DPM_IMP_CARPORT_MELD_01_GT_WEB_From_CarporterfassungModel.CopyBack(items);
            SAP.ApplyImport(sapItems);

            return AppModelMappings.Z_DPM_IMP_CARPORT_MELD_01_GT_WEB_To_CarporterfassungModel.Copy(Z_DPM_IMP_CARPORT_MELD_01.GT_WEB.GetExportListWithExecute(SAP)).ToList();
        }
    }
}
