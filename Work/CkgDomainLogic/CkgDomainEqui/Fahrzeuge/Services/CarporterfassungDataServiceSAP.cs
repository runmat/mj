using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using GeneralTools.Models;
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

        public IDictionary<string, string> GetCarportPdis()
        {
            Z_DPM_READ_CARPID_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            return Z_DPM_READ_CARPID_01.GT_TAB.GetExportListWithExecute(SAP).ToDictionary(s => s.KUNPDI, s => string.Format("{0} - {1}", s.KUNPDI, s.NAME1));
        }

        public List<CarporterfassungModel> SaveFahrzeuge(List<CarporterfassungModel> items, ref string errorMessage)
        {
            try
            {
                Z_DPM_IMP_CARPORT_MELD_01.Init(SAP, "I_WEB_USER", LogonContext.UserName);

                var sapItems = AppModelMappings.Z_DPM_IMP_CARPORT_MELD_01_GT_WEB_From_CarporterfassungModel.CopyBack(items);
                SAP.ApplyImport(sapItems);

                return AppModelMappings.Z_DPM_IMP_CARPORT_MELD_01_GT_WEB_To_CarporterfassungModel.Copy(Z_DPM_IMP_CARPORT_MELD_01.GT_WEB.GetExportListWithExecute(SAP)).ToList();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return items;
        }

        public List<CarportInfo> GetCarportAdressen(string adressKennung)
        {
            Z_DPM_READ_AUFTR_006.Init(SAP, "I_KUNNR, I_KENNUNG", LogonContext.KundenNr.ToSapKunnr(), adressKennung);

            return AppModelMappings.Z_DPM_READ_AUFTR_006_GT_OUT_To_CarportInfo.Copy(Z_DPM_READ_AUFTR_006.GT_OUT.GetExportListWithExecute(SAP)).ToList();
        }
    }
}
